import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, Input, DebugElement, SecurityContext} from '@angular/core';

import { DomSanitizer } from '@angular/platform-browser';
import {SafeHtmlPipe} from './safe-html.pipe';

@Component({selector: 'app-test-host', template: '<span id="testspan">cranium</span>'})
class StubTestHostComponent {
  @Input() public icon;
}

describe('SaveHtmlPipe', () => {

  let component: StubTestHostComponent;
  let fixture: ComponentFixture<StubTestHostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StubTestHostComponent ],
      providers: [
        SafeHtmlPipe,
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StubTestHostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });


  it('should create an instance', () => {
      const pipe = fixture.debugElement.injector.get(SafeHtmlPipe);
      expect(pipe).toBeTruthy();
  });

  const teststrings = [
    {input: '<p style="background-color: red;">cranim</p>', output: '<p>cranim</p>'},
  ];

  teststrings.forEach(testelem => {

    it(testelem.input, () => {
        const htmlSaniziter: DomSanitizer = fixture.debugElement.injector.get(DomSanitizer);
        const result = htmlSaniziter.sanitize(SecurityContext.HTML, testelem.input);
        expect(result).toBe(testelem.output);
    });
  });

});
