import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import {Tag} from '../model/tag';
import {Course} from '../model/course';
import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {LanguageService} from '../language.service';
import {StatusMessageComponent} from '../status-message/status-message.component';


@Component({
  selector: 'app-manage-course',
  templateUrl: './manage-course.component.html',
  styleUrls: ['./manage-course.component.css']
})
export class ManageCourseComponent implements OnInit {

  public course: Course;
  public headingText: string;
  public actionInProgress = false;
  public buttonText: string;

  @ViewChild('statusMessage', { static: true }) statusMessage: StatusMessageComponent;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService,
    public ls: LanguageService) {
    this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
      const id = params.get('id');
      this.handleRouteChanged(+id);
    });

    // Create two options for default.
    this.course = new Course();

  }

  private async handleRouteChanged(id: number): Promise<void> {
    if (id > 0) {
      try {
        this.notificationService.emitLoading();
        this.course = await this.cranDataService.getCourse(id);
        this.notificationService.emitDone();
      } catch (error) {
        this.notificationService.emitError(error);
      }
    }
    this.actionInProgress = false;
  }

  public getSaveButtonText(): string  {
    if (this.course.id > 0) {
      return this.ls.label('save');
    } else {
      return this.ls.label('add');
    }
  }

  public getHeadingText(): string {
    if (this.course.id > 0) {
      return this.ls.label('editcourse', String(this.course.id ));
    } else {
      return this.ls.label('addcourse');
    }
  }

  public async save(): Promise<void> {
    this.actionInProgress = true;

      // save current question
      try {
        this.notificationService.emitLoading();
        if (this.course && this.course.id > 0) {
          await this.cranDataService.updateCourse(this.course);
          this.actionInProgress = false;
          this.statusMessage.showSaveSuccess();
        } else { // crate new question
          const id = await this.cranDataService.insertCourse(this.course);
          this.actionInProgress = false;
          this.router.navigate(['/managecourse', id]);
        }
        this.notificationService.emitDone();
      } catch (error) {

        this.notificationService.emitError(error);
        this.actionInProgress = false;
      }
  }

  ngOnInit() {
  }

}
