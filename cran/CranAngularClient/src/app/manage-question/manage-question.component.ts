import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import {Question} from '../model/question';
import {QuestionOption} from '../model/questionoption';
import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.service';
import {StatusMessageComponent} from '../status-message/status-message.component';


@Component({
  selector: 'app-manage-question',
  templateUrl: './manage-question.component.html',
  styleUrls: ['./manage-question.component.css']
})
export class ManageQuestionComponent implements OnInit {

  public question: Question;

  public actionInProgress = false;

  public headingText: string;

  public buttonText: string;

  @ViewChild('statusMessage') statusMessage: StatusMessageComponent;

  constructor(
    @Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute) {

        this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
          const id = params.get('id');
          this.handleRouteChanged(+id);
        });

        this.question = new Question();
        this.question.options.push({
          isTrue: true,
          text: '',
        });
        this.question.options.push({
          isTrue: false,
          text: '',
        });
  }

  ngOnInit() {
  }

  save() {
    this.actionInProgress = true;

    // save current question
    if (this.question && this.question.id > 0) {
      this.cranDataService.updateQuestion(this.question).then(status => {
        this.actionInProgress = false;
        this.statusMessage.showSaveSuccess();
      }).catch(reason => {
          this.statusMessage.showError(reason);
          this.actionInProgress = false;
      });
    } else { // crate new question
      this.cranDataService.insertQuestion(this.question)
      .then(questionId => {
        this.actionInProgress = false;
        this.router.navigate(['/editquestion', questionId]);
      }).catch(reason => {
          this.statusMessage.showError(reason);
          this.actionInProgress = false;
      });
    }
  }

  private handleRouteChanged(id: number) {
    if (id > 0) {
      this.buttonText = 'Speichern';
      this.headingText = 'Frage ' + id + ' editieren';
      this.cranDataService.getQuestion(id)
        .then(question => this.question = question)
        .catch(reason => this.statusMessage.showError(reason));
    } else {
      this.buttonText = 'Hinzufügen';
      this.headingText = 'Frage hinzufügen';
    }
    this.actionInProgress = false;
  }

  public removeOption(index: number) {
    this.question.options.splice(index, 1);
  }

  public addOpton() {
    const option = new QuestionOption();
    option.isTrue = true;
    this.question.options.push(option);
  }

}
