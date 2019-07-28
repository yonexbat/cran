import { Injectable } from '@angular/core';
import {Subject, Observable} from 'rxjs';

import {NotificationEvent, NotificationType} from './model/notificationEvent';


@Injectable()
export class NotificationService {


  private subject: Subject<NotificationEvent>;

  constructor() {
    this.subject = new Subject<NotificationEvent>();
  }

  public emitError(message: string): void {
    if (message === 'cancel') {
      return;
    }
    const event: NotificationEvent = {
      message,
      type: NotificationType.Error,
    };
    this.subject.next(event);
  }

  public emitLoading(): void {
    const event: NotificationEvent = {
      message: '',
      type: NotificationType.Loading,
    };
    this.subject.next(event);
  }

  public emitDone(): void {
    const event: NotificationEvent = {
      message: '',
      type: NotificationType.Done,
    };
    this.subject.next(event);
  }

  public on(): Observable<NotificationEvent> {
    return this.subject.asObservable();
  }
}
