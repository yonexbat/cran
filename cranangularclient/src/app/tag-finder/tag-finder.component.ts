import { Component, OnInit, Input, Output, Inject, EventEmitter } from '@angular/core';
import { Observable, Subject, of } from 'rxjs';


import { switchMap, debounceTime, distinctUntilChanged, catchError } from 'rxjs/operators';


import {Tag} from '../model/tag';
import {ICranDataService} from '../icrandataservice';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';

@Component({
  selector: 'app-tag-finder',
  templateUrl: './tag-finder.component.html',
  styleUrls: ['./tag-finder.component.css']
})
export class TagFinderComponent implements OnInit {

  public tags: Observable<Tag[]>;
  private searchTerms = new Subject<string>();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService,
    private notificationService: NotificationService) { }


  @Input() public tagsArray: Tag[] = [];

  @Input() public title = 'Tags';

  @Output() public tagSelected = new EventEmitter<Tag>();

  @Output() public tagRemoved = new EventEmitter<Tag>();

  @Output() public tagSelectionChanged = new EventEmitter<void>();

  public searchText = '';

  ngOnInit() {
     this.tags = this.searchTerms
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap(term => term  ? this.cranDataService.findTags(term) : of<Tag[]>([])),
        catchError(error => {
          this.notificationService.emitError(error);
          return of<Tag[]>([]);
        })
      );
  }

  public search(term: string) {
    this.searchTerms.next(term);
  }

  public addTag(tag: Tag) {
    this.tagsArray.push(tag);
    this.searchText = '';
    this.searchTerms.next('');
    this.tagSelected.emit(tag);
    this.tagSelectionChanged.emit();
  }

  public removeTag(tag: Tag) {
    const index = this.tagsArray.indexOf(tag);
    this.tagsArray.splice(index, 1);
    this.tagRemoved.emit(tag);
    this.tagSelectionChanged.emit();
  }

}
