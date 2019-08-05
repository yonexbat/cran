import { Component, OnInit, Inject } from '@angular/core';

import { LanguageService } from '../services/language.service';
import {Text} from '../model/text';
import {LanguageInfo} from '../model/languageInfo';
import {ICranDataService} from '../services/icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../services/cran-data.servicetoken';
import {NotificationService} from '../services/notification.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  text: Text = new Text();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
              private ls: LanguageService,
              private notificationService: NotificationService, ) {

  }

  ngOnInit() {
    this.getInfo();
  }

  private async getInfo() {
    try {
      this.notificationService.emitLoading();
      this.text = await this.cranDataService.getTextDtoByKey('Home');
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  homeContent(): string {
    const languageInfo: LanguageInfo = this.ls.getLanguage();
    switch (languageInfo) {
      case LanguageInfo.De:
        return this.text.contentDe;
      case LanguageInfo.En:
        return this.text.contentEn;
      default:
        return this.text.contentDe;
    }
  }

}
