import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { RouterModule, Routes } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { DebugElement, Component, Input, TemplateRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

import { TestingModule } from './testing/testing.module';
import { CoreModuleRoutingModule } from './coremodule/coremodule-routing.module';
import { UicompsModule } from './uicomps/uicomps.module';
import { CoremoduleModule } from './coremodule/coremodule.module';

import { AppComponent } from './app.component';
import { PushNotificationService } from './services/push-notification.service';
import { ConfirmService } from './services/confirm.service';
import { NotificationService } from './services/notification.service';
import { CranDataServiceMock } from './services/cran-data-mock.service';
import { CRAN_SERVICE_TOKEN } from './services/cran-data.servicetoken';

describe('AppComponent', () => {

  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let de: DebugElement;
  let el: HTMLElement;

  beforeEach(async(() => {

    const pushNotificationService = jasmine.createSpyObj('PushNotificationService', ['checkForSubscripton']);
    const cranDataService = new CranDataServiceMock();

    TestBed.configureTestingModule({
      declarations: [
        AppComponent
      ],
      imports: [
         RouterModule,
         CoreModuleRoutingModule,
         RouterTestingModule,
         FormsModule,
         TestingModule,
         UicompsModule,
         CoremoduleModule,
      ],
      providers: [
        { provide: PushNotificationService, useValue: pushNotificationService },
        { provide: CRAN_SERVICE_TOKEN, useValue: cranDataService },
        ConfirmService,
        NotificationService,
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    de = fixture.debugElement.query(By.css('div'));
    el = de.nativeElement;
  });

  it('should create the app', async(() => {
    expect(component).toBeTruthy();
  }));

  fit('should show elements', async(() => {
    const appmMenu = fixture.debugElement.query(By.css('app-menu'));
    expect(appmMenu.nativeElement).toBeTruthy();

    const confirm = fixture.debugElement.query(By.css('app-confirm'));
    expect(confirm.nativeElement).toBeTruthy();

    const notificationComp = fixture.debugElement.query(By.css('app-notification'));
    expect(notificationComp.nativeElement).toBeTruthy();

  }));
});
