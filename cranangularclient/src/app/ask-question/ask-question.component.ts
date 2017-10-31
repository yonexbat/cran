import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, ActivatedRoute, ParamMap, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {LanguageService} from '../language.service';
import {QuestionToAsk} from '../model/questiontoask';
import {Question} from '../model/question';
import {QuestionOption} from '../model/questionoption';
import {StatusMessageComponent} from '../status-message/status-message.component';
import {QuestionAnswer} from '../model/questionanswer';
import {CourseInstance} from '../model/courseinstance';
import {NotificationService} from '../notification.service';
import {CommentsComponent} from '../comments/comments.component';

@Component({
  selector: 'app-ask-question',
  templateUrl: './ask-question.component.html',
  styleUrls: ['./ask-question.component.css']
})
export class AskQuestionComponent implements OnInit {

  @ViewChild('comments') private commentsControl: CommentsComponent;

  private checkShown: boolean;
  private questionToAsk: QuestionToAsk;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService,
    private ls: LanguageService) {

    this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
      const id = params.get('id');
      this.handleRouteChanged(+id);
    });
  }

  ngOnInit() {

  }

  public async getSolution(): Promise<void> {
    const answer: QuestionAnswer = this.getAnswerDto();
    try {
      this.notificationService.emitLoading();
      const question =  await this.cranDataServiceService.answerQuestionAndGetSolution(answer);
      await this.commentsControl.showComments(question.id);
      this.showSolution(question);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  private showSolution(question: Question) {
    for (let i = 0; i < question.options.length; i++) {
      this.questionToAsk.options[i].isTrue = question.options[i].isTrue;
    }
    this.questionToAsk.question = question;
    this.checkShown = true;
  }

  public async nextQuestion(): Promise<void> {
      const answer: QuestionAnswer = this.getAnswerDto();
      try {
        this.notificationService.emitLoading();
        const data = await this.cranDataServiceService.answerQuestionAndGetNextQuestion(answer);
        this.checkShown = false;
        if (data.idCourseInstanceQuestion > 0) {
          this.router.navigate(['/askquestion', data.idCourseInstanceQuestion]);
        } else {
          this.goToResult();
        }
        this.notificationService.emitDone();
      } catch (error) {
        this.notificationService.emitError(error);
      }
  }

  private getAnswerDto(): QuestionAnswer {
    const answer: QuestionAnswer = new QuestionAnswer();
    answer.idCourseInstanceQuestion = this.questionToAsk.idCourseInstanceQuestion;
    this.questionToAsk.options.forEach(option => {
      answer.answers.push(option.isChecked);
    });
    return answer;
  }

  public goToResult() {
    this.router.navigate(['/resultlist', this.questionToAsk.idCourseInstance]);
  }

  public goToEditQuestion() {
    this.router.navigate(['/editquestion', this.questionToAsk.question.id]);
  }

  private async handleRouteChanged(id: number): Promise<void> {
    try {
      this.notificationService.emitLoading();
      const questionToAsk: QuestionToAsk  = await this.cranDataServiceService.getQuestionToAsk(id);
      let question: Question = null;
      if (questionToAsk.courseEnded) {
        question =  await this.cranDataServiceService.getQuestion(questionToAsk.idQuestion);
        await this.commentsControl.showComments(question.id);
      } else {
        await this.commentsControl.showComments(null);
      }
      this.questionToAsk = questionToAsk;
      if (this.questionToAsk.courseEnded) {
        this.showSolution(question);
      }
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

}
