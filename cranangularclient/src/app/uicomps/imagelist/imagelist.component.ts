import { Component, OnInit, Input, Output, EventEmitter, } from '@angular/core';
import {LanguageService} from '../../services/language.service';
import {Image} from '../../model/image';

@Component({
  selector: 'app-imagelist',
  templateUrl: './imagelist.component.html',
  styleUrls: ['./imagelist.component.css']
})
export class ImagelistComponent implements OnInit {

  @Input() public images: Image[] = [];

  @Input() public imagesDeletable: boolean;

  @Output() onDeleted = new EventEmitter<Image[]>();

  constructor(private ls: LanguageService) { }

  ngOnInit() {
  }

  public removeImage(image) {
    if (this.imagesDeletable) {
      this.onDeleted.emit(image);
    }
  }

}
