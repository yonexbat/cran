import { Injectable, InjectionToken  } from '@angular/core';
import { Headers, Http, RequestOptionsArgs } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import {Courses} from './model/courses';
import {Course} from './model/course';
import {Question} from './model/question';
import {ICranDataService} from './icrandataservice';
import { Tag } from './model/tag';
import {StartCourse} from './model/startcourse';
import {CourseInstance} from './model/courseinstance';

export let CRAN_SERVICE_TOKEN = new InjectionToken<ICranDataService>('ICranDataService');

@Injectable()
export class CranDataService implements ICranDataService {

  constructor(private http: Http) {

  }

  startCourse(courseId: number): Promise<CourseInstance> {
    const param = new StartCourse();
    param.idCourse = courseId;
    debugger;
    return this.http.post('/api/Data/StartCourse', param)
                .toPromise()
                .then(  data => {
                  const result = data.json() as CourseInstance;
                  return result;
                })
                .catch(this.handleError);
  }

  getTags(name: string): Promise<Tag[]> {
   const params: RequestOptionsArgs = {params: {searchTerm: name}};
   return this.http.get('/api/Data/FindTags', params)
               .toPromise()
               .then(response => {
                 const data = response.json() as Tag[];
                 return data;
                })
               .catch(this.handleError);
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

  public updateQuestion(question: Question): Promise<any> {
    return this.http.post('/api/Data/SaveQuestion', question)
                    .toPromise()
                    .then(  data => {
                      return 'Ok';
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

  startCourse(courseId: number): Promise<CourseInstance> {
    const result = new CourseInstance();
    result.idCourseInstance = 234;
    result.idCourseInstanceQuestion = 354;
    return Promise.resolve(result);
  }

  getTags(name: string): Promise<Tag[]> {
    const tags: Tag[] = [];
    tags.push({
      id : 1,
      name : 'JS',
      description : 'Javascipt',
    });

    tags.push({
      id : 2,
      name : 'HTML',
      description : 'Html',
    });

     tags.push({
      id : 3,
      name : 'C#',
      description : 'C SHarp',
    });

    tags.push({
      id : 4,
      name : 'Java',
      description : '',
    });

    return Promise.resolve(tags);
  }
  updateQuestion(question: Question): Promise<any> {
      return new Promise<any>((resolve, reject) => {
        resolve('Ok');
      });
  }

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
        question.id = id;
        resolve(question);
      });
  }

}
