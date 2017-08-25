import { Component, OnInit } from '@angular/core';

import {Question} from '../model/question';

declare var $: any;

@Component({
  selector: 'app-question-preview',
  templateUrl: './question-preview.component.html',
  styleUrls: ['./question-preview.component.css']
})
export class QuestionPreviewComponent implements OnInit {

  constructor() { }

  public question: Question;

  ngOnInit() {
  }

  public showDialog(question: Question) {
    this.question = question;
    $('#questionpreview').modal('show');
  }

}
