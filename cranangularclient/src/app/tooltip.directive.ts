import { Directive, ElementRef, AfterViewInit, OnInit, OnDestroy,
     Input, HostListener, Renderer } from '@angular/core';

import {LanguageService} from './language.service';

declare var $: any;

@Directive({
  selector: '[appTooltip]'
})
export class TooltipDirective implements OnInit, OnDestroy {

  @Input()
  public appTooltip: string;

  constructor(private elementRef: ElementRef,
      private renderer: Renderer,
      private ls: LanguageService) { }

  @HostListener('mouseenter')
  public onMouseEnter(): void {
    const nativeElement = this.elementRef.nativeElement;
    $(nativeElement).tooltip('show');
  }

  @HostListener('mouseleave')
  public onMouseLeave(): void {
    const nativeElement = this.elementRef.nativeElement;
    $(nativeElement).tooltip('dispose');
  }

  ngOnInit(): void {
    const tooltipText = this.ls.label(this.appTooltip);
    this.renderer.setElementAttribute(this.elementRef.nativeElement, 'title', tooltipText);
  }

  ngOnDestroy(): void {
    const nativeElement = this.elementRef.nativeElement;
    $(nativeElement).tooltip('dispose');
  }

}
