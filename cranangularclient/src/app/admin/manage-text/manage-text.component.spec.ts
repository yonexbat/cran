import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageTextComponent } from './manage-text.component';

describe('ManageTextComponent', () => {
  let component: ManageTextComponent;
  let fixture: ComponentFixture<ManageTextComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageTextComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageTextComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
