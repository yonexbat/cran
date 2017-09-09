import { async, ComponentFixture, TestBed, inject, } from '@angular/core/testing';
import {MockBackend, MockConnection, } from '@angular/http/testing';
import { Http, RequestOptions, ResponseOptions, Response, ConnectionBackend } from '@angular/http';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CourseListComponent } from './course-list.component';
import { CranDataService } from '../cran-data.service';
import { Courses } from '../model/courses';
import { Course } from '../model/course';

describe('CourseListComponent', () => {

  let component: CourseListComponent;
  let fixture: ComponentFixture<CourseListComponent>;
  let de: DebugElement;
  let el: HTMLElement;


  beforeEach(async(() => {


    class CranDataServiceSpy {

      constructor() {
        this.courses = new Courses();
        this.courses.courses = [{id : 2, title : 'helo', description: 'mydescription'}];
      }

      courses = new Courses();

      getCourses = jasmine.createSpy('getCourses').and.callFake(
        () => Promise
          .resolve(true)
          .then(() => Object.assign({}, this.courses))
      );

    }

    TestBed.configureTestingModule({
      declarations: [ CourseListComponent ],
      providers: [
        { provide: CranDataService, useClass: CranDataServiceSpy }
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

  it('It is listed', async(() => {
    fixture.whenStable().then(() => {
      fixture.detectChanges();
      const textContext = el.textContent;
      expect(el.textContent).toContain('helo');
    });
  }));

});
