import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

import { ManageQuestionComponent } from './manage-question.component';
import {TooltipDirective} from '../tooltip.directive';
import {IconComponent} from '../icon/icon.component';
import {StatusMessageComponent} from '../status-message/status-message.component';
import {TestingModule, StubRichTextBoxComponent,
  StubImageListComponent, StubTagFinderComponent,
  StubQuestionPreviewComponent, StubFileUploadComponent} from '../testing/testing.module';
import {CRAN_SERVICE_TOKEN} from '../cran-data.servicetoken';
import {ICranDataService} from '../icrandataservice';
import {Question} from '../model/question';



describe('ManageQuestionComponent', () => {
  let component: ManageQuestionComponent;
  let fixture: ComponentFixture<ManageQuestionComponent>;

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes(
          [
            { path: 'editquestion/:id', component: ManageQuestionComponent},
          ]
        ),
        FormsModule,
        TestingModule],
      declarations: [ ManageQuestionComponent, StubQuestionPreviewComponent,
        StubTagFinderComponent, StubRichTextBoxComponent, StubImageListComponent,
        StubFileUploadComponent, TooltipDirective, IconComponent,
      StatusMessageComponent],
      providers: [  ],
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

  it('should add question', async(async() => {

    const cranDataServiceSpy: ICranDataService = fixture.debugElement.injector.get(CRAN_SERVICE_TOKEN);
    let questionCapture: Question;
    cranDataServiceSpy.insertQuestion = (question: Question) => {
      questionCapture = new Question();
      questionCapture.text = question.text;
      questionCapture.title = question.title;
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

    // Click add button;
    await fixture.whenStable();
    fixture.detectChanges();

    expect(component.questionForm.valid).toBeTruthy('form should be valid');

    const addButton: HTMLButtonElement = nativeEl.querySelector('#saveQuestionBtn');
    addButton.click();

    await fixture.whenStable();
    fixture.detectChanges();
    expect(questionCapture.title).toBe('test', 'title should be test');

  }));

});
