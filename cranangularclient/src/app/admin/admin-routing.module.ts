import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ManageTextComponent } from './manage-text/manage-text.component';
import { TextlistComponent } from './textlist/textlist.component';
import {ManageTagsComponent} from '../admin/manage-tags/manage-tags.component';
import {ManageTagComponent} from '../admin/manage-tag/manage-tag.component';


const routes: Routes = [
  { path: 'textlist', component: TextlistComponent},
  { path: 'managetext/:id', component: ManageTextComponent},
  { path: 'managetags', component: ManageTagsComponent},
  { path: 'managetag/:id', component: ManageTagComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  providers: [
  ],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
