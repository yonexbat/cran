import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import {Question} from '../model/question';
import {QuestionOption} from '../model/questionoption';
import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {StatusMessageComponent} from '../status-message/status-message.component';
import {QuestionPreviewComponent} from '../question-preview/question-preview.component';
import {NotificationService} from '../notification.service';
import {Binary} from '../model/binary';
import {Image} from '../model/image';


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

  @ViewChild('questionPreview') questionPreview: QuestionPreviewComponent;

  constructor(
    @Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService) {

        this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
          const id = params.get('id');
          this.handleRouteChanged(+id);
        });

        // Create two options for default.
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

  ngOnInit() {
  }

  private async save(): Promise<void> {
    this.actionInProgress = true;


    // save current question
    try {
      this.notificationService.emitLoading();
      if (this.question && this.question.id > 0) {

          const status = await this.cranDataService.updateQuestion(this.question);
          this.actionInProgress = false;
          this.statusMessage.showSaveSuccess();

      } else { // crate new question

        const questionId = await this.cranDataService.insertQuestion(this.question);
        this.actionInProgress = false;
        this.router.navigate(['/editquestion', questionId]);

      }
      this.notificationService.emitDone();
    } catch (error) {

      this.notificationService.emitError(error);
      this.actionInProgress = false;

    }
  }

  private async handleRouteChanged(id: number): Promise<void> {
    if (id > 0) {
      this.buttonText = 'Speichern';
      this.headingText = 'Frage #' + id + ' editieren';
      try {
        this.notificationService.emitLoading();
        this.question = await this.cranDataService.getQuestion(id);
        this.notificationService.emitDone();
      } catch (error) {
        this.notificationService.emitError(error);
      }
    } else {
      this.buttonText = 'Hinzufügen';
      this.headingText = 'Frage hinzufügen';
    }
    this.actionInProgress = false;
  }

  private removeOption(index: number) {
    this.question.options.splice(index, 1);
  }

  private addOpton() {
    const option = new QuestionOption();
    this.question.options.push(option);
  }

  private showPreview() {
    this.questionPreview.showDialog(this.question);
  }

  private async addImages(images: Binary[]) {
    for ( let i = 0; i < images.length; i++) {
      const binary: Binary = images[i];
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

  private onRemoveImage(image: Image) {
    const index = this.question.images.indexOf(image);
    this.question.images.splice(index, 1);
  }
}
