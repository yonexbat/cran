import { Tag } from '../model/tag';
import { Course } from '../model/course';
import { Question } from '../model/question';

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

export function createQuestionTestObj(id: number): Question {
  const question = new Question();
  question.text = 'Wie alt ist unsere Karotte?';
  question.title = 'Frage über Katze die die Farbe einer Karotte hat';
  question.id = id;
  question.explanation = 'Ist Alt, benimmt sich aber wie ein kleines Büsi';
  if (id > 10) {
    question.status = 1;
  } else {
    question.status = 0;
  }
  question.isEditable = true;
  question.language = 'De';
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

  question.tags = createTagsTestObjs();
  return question;
}

export function createTagsTestObjs(): Tag[] {
  const tags = [
    {id: 34, description: 'helo cran', name: 'cran', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, },
  ];
  return tags;
}



