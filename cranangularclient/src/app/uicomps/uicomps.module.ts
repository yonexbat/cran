import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

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
import { TagFinderComponent } from './tag-finder/tag-finder.component';
import { QuestionselectorComponent } from './questionselector/questionselector.component';
import { CommentsComponent } from './comments/comments.component';
import { VoteComponent } from './vote/vote.component';





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
    TagFinderComponent,
    QuestionselectorComponent,
    CommentsComponent,
    VoteComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
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
    TagFinderComponent,
    QuestionselectorComponent,
    CommentsComponent,
    VoteComponent,
  ]
})
export class UicompsModule { }
