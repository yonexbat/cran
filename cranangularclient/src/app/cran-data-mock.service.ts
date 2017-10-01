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


@Injectable()
export class CranDataServiceMock implements ICranDataService {

  getUserInfo(): Promise<UserInfo> {
    const promiseResult = new Promise<UserInfo>((resolve, reject) => {
      setTimeout(function() {
        const userInfo: UserInfo = {name: 'yonexi'};
        resolve(userInfo);
      }, 1000);
    });
    return promiseResult;
  }

  addImage(image: Image): Promise<Image> {
    const promiseResult = new Promise<Image>((resolve, reject) => {
      setTimeout(function() {
        resolve(image);
      }, 1000);
    });
    return promiseResult;
  }

  vote(votes: Votes): Promise<Votes> {
    const promiseResult = new Promise<Votes>((resolve, reject) => {
      setTimeout(function() {
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
      setTimeout(function() {
        resolve();
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

    const promiseResult = new Promise<PagedResult<Comment>>(function(resolve, reject){
      setTimeout(function() {
        resolve(result);
      }, 1000);
    });
    return promiseResult;
  }

  addComment(comment: Comment): Promise<number> {
    const promiseResult = new Promise<number>(function(resolve, reject){
      setTimeout(function() {
        resolve(9872);
      }, 1000);
    });
    return promiseResult;
  }

  getRolesOfUser(): Promise<string[]> {

    const promiseResult = new Promise<string[]>(function(resolve, reject){
      setTimeout(function() {
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
      {id: 1, title: `Frage ${parameters.page}`, status: 1, tags : [{id : 23, description : '', name : 'MyTag'}]},
      {id: 2, title: 'World', status: 1, tags : []},
      {id: 3, title: 'Frage 3',   status: 1, tags : []},
      {id: 4, title: 'Test mit einer Frage mit einem langen Titel',   status: 0, tags : []},
      {id: 4, title: 'zep',   status: 0, tags : [{id : 18, description : '', name : 'Some Tag'}]},
    ];

    result.data = myList;

    const promiseResult = new Promise<PagedResult<QuestionListEntry>>((resolve, reject) => {
      setTimeout(function() {
        resolve(result);
      }, 1000);
    });
    return promiseResult;
  }

  deleteCourseInstance(id: number): Promise<any> {

    const promiseResult = new Promise<any>(function(resolve, reject){
      setTimeout(function() {
        resolve();
      }, 1000);
    });
    return promiseResult;
  }

  getMyCourseInstances(): Promise<CourseInstanceListEntry[]> {
    const result: CourseInstanceListEntry[] = [
      {idCourseInstance: 1, insertDate: new Date(2017, 9, 8), percentage: 80, title: 'Course1'},
      {idCourseInstance: 2, insertDate: new Date(2017, 9, 25), percentage: 55, title: 'Course2'},
      {idCourseInstance: 3, insertDate: new Date(2017, 9, 25), percentage: 8, title:  'Course3'},
      {idCourseInstance: 4, insertDate: new Date(2017, 9, 25), percentage: 98, title: 'Course4'},
      {idCourseInstance: 5, insertDate: new Date(2017, 9, 25), percentage: 10, title: 'JavaScript'}
    ];


    const promiseResult = new Promise<CourseInstanceListEntry[]>(function(resolve, reject){
      setTimeout(function() {
        resolve(result);
      }, 1000);
    });
    return promiseResult;
  }


  getCourseResult(courseInstanceId: number): Promise<Result> {
    const result: Result = {
      idCourseInstance: 3,
      courseTitle: 'Dies und das',
      questions: [
        {correct: true, idCourseInstanceQuestion: 8000, idQuestion: 800, title: 'some title 1',
          tags: [{id: 2, description: '', name: 'Js'}, {id: 2, description: '', name: 'Tag2'}]},
        {correct: false, idCourseInstanceQuestion: 8001, idQuestion: 801, title: 'some title 2',
        tags: [{id: 2, description: '', name: 'Js'}]},
        {correct: false, idCourseInstanceQuestion: 8002, idQuestion: 802, title: 'some title 3',
        tags: [{id: 2, description: '', name: 'Js'}]},
        {correct: true, idCourseInstanceQuestion: 8003, idQuestion: 803, title: 'some title 4',
        tags: [{id: 2, description: '', name: 'Js'}]},
      ],
    };

    const promiseResult = new Promise<Result>(function(resolve, reject){
      setTimeout(function() {
        resolve(result);
      }, 1000);
    });
    return promiseResult;
  }

  deleteQuestion(id: number): Promise<any> {
    const promiseResult = new Promise<any>(function(resolve, reject){
      setTimeout(function() {
        resolve();
      }, 1000);
    });
    return promiseResult;
  }

  getMyQuestions(): Promise<QuestionListEntry[]> {
    const myList: QuestionListEntry[]  = [
      {id: 1, title: 'Hello', status: 1, tags : [{id : 23, description : '', name : 'MyTag'}]},
      {id: 2, title: 'World', status: 1, tags : []},
      {id: 3, title: 'Frage mit einem sehr langen Titel',   status: 1, tags : []},
      {id: 4, title: 'zep',   status: 0, tags : []},
    ];

    const promiseResult = new Promise<QuestionListEntry[]>(function(resolve, reject){
      setTimeout(function() {
        resolve(myList);
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

    const promiseResult = new Promise<CourseInstance>(function(resolve, reject){
      setTimeout(function() {
        resolve(questionResult);
      }, 1000);
    });
    return promiseResult;
  }

  getQuestionToAsk(id: number): Promise<QuestionToAsk> {
    return this.getQuestion(23).then((question: Question) => {
      const questiontoask: QuestionToAsk = {
        courseEnded: false,
        idCourseInstance: 342423,
        numQuestions: 23,
        idQuestion: 800,
        options: [],
        text: 'Ich frage mal nach',
        idCourseInstanceQuestion: id,
        numQuestionsAsked: 3,
        question: undefined,
        answered: false,
      };

      if (id >= 8000) {
        questiontoask.courseEnded = true;
        questiontoask.answered = true;
      }

      for (const option of question.options) {
        const ota = new QuestionOptionToAsk();
        ota.text = option.text;
        ota.isTrue = option.isTrue;
        questiontoask.options.push(ota);
      }
      return questiontoask;
    });
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

    const promiseResult = new Promise<CourseInstance>(function(resolve, reject){
      setTimeout(function() {
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

    const promiseResult = new Promise<Tag[]>(function(resolve, reject){
      setTimeout(function() {
        resolve(tags);
      }, 1000);
    });
    return promiseResult;
  }


  updateQuestion(question: Question): Promise<any> {
      return new Promise<any>((resolve, reject) => {
        setTimeout(function() {
          resolve('Ok');
        }, 1000);
      });
  }

  constructor(private http: Http) {

  }

  getCourses(): Promise<Courses> {
    const courses: Courses = {
      courses: [
        {id: 1, description: 'Test Kurs bla', title: 'Kursus',
          tags: [{id: 3, name: 'Js', description: 'desc'},
                 {id: 5, name: 'Hello', description: 'desc'}]},
        {id: 1, description: 'Test Kurs bla', title: 'Kursus', tags: [{id: 3, name: 'Js', description: 'desc'}]},
        {id: 1, description: 'Test Kurs bla', title: 'Kursus', tags: [{id: 3, name: 'Js', description: 'desc'}]},
        {id: 1, description: 'Test Kurs bla', title: 'Kursus', tags: [{id: 3, name: 'Js', description: 'desc'}]},
        {id: 1, description: 'Test Kurs bla', title: 'Kursus', tags: [{id: 3, name: 'Js', description: 'desc'}]},
        {id: 1, description: 'GLOBI in den Ferien', title: 'Kursus', tags: [{id: 3, name: 'Js', description: 'desc'}]},
      ],
    };

    const promiseResult: Promise<Courses> = new Promise<Courses>((resolve, reject) => {
      setTimeout(function() {
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

        const question = new Question();
        question.text = 'Wie alt ist unsere Karotte?';
        question.title = 'MyTitle';
        question.id = id;
        question.explanation = 'My explanation';
        question.status = 1;
        question.isEditable = true;
        question.votes = {
          downVotes: 2,
          upVotes: 12,
          idQuestion: question.id,
          myVote: 0,
        };

        question.options = [
          {isTrue : true, text : '1 Jahr'},
          {isTrue : false, text : '2 Jahre'},
          {isTrue : true, text : '4 Jahre'},
          {isTrue : false, text : '5 Jahre'},
        ];

        question.tags = [
          {id: 34, description: 'helo cran', name: 'cran'},
        ];

        setTimeout(function() {
          resolve(question);
        }, 1000);
      });
  }

}
