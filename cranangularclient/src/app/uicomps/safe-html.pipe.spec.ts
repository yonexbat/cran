import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, Input, DebugElement, SecurityContext, Type } from '@angular/core';

import { DomSanitizer, By } from '@angular/platform-browser';
import { SafeHtmlPipe } from './safe-html.pipe';

@Component({ selector: 'app-test-host', template: '<div id="divtotest" [innerHtml] = "htmlText | safeHtml"></div>' })
class StubTestHostComponent {

  public htmlText = '<p style="background-color: red;">cranim</p>';

}

describe('SaveHtmlPipe', () => {

  let component: StubTestHostComponent;
  let fixture: ComponentFixture<StubTestHostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        StubTestHostComponent,
        SafeHtmlPipe,
      ],
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

  it('shlould diplay styles', () => {
    const nativeDiv = fixture.debugElement.query(By.css('#divtotest')).nativeElement;
    const html = nativeDiv.innerHTML;
    expect(html).toBe(component.htmlText);

  });

  const teststrings = [
    { input: '<p style="background-color: red;">cranim</p>', output: '<p>cranim</p>' },
  ];

  teststrings.forEach(testelem => {

    it(testelem.input, () => {
      const htmlSaniziter: DomSanitizer = fixture.debugElement.injector.get<DomSanitizer>(DomSanitizer);
      const result = htmlSaniziter.sanitize(SecurityContext.HTML, testelem.input);
      expect(result).toBe(testelem.output);
    });
  });

});
