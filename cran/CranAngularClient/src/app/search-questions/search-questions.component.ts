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

  private pagedResult: PagedResult<QuestionListEntry> = new PagedResult<QuestionListEntry>();
  private search = new SearchQParameters();

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
    this.search.page = pageNumber;
    return this.search;
  }

  public pageSelected(pageNumber: number) {
    this.searchQuestions(pageNumber);
  }

  public goToQuestion(question: QuestionListEntry) {
    this.router.navigate(['/editquestion', question.id]);
  }

  public deleteQuestion(question: QuestionListEntry) {
    if (confirm('Frage löschen?')) {
      this.cranDataServiceService.deleteQuestion(question.id)
        .then(nores => this.searchQuestions(this.pagedResult.currentPage));
    }
  }

}
