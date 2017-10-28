import { Component, OnInit, Inject, ViewChild, } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import {Tag} from '../model/tag';
import {Course} from '../model/course';
import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
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

  @ViewChild('statusMessage') statusMessage: StatusMessageComponent;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
  private router: Router,
  private activeRoute: ActivatedRoute,
  private notificationService: NotificationService) {
    this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
      const id = params.get('id');
      this.handleRouteChanged(+id);
    });

    // Create two options for default.
    this.course = new Course();

  }

  private async handleRouteChanged(id: number): Promise<void> {
    if (id > 0) {
      this.buttonText = 'Speichern';
      this.headingText = 'Kurs #' + id + ' editieren';
      try {
        this.notificationService.emitLoading();
        this.course = await this.cranDataService.getCourse(id);
        this.notificationService.emitDone();
      } catch (error) {
        this.notificationService.emitError(error);
      }
    } else {
      this.buttonText = 'Hinzufügen';
      this.headingText = 'Kurs hinzufügen';
    }
    this.actionInProgress = false;
  }

  private async save(): Promise<void> {
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
