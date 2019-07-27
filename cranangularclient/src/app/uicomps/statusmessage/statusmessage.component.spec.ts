import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';

import { CRAN_SERVICE_TOKEN } from '../../cran-data.servicetoken';
import {NotificationService} from '../../notification.service';
import {ConfirmService} from '../../confirm.service';
import {LanguageService} from '../../language.service';
import {Tag} from '../../model/tag';
import { StatusmessageComponent } from './statusmessage.component';

describe('StatusMessageComponent', () => {
  let component: StatusmessageComponent;
  let fixture: ComponentFixture<StatusmessageComponent>;

  beforeEach(async(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ StatusmessageComponent ],
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
    fixture = TestBed.createComponent(StatusmessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
