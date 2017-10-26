import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageTagComponent } from './manage-tag.component';

describe('ManageTagComponent', () => {
  let component: ManageTagComponent;
  let fixture: ComponentFixture<ManageTagComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageTagComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageTagComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
