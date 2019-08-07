import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionselectorComponent } from './questionselector.component';
import { FormsModule } from '@angular/forms';
import { Params, ActivatedRoute } from '@angular/router';
import { createQuestiontoAskTestObj } from '../../testing/modelobjcreator';
import { StubActivatedRoute } from 'src/app/testing/stubactivatedroute';
import { RouterTestingModule } from '@angular/router/testing';
import { TestingModule } from 'src/app/testing/testing.module';
import { By } from '@angular/platform-browser';

describe('QuestionselectorComponent', () => {
  let component: QuestionselectorComponent;
  let fixture: ComponentFixture<QuestionselectorComponent>;
  const initRoutePram: Params = { id: 1, };
  const activeRoute = new StubActivatedRoute(initRoutePram);

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuestionselectorComponent ],
      imports: [FormsModule, RouterTestingModule, TestingModule],
      providers: [
        {provide: ActivatedRoute, useValue: activeRoute}
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionselectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show items', () => {
    const questionToAsk = createQuestiontoAskTestObj();
    component.questionSelectors = questionToAsk.questionSelectors;
    component.numCurrentQuestion = questionToAsk.numCurrentQuestion;
    component.numQuestions = questionToAsk.numQuestions;
    fixture.detectChanges();

    const items = fixture.debugElement.queryAll(By.css('a'));
    expect(items.length).toBe(questionToAsk.questionSelectors.length);

    const remainingItems = fixture.debugElement.queryAll(By.css('span'));
    expect(remainingItems.length).toBe(questionToAsk.numQuestions - questionToAsk.questionSelectors.length);
  });

});
