import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseStarterComponent } from './course-starter.component';
import { RouterTestingModule } from '@angular/router/testing';

import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import {NotificationService} from '../../services/notification.service';
import {ConfirmService} from '../../services/confirm.service';
import {LanguageService} from '../../services/language.service';
import {PagedResult} from '../../model/pagedresult';

describe('CourseStarterComponent', () => {
  let component: CourseStarterComponent;
  let fixture: ComponentFixture<CourseStarterComponent>;

  const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
  const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
  const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [ CourseStarterComponent ],
      providers: [
        LanguageService,
        { provide: CRAN_SERVICE_TOKEN, useValue: cranDataService },
        { provide: NotificationService, useValue: notificationService },
        { provide: ConfirmService, useValue: confirmationService },
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CourseStarterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
