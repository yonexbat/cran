import { NgModule, SimpleChanges, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Component, Input, Output,
    EventEmitter, TemplateRef, forwardRef, OnInit} from '@angular/core';
import {ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS,
      Validator, AbstractControl, ValidationErrors, FormsModule} from '@angular/forms';


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
import {htmlRequired} from '../uicomps/rich-text-box/htmlrequired';




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

@Component({
  selector: 'app-rich-text-box',
  template: '<input id="{{elementId}}" required={{required}} [(ngModel)]="content">',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => StubRichTextBoxComponent),
    },
    {
      provide: NG_VALIDATORS,
      useExisting:  forwardRef(() => StubRichTextBoxComponent),
      multi: true,
    },
  ],
})
export class StubRichTextBoxComponent implements ControlValueAccessor,
  Validator, OnInit, OnChanges {

  private _content: string;
  private onChangelistener: any;
  private validateFn: any;

  @Input() elementId: string;
  @Input() public required: boolean;
  @Output() htmlString = new EventEmitter<string>();

  @Input() public set content(content: string) {
    if (this.onChangelistener) {
      this.onChangelistener(content);
    }
  }
  public get content() {
    return this._content;
  }

  writeValue(value: any): void {
    this.content = value;
  }
  registerOnChange(fn: any): void {
    this.onChangelistener = fn;
  }
  registerOnTouched(fn: any): void {
  }
  setDisabledState?(isDisabled: boolean): void {
  }

  validate(c: AbstractControl): ValidationErrors {
    return this.validateFn(c);
  }
  registerOnValidatorChange?(fn: () => void): void {
  }

  ngOnInit(): void {
    this.validateFn = htmlRequired(this.required);
  }

  ngOnChanges(changes: SimpleChanges) {
    this.validateFn = htmlRequired(this.required);
  }
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
  ],
  imports: [
    FormsModule,
  ]
})
export class DummyStubModule { }

