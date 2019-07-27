import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TestcompComponent } from './testcomp/testcomp.component';
import { ItempagerComponent } from './itempager/itempager.component';
import { TooltipDirective } from './tooltip.directive';
import { IconComponent} from './icon/icon.component';
import { StatusmessageComponent } from './statusmessage/statusmessage.component';



@NgModule({
  declarations: [
    TestcompComponent,
    ItempagerComponent,
    TooltipDirective,
    IconComponent,
    StatusmessageComponent,
  ],
  imports: [
    CommonModule
  ],
  exports: [
    TestcompComponent,
    ItempagerComponent,
    TooltipDirective,
    IconComponent,
    StatusmessageComponent,
  ]
})
export class UicompsModule { }
