import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItempagerComponent } from './itempager.component';

describe('ItempagerComponent', () => {
  let component: ItempagerComponent;
  let fixture: ComponentFixture<ItempagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItempagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItempagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
