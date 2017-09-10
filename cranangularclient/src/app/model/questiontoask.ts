import {QuestionOptionToAsk} from './questionoptiontoask';
import {Question} from './question';

export class QuestionToAsk {
    public idCourseInstanceQuestion: number;
    public idCourseInstance: number;
    public idQuestion: number;
    public text: string;
    public numQuestions: number;
    public numQuestionsAsked: number;
    public courseEnded = false;
    public options: QuestionOptionToAsk[] = [];
    public question: Question;
}
