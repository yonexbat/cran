import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormsModule } from '@angular/forms';
import { CommentsComponent } from './comments.component';
import { RouterTestingModule } from '@angular/router/testing';
import { TestingModule } from '../../testing/testing.module';


import { ICranDataService } from '../../services/icrandataservice';
import { Comment } from '../../model/comment';
import { CRAN_SERVICE_TOKEN } from '../../services/cran-data.servicetoken';
import { ConfirmService } from '../../services/confirm.service';
import { IconComponent } from '../icon/icon.component';
import { ItempagerComponent } from '../itempager/itempager.component';

describe('CommentsComponent', () => {
  let component: CommentsComponent;
  let fixture: ComponentFixture<CommentsComponent>;

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, TestingModule, ],
      declarations: [ CommentsComponent, IconComponent, ItempagerComponent, ],
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

  it('shloud show comments', async(async () => {
    component.showComments(1);
    await fixture.whenStable();
    fixture.detectChanges();
    const elem: HTMLElement = fixture.debugElement.nativeElement;
    const text = elem.textContent;
    expect(text).toContain('comment1');
    expect(text).toContain('comment2');
  }));

  it('shloud add comment', async(async () => {
    component.showComments(7);
    await fixture.whenStable();
    fixture.detectChanges();

    const dataServiceSpy: ICranDataService = fixture.debugElement.injector.get(CRAN_SERVICE_TOKEN);
    let calledText: string;
    dataServiceSpy.addComment = spyOn(dataServiceSpy, 'addComment')
                                    .and.callFake((comment: Comment): Promise<number> => {
                                      calledText = comment.commentText;
                                      return new Promise((res, reject) => {
                                        res(2);
                                      });
                                    });

    await fixture.whenStable();

    let elem: HTMLElement = fixture.debugElement.nativeElement;
    const inputText: HTMLInputElement = elem.querySelector('#commentText');
    fixture.detectChanges();
    inputText.value = 'Test add my comment';
    inputText.dispatchEvent(new Event('input'));
    fixture.detectChanges();


    elem = fixture.debugElement.nativeElement;
    const button: HTMLElement = elem.querySelector('button');
    button.click();

    await fixture.whenStable();
    fixture.detectChanges();

    const addCommentSpy = dataServiceSpy.addComment as jasmine.Spy;
    const callInfo = addCommentSpy.calls.first();

    // But in Jasmine. Tracked arguemtns are not cloned.
    // component sets commentText to '' after posting.
    const expectedArgument = new Comment();
    expectedArgument.commentText = '';
    expectedArgument.idQuestion = 7;

    expect(addCommentSpy).toHaveBeenCalledTimes(1);
    expect(addCommentSpy).toHaveBeenCalledWith(expectedArgument);
    expect(calledText).toBe('Test add my comment');
  }));

  it('shloud delete comment', async(async () => {
    component.showComments(7);
    await fixture.whenStable();
    fixture.detectChanges();

    // Prepare dataservice
    const dataServiceSpy: ICranDataService = fixture.debugElement.injector.get(CRAN_SERVICE_TOKEN);
    const deleteCommentFn = dataServiceSpy.deleteComment;
    dataServiceSpy.addComment = spyOn(dataServiceSpy, 'deleteComment')
      .and.callFake(deleteCommentFn);

    // Prempare Confirmservice
    const confirmServiceSpy: ConfirmService = fixture.debugElement.injector.get(ConfirmService);
    const confirmfn = confirmServiceSpy.confirm;
    confirmServiceSpy.confirm = spyOn(confirmServiceSpy, 'confirm')
      .and.callFake(confirmfn);

    await fixture.whenStable();

    const elem: HTMLElement = fixture.debugElement.nativeElement;
    const firstDeleteButton: HTMLElement = elem.querySelector('.deletecommentbtn');
    firstDeleteButton.click();

    await fixture.whenStable();
    fixture.detectChanges();

    const deleteCommentSpy = dataServiceSpy.deleteComment as jasmine.Spy;
    const confirmSpy = confirmServiceSpy.confirm as jasmine.Spy;

    expect(deleteCommentSpy).toHaveBeenCalledTimes(1);
    expect(confirmSpy).toHaveBeenCalled();
  }));

});
