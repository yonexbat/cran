import { Component, OnInit, TemplateRef, Input } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.css']
})
export class PagerComponent implements OnInit {

  constructor() { }

  @Input()
  public itemTemplate: TemplateRef<any>;

  @Input()
  public items: any[] = [];


  ngOnInit() {
  }

}
