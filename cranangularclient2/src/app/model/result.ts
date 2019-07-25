import {QuestionResult} from './questionresult';

export class Result {

    idCourseInstance: number;
    idCourse: number;
    courseTitle: string;
    startedAt: Date;
    endedAt: Date;
    questions: QuestionResult[] = [];
}
