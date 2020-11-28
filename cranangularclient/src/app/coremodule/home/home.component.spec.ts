import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../../uicomps/uicomps.module';

import { HomeComponent } from './home.component';
import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import { NotificationService } from '../../services/notification.service';
import { ConfirmService } from '../../services/confirm.service';
import { LanguageService } from '../../services/language.service';
import { PagedResult } from '../../model/pagedresult';
import { Text } from '../../model/text';

describe('HomeComponent', () => {

  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  const text = new Text();
  text.contentDe = 'TestContent de';
  text.contentEn = 'Testcontent en';
  let cranDataServiceSpy;

  beforeEach(waitForAsync(() => {

    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    cranDataServiceSpy = jasmine.createSpyObj('CranDataService', ['getTextDtoByKey']);
    cranDataServiceSpy.getTextDtoByKey = jasmine.createSpy('getTextDtoByKey() spy')
      .and.callFake(() => {
        return new Promise<Text>((resovle, reject) => {
          resovle(text);
        });
      });

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, UicompsModule],
      declarations: [ HomeComponent, ],
      providers: [
        LanguageService,
        { provide: CRAN_SERVICE_TOKEN, useValue: cranDataServiceSpy },
        { provide: NotificationService, useValue: notificationService },
        { provide: ConfirmService, useValue: confirmationService },
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should display home text', waitForAsync(async () => {

    await fixture.whenStable();
    fixture.detectChanges();

    const ne: HTMLElement = fixture.debugElement.nativeElement;
    const innerText = ne.innerText;
    expect(innerText).toBe(text.contentDe, `home component should display the text ${text.contentDe} (german)`);
    expect(cranDataServiceSpy.getTextDtoByKey.calls.count())
    .toBe(1, `getTextDtoByKey shlould be called exaclty one time, but was called ${cranDataServiceSpy.getTextDtoByKey.calls.count()}`);

  }));
});
