import { Injectable, InjectionToken  } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import {Courses} from './model/courses';
import {Course} from './model/course';
import {Question} from './model/question';
import {ICranDataService} from './icrandataservice';

export let CRAN_SERVICE_TOKEN = new InjectionToken<ICranDataService>('ICranDataService');

@Injectable()
export class CranDataService implements ICranDataService {

  constructor(private http: Http) {

  }

   public getCourses(): Promise<Courses> {
    return this.http.get('/api/Data/Courses')
               .toPromise()
               .then(response => {
                 const data = response.json() as Courses;
                 return data;
                })
               .catch(this.handleError);
  }

  public insertQuestion(question: Question): Promise<number> {  
    return this.http.post('/api/Data/AddQuestion', question)
                    .toPromise()
                    .then(  data => {
                      const result = data.json();
                      return result.newId;
                    })
                    .catch(this.handleError);
  }

  public getQuestion(id: number): Promise<Question> {
    return this.http.get('/api/Data/Question/' + id)
                    .toPromise()
                    .then(  response => {
                      const result = response.json() as Question;
                      return result;
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

@Injectable()
export class CranDataServiceMock implements ICranDataService {

  constructor(private http: Http) {

  }

  getCourses(): Promise<Courses> {
      return this.http.get('/assets/courses.json')
               .toPromise()
               .then(response => {
                 const data = response.json() as Courses;
                 return data;
                });
  }

  insertQuestion(question: Question): Promise<number> {
    return new Promise<number>((resolve, reject) => {
      resolve(3);
    });
  }

  getQuestion(id: number): Promise<Question> {
    return new Promise<Question>((resolve, reject) => {
        const question = new Question();
        question.text = 'Hello';
        question.title = 'MyTitle';
        resolve(question);
      });
  }

}
