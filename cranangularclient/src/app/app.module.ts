import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { NotificationService } from './notification.service';
import { MenuComponent } from './menu/menu.component';
import { CourseListComponent } from './course-list/course-list.component';
import { CRAN_SERVICE_TOKEN } from './cran-data.servicetoken';
import { CranDataService  } from './cran-data.service';
import { CranDataServiceMock } from './cran-data-mock.service';
import { LanguageService } from './language.service';
import { AppRoutingModule } from './app-routing/app-routing.module';
import { HomeComponent } from './home/home.component';
import { ManageQuestionComponent } from './manage-question/manage-question.component';
import { TagFinderComponent } from './tag-finder/tag-finder.component';
import { StatusMessageComponent } from './status-message/status-message.component';
import { AskQuestionComponent } from './ask-question/ask-question.component';
import { QuestionListComponent } from './question-list/question-list.component';
import { ResultListComponent } from './result-list/result-list.component';
import { RichTextBoxComponent } from './rich-text-box/rich-text-box.component';
import { SafeHtmlPipe } from './save-html.pipe';
import { QuestionPreviewComponent } from './question-preview/question-preview.component';
import { CourseInstanceListComponent } from './course-instance-list/course-instance-list.component';
import { PagerComponent } from './pager/pager.component';
import { SearchQuestionsComponent } from './search-questions/search-questions.component';
import { NotificationComponent } from './notification/notification.component';
import { TagsComponent } from './tags/tags.component';
import { ViewQuestionComponent } from './view-question/view-question.component';
import { CommentsComponent } from './comments/comments.component';
import { VoteComponent } from './vote/vote.component';
import { FileUploadComponent } from './file-upload/file-upload.component';
import { ImagelistComponent } from './imagelist/imagelist.component';
import { UserInfoComponent } from './user-info/user-info.component';
import { ManageTagsComponent } from './manage-tags/manage-tags.component';
import { ManageTagComponent } from './manage-tag/manage-tag.component';
import { ManageCourseComponent } from './manage-course/manage-course.component';
import { ConfirmComponent } from './confirm/confirm.component';
import { ConfirmService } from './confirm.service';
import { QuestionListItemComponent } from './question-list-item/question-list-item.component';
import { CourseStarterComponent } from './course-starter/course-starter.component';
import { IconComponent } from './icon/icon.component';
import { TooltipDirective } from './tooltip.directive';
import { TextlistComponent } from './textlist/textlist.component';
import { ManageTextComponent } from './manage-text/manage-text.component';
import { VersionsComponent } from './versions/versions.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';

function isDevelopment() {
  return window.location && window.location.port && window.location.port === '4200';
}

let cranDataService: any = CranDataService;
if (isDevelopment()) {
    cranDataService = CranDataServiceMock;
}

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    CourseListComponent,
    HomeComponent,
    ManageQuestionComponent,
    TagFinderComponent,
    StatusMessageComponent,
    AskQuestionComponent,
    QuestionListComponent,
    ResultListComponent,
    RichTextBoxComponent,
    SafeHtmlPipe,
    QuestionPreviewComponent,
    CourseInstanceListComponent,
    PagerComponent,
    SearchQuestionsComponent,
    NotificationComponent,
    TagsComponent,
    ViewQuestionComponent,
    CommentsComponent,
    VoteComponent,
    FileUploadComponent,
    ImagelistComponent,
    UserInfoComponent,
    ManageTagsComponent,
    ManageTagComponent,
    ManageCourseComponent,
    ConfirmComponent,
    QuestionListItemComponent,
    CourseStarterComponent,
    IconComponent,
    TooltipDirective,
    TextlistComponent,
    ManageTextComponent,
    VersionsComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
    HttpModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
  ],
  providers: [
    { provide: CRAN_SERVICE_TOKEN, useClass: cranDataService },
    HttpModule,
    NotificationService,
    LanguageService,
    ConfirmService,
    DatePipe,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
