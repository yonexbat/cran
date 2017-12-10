import { Injectable, InjectionToken  } from '@angular/core';


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
import {Comment} from './model/comment';
import {GetComments} from './model/getcomments';
import {Votes} from './model/votes';
import {Image} from './model/image';
import {UserInfo} from './model/userinfo';
import {SearchTags} from './model/searchtags';

export interface ICranDataService {
  getCourses(page: number): Promise<PagedResult<Course>>;
  getCourse(id: number): Promise<Course>;
  insertCourse(course: Course): Promise<number>;
  updateCourse(course: Course): Promise<any>;
  insertQuestion(question: Question): Promise<number>;
  copyQuestion(id: number): Promise<number>;
  getQuestion(id: number): Promise<Question>;
  getQuestionToAsk(id: number): Promise<QuestionToAsk>;
  updateQuestion(question: Question): Promise<any>;
  findTags(name: string): Promise<Tag[]>;
  getTag(id: number): Promise<Tag>;
  insertTag(tag: Tag): Promise<number>;
  updateTag(tag: Tag): Promise<any>;
  deleteTag(id: number): Promise<any>;
  searchForTags(parameters: SearchTags): Promise<PagedResult<Tag>>;
  startCourse(courseId: number): Promise<CourseInstance>;
  answerQuestionAndGetSolution(answer: QuestionAnswer): Promise<Question>;
  answerQuestionAndGetNextQuestion(answer: QuestionAnswer): Promise<CourseInstance>;
  getMyQuestions(page: number): Promise<PagedResult<QuestionListEntry>>;
  getMyCourseInstances(page: number): Promise<PagedResult<CourseInstanceListEntry>>;
  deleteQuestion(id: number): Promise<any>;
  deleteCourseInstance(id: number): Promise<any>;
  getCourseResult(courseInstanceId: number): Promise<Result>;
  searchForQuestions(parameters: SearchQParameters): Promise<PagedResult<QuestionListEntry>>;
  getRolesOfUser(): Promise<string[]>;
  addComment(comment: Comment): Promise<number>;
  getComments(parameters: GetComments): Promise<PagedResult<Comment>>;
  deleteComment(id: number): Promise<any>;
  vote(votes: Votes): Promise<Votes>;
  addImage(image: Image): Promise<Image>;
  getUserInfo(): Promise<UserInfo>;
}
