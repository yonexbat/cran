import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../../uicomps/uicomps.module';

import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import {NotificationService} from '../../services/notification.service';
import {ConfirmService} from '../../services/confirm.service';
import {LanguageService} from '../../services/language.service';
import {Tag} from '../../model/tag';
import {QuestionListEntry} from '../../model/questionlistentry';
import {PagedResult} from '../../model/pagedresult';

import { SearchQuestionsComponent } from './search-questions.component';


@Component({selector: 'app-question-list-item', template: ''})
class StubQuestionListItemComponent {
  @Input()  public item: QuestionListEntry;
  @Output()  onItemDeletedClick = new EventEmitter<QuestionListEntry>();
  @Output()   onItemViewClick = new EventEmitter<QuestionListEntry>();
  @Output()  onItemEditclick = new EventEmitter<QuestionListEntry>();
}

@Component({selector: 'app-pager', template: ''})
class StubPagerComponent {
  @Input()
  public itemTemplate: TemplateRef<any>;
  @Input()
  public pagedResult: PagedResult<any>;
  @Input()
  public nodatafoundmessage  = 'Keine Daten gefunden.';
}

describe('SearchQuestionsComponent', () => {
  let component: SearchQuestionsComponent;
  let fixture: ComponentFixture<SearchQuestionsComponent>;

  beforeEach(waitForAsync(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, UicompsModule],
      declarations: [ SearchQuestionsComponent,
        StubQuestionListItemComponent, StubPagerComponent ],
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
    fixture = TestBed.createComponent(SearchQuestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
