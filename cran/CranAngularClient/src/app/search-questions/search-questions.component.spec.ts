import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchQuestionsComponent } from './search-questions.component';

describe('SearchQuestionsComponent', () => {
  let component: SearchQuestionsComponent;
  let fixture: ComponentFixture<SearchQuestionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchQuestionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchQuestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
