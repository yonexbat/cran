import { Component, OnInit, Inject } from '@angular/core';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {UserInfo} from '../model/userinfo';
import {LanguageService} from '../language.service';
import {LanguageInfo} from '../model/languageInfo';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent implements OnInit {

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private notificationService: NotificationService,
    private ls: LanguageService) { }

  userInfo: UserInfo;

  ngOnInit() {
    this.getUserInfo();
  }

  private async getUserInfo() {
    try {
      this.userInfo = await this.cranDataService.getUserInfo();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  private setEn() {
    this.ls.selectLanguage(LanguageInfo.En);
  }

  private setDe() {
    this.ls.selectLanguage(LanguageInfo.De);
  }

  private isDe(): boolean {
    return this.ls.getLanguage() === LanguageInfo.De;
  }

  private isEn(): boolean {
    return this.ls.getLanguage() === LanguageInfo.En;
  }

}
