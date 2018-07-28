import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import {FormsModule} from '@angular/forms';
import { CommentsComponent } from './comments.component';
import { RouterTestingModule } from '@angular/router/testing';
import {TestingModule, StubPagerComponent} from '../testing/testing.module';


import {IconComponent} from '../icon/icon.component';
import {PagerComponent} from '../pager/pager.component';


describe('CommentsComponent', () => {
  let component: CommentsComponent;
  let fixture: ComponentFixture<CommentsComponent>;

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, TestingModule],
      declarations: [ CommentsComponent, IconComponent, PagerComponent ],
      providers: [
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('shloud show comments', async(async() => {
    await fixture.whenStable();
    component.showComments(1);
    await fixture.whenStable();
    fixture.detectChanges();
    const elem: HTMLElement = fixture.debugElement.nativeElement;
    const text = elem.textContent;
    expect(text).toContain('comment1');
    expect(text).toContain('comment2');
  }));

});
