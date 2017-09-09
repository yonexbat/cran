import {QuestionOption} from './questionoption';
import {Tag} from './tag';

export class Question {
    public id: number;
    public title: string;
    public text: string;
    public status: number;
    public explanation: string;
    public isEditable: boolean;
    public options: QuestionOption[] = [];
    public tags: Tag[] = [];
}
