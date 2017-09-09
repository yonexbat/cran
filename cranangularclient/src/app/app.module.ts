import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { NotificationService } from './notification.service';
import { MenuComponent } from './menu/menu.component';
import { CourseListComponent } from './course-list/course-list.component';
import { ICranDataService } from './icrandataservice';
import { CRAN_SERVICE_TOKEN } from './cran-data.servicetoken';
import { CranDataService  } from './cran-data.service';
import { CranDataServiceMock } from './cran-data-mock.service';
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
  ],
  imports: [
    BrowserModule,
    RouterModule,
    HttpModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
  ],
  providers: [
    { provide: CRAN_SERVICE_TOKEN, useClass: cranDataService },
    HttpModule,
    NotificationService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
