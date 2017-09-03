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

  pagedResult: PagedResult<QuestionListEntry> = new PagedResult<QuestionListEntry>();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
  private router: Router) { }

  ngOnInit() {
    this.searchQuestions();
  }

  public async searchQuestions(): Promise<void> {
    const searchParameters = this.getSearchFilter();
    this.pagedResult = await this.cranDataServiceService.searchForQuestions(searchParameters);
  }

  public getSearchFilter(): SearchQParameters {
    return new SearchQParameters();
  }

}
