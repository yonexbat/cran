import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import {Tag} from '../model/tag';
import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';


@Component({
  selector: 'app-manage-tag',
  templateUrl: './manage-tag.component.html',
  styleUrls: ['./manage-tag.component.css']
})
export class ManageTagComponent implements OnInit {

  public tag: Tag;
  public headingText: string;
  public actionInProgress = false;
  public buttonText: string;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
  private router: Router,
  private activeRoute: ActivatedRoute,
  private notificationService: NotificationService) {
    this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
      const id = params.get('id');
      this.handleRouteChanged(+id);
    });

    // Create two options for default.
    this.tag = new Tag();
  }

  ngOnInit() {
  }

  private async handleRouteChanged(id: number): Promise<void> {
    if (id > 0) {
      this.buttonText = 'Speichern';
      this.headingText = 'Tag #' + id + ' editieren';
      try {
        this.notificationService.emitLoading();
        this.tag = await this.cranDataService.getTag(id);
        this.notificationService.emitDone();
      } catch (error) {
        this.notificationService.emitError(error);
      }
    } else {
      this.buttonText = 'Hinzufügen';
      this.headingText = 'Tag hinzufügen';
    }
    this.actionInProgress = false;
  }

  private async save(): Promise<void> {
    this.actionInProgress = true;

      // save current question
      try {
        this.notificationService.emitLoading();
        if (this.tag && this.tag.id > 0) {
          await this.cranDataService.updateTag(this.tag);
          this.actionInProgress = false;
        } else { // crate new question
          const tagId = await this.cranDataService.insertTag(this.tag);
          this.actionInProgress = false;
          this.router.navigate(['/managetag', tagId]);
        }
        this.notificationService.emitDone();
      } catch (error) {

        this.notificationService.emitError(error);
        this.actionInProgress = false;
      }
  }
}
