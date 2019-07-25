import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, DebugElement, TemplateRef, EventEmitter} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';

import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {QuestionListEntry} from '../model/questionlistentry';
import { QuestionListComponent } from './question-list.component';
import {TooltipDirective} from '../tooltip.directive';
import {PagedResult} from '../model/pagedresult';



@Component({selector: 'app-question-list-item', template: ''})
class StubQuestionListItemComponent {
  @Input()  public item: QuestionListEntry;
  @Output()  onItemDeletedClick = new EventEmitter<QuestionListEntry>();
  @Output()   onItemViewClick = new EventEmitter<QuestionListEntry>();
  @Output()  onItemEditclick = new EventEmitter<QuestionListEntry>();
}

@Component({selector: 'app-pager', template: ''})
class StubPagerComponent {
  @Input() public itemTemplate: TemplateRef<any>;
  @Input() public pagedResult: PagedResult<any>;
  @Input() public nodatafoundmessage  = 'Keine Daten gefunden.';
}

describe('QuestionListComponent', () => {
  let component: QuestionListComponent;
  let fixture: ComponentFixture<QuestionListComponent>;

  beforeEach(async(() => {
    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ QuestionListComponent, TooltipDirective,
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
    fixture = TestBed.createComponent(QuestionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
