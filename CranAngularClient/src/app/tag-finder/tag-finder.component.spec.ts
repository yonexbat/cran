import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TagFinderComponent } from './tag-finder.component';

describe('TagFinderComponent', () => {
  let component: TagFinderComponent;
  let fixture: ComponentFixture<TagFinderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TagFinderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TagFinderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
