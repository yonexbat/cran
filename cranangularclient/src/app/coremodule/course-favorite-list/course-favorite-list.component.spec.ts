
import { ComponentFixture, TestBed, inject, waitForAsync } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement, Component, Input, TemplateRef } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../../uicomps/uicomps.module';

import {PagedResult} from '../../model/pagedresult';
import {Tag} from '../../model/tag';
import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import {NotificationService} from '../../services/notification.service';
import {LanguageService} from '../../services/language.service';
import {ConfirmService} from '../../services/confirm.service';


import { CourseFavoriteListComponent } from './course-favorite-list.component';




@Component({selector: 'app-pager', template: ''})
class StubPagerComponent {
  @Input()  public itemTemplate: TemplateRef<any>;
  @Input()  public pagedResult: PagedResult<any>;
  @Input()  public nodatafoundmessage  = 'Keine Daten gefunden.';
}

describe('CourseFavoriteListComponent', () => {
  let component: CourseFavoriteListComponent;
  let fixture: ComponentFixture<CourseFavoriteListComponent>;

  beforeEach(waitForAsync(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, UicompsModule],
      declarations: [ CourseFavoriteListComponent,
        StubPagerComponent, ],
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
    fixture = TestBed.createComponent(CourseFavoriteListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
