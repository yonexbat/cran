import { Component, OnInit, Inject } from '@angular/core';
import { Router, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {QuestionListEntry} from '../model/questionlistentry';
import {SearchQParameters} from '../model/searchqparameters';
import {PagedResult} from '../model/pagedresult';

@Component({
  selector: 'app-search-questions',
  templateUrl: './search-questions.component.html',
  styleUrls: ['./search-questions.component.css']
})
export class SearchQuestionsComponent implements OnInit {

  public pagedResult: PagedResult<QuestionListEntry> = new PagedResult<QuestionListEntry>();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
  private router: Router) { }

  ngOnInit() {
    this.searchQuestions(0);
  }

  public async searchQuestions(pageNumber: number): Promise<void> {
    const searchParameters = this.getSearchFilter(pageNumber);
    this.pagedResult = await this.cranDataServiceService.searchForQuestions(searchParameters);
  }

  public getSearchFilter(pageNumber: number): SearchQParameters {
    const search = new SearchQParameters();
    search.page = pageNumber;
    return search;
  }

  public pageSelected(pageNumber: number) {
    this.searchQuestions(pageNumber);
  }

}
