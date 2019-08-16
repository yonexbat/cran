import { Component, OnInit,
  ViewEncapsulation, Input, Output,
  EventEmitter, HostBinding } from '@angular/core';
import { Router, } from '@angular/router';

import {QuestionListEntry} from '../../model/questionlistentry';
import {LanguageService} from '../../services/language.service';

@Component({
  selector: 'app-question-list-item',
  templateUrl: './question-list-item.component.html',
  styleUrls: ['./question-list-item.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class QuestionListItemComponent implements OnInit {

  @HostBinding('class.list-group-item')
  private groupClass = true;

  @HostBinding('class.questionstatus')
  private questionStatus = true;

  @HostBinding('class.questionstatusnok')
  private get classStatusCreated(): boolean {
    return this.item.status === 0;
  }

  @HostBinding('class.questionstatusok')
  private get classStatusReleased(): boolean {
    return this.item.status === 1;
  }

  @HostBinding('class.questionstatussuperseded')
  private get classStatusSuperseded(): boolean {
    return this.item.status === 2;
  }

  @Input()
  public item: QuestionListEntry;

  @Output()
  onItemDeletedClick = new EventEmitter<QuestionListEntry>();

  @Output()
  onItemViewClick = new EventEmitter<QuestionListEntry>();

  @Output()
  onItemEditclick = new EventEmitter<QuestionListEntry>();

  constructor(private router: Router) { }

  public itemDelete() {
    this.onItemDeletedClick.emit(this.item);
  }

  private itemView() {
    this.onItemViewClick.emit(this.item);
    this.router.navigate(['/viewquestion', this.item.id]);
  }

  private itemEdit() {
    this.onItemEditclick.emit(this.item);
    this.router.navigate(['/admin/editquestion', this.item.id]);
  }

  ngOnInit() {
  }

}
