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

export interface ICranDataService {
  getCourses(): Promise<Courses>;
  insertQuestion(question: Question): Promise<number>;
  getQuestion(id: number): Promise<Question>;
  getQuestionToAsk(id: number): Promise<QuestionToAsk>;
  updateQuestion(question: Question): Promise<any>;
  getTags(name: string): Promise<Tag[]>;
  startCourse(courseId: number): Promise<CourseInstance>;
  answerQuestionAndGetSolution(answer: QuestionAnswer): Promise<Question>;
  answerQuestionAndGetNextQuestion(answer: QuestionAnswer): Promise<CourseInstance>;
  getMyQuestions(): Promise<QuestionListEntry[]>;
  deleteQuestion(id: number): Promise<any>;
}
