import {QuestionOptionToAsk} from './questionoptiontoask';

export class QuestionToAsk {
    public idCourseInstanceQuestion: number;
    public text: string;
    public explanation: string;
    public numQuestions = 0;
    public numQuestionsAsked = 0;
    public options: QuestionOptionToAsk[] = [];
}
