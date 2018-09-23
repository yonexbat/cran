import { TestBed } from '@angular/core/testing';

import { CookieService } from './cookie.service';

describe('CookieService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CookieService = TestBed.get(CookieService);
    expect(service).toBeTruthy();
  });

  it('shlould extract cookie', () => {
    const service: CookieService = TestBed.get(CookieService);
    const documentSiteCookie = 'COOKIE1=SOMEVALUE;XSRF-TOKEN=MYTOKEN';
    const cookieval = service.getCookieFromString('XSRF-TOKEN', documentSiteCookie);
    expect(cookieval).toBe('MYTOKEN');
  });

  it('no cookie', () => {
    const service: CookieService = TestBed.get(CookieService);
    const documentSiteCookie = 'COOKIE1&SOMEVALUE&XSRF-TOKEN&MYTOKEN';
    const cookieval = service.getCookieFromString('XSRF-TOKEN', documentSiteCookie);
    expect(cookieval).toBe('');
  });

});
