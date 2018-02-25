import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TextlistComponent } from './textlist.component';

describe('TextlistComponent', () => {
  let component: TextlistComponent;
  let fixture: ComponentFixture<TextlistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TextlistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TextlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
