import { Component, OnInit } from '@angular/core';

import {LanguageService} from '../language.service';
import {ConfirmService} from '../confirm.service';
import {ConfirmRequest} from '../model/confirmrequest';

declare var $: any;

@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.css']
})
export class ConfirmComponent implements OnInit {

  title  = '';
  text = '';
  promiseResolver: any;

  constructor(private ls: LanguageService,
    private confirmService: ConfirmService) {
  }

  ngOnInit() {
    this.confirmService.on().subscribe((req: ConfirmRequest) => this.confirm(req));
  }

  private confirm(req: ConfirmRequest) {
    this.title = req.title;
    this.text = req.text;
    $('#confirmDialog').modal('show');
  }

  private ok() {
    this.confirmService.ok();
  }

  private nok() {
    this.confirmService.nok();
  }

}
