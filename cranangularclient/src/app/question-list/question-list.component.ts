import { Component, OnInit, Inject, } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Router, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {QuestionListEntry} from '../model/questionlistentry';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';


@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css']
})
export class QuestionListComponent implements OnInit {

  questions: QuestionListEntry[] = [];

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router,
    private notificationService: NotificationService,
    private confirmService: ConfirmService,
    private ls: LanguageService) { }

  ngOnInit() {
    this.loadQuestions();
  }

  public goToQuestion(question: QuestionListEntry) {
    if (question.status === 1) {
      this.router.navigate(['/viewquestion', question.id]);
    } else {
      this.router.navigate(['/editquestion', question.id]);
    }
  }

  public async deleteQuestion(question: QuestionListEntry): Promise<void> {
    try {
      await this.confirmService.confirm(this.ls.label('deletequestion'), this.ls.label('deletequestionq', String(question.id)));
      try {
        this.notificationService.emitLoading();
        await this.cranDataServiceService.deleteQuestion(question.id);
        this.notificationService.emitDone();
      } catch (error) {
        this.notificationService.emitError(error);
      }
      await this.loadQuestions();
    } catch (err) {
      // that is ok
    }
  }

  private async loadQuestions(): Promise<void> {
    try {
      this.notificationService.emitLoading();
      this.questions = await this.cranDataServiceService.getMyQuestions();
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }
}
