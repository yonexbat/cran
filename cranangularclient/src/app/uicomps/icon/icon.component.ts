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
    let internalIcon = 'fas fa-bug';
    switch (this.icon) {
      case 'edit':
        internalIcon = 'fas fa-edit';
        break;
      case 'start':
        internalIcon = 'fas fa-play';
        break;
      case 'thumbs-down':
        internalIcon = 'fas fa-thumbs-up';
        break;
      case 'thumbs-up':
        internalIcon = 'fas fa-thumbs-down';
        break;
      case 'ok':
        internalIcon = 'fas fa-check';
        break;
      case 'nok':
        internalIcon = 'fas fa-times';
        break;
      case 'remove':
        internalIcon = 'fas fa-times';
        break;
      case 'trash':
        internalIcon = 'fas fa-trash';
        break;
      case 'info':
        internalIcon = 'fas fa-info';
        break;
      case 'add':
        internalIcon = 'fas fa-plus';
        break;
      case 'list':
        internalIcon = 'fas fa-list-ul';
        break;
      case 'notification':
        internalIcon = 'fas fa-envelope';
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
