import { Injectable } from '@angular/core';
import {Subject} from 'rxjs/Subject';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';

import {NotificationEvent, NotificationType} from './model/notificationEvent';


@Injectable()
export class NotificationService {


  private _subject: Subject<NotificationEvent>;

  constructor() {
    this._subject = new Subject<NotificationEvent>();
  }

  public emitError(message: string): void {
    if (message === 'cancel') {
      return;
    }
    const event: NotificationEvent = {
      message: message,
      type: NotificationType.Error,
    };
    this._subject.next(event);
  }

  public emitLoading(): void {
    const event: NotificationEvent = {
      message: '',
      type: NotificationType.Loading,
    };
    this._subject.next(event);
  }

  public emitDone(): void {
    const event: NotificationEvent = {
      message: '',
      type: NotificationType.Done,
    };
    this._subject.next(event);
  }

  public on():  Observable<NotificationEvent> {
    return this._subject.asObservable();
  }
}
