import { Component, OnInit, Inject, Input, Output, EventEmitter, } from '@angular/core';

import {Tag, TagType} from '../model/tag';
import {LanguageInfo} from '../model/languageInfo';
import {LanguageService} from '../language.service';

@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.css']
})
export class TagsComponent implements OnInit {

  @Input() public tagList: Tag[] = [];
  @Input() public isEditable = false;

  @Output() onRemoveTagClick = new EventEmitter<Tag>();

  constructor(private ls: LanguageService) { }

  ngOnInit() {

  }

  private tooltip(tag: Tag): string {
    const language: LanguageInfo = this.ls.getLanguage();
    let tooltip = tag.shortDescDe;
    switch (language) {
      case LanguageInfo.De:
        tooltip = tag.shortDescDe;
        break;
      case LanguageInfo.En:
        tooltip = tag.shortDescEn;
        break;
    }
    return tooltip;
  }

  private removeTag(tag: Tag) {
    this.onRemoveTagClick.emit(tag);
  }

  private classNamesForTag(tag: Tag) {
    let tagTypeClass = 'badge-warning';
    switch (tag.idTagType) {
      case TagType.Standard:
        tagTypeClass = 'badge-info';
        break;
      case TagType.Warning:
        tagTypeClass = 'badge-warning';
        break;
    }
    return `badge ${tagTypeClass} crantag`;
  }
}
