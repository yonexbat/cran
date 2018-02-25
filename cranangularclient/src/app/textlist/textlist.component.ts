import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute, ParamMap, NavigationExtras } from '@angular/router';

import {Text} from '../model/text';
import {SearchText} from '../model/searchtext';
import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {StatusMessageComponent} from '../status-message/status-message.component';
import {LanguageService} from '../language.service';
import {PagedResult} from '../model/pagedresult';

@Component({
  selector: 'app-textlist',
  templateUrl: './textlist.component.html',
  styleUrls: ['./textlist.component.css']
})
export class TextlistComponent implements OnInit {

  private pagedResult: PagedResult<Text> = new PagedResult<Text>();
  private search: SearchText = new SearchText();
  private lastParams: ParamMap;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
  private router: Router,
  private activeRoute: ActivatedRoute,
  private notificationService: NotificationService,
  private ls: LanguageService) {
    this.activeRoute.queryParams.subscribe((params: ParamMap)  => {
      this.handleRouteChanged(params);
    });
  }

  ngOnInit() {
  }

  private async handleRouteChanged(params: ParamMap): Promise<void> {

    this.lastParams = params;
    this.search.page = +params['pageNumber'];

    if (isNaN(this.search.page)) {
      this.search.page = 0;
    }

    try {
      this.notificationService.emitLoading();
      this.pagedResult = await this.cranDataService.getTexts(this.search);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  searchTexts(pageNumber: number) {
    const navigationExtras: NavigationExtras = {
      queryParams: {
         pageNumber: pageNumber,
      }
    };

    this.router.navigate(['/textlist'], navigationExtras);
  }

  public pageSelected(pageNumber: number) {
    this.searchTexts(pageNumber);
  }

  private goToText(text: Text) {
    this.router.navigate(['/managetext', text.id]);
  }

}
