import { Component, OnInit, Inject } from '@angular/core';
import { HttpModule } from '@angular/http';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.service';
import {Courses} from '../model/courses';
import {Course} from '../model/course';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {

  courses: Course[] = [];

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService) { }

  ngOnInit() {
    this.cranDataServiceService.getCourses()
      .then(courses => {
          this.courses = courses.courses;
      });
  }

  public startCourse(course: Course) {
    debugger;
    this.cranDataServiceService.startCourse(course.id)
    .then(() => {

    });
  }

}
