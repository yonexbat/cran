import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-icon',
  templateUrl: './icon.component.html',
  styleUrls: ['./icon.component.css']
})
export class IconComponent implements OnInit {

  @Input() public icon;

  constructor() { }

  ngOnInit() {
  }

  public get internalIcon(): string {
    let internalIcon = 'fa-bug';
    switch (this.icon) {
      case 'edit':
        internalIcon = 'fa-edit';
        break;
      case 'start':
        internalIcon = 'fa-play';
        break;
      case 'thumbs-down':
        internalIcon = 'fa-thumbs-o-down';
        break;
      case 'thumbs-up':
        internalIcon = 'fa-thumbs-o-up';
        break;
      case 'ok':
        internalIcon = 'fa-check';
        break;
      case 'nok':
        internalIcon = 'fa-remove';
        break;
      case 'remove':
        internalIcon = 'fa-remove';
        break;
      case 'trash':
        internalIcon = ' fa-trash-o';
        break;
      case 'info':
        internalIcon = 'fa-info';
        break;
      case 'add':
        internalIcon = 'fa-plus';
        break;
      case 'list':
        internalIcon = 'fa-list-ul';
        break;
      case 'notification':
        internalIcon = 'fa-envelope';
        break;
    }
    return internalIcon;
  }

}
