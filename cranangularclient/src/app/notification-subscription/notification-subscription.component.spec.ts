import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationSubscriptionComponent } from './notification-subscription.component';

describe('NotificationSubscriptionComponent', () => {
  let component: NotificationSubscriptionComponent;
  let fixture: ComponentFixture<NotificationSubscriptionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotificationSubscriptionComponent ]
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
