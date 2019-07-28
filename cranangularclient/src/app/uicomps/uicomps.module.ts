import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ItempagerComponent } from './itempager/itempager.component';
import { TooltipDirective } from './tooltip.directive';
import { IconComponent} from './icon/icon.component';
import { StatusmessageComponent } from './statusmessage/statusmessage.component';
import { RichTextBoxComponent } from './rich-text-box/rich-text-box.component';



@NgModule({
  declarations: [
    ItempagerComponent,
    TooltipDirective,
    IconComponent,
    StatusmessageComponent,
    RichTextBoxComponent,
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ItempagerComponent,
    TooltipDirective,
    IconComponent,
    StatusmessageComponent,
    RichTextBoxComponent,
  ]
})
export class UicompsModule { }
