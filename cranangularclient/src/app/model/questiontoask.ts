import {QuestionOptionToAsk} from './questionoptiontoask';
import {Question} from './question';
import {QuestionSelectorInfo} from './questionSelectorInfo';

export class QuestionToAsk {
    public idCourseInstanceQuestion: number;
    public idCourseInstance: number;
    public idQuestion: number;
    public text: string;
    public numQuestions: number;
    public numCurrentQuestion: number;
    public courseEnded = false;
    public answered: boolean;
    public answerShown: boolean;
    public options: QuestionOptionToAsk[] = [];
    public question: Question;
    public questionSelectors: QuestionSelectorInfo[] = [];
}
