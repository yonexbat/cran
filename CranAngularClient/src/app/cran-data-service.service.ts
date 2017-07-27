import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import {Courses} from './model/courses';
import {Course} from './model/course';

@Injectable()
export class CranDataServiceService {

  private courseUrl = '/assets/courses.json';
  constructor(private http: Http) {

  }

   public getCourses(): Promise<Courses> {
    return this.http.get(this.courseUrl)
               .toPromise()
               .then(response => {
                 const data = response.json() as Courses;
                 return data;
                })
               .catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {
    // tslint:disable-next-line:no-debugger
    debugger;
    console.error('An error occurred', error);
    return Promise.reject(error.message || error);
  }
}
