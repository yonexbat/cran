import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, Input, DebugElement} from '@angular/core';

import {FormsModule} from '@angular/forms';
import { AskQuestionComponent } from './ask-question.component';
import { RouterTestingModule } from '@angular/router/testing';

import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {SafeHtmlPipe} from '../save-html.pipe';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';

@Component({selector: 'app-vote', template: ''})
class StubVoteComponent {
  @Input() public votes;
}

@Component({selector: 'app-tags', template: ''})
class StubTagsComponent {
  @Input() public tagList;
}

@Component({selector: 'app-imagelist', template: ''})
class StubAppImageListComponent {
  @Input() public images;
}

@Component({selector: 'app-icon', template: ''})
class StubIconComponent {
  @Input() public icon;
}

@Component({selector: 'app-comments', template: ''})
class CommentsComponent {
}

describe('AskQuestionComponent', () => {
  let component: AskQuestionComponent;
  let fixture: ComponentFixture<AskQuestionComponent>;

  beforeEach(async(() => {
    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ AskQuestionComponent,  StubVoteComponent,
        StubTagsComponent, SafeHtmlPipe, StubAppImageListComponent,
        StubIconComponent, CommentsComponent],
      providers: [
        LanguageService,
        { provide: CRAN_SERVICE_TOKEN, useValue: cranDataService },
        { provide: NotificationService, useValue: notificationService },
        { provide: ConfirmService, useValue: confirmationService },
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AskQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
