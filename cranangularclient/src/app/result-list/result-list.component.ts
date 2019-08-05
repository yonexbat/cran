import { Component, OnInit, Inject, } from '@angular/core';
import { Router, ParamMap, ActivatedRoute, } from '@angular/router';

import {ICranDataService} from '../services/icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../services/cran-data.servicetoken';
import {NotificationService} from '../services/notification.service';
import {Result} from '../model/result';
import {QuestionResult} from '../model/questionresult';
import {LanguageService} from '../services/language.service';

@Component({
  selector: 'app-result-list',
  templateUrl: './result-list.component.html',
  styleUrls: ['./result-list.component.css']
})
export class ResultListComponent implements OnInit {

  public result: Result = new Result();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService,
    public ls: LanguageService) {

      this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
        const id = params.get('id');
        this.handleRouteChanged(+id);
      });
    }

  ngOnInit() {
  }

  public showQuestion(result: QuestionResult) {
    this.router.navigate(['/askquestion', result.idCourseInstanceQuestion]);
  }

  public async startCourse(): Promise<void> {
    try {
      this.notificationService.emitLoading();
      const courseInstance = await this.cranDataService.startCourse(this.result.idCourse);
      if (courseInstance.numQuestionsAlreadyAsked < courseInstance.numQuestionsTotal) {
        this.router.navigate(['/askquestion', courseInstance.idCourseInstanceQuestion]);
      } else {
        const noQuestionsMessage = this.ls.label('noquestionsavailable');
        this.notificationService.emitError(noQuestionsMessage);
      }
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

  private async handleRouteChanged(id: number): Promise<void> {
    try {
      this.notificationService.emitLoading();
      this.result = await this.cranDataService.getCourseResult(id);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }
}
