import { Component, OnInit, Input } from '@angular/core';

import {Image} from '../model/image';

@Component({
  selector: 'app-imagelist',
  templateUrl: './imagelist.component.html',
  styleUrls: ['./imagelist.component.css']
})
export class ImagelistComponent implements OnInit {

  @Input() public images: Image[] = [];

  constructor() { }

  ngOnInit() {
  }

}
