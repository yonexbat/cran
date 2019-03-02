
import { Component, OnInit, Inject, ViewChild } from '@angular/core';

import { SwPush } from '@angular/service-worker';
import { NotificationService } from '../notification.service';
import { ICranDataService } from '../icrandataservice';
import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import { SubscriptionShort } from '../model/subscriptionshort';
import { Notification } from '../model/notification';

@Component({
  selector: 'app-subscriptions',
  templateUrl: './subscriptions.component.html',
  styleUrls: ['./subscriptions.component.css']
})
export class SubscriptionsComponent implements OnInit {

  readonly  VAPID_PUBLIC_KEY = 'BBexMQInwvBFQtqWi9Px9FrhnzEmp0drOs4nkYGcopy_0TQjJ5jUKn7dBDTor_Ma5--Oq8rsseRl2m-dN9iyazU';

  public subscriptionJSON = '-';
  public notificationEnabled = false;
  public selectedSubscription: number;
  public users: SubscriptionShort[];
  public title: string;
  public text: string;


  constructor(private swPush: SwPush, private notificationService: NotificationService,
    @Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService) {
    if (this.swPush.isEnabled) {
      this.notificationEnabled = true;
      this.swPush.messages.subscribe(this.messageSub);
      this.swPush.notificationClicks.subscribe(this.notificationClicksSub);
      this.checkForSubscripton();
    }
    this.getUsers();
  }

  ngOnInit() {
  }

  private async getUsers() {
    const subscriptions = await this.cranDataService.getAllSubscriptions(0);
    this.users = subscriptions.data;
  }

  private async checkForSubscripton() {
    const serviceWorker: ServiceWorkerRegistration = await window.navigator.serviceWorker.ready;
    const subscription: PushSubscription = await serviceWorker.pushManager.getSubscription();
    if (subscription != null ) {
      this.subscriptionJSON = JSON.stringify(subscription);
    }
  }

  public async subscribeToPushNotifications()  {
    try {
      const subscription: PushSubscription = await this.swPush.requestSubscription({
        serverPublicKey: this.VAPID_PUBLIC_KEY
      });
      this.subscriptionJSON = JSON.stringify(subscription);
      try {
        this.notificationService.emitLoading();
        this.cranDataService.addPushRegistration(subscription);
        this.notificationService.emitDone();
      } catch (error) {
        this.notificationService.emitError(error);
      }
    } catch (error) {
    }
  }

  public messageSub(input: any) {
    console.log('message received');
    console.log(JSON.stringify(input));
  }

  public notificationClicksSub(input: any) {
    console.log('notificationClicksSub');
    console.log(JSON.stringify(input));
  }

  public async sendNotification() {
    try {
      this.notificationService.emitLoading();
      const notification = this.getNotificationObject();
      await this.cranDataService.sendNotificationToUser(notification);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  private getNotificationObject(): Notification {
    const notification = new Notification();
    notification.subscriptionId = this.selectedSubscription;
    notification.text = this.text;
    notification.title = this.title;
    return notification;
  }


}
