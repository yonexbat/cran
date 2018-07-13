import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import {FormsModule} from '@angular/forms';
import { CommentsComponent } from './comments.component';
import { Component, Input, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';

import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {SafeHtmlPipe} from '../save-html.pipe';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {PagedResult} from '../model/pagedresult';

@Component({selector: 'app-icon', template: ''})
class StubIconComponent {
  @Input() public icon;
}

@Component({selector: 'app-pager', template: ''})
class StubPagerComponent {
  @Input()
  public itemTemplate: TemplateRef<any>;
  @Input()
  public pagedResult: PagedResult<any>;
  @Input()
  public nodatafoundmessage  = 'Keine Daten gefunden.';
}

describe('CommentsComponent', () => {
  let component: CommentsComponent;
  let fixture: ComponentFixture<CommentsComponent>;

  beforeEach(async(() => {
    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ CommentsComponent, StubIconComponent, StubPagerComponent ],
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
    fixture = TestBed.createComponent(CommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    /*
    expect(component).toBeTruthy();*/
  });
});
