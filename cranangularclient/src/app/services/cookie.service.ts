import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CookieService {

  constructor() { }

  public getCookie(cname): string {
    const cookiestring = document.cookie;
    return this.getCookieFromString(cname, cookiestring);
  }

  public getCookieFromString(cname: string, cookievalue: string): string {
    const decodedCookie = decodeURIComponent(cookievalue);
    const ca = decodedCookie.split(';');
    for (const c of ca) {

        const match = c.match(/(\S+)=(\S+)/i); // S+ any non whitespace
        if (match && match.length === 3 && match[1] === cname) {
          const result =  match[2];
          return result;
        }
    }
    return '';
  }

}
