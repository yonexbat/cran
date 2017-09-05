import { Injectable } from '@angular/core';
import {Subject} from 'rxjs/Subject';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';

import {NotificationEvent} from './model/notificationEvent';


@Injectable()
export class NotificationService {

  private _subject: Subject<NotificationEvent>;

  constructor() {
    this._subject = new Subject<NotificationEvent>();
  }

  public emitError(message: string): void {
    console.log(message);
    const event: NotificationEvent = {
      message: message,
      type: 'error',
    };
    this._subject.next(event);
  }

  public on():  Observable<NotificationEvent> {
    return this._subject.asObservable();
  }
}
