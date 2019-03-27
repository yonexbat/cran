import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseFavoriteListComponent } from './course-favorite-list.component';

describe('CourseFavoriteListComponent', () => {
  let component: CourseFavoriteListComponent;
  let fixture: ComponentFixture<CourseFavoriteListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CourseFavoriteListComponent ]
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
