import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, DebugElement, TemplateRef, EventEmitter} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../uicomps/uicomps.module';

import { CRAN_SERVICE_TOKEN } from '../services/cran-data.servicetoken';
import {NotificationService} from '../services/notification.service';
import {ConfirmService} from '../services/confirm.service';
import {LanguageService} from '../services/language.service';
import {QuestionListEntry} from '../model/questionlistentry';
import { QuestionListComponent } from './question-list.component';



@Component({selector: 'app-question-list-item', template: ''})
class StubQuestionListItemComponent {
  @Input()  public item: QuestionListEntry;
  @Output()  onItemDeletedClick = new EventEmitter<QuestionListEntry>();
  @Output()   onItemViewClick = new EventEmitter<QuestionListEntry>();
  @Output()  onItemEditclick = new EventEmitter<QuestionListEntry>();
}


describe('QuestionListComponent', () => {
  let component: QuestionListComponent;
  let fixture: ComponentFixture<QuestionListComponent>;

  beforeEach(async(() => {
    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, UicompsModule],
      declarations: [ QuestionListComponent,
        StubQuestionListItemComponent, ],
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
    fixture = TestBed.createComponent(QuestionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
