import { Component, OnInit, Input, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';

import 'rxjs/add/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';


import {Question} from '../model/question';
import {Tag} from '../model/tag';
import {CRAN_SERVICE_TOKEN} from '../cran-data.service';
import {ICranDataService} from '../icrandataservice';

@Component({
  selector: 'app-tag-finder',
  templateUrl: './tag-finder.component.html',
  styleUrls: ['./tag-finder.component.css']
})
export class TagFinderComponent implements OnInit {

  public tags: Observable<Tag[]>;
  private searchTerms = new Subject<string>();

  constructor(@Inject(CRAN_SERVICE_TOKEN) private cranDataService: ICranDataService) { }


  @Input() question: Question;

  public searchText = '';

  ngOnInit() {
     this.tags = this.searchTerms
      .debounceTime(300)        // wait 300ms after each keystroke before considering the term
      .distinctUntilChanged()   // ignore if next search term is same as previous
      .switchMap(term => term   // switch to new observable each time the term changes
        // return the http search observable
        ? this.cranDataService.findTags(term)
        // or the observable of empty heroes if there was no search term
        : Observable.of<Tag[]>([]))
      .catch(error => {
        // TODO: add real error handling
        console.log(error);
        return Observable.of<Tag[]>([]);
      });
  }

  public search(term: string) {
    this.searchTerms.next(term);
  }

  public addTag(tag: Tag) {
    this.question.tags.push(tag);
    this.searchText = '';
    this.searchTerms.next('');
  }

  public removeTag(tag: Tag) {
    const index = this.question.tags.indexOf(tag);
    this.question.tags.splice(index, 1);
  }

}
