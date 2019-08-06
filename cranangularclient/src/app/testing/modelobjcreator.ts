import { Tag } from '../model/tag';
import { Course } from '../model/course';

export function createCoursesTestObjs(): Course[] {
    const courseList: Course[] =  [
        {id: 1, language: 'De', description: 'Test Kurs bla', title: 'Kursus',
          numQuestionsToAsk: 3,
          isFavorite: true,
          isEditable: true,
          tags: [{id: 3, name: 'Js', description: 'desc',  shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, },
                 {id: 5, name: 'Hello', description: 'desc',  shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
        {id: 1, language: 'De', numQuestionsToAsk: 3,
          description: 'Test Kurs bla', title: 'Kursus',
          isFavorite: false,
          isEditable: false,
            tags: [{id: 3, name: 'Js', description: 'desc', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
        {id: 1, language: 'De', numQuestionsToAsk: 3,
          isFavorite: true,
          isEditable: true,
          description: 'Test Kurs bla', title: 'Kursus',
            tags: [{id: 3, name: 'Js', description: 'desc', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
        {id: 1, language: 'De', numQuestionsToAsk: 3,
          description: 'Test Kurs bla', title: 'Kursus',
          isFavorite: true,
          isEditable: false,
            tags: [{id: 3, name: 'Js', description: 'desc', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
        {id: 1, language: 'De', numQuestionsToAsk: 3,
          description: 'Test Kurs bla', title: 'Kursus',
          isFavorite: true,
          isEditable: false,
            tags: [{id: 3, name: 'Js', description: 'desc', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
        {id: 1, language: 'De', numQuestionsToAsk: 3,
          isFavorite: true,
          isEditable: true,
          description: 'GLOBI in den Ferien', title: 'Kursus',
         tags: [{id: 3, name: 'Js', description: 'desc', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, }]},
      ];
    return courseList;
}
export function createTags() {

}

