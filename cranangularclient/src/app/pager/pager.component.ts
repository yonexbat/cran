import { Component, OnInit, TemplateRef, Input, EventEmitter, Output } from '@angular/core';

import {PagedResult} from '../model/pagedresult';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.css']
})
export class PagerComponent implements OnInit {

  constructor() { }

  @Input()
  public itemTemplate: TemplateRef<any>;

  @Input()
  public pagedResult: PagedResult<any>;

  @Output() onSelectedPageChanged = new EventEmitter<number>();

  ngOnInit() {
  }

  public getPages(): number[] {
    if (this.pagedResult) {
      const res = Array(this.pagedResult.numpages)
      .fill(0)
      .map((arrayitem: number, iterator: number) => iterator);
      return res;
    }
    return [];
  }

  public pageClicked(pageNumber: number) {
    this.onSelectedPageChanged.emit(pageNumber);
  }

}
