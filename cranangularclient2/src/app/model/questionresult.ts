import {Tag} from './tag';
export class QuestionResult {
    title: string;
    correct: boolean;
    idQuestion: number;
    idCourseInstanceQuestion: number;
    tags: Tag[] = [];
}
