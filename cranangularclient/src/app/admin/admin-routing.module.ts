import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ManageTextComponent } from './manage-text/manage-text.component';
import { TextlistComponent } from './textlist/textlist.component';
import {ManageTagsComponent} from './manage-tags/manage-tags.component';
import {ManageTagComponent} from './manage-tag/manage-tag.component';
import {SubscriptionsComponent} from './subscriptions/subscriptions.component';


const routes: Routes = [
  { path: 'textlist', component: TextlistComponent},
  { path: 'managetext/:id', component: ManageTextComponent},
  { path: 'managetags', component: ManageTagsComponent},
  { path: 'managetag/:id', component: ManageTagComponent},
  { path: 'subscriptions', component: SubscriptionsComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  providers: [
  ],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
