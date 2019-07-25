import {Tag} from './tag';

export class Course {
    public id: number;
    public title: string;
    public description: string;
    public numQuestionsToAsk: number;
    public language = '';
    public isFavorite = false;
    public tags: Tag[] = [];
}
