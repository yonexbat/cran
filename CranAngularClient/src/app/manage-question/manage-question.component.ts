import { Component, OnInit } from '@angular/core';

import {Question} from '../model/question';
import {CranDataService} from '../cran-data.service';

@Component({
  selector: 'app-manage-question',
  templateUrl: './manage-question.component.html',
  styleUrls: ['./manage-question.component.css']
})
export class ManageQuestionComponent implements OnInit {

  public question = new Question;

  constructor(private cranDataService: CranDataService) { }

  ngOnInit() {
  }

  onSubmit() {

  }

  addQuestion() {
    this.cranDataService.insertQuestion(this.question);
  }

}
