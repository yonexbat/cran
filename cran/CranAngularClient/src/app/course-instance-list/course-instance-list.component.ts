import { Component, OnInit, Inject, } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Router, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.service';
import {CourseInstanceListEntry} from '../model/courseinstancelistentry';


@Component({
  selector: 'app-course-instance-list',
  templateUrl: './course-instance-list.component.html',
  styleUrls: ['./course-instance-list.component.css']
})
export class CourseInstanceListComponent implements OnInit {

  courseInstances: CourseInstanceListEntry[] = [];

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
  private router: Router) { }

  ngOnInit() {
    this.loadInstances();
  }

  private loadInstances() {
    this.cranDataServiceService.getMyCourseInstances()
    .then((instances: CourseInstanceListEntry[]) => this.courseInstances = instances);
  }

  public deleteCourseInstance(instance: CourseInstanceListEntry) {
    if (confirm('Resultat löschen?')) {
      this.cranDataServiceService.deleteCourseInstance(instance.idCourseInstance)
        .then((nores: any) => this.loadInstances());
    }
  }

  public goToCourseInstance(instance: CourseInstanceListEntry) {
    this.router.navigate(['/resultlist', instance.idCourseInstance]);
  }
}
