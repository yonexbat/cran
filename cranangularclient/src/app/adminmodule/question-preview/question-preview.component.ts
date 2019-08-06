import { Component, OnInit } from '@angular/core';

import {Question} from '../../model/question';
import {LanguageService} from '../../services/language.service';

declare var $: any;

@Component({
  selector: 'app-question-preview',
  templateUrl: './question-preview.component.html',
  styleUrls: ['./question-preview.component.css']
})
export class QuestionPreviewComponent implements OnInit {

  constructor(public ls: LanguageService) { }

  public question: Question;
  private visible = false;

  ngOnInit() {
  }

  public showDialog(question: Question) {
    this.question = question;
    this.visible = true;
    $('#questionpreview').modal('show');
  }

  public hideDialog() {
    this.visible = false;
    $('#questionpreview').modal('hide');
  }

  public isVisible(): boolean {
    return this.visible;
  }
}
