import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, ActivatedRoute, ParamMap, } from '@angular/router';
import { HttpModule, } from '@angular/http';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {Question} from '../model/question';
import {LanguageService} from '../language.service';
import {ConfirmService} from '../confirm.service';
import {VersionInfo} from '../model/versionInfo';
import {VersionInfoParameters} from '../model/versionInfoParameters';
import {PagedResult} from '../model/pagedresult';
import {QuestionStatus} from '../model/questionstatus';

declare var $: any;

@Component({
  selector: 'app-versions',
  templateUrl: './versions.component.html',
  styleUrls: ['./versions.component.css']
})
export class VersionsComponent implements OnInit {

  private pagedResult: PagedResult<VersionInfo> = new PagedResult<VersionInfo>();
  private idQuestion: number;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
  private router: Router,
  private activeRoute: ActivatedRoute,
  private notificationService: NotificationService,
  private confirmService: ConfirmService,
  private ls: LanguageService) { }

  ngOnInit() {
  }

  public async showDialog(questionId: number): Promise<void> {
    this.idQuestion = questionId;
    await this.getVersions(0);
    $('#versions').modal('show');
  }

  private async pageSelected(page: number): Promise<void> {
    await this.getVersions(page);
  }

  private async getVersions(page: number): Promise<void> {
    const parameters = new VersionInfoParameters();
    parameters.page = page;
    parameters.idQuestion = this.idQuestion;

    try {
      this.notificationService.emitLoading();
      this.pagedResult = await this.cranDataService.getVersions(parameters);
      this.notificationService.emitDone();
    } catch (error) {
        this.notificationService.emitError(error);
        $('#versions').modal('hide');
    }
  }

  private async goToVersion(idQuestion: number): Promise<void> {
    $('#versions').modal('hide');
    this.router.navigate(['/viewquestion', idQuestion]);
  }

  private isCreated(item: VersionInfo) {
    return item.status === QuestionStatus.Created;
  }

  private isReleased(item: VersionInfo) {
    return item.status === QuestionStatus.Released;
  }

  private isObsolete(item: VersionInfo) {
    return item.status === QuestionStatus.Obsolete;
  }
}
