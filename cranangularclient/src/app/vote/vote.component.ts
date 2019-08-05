import { Component, OnInit, Input, Inject } from '@angular/core';

import {ICranDataService} from '../services/icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../services/cran-data.servicetoken';
import {NotificationService} from '../services/notification.service';
import {Votes} from '../model/votes';

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html',
  styleUrls: ['./vote.component.css']
})
export class VoteComponent implements OnInit {

  @Input() public votes: Votes;

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataServiceService: ICranDataService,
              private notificationService: NotificationService) {

  }

  ngOnInit() {
  }

  public async upVote(): Promise<void> {
    if (this.votes.myVote === 1) {
      this.votes.myVote = 0;
    } else {
      this.votes.myVote = 1;
    }
    this.vote();
  }

  public async downVote(): Promise<void> {
    if (this.votes.myVote === -1) {
      this.votes.myVote = 0;
    } else {
      this.votes.myVote = -1;
    }
    this.vote();
  }

  private async vote(): Promise<void> {
    try {
      this.notificationService.emitLoading();
      this.votes = await this.cranDataServiceService.vote(this.votes);
      this.notificationService.emitDone();
    } catch (error) {
      this.notificationService.emitError(error);
    }
  }

}
