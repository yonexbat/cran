import { Component,
  OnInit,
  AfterViewInit,
  ViewChild, ElementRef,
  Output, Input,
  EventEmitter, } from '@angular/core';

import {Binary} from '../model/binary';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit, AfterViewInit {

  @Output() onUploadStarted = new EventEmitter<void>();
  @Output() onError = new EventEmitter<string>();
  @Output() onUploaded = new EventEmitter<File[]>();

  @Input() placeHolderText = 'Upload file...';
  @ViewChild('fileInputParent') fileInputParent: ElementRef;

  constructor() { }

  ngOnInit() {

  }

  ngAfterViewInit(): void {
    this.addFileInput();
  }


  private addFileInput() {
    const fileInputParentNative = this.fileInputParent.nativeElement;
    const oldFileInput = fileInputParentNative.querySelector('input');
    const newFileInput = document.createElement('input');
    newFileInput.type = 'file';
    newFileInput.multiple = true;
    newFileInput.name = 'fileInput';
    const uploadfiles = this.uploadFiles.bind(this);
    newFileInput.onchange = uploadfiles;
    oldFileInput.parentNode.replaceChild(newFileInput, oldFileInput);
  }

  private uploadFiles() {
    this.onUploadStarted.emit();
    const fileInputParentNative = this.fileInputParent.nativeElement;
    const fileInput = fileInputParentNative.querySelector('input');
    if (fileInput.files && fileInput.files.length > 0) {
      const formData = new FormData();
      for (let i = 0; i < fileInput.files.length; i++) {
        formData.append('files', fileInput.files[i]);
      }

      // add antiforery cookie
      debugger;
      const xsrftokenFromCookie = this.getCookie('XSRF-TOKEN');
      formData.append('__RequestVerificationToken', xsrftokenFromCookie);

      const onUploaded = this.onUploaded;
      const onError = this.onError;
      const addFileInput = this.addFileInput.bind(this);
      fetch('/api/Data/UploadFiles', {
        credentials: 'include',
        method: 'POST',
        body: formData,
      }).then((response: any) => {
        if (response.status !== 200) {
          const error = `An error occured. Status: ${response.status}`;
          throw new Error(error);
        }
        return response.json();
      }).then(files => {
        onUploaded.emit(files);
        addFileInput();
      }).catch((error) => {
        onError.emit(error);
      });
    }
  }

  private getCookie(cname) {
    const name = cname + '=';
    const decodedCookie = decodeURIComponent(document.cookie);
    const ca = decodedCookie.split(';');
    for(let i = 0; i < ca.length; i++) {
        let c = ca[i];
        // tslint:disable-next-line:triple-equals
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        // tslint:disable-next-line:triple-equals
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return '';
}

}
