import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import {Courses} from './model/courses';
import {Course} from './model/course';

@Injectable()
export class CranDataServiceService {

  private courseUrl = '/assets/navigation.json';
  constructor(private http: Http) {

  }

   public getIndex(): Promise<Courses> {
    return this.http.get(this.courseUrl)
               .toPromise()
               .then(response => {
                 const data = response.json() as Courses;
                 return data;
                })
               .catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {
    debugger;
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }
}
