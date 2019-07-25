import { async, ComponentFixture, TestBed, inject, } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement, Component, Input, TemplateRef } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';

import { CourseListComponent } from './course-list.component';
import { TooltipDirective } from '../tooltip.directive';
import {PagedResult} from '../model/pagedresult';
import {Tag} from '../model/tag';
import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {LanguageService} from '../language.service';
import {ConfirmService} from '../confirm.service';
import {SafeHtmlPipe} from '../save-html.pipe';

@Component({selector: 'app-icon', template: ''})
class StubIconComponent {
  @Input() public icon;
}

@Component({selector: 'app-tags', template: ''})
class StubTagsComponent {
  @Input() public tagList: Tag[] = [];
  @Input() public isEditable = false;
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

describe('CourseListComponent', () => {

  let component: CourseListComponent;
  let fixture: ComponentFixture<CourseListComponent>;
  let de: DebugElement;
  let el: HTMLElement;


  beforeEach(async(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [ CourseListComponent, TooltipDirective,
        StubIconComponent, StubTagsComponent, SafeHtmlPipe, StubPagerComponent],
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
    fixture = TestBed.createComponent(CourseListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    de = fixture.debugElement.query(By.css('div'));
    el = de.nativeElement;
  });

  it('CourseListComponent should be created', () => {
    expect(component).toBeTruthy();
  });


});
