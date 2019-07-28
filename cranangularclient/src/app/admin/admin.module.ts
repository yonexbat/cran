import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { UicompsModule} from '../uicomps/uicomps.module';
import { AdminRoutingModule } from './admin-routing.module';
import { ManageTextComponent } from './manage-text/manage-text.component';
import { TextlistComponent } from './textlist/textlist.component';
import { ManageTagComponent } from './manage-tag/manage-tag.component';
import { ManageTagsComponent } from './manage-tags/manage-tags.component';



@NgModule({
  declarations: [
    ManageTextComponent,
    TextlistComponent,
    ManageTagComponent,
    ManageTagComponent,
    ManageTagsComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    UicompsModule,
    FormsModule,
  ]
})
export class AdminModule { }
