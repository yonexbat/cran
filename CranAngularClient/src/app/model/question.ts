import {QuestionOption} from './questionoption';

export class Question {
    public id: number;
    public title: string;
    public text: string;
    public options: QuestionOption[] = [];
}
