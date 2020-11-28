import { ComponentFixture, TestBed, inject, waitForAsync } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement, Component, Input, TemplateRef } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../../uicomps/uicomps.module';

import { CourseListComponent } from './course-list.component';
import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import { NotificationService } from '../../services/notification.service';
import { LanguageService } from '../../services/language.service';
import { ConfirmService } from '../../services/confirm.service';


describe('CourseListComponent', () => {

  let component: CourseListComponent;
  let fixture: ComponentFixture<CourseListComponent>;
  let de: DebugElement;
  let el: HTMLElement;


  beforeEach(waitForAsync(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, UicompsModule],
      declarations: [ CourseListComponent],
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
