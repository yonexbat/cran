import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';

import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import {NotificationService} from '../../services/notification.service';
import {ConfirmService} from '../../services/confirm.service';
import {LanguageService} from '../../services/language.service';
import {Tag} from '../../model/tag';
import { StatusmessageComponent } from './statusmessage.component';

describe('StatusMessageComponent', () => {
  let component: StatusmessageComponent;
  let fixture: ComponentFixture<StatusmessageComponent>;

  beforeEach(waitForAsync(() => {

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

  it('should show successmessage', waitForAsync(async () => {

    component.showError('test error message');

    await fixture.whenStable();
    fixture.detectChanges();

    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;
    const text = nativeEl.innerText;
    expect(text).toBe('test error message');

    const alert = nativeEl.querySelector('.alert-danger');
    expect(alert).toBeTruthy();

    component.done();

    await fixture.whenStable();
    fixture.detectChanges();
    expect(nativeEl.childElementCount).toBe(0);

  }));

  it('should show errormessage', waitForAsync(async () => {

    component.showSaveSuccess();

    await fixture.whenStable();
    fixture.detectChanges();

    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;
    const text = nativeEl.innerText;
    expect(text.length).toBeGreaterThan(0);

    const info = nativeEl.querySelector('.alert-success');
    expect(info).toBeTruthy();

    component.done();

    await fixture.whenStable();
    fixture.detectChanges();
    expect(nativeEl.childElementCount).toBe(0);

  }));

  it('should be invisible when there is no message to be shown', waitForAsync(async () => {

    await fixture.whenStable();
    fixture.detectChanges();

    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;
    expect(nativeEl.childElementCount).toBe(0);

  }));


});
