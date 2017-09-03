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
export class CranDataServiceMock implements ICranDataService {

  searchForQuestions(parameters: SearchQParameters): Promise<PagedResult<QuestionListEntry>> {
    const result = new PagedResult<QuestionListEntry>();
    result.currentPage = parameters.page;
    result.numpages = 17;
    result.pagesize = 5;

    const myList: QuestionListEntry[]  = [
      {id: 1, title: `Frage ${parameters.page}`, status: 1, tags : [{id : 23, description : '', name : 'MyTag'}]},
      {id: 2, title: 'World', status: 1, tags : []},
      {id: 3, title: 'Frage 3',   status: 1, tags : []},
      {id: 4, title: 'zep',   status: 0, tags : []},
      {id: 4, title: 'zep',   status: 0, tags : [{id : 18, description : '', name : 'Some Tag'}]},
    ];

    result.data = myList;
    return Promise.resolve(result);
  }

  deleteCourseInstance(id: number): Promise<any> {
    return Promise.resolve();
  }

  getMyCourseInstances(): Promise<CourseInstanceListEntry[]> {
    const result: CourseInstanceListEntry[] = [
      {idCourseInstance: 1, insertDate: new Date(2017, 9, 8), percentage: 80, title: 'Course1'},
      {idCourseInstance: 2, insertDate: new Date(2017, 9, 25), percentage: 55, title: 'Course2'},
      {idCourseInstance: 3, insertDate: new Date(2017, 9, 25), percentage: 8, title:  'Course3'},
      {idCourseInstance: 4, insertDate: new Date(2017, 9, 25), percentage: 98, title: 'Course4'},
      {idCourseInstance: 5, insertDate: new Date(2017, 9, 25), percentage: 10, title: 'JavaScript'}
    ];
    return Promise.resolve(result);
  }


  getCourseResult(courseInstanceId: number): Promise<Result> {
    const result: Result = {
      idCourseInstance: 3,
      courseTitle: 'Dies und das',
      questions: [
        {correct: true, idCourseInstanceQuestion: 1, title: 'some title 1'},
        {correct: false, idCourseInstanceQuestion: 2, title: 'some title 2'},
        {correct: false, idCourseInstanceQuestion: 3, title: 'some title 3'},
        {correct: true, idCourseInstanceQuestion: 4, title: 'some title 4'},
      ],
    };
    return Promise.resolve(result);
  }

  deleteQuestion(id: number): Promise<any> {
    return Promise.resolve();
  }

  getMyQuestions(): Promise<QuestionListEntry[]> {
    const myList: QuestionListEntry[]  = [
      {id: 1, title: 'Hello', status: 1, tags : [{id : 23, description : '', name : 'MyTag'}]},
      {id: 2, title: 'World', status: 1, tags : []},
      {id: 3, title: 'yep',   status: 1, tags : []},
      {id: 4, title: 'zep',   status: 0, tags : []},
    ];
    return Promise.resolve(myList);
  }

  answerQuestionAndGetSolution(answer: QuestionAnswer): Promise<Question> {
    return this.getQuestion(4);
  }

  answerQuestionAndGetNextQuestion(answer: QuestionAnswer): Promise<CourseInstance> {
    const questionResult = new CourseInstance();
    questionResult.idCourseInstanceQuestion = 2432;
    return Promise.resolve(questionResult);
  }

  getQuestionToAsk(id: number): Promise<QuestionToAsk> {
    return this.getQuestion(23).then((question: Question) => {
      const questiontoask = new QuestionToAsk();
      questiontoask.text = question.text;
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
    const result = new CourseInstance();
    result.idCourseInstance = 234;
    result.idCourseInstanceQuestion = 354;
    return Promise.resolve(result);
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
        question.text = 'Wie alt ist unsere Karotte?';
        question.title = 'MyTitle';
        question.id = id;
        question.explanation = 'My explanation';
        question.status = 1;

        question.options = [
          {isTrue : true, text : '1 Jahr'},
          {isTrue : false, text : '2 Jahre'},
          {isTrue : true, text : '4 Jahre'},
          {isTrue : false, text : '5 Jahre'},
        ];

        resolve(question);
      });
  }

}
