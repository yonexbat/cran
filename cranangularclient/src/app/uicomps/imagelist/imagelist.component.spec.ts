import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement, Component, Input, TemplateRef } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';


import { ImagelistComponent } from './imagelist.component';
import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import { NotificationService } from '../../services/notification.service';
import { LanguageService } from '../../services/language.service';
import { ConfirmService } from '../../services/confirm.service';
import { IconComponent } from '../icon/icon.component';
import { Image } from '../../model/image';


describe('ImagelistComponent', () => {
  let component: ImagelistComponent;
  let fixture: ComponentFixture<ImagelistComponent>;

  const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
  const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
  const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);


  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule,],
      declarations: [ ImagelistComponent, IconComponent, ],
      providers: [
        LanguageService,
        { provide: CRAN_SERVICE_TOKEN, useValue: cranDataService },
        { provide: NotificationService, useValue: notificationService },
        { provide: ConfirmService, useValue: confirmationService },
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImagelistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should show 2 images', async(async () => {
    const images: Image[] = createImages();
    component.images = images;
    fixture.detectChanges();
    const imgs: DebugElement[] = fixture.debugElement.queryAll(By.css('img'));
    expect(imgs.length).toBe(images.length);
    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < images.length; i++) {
      const nativeImg = imgs[i].nativeElement;
      const srcShouldBe = `/api/Data/GetFile/${images[i].idBinary}`;
      expect(nativeImg.src).toContain(srcShouldBe);

      const width = nativeImg.style.width;
      expect(width).toContain(images[i].width.toString());
    }

  }));

  it('should delete image', async(async () => {
    const images: Image[] = createImages();
    component.images = images;
    component.imagesDeletable = true;
    component.onDeleted.subscribe((image: Image) => {
      const index = images.indexOf(image);
      images.splice(index, 1);
    });

    fixture.detectChanges();
    const firstButton: DebugElement = fixture.debugElement.query(By.css('div.row button'));
    firstButton.triggerEventHandler('click', null);

    await fixture.whenStable();
    fixture.detectChanges();

    expect(component.images.length).toBe(1);
    const numImages = fixture.debugElement.queryAll(By.css('img')).length;
    expect(numImages).toBe(1);

  }));

  it('should change width', async(async () => {
    const images: Image[] = createImages();
    component.images = images;
    component.imagesDeletable = true;
    component.onDeleted.subscribe((image: Image) => {
      const index = images.indexOf(image);
      images.splice(index, 1);
    });

    fixture.detectChanges();
    await fixture.whenStable();
    const firstInput: DebugElement = fixture.debugElement.query(By.css('input[type=\'number\']'));
    firstInput.nativeElement.value = '55';
    firstInput.nativeElement.dispatchEvent(new Event('change'));


    fixture.detectChanges();
    await fixture.whenStable();

    const nativeImg = fixture.debugElement.query(By.css('img')).nativeElement;
    const width = nativeImg.style.width;
    expect(width).toContain('55');
  }));

});



function waitSomeMillis(): Promise<any> {
  return new Promise((resovle, reject) => {
    setTimeout(() => resovle(), 2000);
  });
}


function createImages(): Image[] {
  const images: Image[] = [];
  images.push({
    id: 3,
    idBinary: 2,
    width: 100,
    full: false,
    height: 100,
  });
  images.push({
    id: 4,
    idBinary: 3,
    width: 80,
    full: false,
    height: 100,
  });
  return images;
}
