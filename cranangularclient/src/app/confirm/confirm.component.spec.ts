import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmComponent } from './confirm.component';
import {ConfirmService} from '../confirm.service';
import { LanguageService } from '../language.service';

describe('ConfirmComponent', () => {
  let component: ConfirmComponent;
  let fixture: ComponentFixture<ConfirmComponent>;

  beforeEach(async(() => {

    const ls = new LanguageService();
    const cs = new ConfirmService(ls);

    TestBed.configureTestingModule({
      imports: [],
      declarations: [ ConfirmComponent ],
      providers: [
        {provide: LanguageService, useValue: ls},
        {provide: ConfirmService, useValue: cs},
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
