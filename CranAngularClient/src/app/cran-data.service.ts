import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import {Courses} from './model/courses';
import {Course} from './model/course';
import {Question} from './model/question';

@Injectable()
export class CranDataService {

  private courseUrl = '/api/Data/Courses';
  private courseUrlMock = '/assets/courses.json';

  constructor(private http: Http) {

  }

   public getCourses(): Promise<Courses> {
    const url = this.getUrl(this.courseUrl, this.courseUrlMock);
    return this.http.get(url)
               .toPromise()
               .then(response => {
                 const data = response.json() as Courses;
                 return data;
                })
               .catch(this.handleError);
  }

  public insertQuestion(question: Question): Promise<number> {
    const url = '/api/Data/AddQuestion';
    return this.http.post(url, question)
                    .toPromise()
                    .then(  data => {
                      const result = data.json();
                      return result.newId;
                    })
                    .catch(this.handleError);
  }


  private getUrl(urlServer: string, urlMock: string) {
    if (window.location && window.location.port && window.location.port === '4200') {
      return urlMock;
    }
    return urlServer;
  }

  private handleError(error: any): Promise<any> {
    // tslint:disable-next-line:no-debugger
    debugger;
    console.error('An error occurred', error);
    return Promise.reject(error.message || error);
  }
}
