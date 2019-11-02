import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { UicompsModule} from '../uicomps/uicomps.module';
import { AdminRoutingModule } from './admin-routing.module';
import { ManageTextComponent } from './manage-text/manage-text.component';
import { TextlistComponent } from './textlist/textlist.component';
import { ManageTagComponent } from './manage-tag/manage-tag.component';
import { ManageTagsComponent } from './manage-tags/manage-tags.component';
import { ManageCourseComponent } from './manage-course/manage-course.component';
import { ManageQuestionComponent } from './manage-question/manage-question.component';
import { QuestionPreviewComponent } from './question-preview/question-preview.component';
import { SubscriptionsComponent } from './subscriptions/subscriptions.component';
import { SearchQuestionsComponent } from './search-questions/search-questions.component';



@NgModule({
  declarations: [
    ManageTextComponent,
    TextlistComponent,
    ManageTagComponent,
    ManageTagComponent,
    ManageTagsComponent,
    ManageCourseComponent,
    ManageQuestionComponent,
    QuestionPreviewComponent,
    SubscriptionsComponent,
    SearchQuestionsComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    UicompsModule,
    FormsModule,
  ]
})
export class AdminModule { }
