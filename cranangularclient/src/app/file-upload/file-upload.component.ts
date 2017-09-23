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

      const onUploaded = this.onUploaded;
      const onError = this.onError;
      const addFileInput = this.addFileInput.bind(this);
      const xhr = new XMLHttpRequest();
      xhr.open('POST', '/api/Data/UploadFiles');
      xhr.onload = function () {
        if (this['status'] === 200) {
            const responseText = this['responseText'];
            const files = JSON.parse(responseText);
            onUploaded.emit(files);
            addFileInput();
        } else {
          onError.emit(this['statusText']);
        }
      };
      xhr.send(formData);
    }

  }

}
