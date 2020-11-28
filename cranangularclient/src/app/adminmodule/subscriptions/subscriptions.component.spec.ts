import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { SubscriptionsComponent } from './subscriptions.component';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../../uicomps/uicomps.module';


import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import {NotificationService} from '../../services/notification.service';
import {ConfirmService} from '../../services/confirm.service';
import {LanguageService} from '../../services/language.service';
import {PagedResult} from '../../model/pagedresult';
import { SwPush } from '@angular/service-worker';
import {SubscriptionShort} from '../../model/subscriptionshort';




describe('SubscriptionsComponent', () => {
  let component: SubscriptionsComponent;
  let fixture: ComponentFixture<SubscriptionsComponent>;

  beforeEach(waitForAsync(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['getAllSubscriptions']);
    const pagedResult = new PagedResult<SubscriptionShort>();
    pagedResult.data = [];
    pagedResult.currentPage = 0;
    pagedResult.numpages = 4;

    for (let i = 0; i < 10; i++) {
      pagedResult.data.push({
        id: i,
        endpoint: `endpoint ${i}`,
        userId: `userid${i}`,
      });
    }
    cranDataService.getAllSubscriptions = () => {
      return new Promise<PagedResult<SubscriptionShort>>((resolve) => {
        resolve(pagedResult);
      });
    };

    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['notificationClicks']);
    const swPush = jasmine.createSpyObj('SwPush', ['messages']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, UicompsModule],
      declarations: [ SubscriptionsComponent],
      providers: [
        LanguageService,
        { provide: CRAN_SERVICE_TOKEN, useValue: cranDataService },
        { provide: NotificationService, useValue: notificationService },
        { provide: ConfirmService, useValue: confirmationService },
        { provide: SwPush, useValue: swPush},
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubscriptionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
