import { ComponentFixture, TestBed, fakeAsync, waitForAsync } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { cold, getTestScheduler } from 'jasmine-marbles';
import { UicompsModule } from '../uicomps.module';

import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import { NotificationService } from '../../services/notification.service';
import { ConfirmService } from '../../services/confirm.service';
import { LanguageService } from '../../services/language.service';
import { Tag } from '../../model/tag';
import { TagFinderComponent } from './tag-finder.component';
import { TagsComponent } from '../tags/tags.component';
import { TooltipDirective } from '../tooltip.directive';
import { IconComponent } from '../icon/icon.component';



describe('TagFinderComponent', () => {
  let component: TagFinderComponent;
  let fixture: ComponentFixture<TagFinderComponent>;
  let cranDataService;

  beforeEach(waitForAsync(() => {

    cranDataService = jasmine.createSpyObj('CranDataService', ['findTags']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    const tagsFound: Tag[] = [
      {id: 1, description: 'desc', name: 'angularcranium', idTagType: 2, shortDescDe: 'DescDe', shortDescEn: 'DescEn'},
      {id: 2, description: 'desc', name: 'angularcranium', idTagType: 2, shortDescDe: 'DescDe', shortDescEn: 'DescEn'},
    ];

    cranDataService.findTags.and.returnValue(Promise.resolve(tagsFound));

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, ],
      declarations: [ TagFinderComponent, TagsComponent, TooltipDirective, IconComponent ],
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
    fixture = TestBed.createComponent(TagFinderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', waitForAsync(() => {
    expect(component).toBeTruthy();
  }));

  it('Sould display selection', waitForAsync( async () => {

    const nameInput: HTMLInputElement = fixture.debugElement.nativeElement.querySelector('input');
    nameInput.value = 'javascript';
    nameInput.dispatchEvent(new Event('keyup'));

    await fixture.whenStable();

    fixture.detectChanges();

    const listEl: HTMLElement = fixture.debugElement.nativeElement.querySelector('.crantagresultanchor');
    const listAsText = listEl.textContent;

    expect(listAsText).toContain('angularcranium', 'tags found shloud be displayed for selection');
    expect(cranDataService.findTags).toHaveBeenCalled();
  }));

  it('Select Tag by click', waitForAsync( async () => {

    const nameInput: HTMLInputElement = fixture.debugElement.nativeElement.querySelector('input');
    nameInput.value = 'javascript';
    nameInput.dispatchEvent(new Event('keyup'));

    await fixture.whenStable();
    fixture.detectChanges();

    const firstCrantagresultEl: HTMLElement = fixture.debugElement.nativeElement.querySelector('.crantagresult');
    firstCrantagresultEl.click();
    await fixture.whenStable();
    fixture.detectChanges();
    expect(component.tagsArray.length).toBe(1, 'Tag shloud be added');
  }));

});
