import {QuestionOptionToAsk} from '../model/questionoptiontoask';
import {QuestionToAsk} from '../model/questiontoask';
import {QuestionType} from '../model/questiontype';
import {LanguageInfo} from '../model/languageInfo';
import {QuestionStatus} from '../model/questionstatus';
import {Votes} from '../model/votes';
import { Question } from '../model/question';


const question: Question = {
    id: 1,
    title: 'hello',
    text: 'how are you?',
    options: [],
    tags: [],
    votes: {
        downVotes: 2,
        idQuestion: 1,
        myVote: 1,
        upVotes: 3,
    },
    questionType: QuestionType.MultipleChoice,
    status: QuestionStatus.Created,
    explanation: 'the explation',
    isEditable: false,
    images: [],
    language: LanguageInfo.De.toString(),
};

const questiontoAsk: QuestionToAsk = {
   idCourseInstance: 1,
   idQuestion: 1,
   answered: false,
   courseEnded: false,
   answerShown: false,
   idCourseInstanceQuestion: 1,
   numQuestions: 6,
   text: 'how are you?',
   numCurrentQuestion: 2,
   questionType: QuestionType.MultipleChoice,
   question: question,
   options: [
    {
        text: 'text option 1',
        idCourseInstanceQuestionOption: 1,
        isChecked: false,
        isEditable: true,
        isTrue: true,
    },
    {
        text: 'text option 2',
        idCourseInstanceQuestionOption: 2,
        isChecked: false,
        isEditable: true,
        isTrue: false,
    },
   ],
   questionSelectors: [],
};

export class CranDataServiceSpy {
    public constructor() {
    }
    public getQuestionToAsk(id: number): Promise<QuestionToAsk> {
        return new Promise<QuestionToAsk>((res, rej) => {
            res(questiontoAsk);
        });
    }
}
