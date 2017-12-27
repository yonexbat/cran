import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseStarterComponent } from './course-starter.component';

describe('CourseStarterComponent', () => {
  let component: CourseStarterComponent;
  let fixture: ComponentFixture<CourseStarterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CourseStarterComponent ]
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
