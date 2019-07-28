
import { async, ComponentFixture, TestBed, inject, } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement, Component, Input, TemplateRef } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../uicomps/uicomps.module';

import {PagedResult} from '../model/pagedresult';
import {Tag} from '../model/tag';
import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {LanguageService} from '../language.service';
import {ConfirmService} from '../confirm.service';
import {SafeHtmlPipe} from '../uicomps/save-html.pipe';

import { CourseFavoriteListComponent } from './course-favorite-list.component';
import { TagsComponent } from '../tags/tags.component';




@Component({selector: 'app-pager', template: ''})
class StubPagerComponent {
  @Input()  public itemTemplate: TemplateRef<any>;
  @Input()  public pagedResult: PagedResult<any>;
  @Input()  public nodatafoundmessage  = 'Keine Daten gefunden.';
}

describe('CourseFavoriteListComponent', () => {
  let component: CourseFavoriteListComponent;
  let fixture: ComponentFixture<CourseFavoriteListComponent>;

  beforeEach(async(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, UicompsModule],
      declarations: [ CourseFavoriteListComponent,
        SafeHtmlPipe, StubPagerComponent, TagsComponent ],
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
