import { Component, OnInit, Inject } from '@angular/core';

import { Router, ActivatedRoute,  ParamMap, Params, NavigationExtras} from '@angular/router';

import {SearchTags} from '../model/searchtags';
import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {PagedResult} from '../model/pagedresult';
import {Tag} from '../model/tag';

@Component({
  selector: 'app-manage-tags',
  templateUrl: './manage-tags.component.html',
  styleUrls: ['./manage-tags.component.css']
})
export class ManageTagsComponent implements OnInit {

  private search: SearchTags = new SearchTags();
  private lastParams: ParamMap;
  private pagedResult: PagedResult<Tag> = new PagedResult<Tag>();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
  private router: Router,
  private activeRoute: ActivatedRoute,
  private notificationService: NotificationService) {
    this.activeRoute.queryParams.subscribe((params: ParamMap)  => {
      this.handleRouteChanged(params);
    });
  }

  ngOnInit() {
  }

  private async handleRouteChanged(params: ParamMap): Promise<void> {
    this.lastParams = params;
    this.search.page = +params['pageNumber'];
    this.search.name = params['name'];

    try {
      this.notificationService.emitLoading();
      this.pagedResult = await this.cranDataServiceService.searchForTags(this.search);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  searchTags(pageNumber: number) {
    const navigationExtras: NavigationExtras = {
      queryParams: {
         pageNumber: pageNumber,
         name: this.search.name,
      }
    };

    this.router.navigate(['/managetags'], navigationExtras);
  }

  public pageSelected(pageNumber: number) {
    this.searchTags(pageNumber);
  }

  private goToTag(tag: Tag) {
    this.router.navigate(['/managetag', tag.id]);
  }

  private async deleteTag(tag: Tag): Promise<any> {
  }

}
