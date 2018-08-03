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



describe('ManageQuestionComponent', () => {
  let component: ManageQuestionComponent;
  let fixture: ComponentFixture<ManageQuestionComponent>;

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, TestingModule],
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
});
