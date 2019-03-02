import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { NotificationSubscriptionComponent } from './notification-subscription.component';
import { RouterTestingModule } from '@angular/router/testing';


import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {PagedResult} from '../model/pagedresult';
import {TooltipDirective} from '../tooltip.directive';
import {IconComponent} from '../icon/icon.component';
import {StatusMessageComponent} from '../status-message/status-message.component';
import { SwPush } from '@angular/service-worker';
import {SubscriptionShort} from '../model/subscriptionshort';



describe('NotificationSubscriptionComponent', () => {
  let component: NotificationSubscriptionComponent;
  let fixture: ComponentFixture<NotificationSubscriptionComponent>;

  beforeEach(async(() => {

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
      imports: [RouterTestingModule, FormsModule],
      declarations: [ NotificationSubscriptionComponent, StatusMessageComponent, IconComponent ],
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
    fixture = TestBed.createComponent(NotificationSubscriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
