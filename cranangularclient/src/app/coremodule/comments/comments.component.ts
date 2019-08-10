import { Component, OnInit, Inject, ViewChild, Input, } from '@angular/core';

import {ICranDataService} from '../../services/icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../../services/cran-data.servicetoken';
import {PagedResult} from '../../model/pagedresult';
import {Comment} from '../../model/comment';
import {GetComments} from '../../model/getcomments';
import {NotificationService} from '../../services/notification.service';
import {LanguageService} from '../../services/language.service';
import {ConfirmService} from '../../services/confirm.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  // tslint:disable-next-line:variable-name
  private _questionid: number;
  @Input()
  set questionId(id: number) {
    if (this._questionid !== id) {
      this.showComments(id);
    }
    this._questionid = id;
  }
  get questionId(): number {
    return this._questionid;
  }

  public comments: PagedResult<Comment>;
  private comment: Comment;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
              private notificationService: NotificationService,
              public ls: LanguageService,
              private confirmService: ConfirmService) {

    this.comment = new Comment();
    this.comment.commentText = '';
  }


  ngOnInit() {
  }

  public async showComments(idQuestion: number): Promise<void> {
    this.comment.idQuestion = idQuestion;
    if (idQuestion > 0) {
      await this.getCommentsPage(0);
    } else {
      this.comments = null;
    }
  }

  private async getCommentsPage(page: number): Promise<void> {

    try {
      this.notificationService.emitLoading();
      const params: GetComments = {idQuestion: this.comment.idQuestion, page};
      this.comments = await this.cranDataServiceService.getComments(params);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
      throw error;
    }
  }

  public async addComment(): Promise<void> {
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

  public async pageSelected(page: number): Promise<void> {
    this.getCommentsPage(page);
  }

  public async deleteComment(comment: Comment): Promise<void> {
    try {
      await this.confirmService.confirm(this.ls.label('deletecomment'), this.ls.label('deletecommentq'));
      this.notificationService.emitLoading();
      await this.cranDataServiceService.deleteComment(comment.idComment);
      this.notificationService.emitDone();
      await this.getCommentsPage(0);
    } catch (error) {
      if (error === 'cancel') {
        // thats ok.
      } else {
        this.notificationService.emitError(error);
      }
    }
  }

}
