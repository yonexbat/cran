import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppModule } from '../app.module';

import { AdminRoutingModule } from './admin-routing.module';
import { ManageTextComponent } from './manage-text/manage-text.component';
import { TextlistComponent } from './textlist/textlist.component';
import { PagerComponent } from '../pager/pager.component';


@NgModule({
  declarations: [
    ManageTextComponent,
    TextlistComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
  ]
})
export class AdminModule { }
