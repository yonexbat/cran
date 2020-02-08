import { QuestionToAsk } from '../model/questiontoask';
import { GetComments } from '../model/getcomments';
import { PagedResult } from '../model/pagedresult';
import { Comment } from '../model/comment';
import { QuestionAnswer } from '../model/questionanswer';
import { Question } from '../model/question';
import { createQuestiontoAskTestObj, createCommentsTestObjs,
 createQuestionTestObj } from './modelobjcreator';
import { Injectable } from "@angular/core";


const questiontoAsk: QuestionToAsk = createQuestiontoAskTestObj();
const comments = createCommentsTestObjs();

@Injectable()
export class CranDataServiceSpy {
    public constructor() {
    }

    public getQuestionToAsk(id: number): Promise<QuestionToAsk> {
        return new Promise<QuestionToAsk>((res, rej) => {
            res(questiontoAsk);
        });
    }

    answerQuestionAndGetSolution(answer: QuestionAnswer): Promise<Question> {
        const question = createQuestionTestObj(1);
        return new Promise((resolve) => {
            resolve(question);
        });
    }

    public getComments(parameters: GetComments): Promise<PagedResult<Comment>> {
        return new Promise<PagedResult<Comment>>((res, rej) => {
            const result: PagedResult<Comment> = {
                data: comments,
                currentPage: 0,
                count: 2,
                numpages: 1,
                pagesize: 5,
            };
            res(result);
        });
    }

    public addComment(comment: Comment): Promise<number> {
        return new Promise<number>((res, rej) => {
            res(2);
        });
    }

    public deleteComment(id: number): Promise<any> {
        return Promise.resolve();
    }

    public insertQuestion(): Promise<number> {
        return Promise.resolve(12);
    }
}
