import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../uicomps/uicomps.module';

import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {Tag} from '../model/tag';
import { ViewQuestionComponent } from './view-question.component';
import {SafeHtmlPipe} from '../save-html.pipe';
import {ImagelistComponent} from '../imagelist/imagelist.component';


@Component({selector: 'app-versions', template: ''})
class StubVersionsComponent {

}

@Component({selector: 'app-vote', template: ''})
class StubVoteComponent {
  @Input() public votes;
}

@Component({selector: 'app-tags', template: ''})
class StubTagsComponent {
  @Input() public tagList: Tag[] = [];
  @Input() public isEditable = false;
}


@Component({selector: 'app-comments', template: ''})
class StubCommentsComponent {
}

describe('ViewQuestionComponent', () => {
  let component: ViewQuestionComponent;
  let fixture: ComponentFixture<ViewQuestionComponent>;

  beforeEach(async(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        FormsModule,
        UicompsModule,
      ],
      declarations: [
        ViewQuestionComponent,
        StubVersionsComponent,
        StubVoteComponent,
        StubTagsComponent, SafeHtmlPipe,
        ImagelistComponent,
        StubCommentsComponent,
      ],
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
    fixture = TestBed.createComponent(ViewQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
