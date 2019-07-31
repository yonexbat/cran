import { Component, OnInit, Inject, } from '@angular/core';

import {NotificationService} from '../notification.service';
import {NotificationEvent, NotificationType} from '../model/notificationEvent';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {

  public message  = '';
  public isError = false;
  public isInfo = false;
  public isWarn = false;
  public isLoading = false;

  constructor(private notificationService: NotificationService) { }

  ngOnInit() {
    this.notificationService.on().subscribe((res: NotificationEvent) => this.notificationReceived(res));
  }

  public close() {
    this.isError = false;
    this.isInfo = false;
    this.isWarn = false;
    this.isLoading = false;
  }

  private notificationReceived(event: NotificationEvent) {

    if (typeof this.message === 'string') {
      this.message = event.message;
    } else if (event.message) {
      this.message = event.message.toString();
    } else {
      this.message = 'Unbekannter Fehler';
    }

    this.close();
    switch (event.type) {
      case NotificationType.Error:
        this.isError = true;
        break;
      case NotificationType.Loading:
        this.isLoading = true;
        break;
      case NotificationType.Done:
        break;
      default:
        throw new RangeError(`Eventtype ${event.type} not supported`);
    }
  }

}
