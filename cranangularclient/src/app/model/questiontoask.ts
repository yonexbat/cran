import {QuestionOptionToAsk} from './questionoptiontoask';
import {Tag} from './tag';

export class QuestionToAsk {
    public idCourseInstanceQuestion: number;
    public idCourseInstance: number;
    public idQuestion: number;
    public isEditable: boolean;
    public text: string;
    public title: string;
    public explanation: string;
    public numQuestions: number;
    public numQuestionsAsked: number;
    public courseEnded = false;
    public options: QuestionOptionToAsk[] = [];
    public tags: Tag[] = [];
}
