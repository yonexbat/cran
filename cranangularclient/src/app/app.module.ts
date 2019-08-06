import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { DatePipe, CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UicompsModule } from './uicomps/uicomps.module';
import { CoremoduleModule } from './coremodule/coremodule.module';

import { AppComponent } from './app.component';
import { NotificationService } from './services/notification.service';
import { CRAN_SERVICE_TOKEN } from './services/cran-data.servicetoken';
import { CranDataService  } from './services/cran-data.service';
import { CranDataServiceMock } from './services/cran-data-mock.service';
import { LanguageService } from './services/language.service';
import { AppRoutingModule } from './app-routing/app-routing.module';
import { QuestionListComponent } from './question-list/question-list.component';
import { ResultListComponent } from './result-list/result-list.component';
import { CourseInstanceListComponent } from './course-instance-list/course-instance-list.component';
import { SearchQuestionsComponent } from './search-questions/search-questions.component';
import { ViewQuestionComponent } from './view-question/view-question.component';
import { ConfirmService } from './services/confirm.service';
import { QuestionListItemComponent } from './question-list-item/question-list-item.component';
import { CourseStarterComponent } from './course-starter/course-starter.component';
import { VersionsComponent } from './versions/versions.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { PushNotificationService } from './services/push-notification.service';
import { CourseFavoriteListComponent } from './course-favorite-list/course-favorite-list.component';


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
    QuestionListComponent,
    ResultListComponent,
    CourseInstanceListComponent,
    SearchQuestionsComponent,
    ViewQuestionComponent,
    QuestionListItemComponent,
    CourseStarterComponent,
    VersionsComponent,
    CourseFavoriteListComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
    HttpClientModule,
    CommonModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ServiceWorkerModule.register('pwacranium.js', { enabled: environment.production }),
    UicompsModule,
    CoremoduleModule,
  ],
  providers: [
    { provide: CRAN_SERVICE_TOKEN, useClass: cranDataService },
    NotificationService,
    LanguageService,
    ConfirmService,
    DatePipe,
    PushNotificationService,
  ],
  bootstrap: [AppComponent],
  exports: [
  ],
})
export class AppModule { }
