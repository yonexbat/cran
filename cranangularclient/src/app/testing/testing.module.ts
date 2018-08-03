import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Component, Input, Output,
    EventEmitter, TemplateRef} from '@angular/core';


import { CranDataServiceSpy } from './crandataservicespy';
import { ConfirmServiceSpy } from './confirmservicespy';
import {NotificationServiceSpy} from './notificationservicespy';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {PagedResult} from '../model/pagedresult';
import {Tag} from '../model/tag';
import {Image} from '../model/image';




@Component({selector: 'app-vote', template: ''})
export class  StubVoteComponent {
  @Input() public votes;
}

@Component({selector: 'app-tags', template: ''})
export class StubTagsComponent {
  @Input() public tagList;
}

@Component({selector: 'app-imagelist', template: ''})
export class StubImageListComponent {
  @Input() public images: Image[] = [];
  @Input() public imagesDeletable: boolean;
  @Output() onDeleted = new EventEmitter<Image[]>();
}

@Component({selector: 'app-comments', template: ''})
export class StubCommentsComponent {
  public showComments(idQuestion: number): Promise<void> {
    return Promise.resolve();
  }
}

@Component({selector: 'app-rich-text-box', template: ''})
export class StubRichTextBoxComponent {
  @Input() elementId: string;
  @Input() public required: boolean;
  @Output() htmlString = new EventEmitter<string>();
  @Input() public set content(content: string) {}
}


@Component({selector: 'app-file-upload', template: ''})
export class StubFileUploadComponent {
}

@Component({selector: 'app-pager', template: ''})
export class StubPagerComponent {
  @Input()
  public itemTemplate: TemplateRef<any>;
  @Input()
  public pagedResult: PagedResult<any>;
  @Input()
  public nodatafoundmessage  = 'Keine Daten gefunden.';
}

@Component({selector: 'app-question-preview', template: ''})
export class StubQuestionPreviewComponent {
}

@Component({selector: 'app-tag-finder', template: ''})
export class StubTagFinderComponent {
  @Input() public tagsArray: Tag[] = [];
  @Input() public title = 'Tags';
  @Output() public tagSelected = new EventEmitter<Tag>();
  @Output() public tagRemoved = new EventEmitter<Tag>();
  @Output() public tagSelectionChanged = new EventEmitter<void>();
}


@NgModule({
  imports: [
    CommonModule
  ],
  exports: [
  ],
  declarations: [ ],
  providers: [
    LanguageService,
    { provide: CRAN_SERVICE_TOKEN, useClass: CranDataServiceSpy },
    { provide: NotificationService, useClass: NotificationServiceSpy},
    { provide: ConfirmService, useClass: ConfirmServiceSpy},
  ],
})
export class TestingModule { }

@NgModule({
  exports: [
    StubVoteComponent,
    StubTagsComponent,
    StubImageListComponent,
    StubCommentsComponent,
    StubPagerComponent,
    StubFileUploadComponent,
    StubRichTextBoxComponent,
    StubQuestionPreviewComponent,
    StubTagFinderComponent,
  ],
  declarations: [
    StubVoteComponent,
    StubTagsComponent,
    StubImageListComponent,
    StubCommentsComponent,
    StubPagerComponent,
    StubFileUploadComponent,
    StubRichTextBoxComponent,
    StubQuestionPreviewComponent,
    StubTagFinderComponent,
  ]
})
export class DummyStubModule { }

