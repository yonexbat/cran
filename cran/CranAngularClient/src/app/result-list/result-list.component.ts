import { Component, OnInit, Inject, } from '@angular/core';
import { HttpModule } from '@angular/http';
import { Router, ParamMap, ActivatedRoute, } from '@angular/router';

import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {Result} from '../model/result';
import {QuestionResult} from '../model/questionresult';

@Component({
  selector: 'app-result-list',
  templateUrl: './result-list.component.html',
  styleUrls: ['./result-list.component.css']
})
export class ResultListComponent implements OnInit {

  public result: Result = new Result();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService) {

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

  private async handleRouteChanged(id: number): Promise<void> {
    try {
      this.result = await this.cranDataServiceService.getCourseResult(id);
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }
}
