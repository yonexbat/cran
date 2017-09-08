import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseInstanceListComponent } from './course-instance-list.component';

describe('CourseInstanceListComponent', () => {
  let component: CourseInstanceListComponent;
  let fixture: ComponentFixture<CourseInstanceListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CourseInstanceListComponent ]
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
