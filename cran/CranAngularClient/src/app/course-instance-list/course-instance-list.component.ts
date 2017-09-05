import { Component, OnInit, Inject, } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Router, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {CourseInstanceListEntry} from '../model/courseinstancelistentry';
import {NotificationService} from '../notification.service';


@Component({
  selector: 'app-course-instance-list',
  templateUrl: './course-instance-list.component.html',
  styleUrls: ['./course-instance-list.component.css']
})
export class CourseInstanceListComponent implements OnInit {

  courseInstances: CourseInstanceListEntry[] = [];

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router,
    private notificationService: NotificationService) { }

  ngOnInit() {
    this.loadInstances();
  }

  private async loadInstances(): Promise<void> {
    try {
      this.courseInstances = await this.cranDataServiceService.getMyCourseInstances();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  public async deleteCourseInstance(instance: CourseInstanceListEntry): Promise<void>  {
    if (confirm('Resultat löschen?')) {
      try {
        await this.cranDataServiceService.deleteCourseInstance(instance.idCourseInstance);
      } catch (error) {
        this.notificationService.emitError(error);
      }
      await this.loadInstances();
    }
  }

  public goToCourseInstance(instance: CourseInstanceListEntry) {
    this.router.navigate(['/resultlist', instance.idCourseInstance]);
  }
}
