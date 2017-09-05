import { Component, OnInit, Inject, } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Router, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {QuestionListEntry} from '../model/questionlistentry';
import {NotificationService} from '../notification.service';


@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css']
})
export class QuestionListComponent implements OnInit {

  questions: QuestionListEntry[] = [];

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router,
    private notificationService: NotificationService) { }

  ngOnInit() {
    this.loadQuestions();
  }

  public goToQuestion(question: QuestionListEntry) {
    this.router.navigate(['/editquestion', question.id]);
  }

  public async deleteQuestion(question: QuestionListEntry): Promise<void> {
    if (confirm('Frage löschen?')) {
      try {
        await this.cranDataServiceService.deleteQuestion(question.id);
      } catch (error) {
        this.notificationService.emitError(error);
      }
      await this.loadQuestions();
    }
  }

  private async loadQuestions(): Promise<void> {
    try {
      this.questions = await this.cranDataServiceService.getMyQuestions();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }
}
