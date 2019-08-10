import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ManageTextComponent } from './manage-text/manage-text.component';
import { TextlistComponent } from './textlist/textlist.component';
import { ManageTagsComponent } from './manage-tags/manage-tags.component';
import { ManageTagComponent } from './manage-tag/manage-tag.component';
import { SubscriptionsComponent } from './subscriptions/subscriptions.component';
import { ManageCourseComponent } from './manage-course/manage-course.component';
import { ManageQuestionComponent } from './manage-question/manage-question.component';


const routes: Routes = [
  { path: 'textlist', component: TextlistComponent},
  { path: 'managetext/:id', component: ManageTextComponent},
  { path: 'managetags', component: ManageTagsComponent},
  { path: 'managetag/:id', component: ManageTagComponent},
  { path: 'subscriptions', component: SubscriptionsComponent},
  { path: 'managecourse/:id', component: ManageCourseComponent},
  { path: 'addquestion', component:  ManageQuestionComponent},
  { path: 'editquestion/:id', component: ManageQuestionComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  providers: [
  ],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
