import {Tag} from './tag';

export class SearchQParameters {

    public title: string;
    public page: number;
    public language = '';
    public statusCreated = true;
    public statusReleased = true;
    public statusObsolete = false;
    public andTags: Tag[] = [];
    public orTags: Tag[] = [];
}
