import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Router, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {Courses} from '../model/courses';
import {Course} from '../model/course';
import {CourseInstance} from '../model/courseinstance';
import {StatusMessageComponent} from '../status-message/status-message.component';


@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {

  courses: Course[] = [];

  @ViewChild('statusMessage') statusMessage: StatusMessageComponent;

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
      if (courseInstance.numQuestionsAlreadyAsked < courseInstance.numQuestionsTotal) {
        this.router.navigate(['/askquestion', courseInstance.idCourseInstanceQuestion]);
      }  else {
        this.statusMessage.showError('Keine Fragen vorhanden');
      }
    });
  }

}
