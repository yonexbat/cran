
import {QuestionStatus} from './questionstatus';

export class VersionInfo {
    public idQuestion: number;
    public version: number;
    public user: string;
    public insertDate: Date;
    public approvalDate: Date;
    public questionStatus: QuestionStatus;
}
