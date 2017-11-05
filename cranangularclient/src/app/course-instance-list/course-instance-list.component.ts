import { Component, OnInit, Inject, } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Router, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {CourseInstanceListEntry} from '../model/courseinstancelistentry';
import {NotificationService} from '../notification.service';
import {LanguageService} from '../language.service';
import {ConfirmService} from '../confirm.service';

@Component({
  selector: 'app-course-instance-list',
  templateUrl: './course-instance-list.component.html',
  styleUrls: ['./course-instance-list.component.css']
})
export class CourseInstanceListComponent implements OnInit {

  courseInstances: CourseInstanceListEntry[];

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router,
    private notificationService: NotificationService,
    private ls: LanguageService,
    private confirmService: ConfirmService) { }

  ngOnInit() {
    this.loadInstances();
  }

  private async loadInstances(): Promise<void> {
    try {
      this.notificationService.emitLoading();
      this.courseInstances = await this.cranDataServiceService.getMyCourseInstances();
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  public async deleteCourseInstance(instance: CourseInstanceListEntry): Promise<void>  {
    try {
      await this.confirmService.confirm(this.ls.label('deletecourseinstance'), this.ls.label('deletecourseinstanceq'));
      try {
        this.notificationService.emitLoading();
        await this.cranDataServiceService.deleteCourseInstance(instance.idCourseInstance);
        this.notificationService.emitDone();
        await this.loadInstances();
      } catch (error) {
        this.notificationService.emitError(error);
      }
    } catch (error) {
      // that is ok, cancel from user.
    }
  }

  public goToCourseInstance(instance: CourseInstanceListEntry) {
    this.router.navigate(['/resultlist', instance.idCourseInstance]);
  }
}
