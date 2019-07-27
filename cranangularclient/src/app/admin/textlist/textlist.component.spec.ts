import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';

import { CRAN_SERVICE_TOKEN } from '../../cran-data.servicetoken';
import {NotificationService} from '../../notification.service';
import {ConfirmService} from '../../confirm.service';
import {LanguageService} from '../../language.service';
import {Tag} from '../../model/tag';
import { TextlistComponent } from './textlist.component';
import {TooltipDirective} from '../../uicomps/tooltip.directive';
import {IconComponent} from '../../uicomps/icon/icon.component';
import {PagedResult} from '../../model/pagedresult';
import {UicompsModule} from '../../uicomps/uicomps.module';

@Component({selector: 'app-pager', template: ''})
class StubPagerComponent {
  @Input()
  public itemTemplate: TemplateRef<any>;
  @Input()
  public pagedResult: PagedResult<any>;
  @Input()
  public nodatafoundmessage  = 'Keine Daten gefunden.';
}

describe('TextlistComponent', () => {
  let component: TextlistComponent;
  let fixture: ComponentFixture<TextlistComponent>;

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
        TextlistComponent,
        StubPagerComponent ],
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
    fixture = TestBed.createComponent(TextlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
