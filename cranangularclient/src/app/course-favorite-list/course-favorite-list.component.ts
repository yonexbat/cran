import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, } from '@angular/router';
import {ICranDataService} from '../services/icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../services/cran-data.servicetoken';
import {Course} from '../model/course';
import {NotificationService} from '../services/notification.service';
import {LanguageService} from '../services/language.service';
import {PagedResult} from '../model/pagedresult';
import {CourseToFavorites} from '../model/coursetofavorites';
import {ConfirmService} from '../services/confirm.service';

@Component({
  selector: 'app-course-favorite-list',
  templateUrl: './course-favorite-list.component.html',
  styleUrls: ['./course-favorite-list.component.css']
})
export class CourseFavoriteListComponent implements OnInit {

  public pagedResult: PagedResult<Course> = new PagedResult<Course>();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private router: Router,
    private notificationService: NotificationService,
    public ls: LanguageService,
    private confirmSerice: ConfirmService) { }

  ngOnInit() {
    this.getCourses(0);
  }

  private async getCourses(page: number): Promise<void> {
    try {
      this.notificationService.emitLoading();
      const result = await this.cranDataService.getFavoriteCourses(page);
      this.pagedResult = result;
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  public pageSelected(pageNumber: number) {
    this.getCourses(pageNumber);
  }

  public async removeFromFavorites(course: Course) {
    try {
      const removeMessage =  this.ls.label('removecoursefromfavoritesq', course.title);
      await this.confirmSerice.confirm(this.ls.label('removecourseq'), removeMessage);
    } catch (error) {
      return;
    }

    try {
      this.notificationService.emitLoading();
      const courseToFavorites = new CourseToFavorites();
      courseToFavorites.courseId = course.id;
      await this.cranDataService.removeCoureFromFavorites(courseToFavorites);
      await this.getCourses(this.pagedResult.currentPage);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  public async startCourse(course: Course): Promise<void> {
    try {
      this.notificationService.emitLoading();
      const courseInstance = await this.cranDataService.startCourse(course.id);
      if (courseInstance.numQuestionsAlreadyAsked < courseInstance.numQuestionsTotal) {
        this.router.navigate(['/askquestion', courseInstance.idCourseInstanceQuestion]);
      }
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

}
