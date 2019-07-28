import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../uicomps/uicomps.module';

import { ManageCourseComponent } from './manage-course.component';
import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {Tag} from '../model/tag';

@Component({selector: 'app-tag-finder', template: ''})
class StubTagFinderComponent {
  @Input() public tagsArray: Tag[] = [];
  @Input() public title = 'Tags';
  @Output() public tagSelected = new EventEmitter<Tag>();
  @Output() public tagRemoved = new EventEmitter<Tag>();
  @Output() public tagSelectionChanged = new EventEmitter<void>();
}

@Component({selector: 'app-rich-text-box', template: ''})
class StubRichTextBoxComponent {
  @Input() elementId: string;
  @Input() public required: boolean;
  @Output() htmlString = new EventEmitter<string>();
  @Input() public set content(content: string) {}
}


@Component({selector: 'app-status-message', template: ''})
class StubStatusMessageComponent {
}



describe('ManageCourseComponent', () => {
  let component: ManageCourseComponent;
  let fixture: ComponentFixture<ManageCourseComponent>;

  beforeEach(async(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, UicompsModule],
      declarations: [ ManageCourseComponent, StubTagFinderComponent, StubStatusMessageComponent ],
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
    fixture = TestBed.createComponent(ManageCourseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
