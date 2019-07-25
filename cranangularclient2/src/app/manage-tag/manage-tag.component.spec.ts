import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';


import { ManageTagComponent } from './manage-tag.component';
import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {Tag} from '../model/tag';
import { RichTextBoxComponent } from '../rich-text-box/rich-text-box.component';
import {StatusMessageComponent} from '../status-message/status-message.component';

@Component({selector: 'app-rich-text-box', template: ''})
class StubRichTextBoxComponent {
  @Input() elementId: string;
  @Input() public required: boolean;
  @Output() htmlString = new EventEmitter<string>();
  @Input() public set content(content: string) {}
}


describe('ManageTagComponent', () => {
  let component: ManageTagComponent;
  let fixture: ComponentFixture<ManageTagComponent>;

  beforeEach(async(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ ManageTagComponent, RichTextBoxComponent, StatusMessageComponent ],
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
    fixture = TestBed.createComponent(ManageTagComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
