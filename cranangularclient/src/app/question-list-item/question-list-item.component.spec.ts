import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';

import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {PagedResult} from '../model/pagedresult';
import {TooltipDirective} from '../tooltip.directive';
import { QuestionListItemComponent } from './question-list-item.component';
import {IconComponent} from '../icon/icon.component';
import {Tag} from '../model/tag';
import {TagsComponent} from '../tags/tags.component';

@Component({selector: 'app-tags', template: ''})
class StubTagsComponent {
  @Input() public tagList: Tag[] = [];
  @Input() public isEditable = false;
  @Output() onRemoveTagClick = new EventEmitter<Tag>();
}
describe('QuestionListItemComponent', () => {
  let component: QuestionListItemComponent;
  let fixture: ComponentFixture<QuestionListItemComponent>;

  beforeEach(async(() => {
    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ QuestionListItemComponent, TooltipDirective, IconComponent, StubTagsComponent ],
      providers: [
        LanguageService, NotificationService, ConfirmService,
        { provide: CRAN_SERVICE_TOKEN, useValue: cranDataService },
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
