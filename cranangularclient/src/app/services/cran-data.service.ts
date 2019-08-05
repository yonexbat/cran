import { Injectable, InjectionToken  } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Course} from '../model/course';
import {Question} from '../model/question';
import {ICranDataService} from './icrandataservice';
import {Tag} from '../model/tag';
import {StartCourse} from '../model/startcourse';
import {CourseInstance} from '../model/courseinstance';
import {QuestionToAsk} from '../model/questiontoask';
import {QuestionAnswer} from '../model/questionanswer';
import {QuestionListEntry} from '../model/questionlistentry';
import {Result} from '../model/result';
import {CourseInstanceListEntry} from '../model/courseinstancelistentry';
import {SearchQParameters} from '../model/searchqparameters';
import {PagedResult} from '../model/pagedresult';
import {Comment} from '../model/comment';
import {GetComments} from '../model/getcomments';
import {Votes} from '../model/votes';
import {Image} from '../model/image';
import {UserInfo} from '../model/userinfo';
import {SearchTags} from '../model/searchtags';
import {SearchText} from '../model/searchtext';
import {Text} from '../model/text';
import {VersionInfo} from '../model/versionInfo';
import {VersionInfoParameters} from '../model/versionInfoParameters';
import {SubscriptionShort} from '../model/subscriptionshort';
import {Notification} from '../model/notification';
import {CourseToFavorites} from '../model/coursetofavorites';

declare var window: any;

@Injectable()
export class CranDataService implements ICranDataService {


  constructor(private http: HttpClient) {

  }

  addCourseToFavorites(favorite: CourseToFavorites): Promise<any> {
    return this.http.post<Notification>('/api/Data/AddCourseToFavorites', favorite)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  removeCoureFromFavorites(favorite: CourseToFavorites): Promise<any> {
    return this.http.post<Notification>('/api/Data/RemoveCoureFromFavorites', favorite)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getFavoriteCourses(page: number): Promise<PagedResult<Course>> {
    return this.http.get<PagedResult<Course>>('/api/Data/GetFavoriteCourses/' + page)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  sendNotificationToUser(message: Notification): Promise<any> {
    return this.http.post<Notification>('/api/Data/SendNotificationToUser', message)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getAllSubscriptions(page: number): Promise<PagedResult<SubscriptionShort>> {
     return this.http.get<PagedResult<SubscriptionShort>>('/api/Data/GetAllSubscriptions/' + page)
     .toPromise()
     .catch((error) => this.handleError(error));
  }

  addPushRegistration(subscription: any): Promise<any> {
    return this.http.post<Text>('/api/Data/AddPushRegistration', subscription)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getVersions(parameters: VersionInfoParameters): Promise<PagedResult<VersionInfo>> {
    return this.http.post<PagedResult<VersionInfo>>('/api/Data/GetVersions', parameters)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getTextDtoByKey(key: string): Promise<Text> {
    return this.http.get<Text>('/api/Data/GetTextDtoByKey/' + key)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getTextDto(id: number): Promise<Text> {
    return this.http.get<Text>('/api/Data/GetTextDto/' + id)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  updateText(text: Text): Promise<any> {
    return this.http.post<Text>('/api/Data/UpdateText', text)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getTexts(search: SearchText): Promise<PagedResult<Text>> {
    return this.http.post<PagedResult<Text>>('/api/Data/GetTexts', search)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getTags(ids: number[]): Promise<Tag[]> {
    return this.http.post<number>('/api/Data/GetTags/', ids)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  versionQuestion(id: number): Promise<number> {
    return this.http.post<number>('/api/Data/VersionQuestion/', id)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  acceptQuestion(id: number): Promise<any> {
    return this.http.post<number>('/api/Data/AcceptQuestion/', id)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  copyQuestion(id: number): Promise<number> {
    return this.http.post<number>('/api/Data/CopyQuestion/', id)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  deleteTag(id: number): Promise<any> {
    return this.http.delete('/api/Data/DeleteTag/' + id)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getCourse(id: number): Promise<Course> {
    return this.http.get<Course>('/api/Data/GetCourse/' + id)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  insertCourse(course: Course): Promise<number> {
    return this.http.post<number>('/api/Data/InsertCourse', course)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  updateCourse(course: Course): Promise<any> {
    return this.http.post('/api/Data/UpdateCourse', course)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getTag(id: number): Promise<Tag> {
    return this.http.get<Tag>('/api/Data/GetTag/' + id)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  insertTag(tag: Tag): Promise<number> {
    return this.http.post<number>('/api/Data/InsertTag', tag)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  updateTag(tag: Tag): Promise<any> {
    return this.http.post('/api/Data/UpdateTag', tag)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  searchForTags(parameters: SearchTags): Promise<PagedResult<Tag>> {
    return this.http.post<PagedResult<Tag>>('/api/Data/SearchForTags', parameters)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getUserInfo(): Promise<UserInfo> {
    return this.http.get<UserInfo>('/api/Data/GetUserInfo')
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  addImage(image: Image): Promise<Image> {
    return this.http.post<Image>('/api/Data/AddImage', image)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  vote(votes: Votes): Promise<Votes> {
    return this.http.post<Votes>('/api/Data/Vote', votes)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  deleteComment(id: number): Promise<any> {
    return this.http.delete('/api/Data/DeleteComment/' + id)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  addComment(comment: Comment): Promise<number> {
    return this.http.post<Comment>('/api/Data/AddComment', comment)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getComments(parameters: GetComments): Promise<PagedResult<Comment>> {
    return this.http.post<PagedResult<Comment>>('/api/Data/GetComments', parameters)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getRolesOfUser(): Promise<string[]> {
    return this.http.get<string[]>('/api/Data/GetRolesOfUser')
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  searchForQuestions(parameters: SearchQParameters): Promise<PagedResult<QuestionListEntry>> {
    return this.http.post<PagedResult<QuestionListEntry>>('/api/Data/SearchForQuestions', parameters)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  deleteCourseInstance(id: number): Promise<any> {
    return this.http.delete('/api/Data/DeleteCourseInstance/' + id)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  getCourseResult(courseInstanceId: number): Promise<Result> {
     return this.http.get<Result>('/api/Data/GetCourseResult/' + courseInstanceId)
               .toPromise()
               .catch((error) => this.handleError(error));
  }

  deleteQuestion(id: number): Promise<any> {
   return this.http.delete('/api/Data/DeleteQuestion/' + id)
            .toPromise()
            .catch((error) => this.handleError(error));
  }

  getMyQuestions(page: number): Promise<PagedResult<QuestionListEntry>>  {
       return this.http.get<PagedResult<QuestionListEntry>>('/api/Data/GetMyQuestions/' + page)
               .toPromise()
               .catch((error) => this.handleError(error));
  }

  getMyCourseInstances(page: number): Promise<PagedResult<CourseInstanceListEntry>> {
    return this.http.get<PagedResult<CourseInstanceListEntry>>('/api/Data/GetMyCourseInstances/' + page)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  answerQuestion(answer: QuestionAnswer): Promise<any> {
    return this.http.post<Question>('/api/Data/AnswerQuestion', answer)
    .toPromise()
    .catch((error) => this.handleError(error));
  }

  answerQuestionAndGetSolution(answer: QuestionAnswer): Promise<Question> {
     return this.http.post<Question>('/api/Data/AnswerQuestionAndGetSolution', answer)
                    .toPromise()
                    .catch((error) => this.handleError(error));
  }

  answerQuestionAndGetNextQuestion(answer: QuestionAnswer): Promise<CourseInstance> {
    return this.http.post<CourseInstance>('/api/Data/AnswerQuestionAndGetNextQuestion', answer)
                    .toPromise()
                    .catch((error) => this.handleError(error));
  }

  getQuestionToAsk(id: number): Promise<QuestionToAsk> {
    return this.http.get<QuestionToAsk>('/api/Data/GetQuestionToAsk/' + id)
                    .toPromise()
                    .catch((error) => this.handleError(error));
  }

  startCourse(courseId: number): Promise<CourseInstance> {
    const param = new StartCourse();
    param.idCourse = courseId;
    return this.http.post<CourseInstance>('/api/Data/StartCourse', param)
                .toPromise()
                .catch((error) => this.handleError(error));
  }

  findTags(name: string): Promise<Tag[]> {
   const requestOptions = {
    params: {searchTerm: name}
   };
   return this.http.get<Tag[]>('/api/Data/FindTags', requestOptions)
               .toPromise()
               .catch((error) => this.handleError(error));
  }

  getCourses(page: number): Promise<PagedResult<Course>> {
    return this.http.get<PagedResult<Course>>('/api/Data/GetCourses/' + page)
               .toPromise()
               .catch((error) => this.handleError(error));
  }

  insertQuestion(question: Question): Promise<number> {
    return this.http.post<number>('/api/Data/InsertQuestion', question)
                    .toPromise()
                    .catch((error) => this.handleError(error));
  }

  updateQuestion(question: Question): Promise<string> {
    return this.http.post<string>('/api/Data/UpdateQuestion', question)
                    .toPromise()
                    .catch((error) => this.handleError(error));
  }

  getQuestion(id: number): Promise<Question> {
    return this.http.get<Question>('/api/Data/GetQuestion/' + id)
                    .toPromise()
                    .catch((error) => this.handleError(error));
  }


  private async handleError(error: any): Promise<any> {
    // tslint:disable-next-line:no-debugger
    debugger;
    if (error.status === 401) {
      try {
        // some redundancy, less pain than reentering this method.
        const roles: string[] = await this.http.get<string[]>('/api/Data/GetRolesOfUser').toPromise();
        if (roles.length === 0) {
          const currentUrl = new URL(window.location);
          const pathname = currentUrl.pathname;
          const redirection = '/Account/Login?ReturnUrl=' + encodeURIComponent(pathname);
          window.location = redirection;
        }
      } catch (innerError) {
        console.log('error getting roles of user.');
      }
    }
    console.error('An error occurred', error);
    return Promise.reject(error.message || error);
  }
}
