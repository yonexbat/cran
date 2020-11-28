import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { RichTextBoxComponent } from './rich-text-box.component';

describe('RichTextBoxComponent', () => {
  let component: RichTextBoxComponent;
  let fixture: ComponentFixture<RichTextBoxComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RichTextBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RichTextBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
