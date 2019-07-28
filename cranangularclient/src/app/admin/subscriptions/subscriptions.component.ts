
import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { SwPush } from '@angular/service-worker';

import { NotificationService } from '../../notification.service';
import { LanguageService } from '../../language.service';
import { ICranDataService } from '../../icrandataservice';
import { CRAN_SERVICE_TOKEN } from '../../cran-data.servicetoken';
import { SubscriptionShort } from '../../model/subscriptionshort';
import { Notification } from '../../model/notification';

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
  public action: string;
  public actionTitle: string;
  public actionUrl: string;


  constructor(private swPush: SwPush, private notificationService: NotificationService,
              @Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
              private ls: LanguageService) {
    if (this.swPush.isEnabled) {
      this.notificationEnabled = true;
    }
    this.getUsers();
  }

  ngOnInit() {
  }

  private async getUsers() {
    const subscriptions = await this.cranDataService.getAllSubscriptions(0);
    this.users = subscriptions.data;
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
    notification.action = this.action;
    notification.actionTitle = this.actionTitle;
    notification.actionUrl = this.actionUrl;
    return notification;
  }


}
