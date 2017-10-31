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
    'title' : {valueDe : 'Titel', valueEn : 'Title'},
    'titelrequired' : {valueDe : 'Titel ist ein Mussfeld', valueEn : 'Title is required'},
    'language' : {valueDe : 'Sprache', valueEn : 'Langauge'},
    'languagerequired' : {valueDe : 'Sprache ist ein Mussfeld', valueEn : 'Language is required'},
    'state' : {valueDe : 'Status', valueEn : 'State'},
    'created' : {valueDe : 'erstellt', valueEn : 'created'},
    'approved' : {valueDe : 'freigeschalten', valueEn : 'approved'},
    'text' : {valueDe : 'Text', valueEn : 'Text'},
    'addimage' : {valueDe : 'Bild hinzufügen', valueEn : 'Add image'},
    'option' : {valueDe : 'Option', valueEn : 'Option'},
    'removeoption' : {valueDe : 'Option entfernen', valueEn : 'Remove option'},
    'addoption' : {valueDe : 'Option hinzufügen', valueEn : 'Add option'},
    'explanation' : {valueDe : 'Erklärung', valueEn : 'Explanation'},
    'preview' : {valueDe : 'Vorschau', valueEn : 'Preview'},
    'save' : {valueDe : 'Speichern', valueEn : 'Save'},
    'editquestion' : {valueDe : 'Frage #{0} editieren', valueEn : 'Edit question #{0}'},
    'add' : {valueDe : 'Hinzufügen', valueEn : 'Add'},
    'editcourse' : {valueDe : 'Kurs #{0} editieren', valueEn : 'Edit question #{0}'},
  };


  private languageInfo: LanguageInfo = LanguageInfo.De;


  constructor() { }

  public label(key: string, ...placeholders: string[]): string {
    const tuple = this.map[key];
    let result;
    switch (this.languageInfo) {
      case LanguageInfo.En:
        result = tuple.valueEn;
        break;
      default:
        result = tuple.valueDe;
        break;
    }

    for (let i = 0; i < placeholders.length; i++) {
      const toReplace = `{${i}}`;
      result = result.replace(toReplace, placeholders[i]);
    }

    return result;
  }

  public selectLanguage(lang: LanguageInfo) {
    this.languageInfo = lang;
  }

  public getLanguage(): LanguageInfo {
    return this.languageInfo;
  }
}
