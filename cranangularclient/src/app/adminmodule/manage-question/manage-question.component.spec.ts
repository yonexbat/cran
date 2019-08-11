import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { UicompsModule } from '../../uicomps/uicomps.module';

import { ManageQuestionComponent } from './manage-question.component';
import {TestingModule,
  StubQuestionPreviewComponent, StubRichTextBoxComponent} from '../../testing/testing.module';
import {CRAN_SERVICE_TOKEN} from '../../services/cran-data.servicetoken';
import {ICranDataService} from '../../services/icrandataservice';
import {Question} from '../../model/question';
import { QuestionType } from '../../model/questiontype';
import { RichTextBoxComponent } from '../../uicomps/rich-text-box/rich-text-box.component';



describe('ManageQuestionComponent', () => {
  let component: ManageQuestionComponent;
  let fixture: ComponentFixture<ManageQuestionComponent>;

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes(
          [
            { path: 'admin/editquestion/:id', component: ManageQuestionComponent},
          ]
        ),
        FormsModule,
        TestingModule,
        UicompsModule,
      ],
      declarations: [
        ManageQuestionComponent,
        StubQuestionPreviewComponent,
        StubRichTextBoxComponent,
      ],
      providers: [  ],
    })
    .overrideModule(UicompsModule, {
      remove: {
              exports: [RichTextBoxComponent]
          },
     })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should add question', async(async () => {


    const cranDataServiceSpy: ICranDataService = fixture.debugElement.injector.get(CRAN_SERVICE_TOKEN);
    let questionCapture: Question;
    cranDataServiceSpy.insertQuestion = (question: Question) => {
      questionCapture = new Question();
      questionCapture.text = question.text;
      questionCapture.title = question.title;
      questionCapture.questionType = question.questionType;
      return Promise.resolve(12);
    };

    await fixture.whenStable();
    fixture.detectChanges();
    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;

    // title
    const titleEl: HTMLInputElement = nativeEl.querySelector('#title');
    titleEl.value = 'test';
    titleEl.dispatchEvent(new Event('input'));

    // langauge
    const languageEl: HTMLSelectElement = nativeEl.querySelector('#language');
    languageEl.selectedIndex = 1;
    languageEl.dispatchEvent(new Event('change'));

    // text
    const textEl: HTMLInputElement = nativeEl.querySelector('#questiontext > input');
    textEl.value = 'Hello question text';
    textEl.dispatchEvent(new Event('input'));

    // Chekcbox, option 1 is true.
    const checkBox1: HTMLInputElement = nativeEl.querySelector('#checkBox0');
    checkBox1.checked = true;
    checkBox1.dispatchEvent(new Event('change'));

    // Chekcbox, option 2 is true.
    const checkBox2: HTMLInputElement = nativeEl.querySelector('#checkBox1');
    checkBox2.checked = false;
    checkBox2.dispatchEvent(new Event('change'));

    // Text, option 1
    const textOption1El: HTMLInputElement = nativeEl.querySelector('#questionoption0 > input');
    textOption1El.value = 'Text option 1';
    textOption1El.dispatchEvent(new Event('input'));


    // Text, option 2
    const textOption2El: HTMLInputElement = nativeEl.querySelector('#questionoption1 > input');
    textOption2El.value = 'Text option 2';
    textOption2El.dispatchEvent(new Event('input'));

    // Explanation
    const textExplanationEl: HTMLInputElement = nativeEl.querySelector('#richtextboxexplanation > input');
    textExplanationEl.value = 'explanation';
    textExplanationEl.dispatchEvent(new Event('input'));


    // Click add button;
    await fixture.whenStable();
    fixture.detectChanges();

    expect(component.questionForm.valid).toBeTruthy('form should be valid');
    const addButton: HTMLButtonElement = nativeEl.querySelector('#saveQuestionBtn');
    addButton.click();

    await fixture.whenStable();
    fixture.detectChanges();
    expect(questionCapture.title).toBe('test', 'title should be test');
    expect(questionCapture.questionType).toBe(QuestionType.SingleChoice, 'single choice expected');

  }));

  it('should not add question, invalid input', async(async () => {

    const cranDataServiceSpy: ICranDataService = fixture.debugElement.injector.get(CRAN_SERVICE_TOKEN);
    let questionCapture: Question;
    cranDataServiceSpy.insertQuestion = (question: Question) => {
      questionCapture = new Question();
      questionCapture.text = question.text;
      questionCapture.title = question.title;
      questionCapture.questionType = question.questionType;
      return Promise.resolve(12);
    };

    await fixture.whenStable();
    fixture.detectChanges();
    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;

    // title
    const titleEl: HTMLInputElement = nativeEl.querySelector('#title');
    titleEl.value = 'test';
    titleEl.dispatchEvent(new Event('input'));

    // langauge
    const languageEl: HTMLSelectElement = nativeEl.querySelector('#language');
    languageEl.selectedIndex = 1;
    languageEl.dispatchEvent(new Event('change'));

    // text
    const textEl: HTMLInputElement = nativeEl.querySelector('#questiontext > input');
    textEl.value = 'Hello question text';
    textEl.dispatchEvent(new Event('input'));

    // Click add button;
    await fixture.whenStable();
    fixture.detectChanges();

    expect(component.questionForm.valid).toBeFalsy('form should be not valid');
    expect(component.questionForm.form.controls['questiontext'].valid).toBeTruthy('text is valid');
  }));

});

function setRichTextBoxText(id, value){

}
