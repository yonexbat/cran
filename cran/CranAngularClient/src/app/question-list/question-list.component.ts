import { Component, OnInit, Inject, } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Router, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.service';
import {QuestionListEntry} from '../model/questionlistentry';


@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css']
})
export class QuestionListComponent implements OnInit {

  questions: QuestionListEntry[] = [];

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router) { }

  ngOnInit() {
    this.cranDataServiceService.getMyQuestions()
    .then(questions => this.questions = questions);
  }

  public goToQuestion(question: QuestionListEntry) {
    this.router.navigate(['/editquestion', question.id]);
  }
}
