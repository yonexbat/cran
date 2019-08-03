import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, DebugElement, TemplateRef, OnInit} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';

import { CRAN_SERVICE_TOKEN } from '../../cran-data.servicetoken';
import {NotificationService} from '../../notification.service';
import {ConfirmService} from '../../confirm.service';
import {LanguageService} from '../../language.service';
import {PagedResult} from '../../model/pagedresult';
import { ItempagerComponent } from './itempager.component';
import {TooltipDirective} from '../tooltip.directive';


@Component({
  selector: 'app-pagerhost',
  template: `
    <div>


      <ng-template #listItem let-item="item">
        <div class='itemtemplate'>
          {{item}}
        </div>
      </ng-template>

      <app-itempager
        [itemTemplate]="listItem"
        [pagedResult]="pagedResult"
        [nodatafoundmessage]="'nothing found'"
        (onSelectedPageChanged)="pageSelected($event)">
      </app-itempager>
    </div>`
})
class PagerHostComponent implements OnInit {

  public pagedResult: PagedResult<string> = new PagedResult<string>();
  public lastPageselected: number;

  ngOnInit() {
    this.pagedResult.data = ['item1', 'item2', 'item3', 'item4', 'item5'];
    this.pagedResult.count = 233;
    this.pagedResult.currentPage = 3;
    this.pagedResult.pagesize = 5;
    this.pagedResult.numpages = 47;
  }

  public pageSelected(pageNumber: number) {
    this.lastPageselected = pageNumber;
    this.pagedResult.currentPage = pageNumber;
  }

}

describe('PagerComponent', () => {
  let component: PagerHostComponent;
  let fixture: ComponentFixture<PagerHostComponent>;

  beforeEach(async(() => {
    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ ItempagerComponent, TooltipDirective, PagerHostComponent ],
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
    fixture = TestBed.createComponent(PagerHostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should show itemtemplate 5 times', () => {
    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;
    const items = nativeEl.querySelectorAll('.itemtemplate');
    expect(items.length).toBe(5);
  });

  it('should call page', async(async () => {
    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;
    const pagingButtons = nativeEl.querySelectorAll('.page-item > span');

    // buttons to be displayed: 1 2 3 4 5 >
    expect(pagingButtons.length).toBe(6);

    pagingButtons[0].dispatchEvent(new Event('click'));
    expect(component.lastPageselected).toBe(0);

    pagingButtons[1].dispatchEvent(new Event('click'));
    expect(component.lastPageselected).toBe(1);

    pagingButtons[5].dispatchEvent(new Event('click'));
    expect(component.lastPageselected).toBe(5);

    fixture.detectChanges();

    // buttons to be displayed: < 6 7 8 9 10 >
    expect(nativeEl.querySelectorAll('.page-item > span').length).toBe(7);

  }));


});
