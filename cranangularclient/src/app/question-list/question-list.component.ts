import { Component, OnInit, Inject, } from '@angular/core';
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

  public pagedResult: PagedResult<QuestionListEntry> = new PagedResult<QuestionListEntry>();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private router: Router,
    private notificationService: NotificationService,
    private confirmService: ConfirmService,
    public ls: LanguageService) { }

  ngOnInit() {
    this.loadQuestions(0);
  }

  public async deleteQuestion(question: QuestionListEntry): Promise<void> {
    try {
      await this.confirmService.confirm(this.ls.label('deletequestion'), this.ls.label('deletequestionq', String(question.id)));
      try {
        this.notificationService.emitLoading();
        await this.cranDataService.deleteQuestion(question.id);
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
      this.pagedResult = await this.cranDataService.getMyQuestions(page);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  public pageSelected(pageNumber: number) {
    this.loadQuestions(pageNumber);
  }
}
