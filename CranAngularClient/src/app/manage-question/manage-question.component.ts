import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import {Question} from '../model/question';
import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.service';


@Component({
  selector: 'app-manage-question',
  templateUrl: './manage-question.component.html',
  styleUrls: ['./manage-question.component.css']
})
export class ManageQuestionComponent implements OnInit {

  public question = new Question;

  public actionInProgress = false;

  public headingText: string;

  public buttonText: string;

  constructor(
    @Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private router: Router,
    private activeRoute: ActivatedRoute) {
        this.activeRoute.paramMap.subscribe((params: ParamMap)  => {
          const id = params.get('id');
          this.handleRouteChanged(+id);
        });
  }

  ngOnInit() {
  }

  addQuestion() {
    this.actionInProgress = true;
    this.cranDataService.insertQuestion(this.question)
    .then(questionId => {
      this.router.navigate(['/editquestion', questionId]);
    });
  }

  private handleRouteChanged(id: number) {
    if (id > 0) {
      this.buttonText = 'Speichern';
      this.headingText = 'Frage ' + id + ' editieren';
    } else {
      this.buttonText = 'Hinzufügen';
      this.headingText = 'Frage hinzufügen';
    }
    this.actionInProgress = false;
  }

}
