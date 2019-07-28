import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ItempagerComponent } from './itempager/itempager.component';
import { TooltipDirective } from './tooltip.directive';
import { IconComponent} from './icon/icon.component';
import { StatusmessageComponent } from './statusmessage/statusmessage.component';
import { RichTextBoxComponent } from './rich-text-box/rich-text-box.component';
import { SafeHtmlPipe } from './safe-html.pipe';



@NgModule({
  declarations: [
    ItempagerComponent,
    TooltipDirective,
    IconComponent,
    StatusmessageComponent,
    RichTextBoxComponent,
    SafeHtmlPipe,
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
    SafeHtmlPipe,
  ]
})
export class UicompsModule { }
