import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import {CourseListComponent} from '../course-list/course-list.component';
import {HomeComponent} from '../home/home.component';
import {AskQuestionComponent} from '../coremodule/ask-question/ask-question.component';
import {QuestionListComponent} from '../question-list/question-list.component';
import {ResultListComponent} from '../result-list/result-list.component';
import {CourseInstanceListComponent} from '../course-instance-list/course-instance-list.component';
import {SearchQuestionsComponent} from '../search-questions/search-questions.component';
import {ViewQuestionComponent} from '../view-question/view-question.component';
import {FileUploadComponent} from '../uicomps/file-upload/file-upload.component';
import {CourseStarterComponent} from '../course-starter/course-starter.component';
import {CourseFavoriteListComponent} from '../course-favorite-list/course-favorite-list.component';

const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'home', component:  HomeComponent},
  { path: 'list', component:  CourseListComponent},
  { path: 'coursefavorites', component:  CourseFavoriteListComponent},
  { path: 'questionlist', component:  QuestionListComponent},
  { path: 'results', component: CourseInstanceListComponent},
  { path: 'askquestion/:id', component:  AskQuestionComponent},
  { path: 'resultlist/:id', component:  ResultListComponent},
  { path: 'searchq', component: SearchQuestionsComponent},
  { path: 'viewquestion/:id', component: ViewQuestionComponent},
  { path: 'fileupload', component: FileUploadComponent}, 
  { path: 'coursestarter/:id', component: CourseStarterComponent}, 
  { path: 'admin', loadChildren: () => import('../admin/admin.module').then(mod => mod.AdminModule)},
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes),
  ],
  declarations: []
})
export class AppRoutingModule { }
