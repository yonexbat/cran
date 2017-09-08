import {Tag} from './tag';

export class SearchQParameters {

    public title: string;
    public page: number;
    public andTags: Tag[] = [];
    public orTags: Tag[] = [];
}
