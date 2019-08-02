import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FileUploadComponent } from './file-upload.component';

describe('FileUploadComponent', () => {
  let component: FileUploadComponent;
  let fixture: ComponentFixture<FileUploadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FileUploadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FileUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;
    const fileInput = nativeEl.querySelector('input[type=\'file\']');
    expect(fileInput).toBeTruthy();
  });

  it('should add file', async(async () => {
    const fetcher: (RequestInfo, init?: RequestInit) => Promise<Response> = (requestInfo, init) => {
      const response = new Response();
      return new Promise((resolve, reject) => {
        resolve(response);
      });
    };
    const files: File[] = [];
    files.push(new File([], 'file1'));
    let uploadstarted = 0;
    let uploadedfiles: File[] = [];
    let error = 0;

    component.onUploadStarted.subscribe(() => {
      uploadstarted += 1;
    });
    component.onUploaded.subscribe((fl) => {
      uploadedfiles = fl;
    });
    component.onError.subscribe(() => {
      error += 1;
    });

    await component.uploadFileList(files, fetcher);
    await fixture.whenStable();
    fixture.detectChanges();

    expect(uploadstarted).toBe(1);
    expect(uploadedfiles.length).toBe(1);
    expect(uploadedfiles[0].name).toBe('file1');
    expect(error).toBe(0);



    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;
    const fileInput = nativeEl.querySelector('input[type=\'file\']');
    expect(fileInput).toBeTruthy();


  }));
});
