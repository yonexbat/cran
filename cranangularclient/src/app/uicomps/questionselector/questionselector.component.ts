import { Component, OnInit, Input } from '@angular/core';
import { QuestionSelectorInfo } from 'src/app/model/questionselectorinfo';

@Component({
  selector: 'app-questionselector',
  templateUrl: './questionselector.component.html',
  styleUrls: ['./questionselector.component.scss']
})
export class QuestionselectorComponent implements OnInit {

  private _questionSelectors: QuestionSelectorInfo[];
  @Input()
  set questionSelectors(questionSelectors: QuestionSelectorInfo[]) {
    this._questionSelectors = questionSelectors;
    this.initremainingQuestions();
  }
  get questionSelectors(): QuestionSelectorInfo[] {
    return this._questionSelectors;
  }

  private _numCurrentQuestion: number;
  @Input()
  set numCurrentQuestion(num: number) {
    this._numCurrentQuestion = num;
  }
  get numCurrentQuestion() {
    return this._numCurrentQuestion;
  }

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
    if (this.questionSelectors) {
      for (let i = this.questionSelectors.length; i < this.numQuestions; i++) {
        this.remainingQuestions.push(i + 1);
      }
    }
  }

}
