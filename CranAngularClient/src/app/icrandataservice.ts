import { Injectable, InjectionToken  } from '@angular/core';

import {Courses} from './model/courses';
import {Course} from './model/course';
import {Question} from './model/question';
import {Tag} from './model/tag';

export interface ICranDataService {
  getCourses(): Promise<Courses>;
  insertQuestion(question: Question): Promise<number>;
  getQuestion(id: number): Promise<Question>;
  updateQuestion(question: Question): Promise<any>;
  getTags(name: string): Promise<Tag[]>;
}
