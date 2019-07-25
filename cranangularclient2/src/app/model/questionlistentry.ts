import {Tag} from './tag';

export class QuestionListEntry {
    public id: number;
    public title: string;
    public status: number;
    public tags: Tag[];
}
