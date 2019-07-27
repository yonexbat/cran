import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StatusmessageComponent } from './statusmessage.component';

describe('StatusmessageComponent', () => {
  let component: StatusmessageComponent;
  let fixture: ComponentFixture<StatusmessageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StatusmessageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StatusmessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
