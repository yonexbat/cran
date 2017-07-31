import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import {CourseListComponent} from '../course-list/course-list.component';
import {HomeComponent} from '../home/home.component';
import {ManageQuestionComponent} from '../manage-question/manage-question.component';

const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'home', component:  HomeComponent},
  { path: 'list', component:  CourseListComponent},
  { path: 'addquestion', component:  ManageQuestionComponent},
  { path: 'editquestion/:id', component: ManageQuestionComponent},
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes),
  ],
  declarations: []
})
export class AppRoutingModule { }
