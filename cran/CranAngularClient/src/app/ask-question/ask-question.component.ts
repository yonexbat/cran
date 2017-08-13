import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, ActivatedRoute, ParamMap,} from '@angular/router';
import { HttpModule, } from '@angular/http';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.service';
import {QuestionToAsk} from '../model/questiontoask';
import {Question} from '../model/question';
import {QuestionOption} from '../model/questionoption';
import {StatusMessageComponent} from '../status-message/status-message.component';
import {QuestionAnswer} from '../model/questionanswer';

@Component({
  selector: 'app-ask-question',
  templateUrl: './ask-question.component.html',
  styleUrls: ['./ask-question.component.css']
})
export class AskQuestionComponent implements OnInit {

  public checkShown: boolean;

  public questionToAsk: QuestionToAsk;

  @ViewChild('statusMessage') statusMessage: StatusMessageComponent;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute) {

    this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
      const id = params.get('id');
      this.handleRouteChanged(+id);
    });

    this.questionToAsk = new QuestionToAsk();

  }

  ngOnInit() {

  }

  public check() {
    const answer: QuestionAnswer = this.getAnswer();
    this.cranDataServiceService.answerQuestionAndGetSolution(answer)
      .then((question: Question) => {
        for (let i = 0; i < question.options.length; i++) {
          this.questionToAsk.options[i].isTrue = question.options[i].isTrue;
        }
        this.checkShown = true;
      })
      .catch(reason => this.statusMessage.showError(reason));
  }

  public nextQuestion() {
      const answer: QuestionAnswer = this.getAnswer();
      this.cranDataServiceService.answerQuestionAndGetNextQuestion(answer)
      .then((data) => {
        this.checkShown = false;
        if (data.idCourseInstanceQuestion > 0) {
            this.router.navigate(['/askquestion', data.idCourseInstanceQuestion]);
        } else {
          this.router.navigate(['/list']);
        }
      })
      .catch(reason => this.statusMessage.showError(reason));
  }

  private getAnswer(): QuestionAnswer {
    const answer: QuestionAnswer = new QuestionAnswer();
    answer.idCourseInstanceQuestion = this.questionToAsk.idCourseInstanceQuestion;
    this.questionToAsk.options.forEach(option => {
      answer.answers.push(option.isChecked);
    });
    return answer;
  }

  private handleRouteChanged(id: number) {
    this.cranDataServiceService.getQuestionToAsk(id)
      .then(data => {
        this.questionToAsk = data;
      })
      .catch(reason => this.statusMessage.showError(reason));
  }

}
