import { async, ComponentFixture, TestBed, inject, tick } from '@angular/core/testing';
import { Component, Input, DebugElement} from '@angular/core';
import { Params, ActivatedRoute } from '@angular/router';
import { UicompsModule } from '../uicomps/uicomps.module';

import {FormsModule} from '@angular/forms';
import { AskQuestionComponent } from './ask-question.component';
import { RouterTestingModule } from '@angular/router/testing';
import {TestingModule,
  StubVoteComponent,
  StubTagsComponent,
  StubImageListComponent,
  StubCommentsComponent,
  } from '../testing/testing.module';
import {StubActivatedRoute} from '../testing/stubactivatedroute';

import {SafeHtmlPipe} from '../uicomps/save-html.pipe';
import { NotificationService } from '../notification.service';



describe('AskQuestionComponent', () => {
  let component: AskQuestionComponent;
  let fixture: ComponentFixture<AskQuestionComponent>;
  const initRoutePram: Params = { id: 1, };
  const activeRoute = new StubActivatedRoute(initRoutePram);

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, TestingModule, UicompsModule],
      declarations: [ AskQuestionComponent,  StubVoteComponent,
        StubTagsComponent, SafeHtmlPipe,
        StubCommentsComponent, StubImageListComponent],
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

  it('show question', inject([NotificationService], async (notificationService: NotificationService) => {

      await fixture.whenStable;

      const spyEmitDone = spyOn(notificationService, 'emitDone');
      const spyEmitError = spyOn(notificationService, 'emitError');

      activeRoute.setParamMap({id: 2, });

      await fixture.whenStable;

      fixture.detectChanges();
      expect(spyEmitError).toHaveBeenCalledTimes(0);
      expect(spyEmitDone).toHaveBeenCalledTimes(1);

      const nativeElement: HTMLElement = fixture.debugElement.nativeElement;
      const text = nativeElement.textContent;
      expect(text).toContain('how are you?');
  }));
});
