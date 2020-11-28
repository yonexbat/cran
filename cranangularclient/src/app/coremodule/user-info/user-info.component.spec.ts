import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../../uicomps/uicomps.module';

import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import {NotificationService} from '../../services/notification.service';
import {ConfirmService} from '../../services/confirm.service';
import {LanguageService} from '../../services/language.service';
import {UserInfo} from '../../model/userinfo';


import { UserInfoComponent } from './user-info.component';

describe('UserInfoComponent', () => {
  let component: UserInfoComponent;
  let fixture: ComponentFixture<UserInfoComponent>;
  const userInfo: UserInfo = {
    name: 'The beaty',
    isAnonymous: false,
  };

  beforeEach(waitForAsync(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['getUserInfo']);
    cranDataService.getUserInfo.and.returnValue(Promise.resolve(userInfo));


    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, UicompsModule],
      declarations: [ UserInfoComponent ],
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
    fixture = TestBed.createComponent(UserInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should show username', waitForAsync(async () => {
    await fixture.whenStable();
    fixture.detectChanges();
    const text = fixture.debugElement.nativeElement.querySelector('#spanuserinfoname').textContent;
    expect(text).toContain(userInfo.name, 'username shall be displayed');
  }));

  it('should change language to English', waitForAsync( async () => {
    await fixture.whenStable();
    fixture.detectChanges();
    const enButton = fixture.debugElement.nativeElement.querySelector('#userinfosetenbutton');
    enButton.click();
    await fixture.whenStable();
    fixture.detectChanges();
    expect(fixture.componentInstance.isEn).toBeTruthy('english expected');
    const spanEnglish = fixture.debugElement.nativeElement.querySelector('.activeLanguage');
    expect(spanEnglish.textContent).toContain('E', 'En should be bold');
  }));
});
