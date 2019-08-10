import { Component, OnInit, Inject } from '@angular/core';

import {ICranDataService} from '../../services/icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../../services/cran-data.servicetoken';
import {NotificationService} from '../../services/notification.service';
import {LanguageService} from '../../services/language.service';
import {PushNotificationService} from '../../services/push-notification.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  public isAdmin = false;
  public isSubscriptionVisible = false;

  constructor(@Inject(CRAN_SERVICE_TOKEN)
    private cranDataService: ICranDataService,
              private notificationService: NotificationService,
              public ls: LanguageService,
              private pushNotificationService: PushNotificationService) { }

  ngOnInit() {
    this.setRoles();
    this.setSubscription(); // don't wait, beacuase promise may never be fullfilled.
  }

  private async setRoles() {
    try {
      const roles: string[] = await this.cranDataService.getRolesOfUser();
      this.isAdmin = roles.filter(x => x === 'admin').length > 0;
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  private async setSubscription() {
    const sub: string =  await this.pushNotificationService.checkForSubscripton();
    this.isSubscriptionVisible =  (sub === '');
  }

  public async subscribeToPushNotifications()  {
     await this.pushNotificationService.subscribeToPushNotifications();
     this.isSubscriptionVisible = false;
  }

}
