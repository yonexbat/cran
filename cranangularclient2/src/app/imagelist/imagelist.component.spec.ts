import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement, Component, Input, TemplateRef } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import {FormsModule} from '@angular/forms';


import { ImagelistComponent } from './imagelist.component';
import { TooltipDirective } from '../tooltip.directive';
import {PagedResult} from '../model/pagedresult';
import {Tag} from '../model/tag';
import { CRAN_SERVICE_TOKEN } from '../cran-data.servicetoken';
import {NotificationService} from '../notification.service';
import {LanguageService} from '../language.service';
import {ConfirmService} from '../confirm.service';
import {SafeHtmlPipe} from '../save-html.pipe';

@Component({selector: 'app-icon', template: ''})
class StubIconComponent {
  @Input() public icon;
}

describe('ImagelistComponent', () => {
  let component: ImagelistComponent;
  let fixture: ComponentFixture<ImagelistComponent>;

  const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
  const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
  const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);


  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ ImagelistComponent,  StubIconComponent],
      providers: [
        LanguageService,
        { provide: CRAN_SERVICE_TOKEN, useValue: cranDataService },
        { provide: NotificationService, useValue: notificationService },
        { provide: ConfirmService, useValue: confirmationService },
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImagelistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
