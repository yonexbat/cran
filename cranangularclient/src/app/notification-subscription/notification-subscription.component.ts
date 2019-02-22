
/*
{"endpoint":"https://fcm.googleapis.com/fcm/send/e5kXWPqXaRI:APA91bGWa1Pd_Nxnj_nMhoK_kZaHhfyXAvGo3Ty1eI6SLsXG_eCiQ7pmNC-ymJ1k3gflJWLj0GZSx2JxuWHqHJdpqSMgGRPzMtuXGJVtn_2B4c198f-EuS6Z0qvU0_pixfD4q48DXz2O","expirationTime":null,"keys":{"p256dh":"BLFquItY-oc5HtYhjLkeGmWNjmlMR8RO8wtK7XImvvroFUPnGPLxevu5yu16ADmh1uDNjZ_NPkONdCbRorGOt24","auth":"pZmugFrCbvTPHpgDCeyY8Q"}}
{"publicKey":"BBexMQInwvBFQtqWi9Px9FrhnzEmp0drOs4nkYGcopy_0TQjJ5jUKn7dBDTor_Ma5--Oq8rsseRl2m-dN9iyazU","privateKey":"TgPuP3hErzuIjTYg_bcYCkOa0GvfGNNUbeiuQpipX3o"}
{"notification": {"title": "Angular News","body": "Newsletter Available!", "vibrate": [100, 50, 100],  "data": {  "primaryKey": 1   },   "actions": [{"action": "explore", "title": "Go to the site"  }]}}


*/
import { Component, OnInit, Inject } from '@angular/core';
import { SwPush } from '@angular/service-worker';
import { NotificationService } from '../notification.service';
import { ICranDataService } from '../icrandataservice';
import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';

@Component({
  selector: 'app-notification-subscription',
  templateUrl: './notification-subscription.component.html',
  styleUrls: ['./notification-subscription.component.css']
})
export class NotificationSubscriptionComponent implements OnInit {

  readonly  VAPID_PUBLIC_KEY = 'BBexMQInwvBFQtqWi9Px9FrhnzEmp0drOs4nkYGcopy_0TQjJ5jUKn7dBDTor_Ma5--Oq8rsseRl2m-dN9iyazU';

  public subscriptionJSON = 'json v2';

  constructor(private swPush: SwPush, private notificationService: NotificationService, 
    @Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService) {
    this.swPush.messages.subscribe(this.messageSub);
    this.swPush.notificationClicks.subscribe(this.notificationClicksSub);
  }

  ngOnInit() {
  }

  public async subscribeToPushNotifications()  {
    try {
      console.log('subscribing2');
      const subscription: PushSubscription = await this.swPush.requestSubscription({
        serverPublicKey: this.VAPID_PUBLIC_KEY
      });
      this.subscriptionJSON = JSON.stringify(subscription);
      this.cranDataService.addPushRegistration(subscription);
      console.log('subscription:' + subscription);
    } catch (error) {
      console.log('error');
      console.log(error);
      this.notificationService.emitError(error);
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

}
