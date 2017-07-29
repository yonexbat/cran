import { Component, OnInit } from '@angular/core';
import { HttpModule } from '@angular/http';

import {CranDataService} from '../cran-data.service';
import {Courses} from '../model/courses';
import {Course} from '../model/course';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {

  courses: Course[] = [];

  constructor(private cranDataServiceService: CranDataService) { }

  ngOnInit() {
    this.cranDataServiceService.getCourses()
      .then(courses => this.courses = courses.courses);
  }

}
