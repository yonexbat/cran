import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../../uicomps/uicomps.module';
import { createQuestionTestObj } from '../../testing/modelobjcreator';

import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import { NotificationService } from '../../services/notification.service';
import { ConfirmService } from '../../services/confirm.service';
import { LanguageService } from '../../services/language.service';
import { QuestionPreviewComponent } from './question-preview.component';




describe('QuestionPreviewComponent', () => {
  let component: QuestionPreviewComponent;
  let fixture: ComponentFixture<QuestionPreviewComponent>;

  beforeEach(waitForAsync(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, UicompsModule],
      declarations: [ QuestionPreviewComponent, ],
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
    fixture = TestBed.createComponent(QuestionPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should show preview', waitForAsync( async () => {

    let dialogDiv = fixture.debugElement.query(By.css('div.modal'));

    const question = createQuestionTestObj(1);
    component.showDialog(question);

    fixture.detectChanges();

    dialogDiv = fixture.debugElement.query(By.css('div.modal'));
    expect(dialogDiv).toBeTruthy('Dialog shlould be visible now');

    const ne: HTMLElement = dialogDiv.nativeElement;
    expect(ne.innerText).toContain('Karotte');

  }));

});

