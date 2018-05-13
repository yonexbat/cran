import { Injectable } from '@angular/core';
import {Subject, Observable} from 'rxjs';
import {ConfirmRequest} from './model/confirmrequest';
import {LanguageService} from './language.service';

@Injectable()
export class ConfirmService {

  private _subject: Subject<ConfirmRequest>;
  private promiseResolver: any;

  constructor(private ls: LanguageService) {
    this._subject = new Subject<ConfirmRequest>();
  }

  public confirm(title: string, text: string): Promise<any> {
    const promise = new Promise<any>((resolve, reject) => {
      this.promiseResolver = {resolve: resolve, reject: reject};
    });
    const confirmRequest: ConfirmRequest = {
      text: text,
      title: title,
    };
    this._subject.next(confirmRequest);
    return promise;
  }

  public ok() {
    this.promiseResolver.resolve();
  }

  public nok() {
    this.promiseResolver.reject('cancel');
  }

  public on():  Observable<ConfirmRequest> {
    return this._subject.asObservable();
  }
}
