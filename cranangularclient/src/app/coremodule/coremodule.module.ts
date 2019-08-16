import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UicompsModule } from '../uicomps/uicomps.module';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


import { AskQuestionComponent } from './ask-question/ask-question.component';
import { UserInfoComponent } from './user-info/user-info.component';
import { CourseListComponent } from './course-list/course-list.component';
import { MenuComponent } from './menu/menu.component';
import { HomeComponent } from './home/home.component';
import { ViewQuestionComponent } from './view-question/view-question.component';
import { CourseStarterComponent } from './course-starter/course-starter.component';
import { CourseFavoriteListComponent } from './course-favorite-list/course-favorite-list.component';
import { CourseInstanceListComponent } from './course-instance-list/course-instance-list.component';
import { QuestionListComponent } from './question-list/question-list.component';
import { QuestionListItemComponent } from './question-list-item/question-list-item.component';
import { ResultListComponent } from './result-list/result-list.component';
import { SearchQuestionsComponent } from './search-questions/search-questions.component';
import { VersionsComponent } from './versions/versions.component';


@NgModule({
  declarations: [
    AskQuestionComponent,
    UserInfoComponent,
    CourseListComponent,
    MenuComponent,
    HomeComponent,
    ViewQuestionComponent,
    CourseInstanceListComponent,
    CourseStarterComponent,
    CourseFavoriteListComponent,
    QuestionListComponent,
    QuestionListItemComponent,
    ResultListComponent,
    SearchQuestionsComponent,
    VersionsComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    UicompsModule,
  ],
  exports: [
    AskQuestionComponent,
    UserInfoComponent,
    CourseListComponent,
    MenuComponent,
    HomeComponent,
    ViewQuestionComponent,
    CourseInstanceListComponent,
    CourseStarterComponent,
    CourseFavoriteListComponent,
    QuestionListComponent,
    QuestionListItemComponent,
    ResultListComponent,
    SearchQuestionsComponent,
    VersionsComponent,
  ]

})
export class CoremoduleModule { }
