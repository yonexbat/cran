import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageQuestionComponent } from './manage-question.component';

describe('ManageQuestionComponent', () => {
  let component: ManageQuestionComponent;
  let fixture: ComponentFixture<ManageQuestionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageQuestionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
