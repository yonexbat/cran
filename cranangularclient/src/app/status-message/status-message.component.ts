import { Component, OnInit } from '@angular/core';
import {trigger, state, animate, transition, style} from '@angular/animations';
import {LanguageService} from '../language.service';

@Component({
  selector: 'app-status-message',
  templateUrl: './status-message.component.html',
  styleUrls: ['./status-message.component.css'],
  animations: [
    trigger('visibilityChanged', [
      state('shown' , style({ opacity: 1})),
      state('hidden', style({ opacity: 0})),
      transition('* => hidden', animate('300ms ease-out')),
      transition('* => shown', animate('500ms ease-in')),
    ]
    )
  ]
})
export class StatusMessageComponent implements OnInit {

  public messageVisible = false;
  private visibility = 'hidden';
  private message: string;
  private clazzes: string;


  constructor(private ls: LanguageService) { }

  ngOnInit() {
  }

  public showSaveSuccess() {
    this.message = this.ls.label('saveok');
    this.messageVisible = true;
    this.visibility = 'shown';
    this.clazzes = 'alert-success';
    setTimeout(() => this.done(), 2000);
  }

  public showError(message: string) {
    this.message = message;
    this.messageVisible = true;
    this.visibility = 'shown';
    this.clazzes = 'alert-danger';
    setTimeout(() => this.done(), 2000);
  }

  public done() {
    this.messageVisible = false;
  }

  public animationDone(event: any) {

    if (event.toState === 'shown' && event.phaseName === 'done') {
        this.visibility = 'hidden';
    }

  }
}
