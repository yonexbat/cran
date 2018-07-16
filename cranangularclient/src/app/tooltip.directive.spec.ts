import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, Input, DebugElement} from '@angular/core';

import { TooltipDirective } from './tooltip.directive';
import {LanguageService} from './language.service';

@Component({selector: 'app-test-host', template: '<span id="testspan" [appTooltip] = "\'delete\'">Cranium</span>'})
class StubTestHostComponent {
  @Input() public icon;
}

describe('TooltipDirective', () => {

  let component: StubTestHostComponent;
  let fixture: ComponentFixture<StubTestHostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StubTestHostComponent, TooltipDirective ],
      providers: [
        LanguageService,
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
    const directive = new TooltipDirective(null, null, null);
    expect(directive).toBeTruthy();
  });

  it('should show warning', async(() => {

    // Maybe this test is not so valuable because it depends on inner workings of bootstrap.
    const spanEl: HTMLElement = fixture.debugElement.nativeElement.querySelector('#testspan');
    let title = spanEl.getAttribute('title');
    expect(title).toBe('Löschen', 'attribute title has original value when no mouseover');

    spanEl.dispatchEvent(new Event('mouseenter'));
    fixture.detectChanges();
    title = spanEl.getAttribute('title');
    expect(title).toBe('', 'bootstrap removes titel and shows tooltip');

  }));

});
