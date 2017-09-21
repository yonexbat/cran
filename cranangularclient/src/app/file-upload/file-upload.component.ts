import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {

  @ViewChild('fileInput') fileInput;

  @Output() onUploadStarted = new EventEmitter<void>();
  @Output() onError = new EventEmitter<any>();
  @Output() onUploaded = new EventEmitter<any>();

  constructor() { }

  ngOnInit() {
  }

  private fileChangeEvent(event: any) {
    try {
      this.onUploadStarted.emit();
      this.upload();
    } catch (error) {

    }
  }

  private upload() {
    const fileBrowser = this.fileInput.nativeElement;
    if (fileBrowser.files && fileBrowser.files.length > 0) {
      const formData = new FormData();
      for (let i = 0; i < fileBrowser.files.length; i++) {
        formData.append('files', fileBrowser.files[i]);
      }

      const onUploaded = this.onUploaded;
      const onError = this.onError;
      const xhr = new XMLHttpRequest();
      xhr.open('POST', '/api/Data/UploadFiles', true);
      xhr.onload = function () {
        if (this['status'] === 200) {
            const responseText = this['responseText'];
            const files = JSON.parse(responseText);
            onUploaded.emit(files);
        } else {
          onError.emit(this['status']);
        }
      };
      xhr.send(formData);
    }
  }

}
