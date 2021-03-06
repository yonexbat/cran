import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ConfirmComponent } from './confirm.component';
import {ConfirmService} from '../../services/confirm.service';
import { LanguageService } from '../../services/language.service';
import { defer } from 'q';

describe('ConfirmComponent', () => {
  let component: ConfirmComponent;
  let fixture: ComponentFixture<ConfirmComponent>;

  beforeEach(waitForAsync(() => {

    TestBed.configureTestingModule({
      imports: [],
      declarations: [ ConfirmComponent ],
      providers: [
        LanguageService,
        ConfirmService,
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    // remove all modal stuff
    const list = document.querySelectorAll('.modal-backdrop');
    list.forEach(elem => {
      elem.remove();
    });
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should show dialog', waitForAsync(() => {
    const confirmService = fixture.debugElement.injector.get(ConfirmService);
    confirmService.confirm('show this', 'please show this message');
    fixture.detectChanges();
    const text = fixture.debugElement.nativeElement.textContent;
    expect(text).toContain('show this', 'show this should be diplayed');
  }));

  it('press ok button', waitForAsync(() => {
    const confirmService = fixture.debugElement.injector.get(ConfirmService);
    confirmService.confirm('show this', 'please show this message')
      .then(() => {
        expect(true).toBeTruthy('this method shall be called afer clicking ok button');
      }).catch(() => {
        expect(true).toBeFalsy('no fail expected');
      });
    fixture.detectChanges();
    const okbutton = fixture.debugElement.nativeElement.querySelector('#confirmdialogokbutton');
    okbutton.click();
    fixture.detectChanges();
  }));

  it('press nok button', waitForAsync(() => {
    const confirmService = fixture.debugElement.injector.get(ConfirmService);
    confirmService.confirm('show this', 'please show this message')
      .then(() => {
        expect(false).toBeTruthy('no then expected');
      }).catch(() => {
        expect(false).toBeFalsy('catch expected');
      });
    fixture.detectChanges();
    const okbutton = fixture.debugElement.nativeElement.querySelector('#confirmdialognokbutton');
    okbutton.click();
    fixture.detectChanges();
  }));

});
