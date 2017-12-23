import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, ActivatedRoute, ParamMap, } from '@angular/router';
import { HttpModule, } from '@angular/http';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {Question} from '../model/question';
import {CommentsComponent} from '../comments/comments.component';
import {LanguageService} from '../language.service';
import { ConfirmService } from '../confirm.service';

@Component({
  selector: 'app-view-question',
  templateUrl: './view-question.component.html',
  styleUrls: ['./view-question.component.css']
})
export class ViewQuestionComponent implements OnInit {

  @ViewChild('comments') comments: CommentsComponent;

  private question: Question;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService,
    private confirmService: ConfirmService,
    private ls: LanguageService) {
      this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
      const id = params.get('id');
      this.handleRouteChanged(+id);
    });
  }

  ngOnInit() {
  }

  public goToEditQuestion() {
    this.router.navigate(['/editquestion', this.question.id]);
  }

  private async createNewVersion() {
    // save current question
    try {
      await this.confirmService.confirm(this.ls.label('version'), this.ls.label('versionq'));
      await this.doCreateNewVersion();
    } catch (error) {
      // thats ok.
    }
  }

  private async doCreateNewVersion() {
    try {
      this.notificationService.emitLoading();
      const newId = await this.cranDataService.versionQuestion(this.question.id);
      this.notificationService.emitDone();
      this.router.navigate(['/editquestion', newId]);
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }


  private async handleRouteChanged(id: number): Promise<void> {
    try {
      this.notificationService.emitLoading();
      this.question = await this.cranDataService.getQuestion(id);
      await this.comments.showComments(this.question.id);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

}
