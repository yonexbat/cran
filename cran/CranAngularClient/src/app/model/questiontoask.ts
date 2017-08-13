import {QuestionOptionToAsk} from './questionoptiontoask';

export class QuestionToAsk {
    public idCourseInstanceQuestion: number;
    public text: string;
    public explanation: string;
    public options: QuestionOptionToAsk[] = [];
}
