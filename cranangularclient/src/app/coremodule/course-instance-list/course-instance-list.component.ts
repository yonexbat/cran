import { Component, OnInit, Inject,  } from '@angular/core';
import { Router, } from '@angular/router';
import { DatePipe,  } from '@angular/common';

import {ICranDataService} from '../../services/icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../../services/cran-data.servicetoken';
import {CourseInstanceListEntry} from '../../model/courseinstancelistentry';
import {NotificationService} from '../../services/notification.service';
import {LanguageService} from '../../services/language.service';
import {ConfirmService} from '../../services/confirm.service';
import {PagedResult} from '../../model/pagedresult';

@Component({
  selector: 'app-course-instance-list',
  templateUrl: './course-instance-list.component.html',
  styleUrls: ['./course-instance-list.component.css']
})
export class CourseInstanceListComponent implements OnInit {

  public pagedResult: PagedResult<CourseInstanceListEntry> = new PagedResult<CourseInstanceListEntry>();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
              private router: Router,
              private notificationService: NotificationService,
              public ls: LanguageService,
              private confirmService: ConfirmService,
              private datePipe: DatePipe) { }

  ngOnInit() {
    this.loadInstances(0);
  }

  private async loadInstances(page: number): Promise<void> {
    try {
      this.notificationService.emitLoading();
      this.pagedResult = await this.cranDataServiceService.getMyCourseInstances(page);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  public async deleteCourseInstance(instance: CourseInstanceListEntry): Promise<void>  {
    try {
      const timespamp = this.datePipe.transform(instance.insertDate, 'dd.MM.yyyy HH:mm');
      await this.confirmService.confirm(this.ls.label('deletecourseinstance'),
        this.ls.label('deletecourseinstanceq', `${instance.title} ${timespamp}`));
      try {
        this.notificationService.emitLoading();
        await this.cranDataServiceService.deleteCourseInstance(instance.idCourseInstance);
        this.notificationService.emitDone();
        await this.loadInstances(0);
      } catch (error) {
        this.notificationService.emitError(error);
      }
    } catch (error) {
      // that is ok, cancel from user.
    }
  }

  public pageSelected(pageNumber: number) {
    this.loadInstances(pageNumber);
  }

  public goToCourseInstance(instance: CourseInstanceListEntry) {
    this.router.navigate(['/resultlist', instance.idCourseInstance]);
  }
}
