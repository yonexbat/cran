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
    this.comment.idQuestion = idQuestion;
    await this.getCommentsPage(0);
  }

  private async getCommentsPage(page: number): Promise<void> {

    try {
      this.notificationService.emitLoading();
      const params: GetComments = {idQuestion: this.comment.idQuestion, page : page};
      this.comments = await this.cranDataServiceService.getComments(params);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
      throw error;
    }
  }

  private async addComment(): Promise<void> {
    try {
      this.notificationService.emitLoading();
      await this.cranDataServiceService.addComment(this.comment);
      this.notificationService.emitDone();
      this.comment.commentText = '';
      await this.getCommentsPage(this.comments.currentPage);
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  private async pageSelected(page: number): Promise<void> {
    this.getCommentsPage(page);
  }

  private async deleteComment(comment: Comment): Promise<void> {
    if (!confirm('Kommentar löschen?')) {
      return;
    }
    try {
      this.notificationService.emitLoading();
      await this.cranDataServiceService.deleteComment(comment.idComment);
      this.notificationService.emitDone();
      await this.getCommentsPage(0);
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

}
