import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ItempagerComponent } from './itempager/itempager.component';
import { TooltipDirective } from './tooltip.directive';
import { IconComponent} from './icon/icon.component';
import { StatusmessageComponent } from './statusmessage/statusmessage.component';
import { RichTextBoxComponent } from './rich-text-box/rich-text-box.component';
import { SafeHtmlPipe } from './safe-html.pipe';
import { TagsComponent } from './tags/tags.component';
import { FileUploadComponent } from './file-upload/file-upload.component';
import { ImagelistComponent } from './imagelist/imagelist.component';
import { ConfirmComponent } from './confirm/confirm.component';
import { NotificationComponent } from './notification/notification.component';




@NgModule({
  declarations: [
    ItempagerComponent,
    TooltipDirective,
    IconComponent,
    StatusmessageComponent,
    RichTextBoxComponent,
    SafeHtmlPipe,
    TagsComponent,
    FileUploadComponent,
    ImagelistComponent,
    ConfirmComponent,
    NotificationComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
  ],
  exports: [
    ItempagerComponent,
    TooltipDirective,
    IconComponent,
    StatusmessageComponent,
    RichTextBoxComponent,
    SafeHtmlPipe,
    TagsComponent,
    FileUploadComponent,
    ImagelistComponent,
    ConfirmComponent,
    NotificationComponent,
  ]
})
export class UicompsModule { }
