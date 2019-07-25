import { TestBed } from '@angular/core/testing';
import { PushNotificationService } from './push-notification.service';
import { SwPush } from '@angular/service-worker';
import {ICranDataService} from './icrandataservice';
import {CRAN_SERVICE_TOKEN} from './cran-data.servicetoken';
import { LanguageService } from './language.service';
import { NotificationService } from './notification.service';


describe('PushNotificationService', () => {

  const swPush  =  jasmine.createSpyObj('SwPush', ['subscribe']);
  swPush.notificationClicks = {};
  swPush.notificationClicks.subscribe = () => {};
  const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
  const notificationService = jasmine.createSpyObj('NotificationService', ['some']);


  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      PushNotificationService,
      LanguageService,
      { provide: SwPush, useValue: swPush },
      { provide: CRAN_SERVICE_TOKEN, useValue: cranDataService },
      { provide: NotificationService, useValue: notificationService },
    ]
  }));

  it('should be created', () => {
    const service: PushNotificationService = TestBed.get(PushNotificationService);
    expect(service).toBeTruthy();
  });
});
