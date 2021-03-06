import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute, ParamMap, NavigationExtras } from '@angular/router';

import {Text} from '../../model/text';
import {SearchText} from '../../model/searchtext';
import {ICranDataService} from '../../services/icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../../services/cran-data.servicetoken';
import {NotificationService} from '../../services/notification.service';
import {LanguageService} from '../../services/language.service';
import {PagedResult} from '../../model/pagedresult';

@Component({
  selector: 'app-textlist',
  templateUrl: './textlist.component.html',
  styleUrls: ['./textlist.component.scss']
})
export class TextlistComponent implements OnInit {

  public pagedResult: PagedResult<Text> = new PagedResult<Text>();
  public search: SearchText = new SearchText();
  private lastParams: ParamMap;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
              private router: Router,
              private activeRoute: ActivatedRoute,
              private notificationService: NotificationService,
              public ls: LanguageService) {
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

  private gotoPage(pageNumber: number) {
    const navigationExtras: NavigationExtras = {
      queryParams: {
         pageNumber,
      },
    };

    this.router.navigate(['/admin/textlist'], navigationExtras);
  }

  public pageSelected(pageNumber: number) {
    this.gotoPage(pageNumber);
  }

  private goToText(text: Text) {
    this.router.navigate(['/admin/managetext', text.id]);
  }

}
