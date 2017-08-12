import {QuestionOptionToAsk} from './questionoptiontoask';

export class QuestionToAsk
{
    public courseInstanceQuestionId: number;   
    public text: string;
    public options: QuestionOptionToAsk[] = []; 
}