import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, ActivatedRoute, ParamMap, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {PagedResult} from '../model/pagedresult';
import {Comment} from '../model/comment';
import {GetComments} from '../model/getcomments';
import {NotificationService} from '../notification.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  private comments: PagedResult<Comment>;
  private comment: Comment;

  private idQuestion: number;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private notificationService: NotificationService,
    private router: Router,
    private activeRoute: ActivatedRoute) {
      this.comment = new Comment();
      this.comment.commentText = '';
    }


  ngOnInit() {
  }

  public async showComments(idQuestion: number): Promise<void> {
    this.idQuestion = idQuestion;
    await this.getCommentsPage(0);
  }

  private async getCommentsPage(page: number): Promise<void> {

    try {
      this.notificationService.emitLoading();
      const params: GetComments = {idQuestion: this.idQuestion, page : 0};
      this.comments = await this.cranDataServiceService.getComments(params);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  private async addComment(): Promise<void> {
    try {
      this.notificationService.emitLoading();
      await this.cranDataServiceService.addComment(this.comment);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

}
