import { Component, OnInit, Inject } from '@angular/core';
import { SwPush } from '@angular/service-worker';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {LanguageService} from '../language.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  public isAdmin = false;
  public subscriptionJSON = '-';
  readonly  VAPID_PUBLIC_KEY = 'BBexMQInwvBFQtqWi9Px9FrhnzEmp0drOs4nkYGcopy_0TQjJ5jUKn7dBDTor_Ma5--Oq8rsseRl2m-dN9iyazU';

  constructor(@Inject(CRAN_SERVICE_TOKEN)
    private cranDataService: ICranDataService,
    private notificationService: NotificationService,
    public ls: LanguageService,
    private swPush: SwPush) { }

  ngOnInit() {
    this.setRoles();
    this.checkForSubscripton();
  }

  private async setRoles() {
    try {
      const roles: string[] = await this.cranDataService.getRolesOfUser();
      this.isAdmin = roles.filter(x => x === 'admin').length > 0;
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  private async checkForSubscripton() {
    try {
      const serviceWorker: ServiceWorkerRegistration = await window.navigator.serviceWorker.ready;
      const subscription: PushSubscription = await serviceWorker.pushManager.getSubscription();
      if (subscription != null ) {
        this.subscriptionJSON = JSON.stringify(subscription);
        console.log(`got it ' + ${this.subscriptionJSON}`);
      } else {
        console.log('subscription not here');
      }
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  public async subscribeToPushNotifications()  {
    if (!this.swPush.isEnabled) {
      this.notificationService.emitError(this.ls.label('notificationsnotpossible'));
      return;
    }
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
      this.notificationService.emitError(error);
    }
  }

}
