import { Component, OnInit, Inject } from '@angular/core';

import { Router, ActivatedRoute,  ParamMap, Params, NavigationExtras} from '@angular/router';

import {SearchTags} from '../model/searchtags';
import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {PagedResult} from '../model/pagedresult';
import {Tag} from '../model/tag';
import {LanguageService} from '../language.service';
import {ConfirmService} from '../confirm.service';

@Component({
  selector: 'app-manage-tags',
  templateUrl: './manage-tags.component.html',
  styleUrls: ['./manage-tags.component.css']
})
export class ManageTagsComponent implements OnInit {

  private search: SearchTags = new SearchTags();
  private lastParams: ParamMap;
  private pagedResult: PagedResult<Tag> = new PagedResult<Tag>();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService,
    private ls: LanguageService,
    private confirmService: ConfirmService) {
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

    if (isNaN(this.search.page)) {
      this.search.page = 0;
    }

    try {
      this.notificationService.emitLoading();
      this.pagedResult = await this.cranDataService.searchForTags(this.search);
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

  private executeSearch() {
    this.searchTags(0);
  }

  public pageSelected(pageNumber: number) {
    this.searchTags(pageNumber);
  }

  private goToTag(tag: Tag) {
    this.router.navigate(['/managetag', tag.id]);
  }

  private async listQuestions(tag: Tag): Promise<any> {
    const navigationExtras: NavigationExtras = {
      queryParams: {
         andTags: [tag.id],
      }
    };
    this.router.navigate(['/searchq'], navigationExtras);
  }

  private async deleteTag(tag: Tag): Promise<any> {
    try {
      await this.confirmService.confirm(this.ls.label('deletetag'), this.ls.label('deletetagq', tag.name));
      try {
        this.notificationService.emitLoading();
        await this.cranDataService.deleteTag(tag.id);
        this.notificationService.emitDone();
        await this.handleRouteChanged(this.lastParams);
      } catch (error) {
        this.notificationService.emitError(error);
      }
    } catch (error) {
      // that is ok, cancel from user.
    }
  }
}
