import { Injectable, Inject } from '@angular/core';
import { SwPush } from '@angular/service-worker';
import {ICranDataService} from './icrandataservice';
import {CRAN_SERVICE_TOKEN} from './cran-data.servicetoken';
import { NotificationService } from './notification.service';
import { LanguageService } from './language.service';


@Injectable()
export class PushNotificationService {

  readonly  VAPID_PUBLIC_KEY = 'BBexMQInwvBFQtqWi9Px9FrhnzEmp0drOs4nkYGcopy_0TQjJ5jUKn7dBDTor_Ma5--Oq8rsseRl2m-dN9iyazU';

  constructor(
    private swPush: SwPush,
    @Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private notificationService: NotificationService,
    private ls: LanguageService) {

      swPush.notificationClicks.subscribe(arg => {
        console.log('PushNotificationService, notificationClicks');
      });

  }



  public async checkForSubscripton(): Promise<string> {
    console.log('checking for subscription');
    let subscriptionJSON = '';
    try {
      const serviceWorker: ServiceWorkerRegistration = await window.navigator.serviceWorker.ready;
      console.log('got serviceworker');
      const subscription: PushSubscription = await serviceWorker.pushManager.getSubscription();
      if (subscription != null ) {
        subscriptionJSON = JSON.stringify(subscription);
        console.log(`got subscription ' + ${subscriptionJSON}`);
      } else {
        console.log('subscription not here');
      }
    } catch (error) {
      console.log(error);
    }
    return subscriptionJSON;
  }

  public async subscribeToPushNotifications(): Promise<any>  {

    console.log('subscribe');

    if (!this.swPush.isEnabled) {
      this.notificationService.emitError(this.ls.label('notificationsnotpossible'));
      return;
    }

    try {
      const subscription: PushSubscription = await this.swPush.requestSubscription({
        serverPublicKey: this.VAPID_PUBLIC_KEY
      });
      try {
        this.notificationService.emitLoading();
        this.cranDataService.addPushRegistration(subscription);
        this.notificationService.emitDone();
      } catch (error) {
        console.log(`error ${error}`);
        this.notificationService.emitError(error);
      }
    } catch (error) {
      console.log(`error ${error}`);
      this.notificationService.emitError(error);
    }
  }

}
