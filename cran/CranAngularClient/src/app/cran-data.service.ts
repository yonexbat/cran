import { Injectable, InjectionToken  } from '@angular/core';
import { Headers, Http, RequestOptionsArgs } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import {Courses} from './model/courses';
import {Course} from './model/course';
import {Question} from './model/question';
import {QuestionOption} from './model/questionoption';
import {ICranDataService} from './icrandataservice';
import { Tag } from './model/tag';
import {StartCourse} from './model/startcourse';
import {CourseInstance} from './model/courseinstance';
import {QuestionToAsk} from './model/questiontoask';
import {QuestionOptionToAsk} from './model/questionoptiontoask';
import {QuestionAnswer} from './model/questionanswer';
import {QuestionListEntry} from './model/questionlistentry';
import {Result} from './model/result';
import {QuestionResult} from './model/questionresult';


export let CRAN_SERVICE_TOKEN = new InjectionToken<ICranDataService>('ICranDataService');

@Injectable()
export class CranDataService implements ICranDataService {

  constructor(private http: Http) {

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

@Injectable()
export class CranDataServiceMock implements ICranDataService {


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
      {id: 2, title: 'Hello', status: 1, tags : [{id : 23, description : '', name : 'MyTag'}]},
      {id: 2, title: 'World', status: 1, tags : []},
      {id: 2, title: 'yep',   status: 1, tags : []},
      {id: 2, title: 'zep',   status: 0, tags : []},
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
