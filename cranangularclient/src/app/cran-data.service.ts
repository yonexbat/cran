import { Injectable, InjectionToken  } from '@angular/core';
import {HttpClient} from '@angular/common/http';
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
import {InsertAction} from './model/insertaction';

@Injectable()
export class CranDataService implements ICranDataService {

  constructor(private http: HttpClient) {

  }

  deleteTag(id: number): Promise<any> {
    return this.http.delete('/api/Data/DeleteTag/' + id)
    .toPromise()
    .catch(this.handleError);
  }

  getCourse(id: number): Promise<Course> {
    return this.http.get<Course>('/api/Data/GetCourse/' + id)
    .toPromise()
    .catch(this.handleError);
  }

  insertCourse(course: Course): Promise<number> {
    return this.http.post<InsertAction>('/api/Data/InsertCourse', course)
    .toPromise()
    .then(  data => {
      return data.newId;
    })
    .catch(this.handleError);
  }

  updateCourse(course: Course): Promise<any> {
    return this.http.post('/api/Data/UpdateCourse', course)
    .toPromise()
    .catch(this.handleError);
  }

  getTag(id: number): Promise<Tag> {
    return this.http.get<Tag>('/api/Data/GetTag/' + id)
    .toPromise()
    .catch(this.handleError);
  }

  insertTag(tag: Tag): Promise<number> {
    return this.http.post<InsertAction>('/api/Data/InsertTag', tag)
    .toPromise()
    .catch(this.handleError);
  }

  updateTag(tag: Tag): Promise<any> {
    return this.http.post('/api/Data/UpdateTag', tag)
    .toPromise()
    .catch(this.handleError);
  }

  searchForTags(parameters: SearchTags): Promise<PagedResult<Tag>> {
    return this.http.post<PagedResult<Tag>>('/api/Data/SearchForTags', parameters)
    .toPromise()
    .catch(this.handleError);
  }

  getUserInfo(): Promise<UserInfo> {
    return this.http.get<UserInfo>('/api/Data/GetUserInfo')
    .toPromise()
    .catch(this.handleError);
  }

  addImage(image: Image): Promise<Image> {
    return this.http.post<Image>('/api/Data/AddImage', image)
    .toPromise()
    .catch(this.handleError);
  }

  vote(votes: Votes): Promise<Votes> {
    return this.http.post<Votes>('/api/Data/Vote', votes)
    .toPromise()
    .catch(this.handleError);
  }

  deleteComment(id: number): Promise<any> {
    return this.http.delete('/api/Data/DeleteComment/' + id)
    .toPromise()
    .catch(this.handleError);
  }

  addComment(comment: Comment): Promise<number> {
    return this.http.post<Comment>('/api/Data/AddComment', comment)
    .toPromise()
    .catch(this.handleError);
  }

  getComments(parameters: GetComments): Promise<PagedResult<Comment>> {
    return this.http.post<PagedResult<Comment>>('/api/Data/GetComments', parameters)
    .toPromise()
    .catch(this.handleError);
  }

  getRolesOfUser(): Promise<string[]> {
    return this.http.get<string[]>('/api/Data/GetRolesOfUser')
    .toPromise()
    .catch(this.handleError);
  }

  searchForQuestions(parameters: SearchQParameters): Promise<PagedResult<QuestionListEntry>> {
    return this.http.post<PagedResult<QuestionListEntry>>('/api/Data/SearchForQuestions', parameters)
    .toPromise()
    .catch(this.handleError);
  }

  deleteCourseInstance(id: number): Promise<any> {
    return this.http.delete('/api/Data/DeleteCourseInstance/' + id)
    .toPromise()
    .catch(this.handleError);
  }

  getCourseResult(courseInstanceId: number): Promise<Result> {
     return this.http.get<Result>('/api/Data/GetCourseResult/' + courseInstanceId)
               .toPromise()
               .catch(this.handleError);
  }

  deleteQuestion(id: number): Promise<any> {
   return this.http.delete('/api/Data/DeleteQuestion/' + id)
            .toPromise()
            .catch(this.handleError);
  }

  getMyQuestions(): Promise<QuestionListEntry[]> {
       return this.http.get<QuestionListEntry[]>('/api/Data/GetMyQuestions')
               .toPromise()
               .catch(this.handleError);
  }

  getMyCourseInstances(): Promise<CourseInstanceListEntry[]> {
    return this.http.get<CourseInstanceListEntry[]>('/api/Data/GetMyCourseInstances')
    .toPromise()
    .catch(this.handleError);
  }

  answerQuestionAndGetSolution(answer: QuestionAnswer): Promise<Question> {
     return this.http.post<Question>('/api/Data/AnswerQuestionAndGetSolution', answer)
                    .toPromise()
                    .catch(this.handleError);
  }

  answerQuestionAndGetNextQuestion(answer: QuestionAnswer): Promise<CourseInstance> {
    return this.http.post<CourseInstance>('/api/Data/AnswerQuestionAndGetNextQuestion', answer)
                    .toPromise()
                    .catch(this.handleError);
  }

  getQuestionToAsk(id: number): Promise<QuestionToAsk> {
    return this.http.get<QuestionToAsk>('/api/Data/GetQuestionToAsk/' + id)
                    .toPromise()
                    .catch(this.handleError);
  }

  startCourse(courseId: number): Promise<CourseInstance> {
    const param = new StartCourse();
    param.idCourse = courseId;
    return this.http.post<CourseInstance>('/api/Data/StartCourse', param)
                .toPromise()
                .catch(this.handleError);
  }

  findTags(name: string): Promise<Tag[]> {
   const requestOptions = {
    params: {searchTerm: name}
   };
   return this.http.get<Tag[]>('/api/Data/FindTags', requestOptions)
               .toPromise()
               .catch(this.handleError);
  }

  public getCourses(): Promise<Courses> {
    return this.http.get<Courses>('/api/Data/GetCourses')
               .toPromise()
               .catch(this.handleError);
  }

  public insertQuestion(question: Question): Promise<number> {
    return this.http.post<InsertAction>('/api/Data/InsertQuestion', question)
                    .toPromise()
                    .then(  data => {
                      return data.newId;
                    })
                    .catch(this.handleError);
  }

  public updateQuestion(question: Question): Promise<string> {
    return this.http.post<string>('/api/Data/UpdateQuestion', question)
                    .toPromise()
                    .catch(this.handleError);
  }

  public getQuestion(id: number): Promise<Question> {
    return this.http.get<Question>('/api/Data/GetQuestion/' + id)
                    .toPromise()
                    .catch(this.handleError);
  }


  private handleError(error: any): Promise<any> {
    // tslint:disable-next-line:no-debugger
    debugger;
    console.error('An error occurred', error);
    return Promise.reject(error.message || error);
  }
}
