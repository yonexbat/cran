import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {

  @ViewChild('fileInput') fileInput;
  @ViewChild('fileForm') fileForm;

  constructor() { }

  ngOnInit() {
  }

  private upload() {
    debugger;
    const fileBrowser = this.fileInput.nativeElement;
    if (fileBrowser.files && fileBrowser.files[0]) {
      const formData = new FormData();
      formData.append('files', fileBrowser.files[0]);
      const xhr = new XMLHttpRequest();
      xhr.open('POST', '/api/Data/UploadFiles', true);
      // Set up a handler for when the request finishes.
      xhr.onload = function () {
        if (this['status'] === 200) {
            // File(s) uploaded.
            const responseText = this['responseText'];
            const files = JSON.parse(responseText);
        } else {
          debugger;
        }
      };
      xhr.send(formData);
    }
  }

}
