import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, Input, DebugElement, Directive, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { DatePipe } from '@angular/common';

import { CourseInstanceListComponent } from './course-instance-list.component';
import { TooltipDirective } from '../tooltip.directive';
import {PagedResult} from '../model/pagedresult';
import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {LanguageService} from '../language.service';
import {ConfirmService} from '../confirm.service';

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

describe('CourseInstanceListComponent', () => {
  let component: CourseInstanceListComponent;
  let fixture: ComponentFixture<CourseInstanceListComponent>;
  const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
  const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [TooltipDirective, CourseInstanceListComponent, StubIconComponent,
        StubPagerComponent],
      providers: [
        LanguageService, ConfirmService, DatePipe,
        { provide: CRAN_SERVICE_TOKEN, useValue: cranDataService },
        { provide: NotificationService, useValue: notificationService },
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CourseInstanceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
