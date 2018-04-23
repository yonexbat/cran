import {QuestionOption} from './questionoption';
import {Tag} from './tag';
import {Votes} from './votes';
import {Image} from './image';
import {LanguageInfo} from './languageInfo';
import {QuestionStatus} from './questionstatus';

export class Question {
    public id: number;
    public title: string;
    public text: string;
    public status: QuestionStatus;
    public language = '';
    public explanation: string;
    public isEditable: boolean;
    public options: QuestionOption[] = [];
    public tags: Tag[] = [];
    public images: Image[] = [];
    public votes: Votes;
}
