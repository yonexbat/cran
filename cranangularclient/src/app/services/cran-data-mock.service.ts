import { Injectable, InjectionToken  } from '@angular/core';


import {Course} from '../model/course';
import {Question} from '../model/question';
import {QuestionOption} from '../model/questionoption';
import {ICranDataService} from './icrandataservice';
import {Tag} from '../model/tag';
import {StartCourse} from '../model/startcourse';
import {CourseInstance} from '../model/courseinstance';
import {QuestionToAsk} from '../model/questiontoask';
import {QuestionOptionToAsk} from '../model/questionoptiontoask';
import {QuestionAnswer} from '../model/questionanswer';
import {QuestionListEntry} from '../model/questionlistentry';
import {Result} from '../model/result';
import {QuestionResult} from '../model/questionresult';
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
import {QuestionStatus} from '../model/questionstatus';
import {QuestionType} from '../model/questiontype';
import {SubscriptionShort} from '../model/subscriptionshort';
import {Notification} from '../model/notification';
import {CourseToFavorites} from '../model/coursetofavorites';
import {createCoursesTestObjs, createQuestionTestObj,
  createQuestiontoAskTestObj} from '../testing/modelobjcreator';


@Injectable()
export class CranDataServiceMock implements ICranDataService {

  addCourseToFavorites(favorite: CourseToFavorites): Promise<any> {
    const promiseResult = new Promise<any>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
    });
    return promiseResult;
  }
  removeCoureFromFavorites(favorite: CourseToFavorites): Promise<any> {
    const promiseResult = new Promise<any>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
    });
    return promiseResult;
  }

  getFavoriteCourses(page: number): Promise<PagedResult<Course>> {
    return this.getCourses(page);
  }

  sendNotificationToUser(message: Notification): Promise<any> {
    const promiseResult = new Promise<any>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
    });
    return promiseResult;
  }

  getAllSubscriptions(page: number): Promise<PagedResult<SubscriptionShort>> {

    const pagedResult = new PagedResult<SubscriptionShort>();
    pagedResult.data = [];
    pagedResult.currentPage = 0;
    pagedResult.numpages = 4;

    for (let i = 0; i < 10; i++) {
      pagedResult.data.push({
        id: i,
        endpoint: `endpoint ${i}`,
        userId: `userid${i}`,
      });
    }

    const promiseResult = new Promise<PagedResult<SubscriptionShort>>((resolve, reject) => {
      setTimeout(() => {
        resolve(pagedResult);
      }, 1000);
    });
    return promiseResult;
  }

  addPushRegistration(subscription: any): Promise<any> {
    const promiseResult = new Promise<any>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
    });
    return promiseResult;
  }

  getVersions(parameters: VersionInfoParameters): Promise<PagedResult<VersionInfo>> {
    const texts: VersionInfo[] = [];
    for (let i = 0; i < 5; i++) {
      texts.push({idQuestion: i, user: 'cran', version: i + 1, insertDate: new Date(), approvalDate: new Date(),
      status: QuestionStatus.Released});
    }
    const pagedResult = new PagedResult<VersionInfo>();
    pagedResult.data = texts;
    pagedResult.currentPage = parameters.page;
    pagedResult.numpages = 4;
    pagedResult.count = 20;
    const promiseResult = new Promise<PagedResult<VersionInfo>>((resolve, reject) => {
      setTimeout(() => {
        resolve(pagedResult);
      }, 1000);
    });
    return promiseResult;
  }

  getTextDtoByKey(key: string): Promise<Text> {
    const text: Text = {
      contentDe: 'ContentDe <b>boldy</b>',
      contentEn: 'ContentEn',
      key: 'MyKey',
      id: 1,
    };
    const promiseResult = new Promise<Text>((resolve, reject) => {
      setTimeout(() => {
        resolve(text);
      }, 1000);
    });
    return promiseResult;
  }

  getTextDto(id: number): Promise<Text> {
    const text: Text = {
      contentDe: 'ContentDe',
      contentEn: 'ContentEn',
      key: 'MyKey',
      id: 1,
    };
    const promiseResult = new Promise<Text>((resolve, reject) => {
      setTimeout(() => {
        resolve(text);
      }, 1000);
    });
    return promiseResult;
  }

  updateText(text: Text): Promise<any> {
    const promiseResult = new Promise<Text>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
    });
    return promiseResult;
  }

  getTexts(search: SearchText): Promise<PagedResult<Text>> {
    const texts: Text[] = [];
    for (let i = 0; i < 10; i++) {
      texts.push({
          contentDe: 'ContentDe',
          contentEn: 'ContentEn',
          key: 'MyKey',
          id: 1,
      });
    }
    const pagedResult = new PagedResult<Text>();
    pagedResult.data = texts;
    pagedResult.currentPage = 0;
    pagedResult.numpages = 4;
    const promiseResult = new Promise<PagedResult<Text>>((resolve, reject) => {
      setTimeout(() => {
        resolve(pagedResult);
      }, 1000);
    });
    return promiseResult;
  }

  getTags(ids: number[]): Promise<Tag[]> {

    const tags: Tag[]  = ids.map(id => {
      const tag: Tag = {
        id,
        description: 'desc',
        name: 'name',
        shortDescDe: 'ShortDesc de',
        shortDescEn: 'ShortDesc en',
        idTagType: 1,
      };
      return tag;
    });

    const promiseResult = new Promise<Tag[]>((resolve, reject) => {
      setTimeout(() => {
        resolve(tags);
      }, 1000);
    });
    return promiseResult;
  }

  answerQuestion(answer: QuestionAnswer): Promise<any> {
    const promiseResult = new Promise<any>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
    });
    return promiseResult;
  }

  versionQuestion(id: number): Promise<number> {
     const promiseResult = new Promise<number>((resolve, reject) => {
      setTimeout(() => {
        resolve(id + 1);
      }, 1000);
     });
     return promiseResult;
  }

  acceptQuestion(id: number): Promise<any> {
      const promiseResult = new Promise<any>((resolve, reject) => {
      setTimeout(() => {
          resolve('OK');
        }, 1000);
      });
      return promiseResult;
  }

  copyQuestion(id: number): Promise<number> {
    const promiseResult = new Promise<number>((resolve, reject) => {
      setTimeout(() => {
        resolve(id - 1);
      }, 1000);
    });
    return promiseResult;
  }

  deleteTag(id: number): Promise<any> {
    const promiseResult = new Promise<any>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
    });
    return promiseResult;
  }

  getCourse(id: number): Promise<Course> {
    const promiseResult = new Promise<Course>((resolve, reject) => {
      setTimeout(() => {
        const athing: Course = {
            id,  language: 'De',
            title: 'CouseTitle' ,
            description: 'desc',
            numQuestionsToAsk: 5,
            isFavorite: false,
            isEditable: true,
          tags: [
            {id: 1, description: 'Desc', name: 'TagName', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }
          ] };
        resolve(athing);
      }, 1000);
    });
    return promiseResult;
  }

  insertCourse(course: Course): Promise<number> {
    const promiseResult = new Promise<number>((resolve, reject) => {
      setTimeout(() => {
        resolve(1);
      }, 1000);
    });
    return promiseResult;
  }

  updateCourse(course: Course): Promise<any> {
    const promiseResult = new Promise<any>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
    });
    return promiseResult;
  }

  getTag(id: number): Promise<Tag> {
    const promiseResult = new Promise<Tag>((resolve, reject) => {
      setTimeout(() => {
        const tag: Tag = {
          id,
          description: 'desc',
          name: 'tag1',
          shortDescDe: 'ShortDesc de',
          shortDescEn: 'ShortDesc en',
          idTagType: 1,
        };
        resolve(tag);
      }, 1000);
    });
    return promiseResult;
  }

  insertTag(tag: Tag): Promise<number> {
    const promiseResult = new Promise<number>((resolve, reject) => {
      setTimeout(() => {
        resolve(16);
      }, 1000);
    });
    return promiseResult;
  }

  updateTag(tag: Tag): Promise<any> {
    const promiseResult = new Promise<number>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
     });
    return promiseResult;
  }

  searchForTags(parameters: SearchTags): Promise<PagedResult<Tag>> {
    const tags: Tag[] = [
      {id: 1, description: 'Desc tag 1', name: 'Tag1', shortDescDe: 'ShortDesc de', shortDescEn: 'ShortDesc en', idTagType: 1, },
      {id: 2, description: 'Desc tag 2', name: 'Tag2', shortDescDe: 'ShortDesc de', shortDescEn: 'ShortDesc en', idTagType: 1, },
      {id: 3, description: 'Desc tag 3', name: 'Tag3', shortDescDe: 'ShortDesc de', shortDescEn: 'ShortDesc en', idTagType: 1, },
      {id: 4, description: 'Desc tag 4', name: 'Tag4', shortDescDe: 'ShortDesc de', shortDescEn: 'ShortDesc en', idTagType: 1, },
      {id: 5, description: 'Desc tag 5', name: 'Tag5', shortDescDe: 'ShortDesc de', shortDescEn: 'ShortDesc en', idTagType: 1, },
    ];

    const pagedResult = new PagedResult<Tag>();
    pagedResult.data = tags;
    pagedResult.currentPage = 0;
    pagedResult.count = tags.length;

    const promiseResult = new Promise<PagedResult<Tag>>((resolve, reject) => {
      setTimeout(() => {
        resolve(pagedResult);
      }, 1000);
    });
    return promiseResult;
  }



  getUserInfo(): Promise<UserInfo> {
    const promiseResult = new Promise<UserInfo>((resolve, reject) => {
      setTimeout(() => {
        const userInfo: UserInfo = {name: 'yonexi', isAnonymous: false, };
        resolve(userInfo);
      }, 1000);
    });
    return promiseResult;
  }

  addImage(image: Image): Promise<Image> {
    const promiseResult = new Promise<Image>((resolve, reject) => {
      setTimeout(() => {
        resolve(image);
      }, 1000);
    });
    return promiseResult;
  }

  vote(votes: Votes): Promise<Votes> {
    const promiseResult = new Promise<Votes>((resolve, reject) => {
      setTimeout(() => {
        if (votes.myVote > 0) {
          votes.upVotes++;
        }
        if (votes.myVote < 0) {
          votes.downVotes++;
        }
        resolve(votes);
      }, 1000);
    });
    return promiseResult;
  }

  deleteComment(id: number): Promise<any> {
    const promiseResult = new Promise<any>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
    });
    return promiseResult;
  }

  getComments(parameters: GetComments): Promise<PagedResult<Comment>> {

    const result = new PagedResult<Comment>();
    result.currentPage = parameters.page;
    result.numpages = 4;
    result.count = 23;
    result.data = [
      {commentText: 'mein kommentar', idComment: 2, isEditable: false, idQuestion: parameters.idQuestion,
        userId: 'yoni', insertDate: new Date(2017, 9, 8), updateDate:  new Date(2017, 9, 8)},
      {commentText: 'mein kommentar', idComment: 2, isEditable: false, idQuestion: parameters.idQuestion,
      userId: 'yoni', insertDate: new Date(2017, 9, 8), updateDate:  new Date(2017, 9, 8)},
      {commentText: 'mein kommentar', idComment: 2, isEditable: false, idQuestion: parameters.idQuestion,
      userId: 'yoni', insertDate: new Date(2017, 9, 8), updateDate:  new Date(2017, 9, 8)},
      {commentText: 'mein kommentar', idComment: 2, isEditable: false, idQuestion: parameters.idQuestion,
      userId: 'yoni', insertDate: new Date(2017, 9, 8), updateDate:  new Date(2017, 9, 8)},
      {commentText: 'mein kommentar', idComment: 2, isEditable: false, idQuestion: parameters.idQuestion,
      userId: 'yoni', insertDate: new Date(2017, 9, 8), updateDate:  new Date(2017, 9, 8)},
      {commentText: 'mein kommentar', idComment: 2, isEditable: false, idQuestion: parameters.idQuestion,
      userId: 'yoni', insertDate: new Date(2017, 9, 8), updateDate:  new Date(2017, 9, 8)},
    ];

    const promiseResult = new Promise<PagedResult<Comment>>((resolve, reject) => {
      setTimeout(() => {
        resolve(result);
      }, 1000);
    });
    return promiseResult;
  }

  addComment(comment: Comment): Promise<number> {
    const promiseResult = new Promise<number>((resolve, reject) => {
      setTimeout(() => {
        resolve(9872);
      }, 1000);
    });
    return promiseResult;
  }

  getRolesOfUser(): Promise<string[]> {

    const promiseResult = new Promise<string[]>((resolve, reject) => {
      setTimeout(() => {
        resolve(['admin', 'user']);
      }, 1000);
    });
    return promiseResult;
  }

  searchForQuestions(parameters: SearchQParameters): Promise<PagedResult<QuestionListEntry>> {
    const result = new PagedResult<QuestionListEntry>();
    result.currentPage = parameters.page;
    result.numpages = 13;
    result.pagesize = 5;
    result.count = 88;

    const myList: QuestionListEntry[]  = [
      {id: 1, title: `Frage ${parameters.page}`, status: 1,
        tags : [{id : 23, description : '', name : 'MyTag', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
      {id: 2, title: 'World', status: 1, tags : []},
      {id: 3, title: 'Frage 3',   status: 1, tags : []},
      {id: 4, title: 'Test mit einer Frage mit einem langen Titel',   status: 0, tags : []},
      {id: 4, title: 'zep',   status: 0,
        tags : [{id : 18, description : '', name : 'Some Tag', shortDescDe: 'sd de', shortDescEn: 'sd en', idTagType: 1, }]},
    ];

    result.data = myList;

    const promiseResult = new Promise<PagedResult<QuestionListEntry>>((resolve, reject) => {
      setTimeout(() => {
        resolve(result);
      }, 1000);
    });
    return promiseResult;
  }

  deleteCourseInstance(id: number): Promise<any> {

    const promiseResult = new Promise<any>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
    });
    return promiseResult;
  }

  getMyCourseInstances(page: number): Promise<PagedResult<CourseInstanceListEntry>> {
    const instanceList: CourseInstanceListEntry[] = [
      {idCourseInstance: 1, insertDate: new Date(2017, 9, 8), percentage: 80, title: 'Course1'},
      {idCourseInstance: 2, insertDate: new Date(2017, 9, 25), percentage: 55, title: 'Course2'},
      {idCourseInstance: 3, insertDate: new Date(2017, 9, 25), percentage: 8, title:  'Course3'},
      {idCourseInstance: 4, insertDate: new Date(2017, 9, 25), percentage: 98, title: 'Course4'},
      {idCourseInstance: 5, insertDate: new Date(2017, 9, 25), percentage: 10, title: 'JavaScript'}
    ];
    const result: PagedResult<CourseInstanceListEntry> = new PagedResult<CourseInstanceListEntry>();
    result.pagesize = 5;
    result.count = 100;
    result.data = instanceList;
    result.currentPage = page;
    result.numpages = 20;

    const promiseResult = new Promise<PagedResult<CourseInstanceListEntry>>((resolve, reject) => {
      setTimeout(() => {
        resolve(result);
      }, 1000);
    });
    return promiseResult;
  }


  getCourseResult(courseInstanceId: number): Promise<Result> {



    const questions: QuestionResult[] =  [
      {correct: true, idCourseInstanceQuestion: 8000, idQuestion: 800, title: 'some title 1',
        tags: [{id: 2, description: '', name: 'Js',  shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, },
         {id: 2, description: '', name: 'Tag2', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
      {correct: false, idCourseInstanceQuestion: 8001, idQuestion: 801, title: 'some title 2',
      tags: [{id: 2, description: '', name: 'Js', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
      {correct: false, idCourseInstanceQuestion: 8002, idQuestion: 802, title: 'some title 3',
      tags: [{id: 2, description: '', name: 'Js', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
      {correct: true, idCourseInstanceQuestion: 8003, idQuestion: 803, title: 'some title 4',
      tags: [{id: 2, description: '', name: 'Js', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
    ];

    const result: Result = {
      idCourseInstance: courseInstanceId,
      idCourse: 2,
      courseTitle: 'Dies und das',
      startedAt: new Date('2018-12-17T03:24:00'),
      endedAt: new Date('2018-12-17T03:50:00'),
      questions,
    };

    const promiseResult = new Promise<Result>((resolve, reject) => {
      setTimeout(() => {
        resolve(result);
      }, 1000);
    });
    return promiseResult;
  }

  deleteQuestion(id: number): Promise<any> {
    const promiseResult = new Promise<any>((resolve, reject) => {
      setTimeout(() => {
        resolve(undefined);
      }, 1000);
    });
    return promiseResult;
  }

  getMyQuestions(page: number): Promise<PagedResult<QuestionListEntry>> {
    const myList: QuestionListEntry[]  = [
      {id: 1, title: 'Hello', status: 1,
        tags : [{id : 23, description : '', name : 'MyTag', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
      {id: 2, title: 'World', status: 1, tags : []},
      {id: 3, title: 'Frage mit einem sehr langen Titel',   status: 1, tags : []},
      {id: 4, title: 'zep',   status: 0, tags : []},
    ];

    const result: PagedResult<QuestionListEntry> = new  PagedResult<QuestionListEntry>();
    result.data = myList;
    result.count = myList.length;
    result.pagesize = 5;
    result.numpages = 5;
    result.currentPage = page;

    const promiseResult = new Promise<PagedResult<QuestionListEntry>>((resolve, reject) => {
      setTimeout(() => {
        resolve(result);
      }, 1000);
    });
    return promiseResult;
  }

  answerQuestionAndGetSolution(answer: QuestionAnswer): Promise<Question> {
    return this.getQuestion(4);
  }

  answerQuestionAndGetNextQuestion(answer: QuestionAnswer): Promise<CourseInstance> {
    const questionResult = new CourseInstance();
    questionResult.idCourseInstanceQuestion = 2432;

    const promiseResult = new Promise<CourseInstance>((resolve, reject) => {
      setTimeout(() => {
        resolve(questionResult);
      }, 1000);
    });
    return promiseResult;
  }

  getQuestionToAsk(id: number): Promise<QuestionToAsk> {
    const questionToAsk = createQuestiontoAskTestObj();
    if (id >= 8000) {
      questionToAsk.courseEnded = true;
      questionToAsk.answered = true;
    }

    const promiseResult = new Promise<QuestionToAsk>((resolve, reject) => {
      setTimeout(() => {
        resolve(questionToAsk);
      }, 1000);
    });
    return promiseResult;
  }

  startCourse(courseId: number): Promise<CourseInstance> {

    const result: CourseInstance = {
      idCourseInstance: 23,
      answeredCorrectly: false,
      idCourseInstanceQuestion: 234,
      done: false,
      numQuestionsAlreadyAsked: 12,
      numQuestionsTotal: 14,
    };

    const promiseResult = new Promise<CourseInstance>((resolve, reject) => {
      setTimeout(() => {
        resolve(result);
      }, 1000);
    });
    return promiseResult;
  }

  findTags(name: string): Promise<Tag[]> {
    const tags: Tag[] = [];
    tags.push({
      id : 1,
      name : 'JS',
      description : 'Javascipt',
      shortDescDe: 'ShortDesc de',
      shortDescEn: 'ShortDesc en',
      idTagType: 1,
    });

    tags.push({
      id : 2,
      name : 'HTML',
      description : 'Html',
      shortDescDe: 'ShortDesc de',
      shortDescEn: 'ShortDesc en',
      idTagType: 1,
    });

    tags.push({
      id : 3,
      name : 'C#',
      description : 'C SHarp',
      shortDescDe: 'ShortDesc de',
      shortDescEn: 'ShortDesc en',
      idTagType: 1,
    });

    tags.push({
      id : 4,
      name : 'Java',
      description : '',
      shortDescDe: 'ShortDesc de',
      shortDescEn: 'ShortDesc en',
      idTagType: 1,
    });

    const promiseResult = new Promise<Tag[]>((resolve, reject) => {
      setTimeout(() => {
        resolve(tags);
      }, 1000);
    });
    return promiseResult;
  }


  updateQuestion(question: Question): Promise<any> {
      return new Promise<any>((resolve, reject) => {
        setTimeout(() => {
          resolve('Ok');
        }, 1000);
      });
  }

  constructor() {

  }

  getCourses(page: number): Promise<PagedResult<Course>> {
    const courseList: Course[] = createCoursesTestObjs();

    const courses: PagedResult<Course> = {
      data: courseList,
      count: 100,
      currentPage: page,
      numpages: 100,
      pagesize: 5,
    };

    const promiseResult: Promise<PagedResult<Course>> = new Promise<PagedResult<Course>>((resolve, reject) => {
      setTimeout(() => {
        resolve(courses);
      }, 1000);
    });
    return promiseResult;
  }

  insertQuestion(question: Question): Promise<number> {
    return new Promise<number>((resolve, reject) => {
      resolve(3);
    });
  }

  getQuestion(id: number): Promise<Question> {
    return new Promise<Question>((resolve, reject) => {
        const question =  createQuestionTestObj(id);
        setTimeout(() => {
          resolve(question);
        }, 1000);
      });
  }

}
