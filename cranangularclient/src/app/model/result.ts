import {QuestionResult} from './questionresult';

export class Result {

    idCourseInstance: number;
    idCourse: number;
    courseTitle: string;
    questions: QuestionResult[] = [];
}
