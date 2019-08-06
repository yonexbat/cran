import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UicompsModule } from '../uicomps/uicomps.module';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { CommentsComponent } from './comments/comments.component';
import { AskQuestionComponent } from './ask-question/ask-question.component';
import { VoteComponent } from './vote/vote.component';



@NgModule({
  declarations: [
    CommentsComponent,
    AskQuestionComponent,
    VoteComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    UicompsModule,
  ],
  exports: [
    CommentsComponent,
    AskQuestionComponent,
    VoteComponent,
  ]

})
export class CoremoduleModule { }
