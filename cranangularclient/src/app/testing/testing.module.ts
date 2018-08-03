import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Component, Input, TemplateRef} from '@angular/core';


import { CranDataServiceSpy } from './crandataservicespy';
import { ConfirmServiceSpy } from './confirmservicespy';
import {NotificationServiceSpy} from './notificationservicespy';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {ConfirmService} from '../confirm.service';
import {LanguageService} from '../language.service';
import {PagedResult} from '../model/pagedresult';




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

@Component({selector: 'app-pager', template: ''})
export class StubPagerComponent {
  @Input()
  public itemTemplate: TemplateRef<any>;
  @Input()
  public pagedResult: PagedResult<any>;
  @Input()
  public nodatafoundmessage  = 'Keine Daten gefunden.';
}

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [StubVoteComponent, StubTagsComponent,
    StubCommentsComponent, StubPagerComponent,
    StubAppImageListComponent],
  providers: [
    LanguageService,
    { provide: CRAN_SERVICE_TOKEN, useClass: CranDataServiceSpy },
    { provide: NotificationService, useClass: NotificationServiceSpy},
    { provide: ConfirmService, useClass: ConfirmServiceSpy},
  ],
})
export class TestingModule { }
