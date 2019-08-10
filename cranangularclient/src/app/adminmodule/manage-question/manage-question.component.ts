import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { NgForm } from '@angular/forms';

import {Question} from '../../model/question';
import {QuestionOption} from '../../model/questionoption';
import {ICranDataService} from '../../services/icrandataservice';
import {ConfirmService} from '../../services/confirm.service';
import {CRAN_SERVICE_TOKEN} from '../../services/cran-data.servicetoken';
import {StatusmessageComponent} from '../../uicomps/statusmessage/statusmessage.component';
import {QuestionPreviewComponent} from '../question-preview/question-preview.component';
import {NotificationService} from '../../services/notification.service';
import {LanguageService} from '../../services/language.service';
import {Binary} from '../../model/binary';
import {Image} from '../../model/image';
import { QuestionType } from '../../model/questiontype';


@Component({
  selector: 'app-manage-question',
  templateUrl: './manage-question.component.html',
  styleUrls: ['./manage-question.component.css']
})
export class ManageQuestionComponent implements OnInit {

  public question: Question;

  public actionInProgress = false;

  @ViewChild('statusMessage', { static: false }) statusMessage: StatusmessageComponent;

  @ViewChild('questionPreview', { static: false }) questionPreview: QuestionPreviewComponent;

  @ViewChild('questionForm', { static: false }) questionForm: NgForm;

  constructor(
    @Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService,
    public ls: LanguageService,
    private confirmService: ConfirmService) {

        this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
          const id = params.get('id');
          this.handleRouteChanged(+id);
        });
  }

  ngOnInit() {
  }

  public async save(): Promise<void> {

    this.actionInProgress = true;

    // save current question
    try {
      this.notificationService.emitLoading();
      this.question.questionType = this.getQuestionType();
      if (this.question && this.question.id > 0) {
          await this.cranDataService.updateQuestion(this.question);
          this.actionInProgress = false;
          this.statusMessage.showSaveSuccess();

      } else { // crate new question
        const questionId = await this.cranDataService.insertQuestion(this.question);
        this.actionInProgress = false;
        this.router.navigate(['/admin/editquestion', questionId]);

      }
      this.notificationService.emitDone();
    } catch (error) {

      this.notificationService.emitError(error);
      this.actionInProgress = false;

    }
  }

  private getQuestionType(): QuestionType {
    const numOptions = this.question.options.length;
    const numCorrect = this.question.options.filter((x: QuestionOption) => x.isTrue).length;
    if (numOptions > 1 && numCorrect === 1) {
      return QuestionType.SingleChoice;
    }
    return QuestionType.MultipleChoice;
  }

  private async handleRouteChanged(id: number): Promise<void> {
    if (id > 0) {
      try {
        this.notificationService.emitLoading();
        this.question = await this.cranDataService.getQuestion(id);
        this.notificationService.emitDone();
        if (this.question.status !== 0) {
          this.router.navigate(['/viewquestion', this.question.id]);
        }
      } catch (error) {
        this.notificationService.emitError(error);
      }
    } else {
      this.initNewQuestion();
    }
    this.actionInProgress = false;
  }

  private initNewQuestion() {
    this.question = new Question();

    this.question.options.push({
      isTrue: false,
      text: '',
    });
    this.question.options.push({
      isTrue: false,
      text: '',
    });
  }

  public getSaveButtonText(): string  {
    if (this.question.id > 0) {
      return this.ls.label('save');
    } else {
      return this.ls.label('add');
    }
  }

  public getHeadingText(): string {
    if (this.question != null && this.question.id > 0) {
      return this.ls.label('editquestion', String(this.question.id ));
    } else {
      return this.ls.label('addquestion');
    }
  }

  public removeOption(index: number) {
    this.question.options.splice(index, 1);
  }

  public addOption() {
    const option = new QuestionOption();
    this.question.options.push(option);
  }

  public async showPreview() {
    if (this.question.id > 0) {
      await this.save();
      this.router.navigate(['/viewquestion', this.question.id]);
    } else {
      this.questionPreview.showDialog(this.question);
    }
  }

  public async addImages(images: Binary[]) {
    for (const binary of images) {
      const image = new Image();
      image.idBinary = binary.id;
      image.width = 100;
      this.cranDataService.addImage(image)
      .then((uploadedImage: Image) => {
        this.question.images.push(uploadedImage);
      })
      .catch((error) => {
        this.notificationService.emitError(error);
      });
    }
  }

  public addImagesError(error: string) {
    this.notificationService.emitError(error);
  }

  public onRemoveImage(image: Image) {
    const index = this.question.images.indexOf(image);
    this.question.images.splice(index, 1);
  }
}
