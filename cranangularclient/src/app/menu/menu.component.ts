import { Component, OnInit, Inject } from '@angular/core';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {LanguageService} from '../language.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  private isAdmin = false;

  constructor(@Inject(CRAN_SERVICE_TOKEN)
    private cranDataService: ICranDataService,
    private notificationService: NotificationService,
    private ls: LanguageService) { }

  ngOnInit() {
    this.setRoles();
  }

  private async setRoles() {
    try {
      const roles: string[] = await this.cranDataService.getRolesOfUser();
      this.isAdmin = roles.filter(x => x === 'admin').length > 0;
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

}
