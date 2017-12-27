import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {ConfirmService} from '../confirm.service';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {LanguageService} from '../language.service';

@Component({
  selector: 'app-course-starter',
  templateUrl: './course-starter.component.html',
  styleUrls: ['./course-starter.component.css']
})
export class CourseStarterComponent implements OnInit {

  constructor(  @Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
  private router: Router,
  private activeRoute: ActivatedRoute,
  private notificationService: NotificationService,
  private ls: LanguageService,
  private confirmService: ConfirmService) {
      this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
      const id = params.get('id');
      this.handleRouteChanged(+id);
    });
  }

  ngOnInit() {
  }

  private async handleRouteChanged(id: number): Promise<void> {
    if (id > 0) {
      try {
        this.notificationService.emitLoading();
        const courseInstance = await this.cranDataService.startCourse(id);
        if (courseInstance.numQuestionsAlreadyAsked < courseInstance.numQuestionsTotal) {
          this.router.navigate(['/askquestion', courseInstance.idCourseInstanceQuestion]);
        }
        this.notificationService.emitDone();
      } catch (error) {
        this.notificationService.emitError(error);
      }
    }
  }

}
