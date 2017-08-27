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

const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'home', component:  HomeComponent},
  { path: 'list', component:  CourseListComponent},
  { path: 'questionlist', component:  QuestionListComponent},
  { path: 'results', component: CourseInstanceListComponent},
  { path: 'addquestion', component:  ManageQuestionComponent},
  { path: 'editquestion/:id', component: ManageQuestionComponent},
  { path: 'askquestion/:id', component:  AskQuestionComponent},
  { path: 'resultlist/:id', component:  ResultListComponent}
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes),
  ],
  declarations: []
})
export class AppRoutingModule { }
