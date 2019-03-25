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
        internalIcon = 'fa fa-edit';
        break;
      case 'start':
        internalIcon = 'fa fa-play';
        break;
      case 'thumbs-down':
        internalIcon = 'fa fa-thumbs-o-down';
        break;
      case 'thumbs-up':
        internalIcon = 'fa fa-thumbs-o-up';
        break;
      case 'ok':
        internalIcon = 'fa fa-check';
        break;
      case 'nok':
        internalIcon = 'fa fa-remove';
        break;
      case 'remove':
        internalIcon = 'fa fa-remove';
        break;
      case 'trash':
        internalIcon = 'fa fa-trash-o';
        break;
      case 'info':
        internalIcon = 'fa fa-info';
        break;
      case 'add':
        internalIcon = 'fa fa-plus';
        break;
      case 'list':
        internalIcon = 'fa fa-list-ul';
        break;
      case 'notification':
        internalIcon = 'fa fa-envelope';
        break;
      case 'favoriteon':
        internalIcon = 'fas fa-star';
        break;
      case 'favoriteoff':
        internalIcon = 'far fa-star';
        break;
    }
    return internalIcon;
  }

}
