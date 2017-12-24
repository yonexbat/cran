import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionListItemComponent } from './question-list-item.component';

describe('QuestionListItemComponent', () => {
  let component: QuestionListItemComponent;
  let fixture: ComponentFixture<QuestionListItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuestionListItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
