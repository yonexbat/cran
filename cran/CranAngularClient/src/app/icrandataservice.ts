import { Injectable, InjectionToken  } from '@angular/core';

import {Courses} from './model/courses';
import {Course} from './model/course';
import {Question} from './model/question';
import {Tag} from './model/tag';
import {StartCourse} from './model/startcourse';
import {CourseInstance} from './model/courseinstance';
import {QuestionToAsk} from './model/questiontoask';
import {QuestionAnswer} from './model/questionanswer';
import {QuestionListEntry} from './model/questionlistentry';
import {Result} from './model/result';
import {QuestionResult} from './model/questionresult';
import {CourseInstanceListEntry} from './model/courseinstancelistentry';
import {SearchQParameters} from './model/searchqparameters';
import {PagedResult} from './model/pagedresult';

export interface ICranDataService {
  getCourses(): Promise<Courses>;
  insertQuestion(question: Question): Promise<number>;
  getQuestion(id: number): Promise<Question>;
  getQuestionToAsk(id: number): Promise<QuestionToAsk>;
  updateQuestion(question: Question): Promise<any>;
  findTags(name: string): Promise<Tag[]>;
  startCourse(courseId: number): Promise<CourseInstance>;
  answerQuestionAndGetSolution(answer: QuestionAnswer): Promise<Question>;
  answerQuestionAndGetNextQuestion(answer: QuestionAnswer): Promise<CourseInstance>;
  getMyQuestions(): Promise<QuestionListEntry[]>;
  getMyCourseInstances(): Promise<CourseInstanceListEntry[]>;
  deleteQuestion(id: number): Promise<any>;
  deleteCourseInstance(id: number): Promise<any>;
  getCourseResult(courseInstanceId: number): Promise<Result>;
  searchForQuestions(parameters: SearchQParameters): Promise<PagedResult<QuestionListEntry>>;
  getRolesOfUser(): Promise<string[]>;
}
