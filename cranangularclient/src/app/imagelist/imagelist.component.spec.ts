import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImagelistComponent } from './imagelist.component';

describe('ImagelistComponent', () => {
  let component: ImagelistComponent;
  let fixture: ComponentFixture<ImagelistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImagelistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImagelistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
