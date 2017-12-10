import { Component, OnInit, Inject, } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Router, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {QuestionListEntry} from '../model/questionlistentry';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {PagedResult} from '../model/pagedresult';


@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css']
})
export class QuestionListComponent implements OnInit {

  private pagedResult: PagedResult<QuestionListEntry> = new PagedResult<QuestionListEntry>();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router,
    private notificationService: NotificationService,
    private confirmService: ConfirmService,
    private ls: LanguageService) { }

  ngOnInit() {
    this.loadQuestions(0);
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
      await this.loadQuestions(0);
    } catch (err) {
      // that is ok
    }
  }

  private async loadQuestions(page: number): Promise<void> {
    try {
      this.notificationService.emitLoading();
      this.pagedResult = await this.cranDataServiceService.getMyQuestions(page);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  private pageSelected(pageNumber: number) {
    this.loadQuestions(pageNumber);
  }
}
