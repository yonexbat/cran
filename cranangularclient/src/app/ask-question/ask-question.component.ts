import {Component, OnInit, Inject, ViewChild, } from '@angular/core';
import {Router, ActivatedRoute, ParamMap, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {LanguageService} from '../language.service';
import {QuestionToAsk} from '../model/questiontoask';
import {Question} from '../model/question';
import {QuestionAnswer} from '../model/questionanswer';
import {CourseInstance} from '../model/courseinstance';
import {NotificationService} from '../notification.service';
import {CommentsComponent} from '../comments/comments.component';
import {ConfirmService} from '../confirm.service';
import {QuestionType} from '../model/questiontype';
import {QuestionOptionToAsk} from '../model/questionoptiontoask';


@Component({
  selector: 'app-ask-question',
  templateUrl: './ask-question.component.html',
  styleUrls: ['./ask-question.component.css']
})
export class AskQuestionComponent implements OnInit {

  @ViewChild('comments') private commentsControl: CommentsComponent;

  private checkShown: boolean;
  public questionToAsk: QuestionToAsk;
  private remainingQuestions: number[];
  public selectedOption: string;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService,
    private confirmService: ConfirmService,
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
      const answeredCorrectly = this.showSolution(question);
      const questionSelector =
        this.questionToAsk.questionSelectors.find(x => x.idCourseInstanceQuestion === this.questionToAsk.idCourseInstanceQuestion);
      questionSelector.answerShown = true;
      questionSelector.correct = answeredCorrectly;
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  private showSolution(question: Question): boolean {
    let answeredCorrectly = true;
    for (let i = 0; i < question.options.length; i++) {
      this.questionToAsk.options[i].isTrue = question.options[i].isTrue;
      if (this.questionToAsk.options[i].isTrue !== this.questionToAsk.options[i].isChecked) {
        answeredCorrectly = false;
      }
    }
    this.questionToAsk.question = question;
    this.checkShown = true;
    return answeredCorrectly;
  }

  public async nextQuestion(): Promise<void> {
      const answer: QuestionAnswer = this.getAnswerDto();
      try {
        this.notificationService.emitLoading();
        const data: CourseInstance = await this.cranDataServiceService.answerQuestionAndGetNextQuestion(answer);
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

  private async createNewVersion() {
    // save current question
    try {
      await this.confirmService.confirm(this.ls.label('version'), this.ls.label('versionq'));
      this.notificationService.emitLoading();
      const newId = await this.cranDataServiceService.versionQuestion( this.questionToAsk.question.id);
      this.notificationService.emitDone();
      this.router.navigate(['/editquestion', newId]);
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  public async saveAnswer(): Promise<void> {
      const answer: QuestionAnswer = this.getAnswerDto();
      try {
        this.notificationService.emitLoading();
        await this.cranDataServiceService.answerQuestion(answer);
        this.notificationService.emitDone();
      } catch (error) {
        this.notificationService.emitError(error);
      }
  }

  private async handleRouteChanged(id: number): Promise<void> {
    try {
      this.checkShown = false;
      this.notificationService.emitLoading();
      this.questionToAsk   = await this.cranDataServiceService.getQuestionToAsk(id);
      this.initRadioButtons();

      if (this.questionToAsk.courseEnded || this.questionToAsk.answerShown) {
        const question =  await this.cranDataServiceService.getQuestion(this.questionToAsk.idQuestion);
        await this.commentsControl.showComments(question.id);
        this.showSolution(question);
      } else {
        await this.commentsControl.showComments(null);
      }
      this.remainingQuestions = [];
      for (let i = this.questionToAsk.questionSelectors.length; i < this.questionToAsk.numQuestions; i++) {
        this.remainingQuestions.push(i + 1);
      }
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  private initRadioButtons() {
    if (this.isSingeleChoice) {
      const firstChecked: QuestionOptionToAsk =  this.questionToAsk.options.find(x => x.isChecked);
      if (firstChecked != null) {
        const index: number = this.questionToAsk.options.indexOf(firstChecked);
        this.selectedOption = index.toString();
      }
    }
  }

  public async radioButtonChanged(selectedIndex) {
      for (let i = 0; i < this.questionToAsk.options.length; i++) {
        this.questionToAsk.options[i].isChecked = selectedIndex === i;
      }
  }

  get isMultipleChoice(): boolean {
    return this.questionToAsk.questionType === QuestionType.MultipleChoice;
  }

  get isSingeleChoice(): boolean {
    return this.questionToAsk.questionType === QuestionType.SingleChoice;
  }

}
