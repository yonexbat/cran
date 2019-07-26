import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ManageTextComponent } from './manage-text/manage-text.component';
import { TextlistComponent } from './textlist/textlist.component';


const routes: Routes = [
  { path: 'textlist', component: TextlistComponent},
  {path: 'managetext/:id', component: ManageTextComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
