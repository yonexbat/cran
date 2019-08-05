import { Injectable } from '@angular/core';
import {Subject, Observable} from 'rxjs';
import {ConfirmRequest} from '../model/confirmrequest';

@Injectable()
export class ConfirmService {

  private subject: Subject<ConfirmRequest>;
  private promiseResolver: any;

  constructor() {
    this.subject = new Subject<ConfirmRequest>();
  }

  public confirm(title: string, text: string): Promise<any> {
    return this.showDialog(title, text, true);
  }

  public inform(title: string, text: string): Promise<any> {
    return this.showDialog(title, text, false);
  }

  private showDialog(title: string, text: string, showCancel: boolean): Promise<any> {
    const promise = new Promise<any>((resolve, reject) => {
      this.promiseResolver = {resolve, reject};
    });
    const confirmRequest: ConfirmRequest = {
      text,
      title,
      showCacelButton: showCancel,
    };
    this.subject.next(confirmRequest);
    return promise;
  }

  public ok() {
    this.promiseResolver.resolve();
  }

  public nok() {
    this.promiseResolver.reject('cancel');
  }

  public on(): Observable<ConfirmRequest> {
    return this.subject.asObservable();
  }
}
