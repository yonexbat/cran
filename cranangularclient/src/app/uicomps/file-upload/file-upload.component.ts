import { Component,
  OnInit,
  AfterViewInit,
  ViewChild, ElementRef,
  Output, Input,
  EventEmitter,
  Renderer2} from '@angular/core';

import {Binary} from '../../model/binary';
import {CookieService} from '../../services/cookie.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit, AfterViewInit {

  @Output() onUploadStarted = new EventEmitter<void>();
  @Output() onError = new EventEmitter<string>();
  @Output() onUploaded = new EventEmitter<Binary[]>();

  @Input() placeHolderText = 'Upload file...';
  @ViewChild('fileInputParent', { static: true }) fileInputParent: ElementRef;

  constructor(private cookieService: CookieService, private renderer: Renderer2) { }

  ngOnInit() {

  }

  ngAfterViewInit(): void {
    this.addFileInput();
  }


  private addFileInput() {

    const newFileInput = this.renderer.createElement('input');
    this.renderer.setAttribute(newFileInput, 'type', 'file');
    this.renderer.setAttribute(newFileInput, 'multiple', 'true');
    this.renderer.setAttribute(newFileInput, 'name', 'fileInput');
    const uploadfiles = this.uploadFiles.bind(this);
    this.renderer.listen(newFileInput, 'change', uploadfiles);

    const fileInputParentNative = this.fileInputParent.nativeElement;
    const childElements = fileInputParentNative.childNodes;
    for (const child of childElements) {
      this.renderer.removeChild(fileInputParentNative, child);
    }
    this.renderer.appendChild(fileInputParentNative, newFileInput);
  }

  private uploadFiles() {
    const fileInputParentNative = this.fileInputParent.nativeElement;
    const fileInput = fileInputParentNative.querySelector('input[type=\'file\']');
    const fileListe = fileInput.files;
    if (fileListe && fileListe.length > 0) {
      this.uploadFileList(fileListe, fetch);
    }
  }

  // done this way for testability It is not possible to manipulate input[file] in javascript.
  public async uploadFileList(files: File[], fetcher: (RequestInfo, init?: RequestInit) => Promise<Response>): Promise<any> {

    this.onUploadStarted.emit();

    const formData = new FormData();
    for (const file of files) {
      formData.append('files', file);
    }

    // add antiforery cookie
    const xsrftokenFromCookie = this.cookieService.getCookie('XSRF-TOKEN');
    formData.append('__RequestVerificationToken', xsrftokenFromCookie);

    const onUploaded = this.onUploaded;
    const onError = this.onError;
    const addFileInput = this.addFileInput.bind(this);
    const fetchdata: RequestInit = {
      credentials: 'include',
      method: 'POST',
      body: formData,
    };
    try {
      const response = await fetcher('/api/Data/UploadFiles', fetchdata);
      if (response.status !== 200) {
        const error = `An error occured. Status: ${response.status}`;
        throw new Error(error);
      }
      const json = await response.json();
      onUploaded.emit(json);
    } catch (error) {
      onError.emit(error);
    } finally {
      addFileInput();
    }
  }
}
