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
import { CoreModuleRoutingModule } from './coremodule/coremodule-routing.module';


import { ConfirmService } from './services/confirm.service';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { PushNotificationService } from './services/push-notification.service';



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
  ],
  imports: [
    BrowserModule,
    RouterModule,
    HttpClientModule,
    CommonModule,
    FormsModule,
    CoreModuleRoutingModule,
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
