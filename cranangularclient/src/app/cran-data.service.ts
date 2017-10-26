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
import {Comment} from './model/comment';
import {GetComments} from './model/getcomments';
import {Votes} from './model/votes';
import {Image} from './model/image';
import {UserInfo} from './model/userinfo';
import {SearchTags} from './model/searchtags';

@Injectable()
export class CranDataService implements ICranDataService {

  constructor(private http: Http) {

  }

  searchForTags(parameters: SearchTags): Promise<PagedResult<Tag>> {
    return this.http.post('/api/Data/SearchForTags', parameters)
    .toPromise()
    .then(  response => {
      const result = response.json() as PagedResult<Tag>;
      return result;
    })
    .catch(this.handleError);
  }

  getUserInfo(): Promise<UserInfo> {
    return this.http.get('/api/Data/GetUserInfo')
    .toPromise()
    .then(response => {
      const data = response.json() as UserInfo;
      return data;
     })
    .catch(this.handleError);
  }

  addImage(image: Image): Promise<Image> {
    return this.http.post('/api/Data/AddImage', image)
    .toPromise()
    .then(  response => {
      const result = response.json() as Votes;
      return result;
    })
    .catch(this.handleError);
  }

  vote(votes: Votes): Promise<Votes> {
    return this.http.post('/api/Data/Vote', votes)
    .toPromise()
    .then(  response => {
      const result = response.json() as Votes;
      return result;
    })
    .catch(this.handleError);
  }

  deleteComment(id: number): Promise<any> {
    return this.http.delete('/api/Data/DeleteComment/' + id)
    .toPromise()
    .catch(this.handleError);
  }

  addComment(comment: Comment): Promise<number> {
    return this.http.post('/api/Data/AddComment', comment)
    .toPromise()
    .then(  response => {
      const result = response.json() as number;
      return result;
    })
    .catch(this.handleError);
  }

  getComments(parameters: GetComments): Promise<PagedResult<Comment>> {
    return this.http.post('/api/Data/GetComments', parameters)
    .toPromise()
    .then(  response => {
      const result = response.json() as PagedResult<Comment>;
      return result;
    })
    .catch(this.handleError);
  }

  getRolesOfUser(): Promise<string[]> {
    return this.http.get('/api/Data/GetRolesOfUser')
    .toPromise()
    .then(response => {
      const data = response.json() as string[];
      return data;
     })
    .catch(this.handleError);
  }

  searchForQuestions(parameters: SearchQParameters): Promise<PagedResult<QuestionListEntry>> {
    return this.http.post('/api/Data/SearchForQuestions', parameters)
    .toPromise()
    .then(  response => {
      const result = response.json() as PagedResult<QuestionListEntry>;
      return result;
    })
    .catch(this.handleError);
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
            .toPromise()
            .catch(this.handleError);
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
