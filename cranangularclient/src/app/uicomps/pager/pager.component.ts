import { Component, OnInit, TemplateRef, Input, EventEmitter, Output } from '@angular/core';

import {PagedResult} from '../../model/pagedresult';
import {LanguageService} from '../../language.service';

const numpages = 5;

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {

  constructor(private ls: LanguageService) { }

  @Input()
  public itemTemplate: TemplateRef<any>;

  @Input()
  public pagedResult: PagedResult<any>;

  @Input()
  public nodatafoundmessage  = 'Keine Daten gefunden.';

  @Output() onSelectedPageChanged = new EventEmitter<number>();

  ngOnInit() {
  }

  private getPages(): number[] {

    const firstpage = this.firstPageCurrentPages();
    const lastpage = this.lastPageCurrentPages();

    let numPageSymoblsToShow = lastpage - firstpage;

    if (numPageSymoblsToShow <= 0) {
      numPageSymoblsToShow = 0;
    }

    if (this.pagedResult) {
      const res = Array(numPageSymoblsToShow)
      .fill(0)
      .map((arrayitem: number, iterator: number) => iterator + firstpage);
      return res;
    }
    return [];
  }

  private firstPageCurrentPages(): number {
    return Math.floor(this.pagedResult.currentPage / numpages) * numpages;
  }

  private lastPageCurrentPages(): number {
    const firstpage = this.firstPageCurrentPages();
    let lastpage = firstpage + numpages;
    if (lastpage > this.pagedResult.numpages) {
      lastpage = this.pagedResult.numpages;
    }
    return lastpage;
  }

  private showPreviousPagesButton(): boolean {
    const firstpageOfCurrentPages = this.firstPageCurrentPages();
    return firstpageOfCurrentPages > 0;
  }

  private showNextPagesButton(): boolean {
    const nextPagesFirstPage = this.nextPagesFirstPage();
    return nextPagesFirstPage < this.pagedResult.numpages;
  }

  private previousPagesFirstPage(): number {
    const firstpage = this.firstPageCurrentPages();
    return firstpage - numpages;
  }

  private nextPagesFirstPage(): number {
    const firstpage = this.firstPageCurrentPages();
    return firstpage + numpages;
  }


  public pageClicked(pageNumber: number) {
    this.onSelectedPageChanged.emit(pageNumber);
  }

}
