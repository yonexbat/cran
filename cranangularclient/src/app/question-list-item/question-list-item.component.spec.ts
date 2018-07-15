import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef, ViewChild} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';

import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {TooltipDirective} from '../tooltip.directive';
import { QuestionListItemComponent } from './question-list-item.component';
import {IconComponent} from '../icon/icon.component';
import {TagsComponent} from '../tags/tags.component';
import {QuestionListEntry} from '../model/questionlistentry';


// This compoment uses a host. we are testing the host.
// tslint:disable-next-line:max-line-length
@Component({selector: 'app-host', template: '<div><app-question-list-item #questionlistitem [item]="questionListEntry" (onItemDeletedClick)="deleteQuestion($event)"></app-question-list-item></div>'})
class StubHostComponent {

  @ViewChild('questionlistitem') public questionListItemComponent: QuestionListItemComponent;

  public questionListEntry: QuestionListEntry = {
    id: 2,
    status: 1,
    title: 'Hello Angular',
    tags: [{id: 2, description: 'Desc', idTagType: 1, name: 'Angular.io', shortDescDe: 'de', shortDescEn: 'en'}],
  };
}

describe('QuestionListItemComponent', () => {
  let component: StubHostComponent;
  let fixture: ComponentFixture<StubHostComponent>;

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ QuestionListItemComponent, TooltipDirective, IconComponent, TagsComponent, StubHostComponent ],
      providers: [
        LanguageService, NotificationService, ConfirmService,
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StubHostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display question item', () => {
    const textcontent = fixture.debugElement.nativeElement.textContent;
    expect(textcontent).toContain('Hello Angular', 'Hello Angular displayed someshwere');
    const hostEl = fixture.debugElement.nativeElement.querySelector('.questionstatusok');
    expect(hostEl).toBeTruthy('class questionstatusok shloud be somewhere');
    const hostElNok = fixture.debugElement.nativeElement.querySelector('.questionstatusnok');
    expect(hostElNok).toBeFalsy('element class questionstatusnok shloud not exist');
  });

  it('shlould have nok class', () => {
    component.questionListEntry.status = 0;
    fixture.detectChanges();
    const hostElNok = fixture.debugElement.nativeElement.querySelector('.questionstatusnok');
    expect(hostElNok).toBeTruthy('class questionstatusnok shloud exist');
  });

});
