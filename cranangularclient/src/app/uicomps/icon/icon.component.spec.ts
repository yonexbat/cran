import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { IconComponent } from './icon.component';

describe('IconComponent', () => {
  let component: IconComponent;
  let fixture: ComponentFixture<IconComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ IconComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show edit icon', waitForAsync(async () => {
    const nativeEl: HTMLElement = fixture.debugElement.nativeElement;
    const ibug = nativeEl.querySelector('i.fa-bug');
    expect(ibug).toBeTruthy();

    component.icon = 'edit';

    fixture.detectChanges();

    const iedit = nativeEl.querySelector('i.fa-edit');
    expect(iedit).toBeTruthy();
  }));

});
