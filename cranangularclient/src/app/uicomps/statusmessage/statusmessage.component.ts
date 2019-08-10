import { Component, OnInit } from '@angular/core';
import {trigger, state, animate, transition, style} from '@angular/animations';
import {LanguageService} from '../../services/language.service';

@Component({
  selector: 'app-statusmessage',
  templateUrl: './statusmessage.component.html',
  styleUrls: ['./statusmessage.component.scss'],
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
export class StatusmessageComponent implements OnInit {

  public messageVisible = false;
  public visibility = 'hidden';
  public message: string;
  public clazzes: string;


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
