import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, Input, DebugElement} from '@angular/core';


import { CranDataServiceSpy } from './crandataservicespy';
import { ConfirmServiceSpy } from './confirmservicespy';
import {NotificationServiceSpy} from './notificationservicespy';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';



@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  providers: [
    LanguageService,
    { provide: CRAN_SERVICE_TOKEN, useClass: CranDataServiceSpy },
    { provide: NotificationService, useClass: NotificationServiceSpy},
    { provide: ConfirmService, useClass: ConfirmServiceSpy},
  ],
})
export class TestingModule { }

@Component({selector: 'app-vote', template: ''})
export class  StubVoteComponent {
  @Input() public votes;
}

@Component({selector: 'app-tags', template: ''})
export class StubTagsComponent {
  @Input() public tagList;
}

@Component({selector: 'app-imagelist', template: ''})
export class StubAppImageListComponent {
  @Input() public images;
}

@Component({selector: 'app-comments', template: ''})
export class StubCommentsComponent {
  public showComments(idQuestion: number): Promise<void> {
    return Promise.resolve();
  }
}
