import { browser, by, element } from 'protractor';

export class CranAngularClientPage {
  navigateTo() {
    return browser.get('/');
  }

  getParagraphText() {
    return element(by.css('app-root h1')).getText();
  }

  getMenu() {
    return element(by.css('app-root')).getText();
  }
}
