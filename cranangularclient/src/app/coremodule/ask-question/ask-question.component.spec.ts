import { async, ComponentFixture, TestBed, inject, tick } from '@angular/core/testing';
import { Component, Input, DebugElement} from '@angular/core';
import { Params, ActivatedRoute } from '@angular/router';
import { UicompsModule } from '../../uicomps/uicomps.module';

import {FormsModule} from '@angular/forms';
import { AskQuestionComponent } from './ask-question.component';
import { RouterTestingModule } from '@angular/router/testing';
import {TestingModule,
  } from '../../testing/testing.module';
import {StubActivatedRoute} from '../../testing/stubactivatedroute';

import { NotificationService } from '../../services/notification.service';
import { By } from '@angular/platform-browser';
import { IconComponent } from 'src/app/uicomps/icon/icon.component';



describe('AskQuestionComponent', () => {
  let component: AskQuestionComponent;
  let fixture: ComponentFixture<AskQuestionComponent>;
  const initRoutePram: Params = { id: 1, };
  const activeRoute = new StubActivatedRoute(initRoutePram);

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, TestingModule, UicompsModule],
      declarations: [
          AskQuestionComponent,
        ],
        providers: [
          {provide: ActivatedRoute, useValue: activeRoute}
        ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AskQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should show question', inject([NotificationService], async (notificationService: NotificationService) => {

      await fixture.whenStable;

      const spyEmitDone = spyOn(notificationService, 'emitDone');
      const spyEmitError = spyOn(notificationService, 'emitError');

      spyEmitDone.calls.reset();
      spyEmitError.calls.reset();

      activeRoute.setParamMap({id: 2, });

      await fixture.whenStable;
      fixture.detectChanges();

      expect(spyEmitError).toHaveBeenCalledTimes(0);
      expect(spyEmitDone).toHaveBeenCalledTimes(1);

      const nativeElement: HTMLElement = fixture.debugElement.nativeElement;
      const text = nativeElement.textContent;
      expect(text).toContain('Karotte');
  }));

  it('should show all green', async(async () => {
    await fixture.whenStable();

    activeRoute.setParamMap({id: 2, });

    fixture.detectChanges();
    await fixture.whenStable();


    const checkBoxes: HTMLInputElement[] = fixture.debugElement.queryAll(By.css('input[type=\'checkbox\']'))
      .map(x => x.nativeElement);
    checkBoxes[0].checked = true;
    checkBoxes[2].checked = true;
    checkBoxes[0].dispatchEvent(new Event('change'));
    checkBoxes[2].dispatchEvent(new Event('change'));

    await fixture.whenStable();
    fixture.detectChanges();

    const checkButton: HTMLElement = fixture.debugElement.query(By.css('button[name=\'check\']')).nativeElement;
    checkButton.dispatchEvent(new Event('click'));

    await fixture.whenStable();
    fixture.detectChanges();

    expect(component.checkShown).toBeTruthy();
    const greendivs = fixture.debugElement.queryAll(By.css('div.crananswercorrect'));
    expect(greendivs.length).toBe(4);

    const icons = fixture.debugElement.queryAll(By.css('app-icon.oknokicon'));
    expect(icons.length).toBe(4); // 4 Options


    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < icons.length; i++) {
      const appIcon = icons[i];
      const option = component.question.options[i];
      const theIcon: IconComponent = appIcon.componentInstance;
      const iconExpected = option.isTrue ? 'ok' : 'nok';
      expect(theIcon.icon).toBe(iconExpected);
    }


  }));

});
