import { Component, OnInit, Inject } from '@angular/core';

import {ICranDataService} from '../services/icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../services/cran-data.servicetoken';
import {NotificationService} from '../services/notification.service';
import {UserInfo} from '../model/userinfo';
import {LanguageService} from '../services/language.service';
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

  public setEn() {
    this.ls.selectLanguage(LanguageInfo.En);
  }

  public setDe() {
    this.ls.selectLanguage(LanguageInfo.De);
  }

  public isDe(): boolean {
    return this.ls.getLanguage() === LanguageInfo.De;
  }

  public isEn(): boolean {
    return this.ls.getLanguage() === LanguageInfo.En;
  }

}
