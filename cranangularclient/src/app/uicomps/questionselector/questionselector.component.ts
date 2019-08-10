import { Component, OnInit, Input } from '@angular/core';
import { QuestionSelectorInfo } from 'src/app/model/questionselectorinfo';

@Component({
  selector: 'app-questionselector',
  templateUrl: './questionselector.component.html',
  styleUrls: ['./questionselector.component.scss']
})
export class QuestionselectorComponent implements OnInit {

  // tslint:disable-next-line:variable-name
  private _questionSelectors: QuestionSelectorInfo[];
  @Input()
  set questionSelectors(questionSelectors: QuestionSelectorInfo[]) {
    this._questionSelectors = questionSelectors;
    this.initremainingQuestions();
  }
  get questionSelectors(): QuestionSelectorInfo[] {
    return this._questionSelectors;
  }

  // tslint:disable-next-line:variable-name
  private _numCurrentQuestion: number;
  @Input()
  set numCurrentQuestion(num: number) {
    this._numCurrentQuestion = num;
  }
  get numCurrentQuestion() {
    return this._numCurrentQuestion;
  }

  // tslint:disable-next-line:variable-name
  private _numQuestions: number;
  @Input()
  set numQuestions(num: number) {
    this._numQuestions = num;
    this.initremainingQuestions();
  }
  get numQuestions() {
    return this._numQuestions;
  }

  public remainingQuestions: number[];

  constructor() { }

  ngOnInit() {
  }

  private initremainingQuestions() {
    this.remainingQuestions = [];
    for (let i = this.questionSelectors.length; i < this.numQuestions; i++) {
      this.remainingQuestions.push(i + 1);
    }
  }

}
