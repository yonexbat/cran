import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, DebugElement, TemplateRef, EventEmitter} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';

import { ManageQuestionComponent } from './manage-question.component';
import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {PagedResult} from '../model/pagedresult';
import {Tag} from '../model/tag';
import {Image} from '../model/image';
import {TooltipDirective} from '../tooltip.directive';
import {IconComponent} from '../icon/icon.component';
import {StatusMessageComponent} from '../status-message/status-message.component';

@Component({selector: 'app-question-preview', template: ''})
class StubQuestionPreviewComponent {
}

@Component({selector: 'app-tag-finder', template: ''})
class StubTagFinderComponent {
  @Input() public tagsArray: Tag[] = [];
  @Input() public title = 'Tags';
  @Output() public tagSelected = new EventEmitter<Tag>();
  @Output() public tagRemoved = new EventEmitter<Tag>();
  @Output() public tagSelectionChanged = new EventEmitter<void>();
}

@Component({selector: 'app-imagelist', template: ''})
class StubImageListComponent {
  @Input() public images: Image[] = [];
  @Input() public imagesDeletable: boolean;
  @Output() onDeleted = new EventEmitter<Image[]>();
}

@Component({selector: 'app-rich-text-box', template: ''})
class StubRichTextBoxComponent {
  @Input() elementId: string;
  @Input() public required: boolean;
  @Output() htmlString = new EventEmitter<string>();
  @Input() public set content(content: string) {}
}


@Component({selector: 'app-file-upload', template: ''})
class StubFileUploadComponent {
}



describe('ManageQuestionComponent', () => {
  let component: ManageQuestionComponent;
  let fixture: ComponentFixture<ManageQuestionComponent>;

  beforeEach(async(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ ManageQuestionComponent, StubQuestionPreviewComponent,
        StubTagFinderComponent, StubRichTextBoxComponent, StubImageListComponent,
        StubFileUploadComponent, TooltipDirective, IconComponent,
      StatusMessageComponent],
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
    fixture = TestBed.createComponent(ManageQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
