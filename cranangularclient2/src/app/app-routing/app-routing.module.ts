import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import {CourseListComponent} from '../course-list/course-list.component';
import {HomeComponent} from '../home/home.component';
import {ManageQuestionComponent} from '../manage-question/manage-question.component';
import {AskQuestionComponent} from '../ask-question/ask-question.component';
import {QuestionListComponent} from '../question-list/question-list.component';
import {ResultListComponent} from '../result-list/result-list.component';
import {CourseInstanceListComponent} from '../course-instance-list/course-instance-list.component';
import {SearchQuestionsComponent} from '../search-questions/search-questions.component';
import {ViewQuestionComponent} from '../view-question/view-question.component';
import {FileUploadComponent} from '../file-upload/file-upload.component';
import {ManageTagsComponent} from '../manage-tags/manage-tags.component';
import {ManageTagComponent} from '../manage-tag/manage-tag.component';
import {ManageCourseComponent} from '../manage-course/manage-course.component';
import {CourseStarterComponent} from '../course-starter/course-starter.component';
import {TextlistComponent} from '../textlist/textlist.component';
import {ManageTextComponent} from '../manage-text/manage-text.component';
import {SubscriptionsComponent} from '../subscriptions/subscriptions.component';
import {CourseFavoriteListComponent} from '../course-favorite-list/course-favorite-list.component';

const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'home', component:  HomeComponent},
  { path: 'list', component:  CourseListComponent},
  { path: 'coursefavorites', component:  CourseFavoriteListComponent},
  { path: 'questionlist', component:  QuestionListComponent},
  { path: 'results', component: CourseInstanceListComponent},
  { path: 'addquestion', component:  ManageQuestionComponent},
  { path: 'editquestion/:id', component: ManageQuestionComponent},
  { path: 'askquestion/:id', component:  AskQuestionComponent},
  { path: 'resultlist/:id', component:  ResultListComponent},
  { path: 'searchq', component: SearchQuestionsComponent},
  { path: 'viewquestion/:id', component: ViewQuestionComponent},
  { path: 'fileupload', component: FileUploadComponent},
  { path: 'managetags', component: ManageTagsComponent},
  { path: 'managetag/:id', component: ManageTagComponent},
  { path: 'managecourse/:id', component: ManageCourseComponent},
  { path: 'coursestarter/:id', component: CourseStarterComponent},
  { path: 'textlist', component: TextlistComponent},
  { path: 'managetext/:id', component: ManageTextComponent},
  { path: 'subscriptions', component: SubscriptionsComponent}
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes),
  ],
  declarations: []
})
export class AppRoutingModule { }
