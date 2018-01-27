import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute,  ParamMap, Params, NavigationExtras} from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {QuestionListEntry} from '../model/questionlistentry';
import {SearchQParameters} from '../model/searchqparameters';
import {PagedResult} from '../model/pagedresult';
import {NotificationService} from '../notification.service';
import {LanguageService} from '../language.service';
import {ConfirmService} from '../confirm.service';

@Component({
  selector: 'app-search-questions',
  templateUrl: './search-questions.component.html',
  styleUrls: ['./search-questions.component.css']
})
export class SearchQuestionsComponent implements OnInit {

  private pagedResult: PagedResult<QuestionListEntry> = new PagedResult<QuestionListEntry>();
  private search = new SearchQParameters();
  private lastParams: ParamMap;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService,
    private ls: LanguageService,
    private confirmService: ConfirmService) {

      this.activeRoute.queryParams.subscribe((params: ParamMap)  => {
        this.handleRouteChanged(params);
      });
  }

  private async handleRouteChanged(params: ParamMap): Promise<void> {

    this.lastParams = params;
    this.search.page = +params['pageNumber'];
    this.search.title = params['title'];
    this.search.language = params['language'];

    if (params['statusCreated']) {
      this.search.statusCreated = params['statusCreated'] === 'true';
    }
    if (params['statusReleased']) {
      this.search.statusReleased =  params['statusReleased'] === 'true';
    }
    if (params['statusObsolete']) {
      this.search.statusObsolete = params['statusObsolete'] === 'true';
    }

    const andTagsJson: number[] = params['andTags'];
    const orTagsJson: number[] = params['orTags'];

    if (andTagsJson && andTagsJson.length > 0) {
      this.search.andTags = await this.cranDataServiceService.getTags(andTagsJson);
    }
    if (orTagsJson && orTagsJson.length  > 0) {
      this.search.orTags = await this.cranDataServiceService.getTags(orTagsJson);
    }
    if (isNaN(this.search.page)) {
      this.search.page = 0;
    }
    try {
      this.notificationService.emitLoading();
      this.pagedResult = await this.cranDataServiceService.searchForQuestions(this.search);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  ngOnInit() {

  }

  public searchQuestions(pageNumber: number) {

    const andTags: number[] = this.search.andTags.map(x => x.id);
    const orTags: number[] = this.search.orTags.map(x => x.id);

    const navigationExtras: NavigationExtras = {
      queryParams: {
         pageNumber: pageNumber,
         title: this.search.title,
         andTags: andTags,
         orTags: orTags,
         language: this.search.language,
         statusCreated: this.search.statusCreated,
         statusReleased: this.search.statusReleased,
         statusObsolete: this.search.statusObsolete,
      }
    };

    this.router.navigate(['/searchq'], navigationExtras);
  }

  public pageSelected(pageNumber: number) {
    this.searchQuestions(pageNumber);
  }

  private executeSearch() {
    this.searchQuestions(0);
  }

  public async deleteQuestion(question: QuestionListEntry): Promise<void> {
    try {
      await this.confirmService.confirm(this.ls.label('deletequestion'), this.ls.label('deletequestionq', String(question.id)));
      try {
        this.notificationService.emitLoading();
        await this.cranDataServiceService.deleteQuestion(question.id);
        this.notificationService.emitDone();
        if (this.lastParams) {
          await this.handleRouteChanged(this.lastParams);
        }
      } catch (error) {
        this.notificationService.emitError(error);
      }
    } catch (err) {
      // Thats ok, user cancels deletion
    }
  }

}
