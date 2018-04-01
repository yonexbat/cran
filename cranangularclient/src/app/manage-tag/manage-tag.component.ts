import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import {Tag} from '../model/tag';
import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {StatusMessageComponent} from '../status-message/status-message.component';
import {LanguageService} from '../language.service';


@Component({
  selector: 'app-manage-tag',
  templateUrl: './manage-tag.component.html',
  styleUrls: ['./manage-tag.component.css']
})
export class ManageTagComponent implements OnInit {

  public tag: Tag = new Tag();
  public headingText: string;
  public actionInProgress = false;
  public buttonText: string;

  @ViewChild('statusMessage') statusMessage: StatusMessageComponent;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
  private router: Router,
  private activeRoute: ActivatedRoute,
  private notificationService: NotificationService,
  private ls: LanguageService) {
    this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
      const id = params.get('id');
      this.handleRouteChanged(+id);
    });
  }

  ngOnInit() {
  }

  private async handleRouteChanged(id: number): Promise<void> {
    if (id > 0) {
      this.buttonText = this.ls.label('save');
      this.headingText =  this.ls.label('edittag', id.toString());
      try {
        this.notificationService.emitLoading();
        this.tag = await this.cranDataService.getTag(id);
        this.notificationService.emitDone();
      } catch (error) {
        this.notificationService.emitError(error);
      }
    } else {
      this.buttonText = this.ls.label('add');
      this.headingText = this.ls.label('addtag');
    }
    this.actionInProgress = false;
  }

  private getSaveButtonText(): string  {
    if (this.tag.id > 0) {
      return this.ls.label('save');
    } else {
      return this.ls.label('add');
    }
  }

  private getHeadingText(): string {
    if (this.tag.id > 0) {
      return this.ls.label('edittag', String(this.tag.id ));
    } else {
      return this.ls.label('addtag');
    }
  }

  private async save(): Promise<void> {
    this.actionInProgress = true;

      // save current question
      try {
        this.notificationService.emitLoading();
        if (this.tag && this.tag.id > 0) {
          await this.cranDataService.updateTag(this.tag);
          this.actionInProgress = false;
          this.statusMessage.showSaveSuccess();
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
