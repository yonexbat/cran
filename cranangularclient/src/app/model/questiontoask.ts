import {QuestionOptionToAsk} from './questionoptiontoask';
import {Question} from './question';
import {QuestionSelectorInfo} from './questionselectorinfo';
import {QuestionType} from './questiontype';

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
    public questionType: QuestionType;
    public questionSelectors: QuestionSelectorInfo[] = [];
}
