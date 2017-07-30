import { Component, OnInit } from '@angular/core';

import {Question} from '../model/question';

@Component({
  selector: 'app-manage-question',
  templateUrl: './manage-question.component.html',
  styleUrls: ['./manage-question.component.css']
})
export class ManageQuestionComponent implements OnInit {

  public question = new Question;

  constructor() { }

  ngOnInit() {
  }

  onSubmit() {
  }

}
