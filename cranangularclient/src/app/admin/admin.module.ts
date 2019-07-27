import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppModule } from '../app.module';
import { FormsModule } from '@angular/forms';

import { UicompsModule} from '../uicomps/uicomps.module';
import { AdminRoutingModule } from './admin-routing.module';
import { ManageTextComponent } from './manage-text/manage-text.component';
import { TextlistComponent } from './textlist/textlist.component';


@NgModule({
  declarations: [
    ManageTextComponent,
    TextlistComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    UicompsModule,
    FormsModule,
  ]
})
export class AdminModule { }
