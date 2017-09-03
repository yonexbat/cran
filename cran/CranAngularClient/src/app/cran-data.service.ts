import { Injectable, InjectionToken  } from '@angular/core';
import { Headers, Http, RequestOptionsArgs } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import {Courses} from './model/courses';
import {Course} from './model/course';
import {Question} from './model/question';
import {QuestionOption} from './model/questionoption';
import {ICranDataService} from './icrandataservice';
import {Tag} from './model/tag';
import {StartCourse} from './model/startcourse';
import {CourseInstance} from './model/courseinstance';
import {QuestionToAsk} from './model/questiontoask';
import {QuestionOptionToAsk} from './model/questionoptiontoask';
import {QuestionAnswer} from './model/questionanswer';
import {QuestionListEntry} from './model/questionlistentry';
import {Result} from './model/result';
import {QuestionResult} from './model/questionresult';
import {CourseInstanceListEntry} from './model/courseinstancelistentry';
import {SearchQParameters} from './model/searchqparameters';
import {PagedResult} from './model/pagedresult';


@Injectable()
export class CranDataService implements ICranDataService {

  constructor(private http: Http) {

  }

  searchForQuestions(parameters: SearchQParameters): Promise<PagedResult<QuestionListEntry>> {
    throw new Error('Method not implemented.');
  }

  deleteCourseInstance(id: number): Promise<any> {
    return this.http.delete('/api/Data/DeleteCourseInstance/' + id)
    .toPromise()
    .catch(this.handleError);
  }


  getCourseResult(courseInstanceId: number): Promise<Result> {
     return this.http.get('/api/Data/GetCourseResult/' + courseInstanceId)
               .toPromise()
               .then(response => {
                 const data = response.json() as QuestionListEntry[];
                 return data;
                })
               .catch(this.handleError);
  }

  deleteQuestion(id: number): Promise<any> {
   return this.http.delete('/api/Data/DeleteQuestion/' + id)
            .toPromise();
  }

  getMyQuestions(): Promise<QuestionListEntry[]> {
       return this.http.get('/api/Data/GetMyQuestions')
               .toPromise()
               .then(response => {
                 const data = response.json() as QuestionListEntry[];
                 return data;
                })
               .catch(this.handleError);
  }

  getMyCourseInstances(): Promise<CourseInstanceListEntry[]> {
    return this.http.get('/api/Data/GetMyCourseInstances')
    .toPromise()
    .then(response => {
      const data = response.json() as CourseInstanceListEntry[];
      return data;
     })
    .catch(this.handleError);
  }

  answerQuestionAndGetSolution(answer: QuestionAnswer): Promise<Question> {
     return this.http.post('/api/Data/AnswerQuestionAndGetSolution', answer)
                    .toPromise()
                    .then(  response => {
                      const result = response.json() as Question;
                      return result;
                    })
                    .catch(this.handleError);
  }

  answerQuestionAndGetNextQuestion(answer: QuestionAnswer): Promise<CourseInstance> {
    return this.http.post('/api/Data/AnswerQuestionAndGetNextQuestion', answer)
                    .toPromise()
                    .then(  data => {
                      const result = data.json() as CourseInstance;
                      return result;
                    })
                    .catch(this.handleError);
  }

  getQuestionToAsk(id: number): Promise<QuestionToAsk> {
    return this.http.get('/api/Data/GetQuestionToAsk/' + id)
                    .toPromise()
                    .then(  response => {
                      const result = response.json() as QuestionToAsk;
                      return result;
                    })
                    .catch(this.handleError);
  }

  startCourse(courseId: number): Promise<CourseInstance> {
    const param = new StartCourse();
    param.idCourse = courseId;
    return this.http.post('/api/Data/StartCourse', param)
                .toPromise()
                .then(  data => {
                  const result = data.json() as CourseInstance;
                  return result;
                })
                .catch(this.handleError);
  }

  findTags(name: string): Promise<Tag[]> {
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
    return this.http.get('/api/Data/GetCourses')
               .toPromise()
               .then(response => {
                 const data = response.json() as Courses;
                 return data;
                })
               .catch(this.handleError);
  }

  public insertQuestion(question: Question): Promise<number> {
    return this.http.post('/api/Data/InsertQuestion', question)
                    .toPromise()
                    .then(  data => {
                      const result = data.json();
                      return result.newId;
                    })
                    .catch(this.handleError);
  }

  public updateQuestion(question: Question): Promise<any> {
    return this.http.post('/api/Data/UpdateQuestion', question)
                    .toPromise()
                    .then(  data => {
                      return 'Ok';
                    })
                    .catch(this.handleError);
  }

  public getQuestion(id: number): Promise<Question> {
    return this.http.get('/api/Data/GetQuestion/' + id)
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
