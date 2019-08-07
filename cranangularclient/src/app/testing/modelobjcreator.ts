import { Tag } from '../model/tag';
import { Course } from '../model/course';
import { Question } from '../model/question';
import { QuestionToAsk } from '../model/questiontoask';
import { QuestionType } from '../model/questiontype';
import { Comment } from '../model/comment';

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

export function createQuestiontoAskTestObj(): QuestionToAsk {
  const question = createQuestionTestObj(1);
  const questiontoAsk: QuestionToAsk = {
    idCourseInstance: 1,
    idQuestion: 1,
    answered: false,
    courseEnded: false,
    answerShown: false,
    idCourseInstanceQuestion: 1,
    numQuestions: 6,
    text: question.text,
    numCurrentQuestion: 2,
    questionType: QuestionType.MultipleChoice,
    question,
    options: [],
    questionSelectors: [],
  };
  let i = 1000;
  for (const option of question.options) {
    questiontoAsk.options.push(
      {
        text: option.text,
        idCourseInstanceQuestionOption: i++,
        isChecked: false,
        isEditable: true,
        isTrue: true,
    });
  }
  questiontoAsk.questionSelectors.push({
    answerShown: false,
    idCourseInstanceQuestion: 1,
    correct: null,
    number: 1,
  });
  questiontoAsk.questionSelectors.push({
    answerShown: false,
    idCourseInstanceQuestion: i + 101,
    correct: null,
    number: 2,
  });
  return questiontoAsk;
}

export function createTagsTestObjs(): Tag[] {
  const tags = [
    {id: 34, description: 'helo cran', name: 'cran', shortDescDe: 'short desc de', shortDescEn: 'short desc en', idTagType: 1, },
  ];
  return tags;
}

export function createCommentsTestObjs(): Comment[] {
  const comment1: Comment = {
    commentText: 'comment1',
    idComment: 1,
    idQuestion: 1,
    insertDate: new Date(),
    isEditable: true,
    updateDate: new Date(),
    userId: 'cranium',
  };

  const comment2: Comment = {
    commentText: 'comment2',
    idComment: 1,
    idQuestion: 1,
    insertDate: new Date(),
    isEditable: false,
    updateDate: new Date(),
    userId: 'cranium',
  };

  return [comment1, comment2];
}



