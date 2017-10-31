import { Injectable } from '@angular/core';

import {LabelTuple} from './model/LabelTuple';
import {LanguageInfo} from './model/languageInfo';

@Injectable()
export class LanguageService {

  private map: { [key: string]: LabelTuple; } = {
    'welcometext' : {valueDe : 'Wilkommen hier im Cranium!', valueEn : 'Welcome!'},
    'home' : {valueDe : 'Zuhause', valueEn : 'Home'},
    'courses' : {valueDe : 'Kurse', valueEn : 'Courses'},
    'myresults' : {valueDe : 'Meine Resultate', valueEn : 'My results'},
    'questions' : {valueDe : 'Fragen', valueEn : 'Questions'},
    'addquestion' : {valueDe : 'Frage hinzufügen', valueEn : 'Add question'},
    'searchquestion' : {valueDe : 'Fragen suchen', valueEn : 'Search for questions'},
    'myquestions' : {valueDe : 'Meine Fragen', valueEn : 'My questions'},
    'admin' : {valueDe : 'Admin', valueEn : 'Admin'},
    'tags' : {valueDe : 'Tags', valueEn : 'Tags'},
    'addtag' : {valueDe : 'Tag hinzufügen', valueEn : 'Add a tag'},
    'addcourse' : {valueDe : 'Kurs hinzufügen', valueEn : 'Add course'},
    'logout' : {valueDe : 'Ausloggen', valueEn : 'Log out'},
  };


  private languageInfo: LanguageInfo = LanguageInfo.De;


  constructor() { }

  public label(key: string): string {
    const tuple = this.map[key];
    switch (this.languageInfo) {
      case LanguageInfo.En:
        return tuple.valueEn;
      default:
        return tuple.valueDe;
    }
  }

  public selectLanguage(lang: LanguageInfo) {
    this.languageInfo = lang;
  }

  public getLanguage(): LanguageInfo {
    return this.languageInfo;
  }
}
