import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, Input, DebugElement} from '@angular/core';

import { VoteComponent } from './vote.component';
import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {Votes} from '../model/votes';

@Component({selector: 'app-icon ', template: ''})
class StubIconComponent {
  @Input() public icon;
}

describe('VoteComponent', () => {
  let component: VoteComponent;
  let fixture: ComponentFixture<VoteComponent>;

  beforeEach(async(() => {

    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const vote: Votes = {
      idQuestion: 12,
      downVotes: 1,
      upVotes: 3,
      myVote: 1,
    };
    cranDataService.vote.and.returnValue(Promise.resolve(vote));

    const notificationService = jasmine.createSpyObj('NotificationSercice', ['emitLoading', 'emitDone', 'emitError']);

    TestBed.configureTestingModule({
      declarations: [ VoteComponent, StubIconComponent ],
      providers: [
        { provide: CRAN_SERVICE_TOKEN, useValue: cranDataService },
        { provide: NotificationService, useValue: notificationService },
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VoteComponent);
    component = fixture.componentInstance;
    component.votes = {
      downVotes: 2,
      upVotes: 4,
      idQuestion: 1,
      myVote: 1,
    };
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('default vote', () => {
    const debugElement: DebugElement = fixture.debugElement;
    const htmlElement: HTMLElement = debugElement.nativeElement;
    expect(htmlElement.innerText).toMatch(/2 4/i, '2 downvotes and 4 upvotes');
  });

  it('upvote', async(() => {

    component.upVote();

    fixture.whenStable().then(() => {
      fixture.detectChanges();
      const debugElement: DebugElement = fixture.debugElement;
      const htmlElement: HTMLElement = debugElement.nativeElement;
      expect(htmlElement.innerText).toMatch(/1 3/i, '1 downvotes and 3 upvotes');
    });
  }));

});
