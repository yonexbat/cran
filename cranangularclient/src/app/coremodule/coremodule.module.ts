import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UicompsModule } from '../uicomps/uicomps.module';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { CommentsComponent } from './comments/comments.component';
import { AskQuestionComponent } from './ask-question/ask-question.component';
import { VoteComponent } from './vote/vote.component';
import { UserInfoComponent } from './user-info/user-info.component';
import { CourseListComponent } from './course-list/course-list.component';
import { MenuComponent } from './menu/menu.component';
import { HomeComponent } from './home/home.component';



@NgModule({
  declarations: [
    CommentsComponent,
    AskQuestionComponent,
    VoteComponent,
    UserInfoComponent,
    CourseListComponent,
    MenuComponent,
    HomeComponent,
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
    UserInfoComponent,
    CourseListComponent,
    MenuComponent,
    HomeComponent,
  ]

})
export class CoremoduleModule { }
