import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { Component, Input, Output, EventEmitter, DebugElement, TemplateRef} from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';

import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import {NotificationService} from '../../services/notification.service';
import {ConfirmService} from '../../services/confirm.service';
import {LanguageService} from '../../services/language.service';
import { TagsComponent } from './tags.component';
import {TooltipDirective} from '../tooltip.directive';
import {IconComponent} from '../icon/icon.component';
import { Tag } from 'src/app/model/tag';


describe('TagsComponent', () => {
  let component: TagsComponent;
  let fixture: ComponentFixture<TagsComponent>;

  beforeEach(async(() => {
    const cranDataService = jasmine.createSpyObj('CranDataService', ['vote']);
    const notificationService = jasmine.createSpyObj('NotificationService', ['emitLoading', 'emitDone', 'emitError']);
    const confirmationService = jasmine.createSpyObj('ConfirmService', ['some']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule],
      declarations: [ TagsComponent, TooltipDirective, IconComponent ],
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
    fixture = TestBed.createComponent(TagsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('tag should be shown', async(async () => {
    component.isEditable = false;
    component.tagList.push(createTag(2, 'test1'));
    component.tagList.push(createTag(2, 'test2'));
    component.tagList.push(createTag(2, 'test3'));

    await fixture.whenStable();
    fixture.detectChanges();

    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;
    const deleteIcons = nativeEl.querySelectorAll('app-icon');
    expect(deleteIcons.length).toBe(0);

    const spans = nativeEl.querySelectorAll('span');
    expect(spans.length).toBe(3);

  }));

  it('tag should be deletable', async(async () => {
    component.isEditable = true;
    component.tagList.push(createTag(2, 'test1'));
    component.tagList.push(createTag(2, 'test2'));
    component.tagList.push(createTag(2, 'test3'));

    await fixture.whenStable();
    fixture.detectChanges();

    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;
    const deleteIcons = nativeEl.querySelectorAll('app-icon');
    expect(deleteIcons.length).toBe(3);

  }));
});

function createTag(id, name): Tag {
  const tag = new Tag();
  tag.id = id;
  tag.name = name;
  tag.shortDescDe = `${name} Desc`;
  tag.idTagType = 1;
  return tag;
}

