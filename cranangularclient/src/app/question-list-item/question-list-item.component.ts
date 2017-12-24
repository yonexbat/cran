import { Component, OnInit,
  ViewEncapsulation, Input, Output,
  EventEmitter } from '@angular/core';

import {QuestionListEntry} from '../model/questionlistentry';

@Component({
  selector: 'app-question-list-item',
  templateUrl: './question-list-item.component.html',
  styleUrls: ['./question-list-item.component.css'],
  encapsulation: ViewEncapsulation.None,
  host: {'[class.list-group-item]': 'true', 
         '[class.questionstatus]': 'true',
         '[class.questionstatusok]':'item.status === 1',
         '[class.questionstatussuperseded]':'item.status === 2',
         '[class.questionstatusnok]':'item.status === 0',}
})
export class QuestionListItemComponent implements OnInit {

  @Input()
  private item: QuestionListEntry;

  @Output()
  onItemDeletedClick = new EventEmitter<QuestionListEntry>();

  @Output()
  onItemViewClick = new EventEmitter<QuestionListEntry>();

  @Output()
  onItemEditclick = new EventEmitter<QuestionListEntry>();

  constructor() { }

  private itemDelete() {
    this.onItemDeletedClick.emit(this.item);
  }

  private itemView() {
    this.onItemViewClick.emit(this.item);
  }

  private itemEdit() {
    this.onItemEditclick.emit(this.item);
  }

  ngOnInit() {
  }

}
