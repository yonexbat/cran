import { Component, OnInit, Inject, } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Router, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.service';
import {Courses} from '../model/courses';
import {Course} from '../model/course';
import {CourseInstance} from '../model/courseinstance';


@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {

  courses: Course[] = [];

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router) { }

  ngOnInit() {
    this.cranDataServiceService.getCourses()
      .then(courses => {
          this.courses = courses.courses;
      });
  }

  public startCourse(course: Course) {
    this.cranDataServiceService.startCourse(course.id)
    .then((courseInstance: CourseInstance) => {
      this.router.navigate(['/askquestion', courseInstance.idCourseInstanceQuestion]);
    });
  }

}
