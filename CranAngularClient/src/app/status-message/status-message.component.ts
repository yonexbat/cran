import { Component, OnInit, trigger, state, animate, transition, style } from '@angular/core';

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

  visibility = 'hidden';

  readonly SaveOk = `Die Daten wurden gespeichert`;

  public message: string;
  public clazzes: string;


  constructor() { }

  ngOnInit() {
  }

  public showSaveSuccess() {
    this.message = this.SaveOk;
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
        // this.messageVisible = false;
        this.visibility = 'hidden';
    }

  }
}
