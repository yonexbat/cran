import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Router, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {Courses} from '../model/courses';
import {Course} from '../model/course';
import {CourseInstance} from '../model/courseinstance';
import {StatusMessageComponent} from '../status-message/status-message.component';
import {NotificationService} from '../notification.service';


@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {

  courses: Course[] = [];

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router,
    private notificationService: NotificationService) { }

  ngOnInit() {
    this.getCourses();
  }

  private async getCourses(): Promise<void> {
    try {
      const result = await this.cranDataServiceService.getCourses();
      this.courses = result.courses;
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  public async startCourse(course: Course): Promise<void> {
    try {
      const courseInstance = await this.cranDataServiceService.startCourse(course.id);
      if (courseInstance.numQuestionsAlreadyAsked < courseInstance.numQuestionsTotal) {
        this.router.navigate(['/askquestion', courseInstance.idCourseInstanceQuestion]);
      }
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

}
