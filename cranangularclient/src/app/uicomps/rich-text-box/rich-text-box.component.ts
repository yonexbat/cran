import { Component,
  OnDestroy,
  AfterViewInit,
  EventEmitter,
  Input,
  Output,
  OnInit,
  ViewChild,
  NgZone,
  forwardRef,
  ElementRef,
  Renderer2 } from '@angular/core';
import {ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS,
  Validator, AbstractControl, ValidationErrors} from '@angular/forms';
import {htmlRequired} from './htmlrequired';
import {generateGuid} from '../guidgenerator';

declare const tinymce: any;

@Component({
  selector: 'app-rich-text-box',
  templateUrl: './rich-text-box.component.html',
  styleUrls: ['./rich-text-box.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => RichTextBoxComponent),
    },
    {
      provide: NG_VALIDATORS,
      useExisting:  forwardRef(() => RichTextBoxComponent),
      multi: true,
    },
  ],
})
export class RichTextBoxComponent implements OnInit, AfterViewInit,
  OnDestroy, ControlValueAccessor, Validator {


  private _content: string;
  private editor: any;
  private onChangelistener: any;
  private validateFn: any;
  private elementId: string;

  @Input() public required: boolean;
  @Output() htmlString = new EventEmitter<string>();
  @ViewChild('richtextboxcontainer', {static: true}) container: ElementRef;

  constructor(private zone: NgZone, private renderer: Renderer2) {
    this.elementId = generateGuid();
  }

  writeValue(value: any): void {
    this.content = value;
  }

  registerOnChange(onChangeListener: any): void {
    this.onChangelistener = onChangeListener;
  }

  registerOnTouched(fn: any): void {
  }

  setDisabledState?(isDisabled: boolean): void {
  }

  @Input() public set content(content: string) {
    this._content = content;
    if (this.editor && this.editor.getContent() !== content) {
      this.showContent();
    }
  }

  public get content(): string {
    return this._content;
  }

  ngOnInit() {
    this.validateFn = htmlRequired(this.required);
    const nativeContainer = this.container.nativeElement;
    while (nativeContainer.firstChild) {
      this.renderer.removeChild(nativeContainer, nativeContainer.firstChild);
    }

    const textarea = this.renderer.createElement('textarea');
    this.renderer.setAttribute(textarea, 'id', this.elementId);
    this.renderer.setAttribute(textarea, 'required', 'true');
    this.renderer.appendChild(nativeContainer, textarea);
  }

  ngOnDestroy(): void {
    tinymce.remove(this.editor);
    this.onChangelistener = null;
  }

  ngAfterViewInit(): void {
    const id = '#' + this.elementId;

    tinymce.init({
          selector: id,
          plugins: ['link', 'paste', 'table'],
          skin_url: '/assets/skins/lightgray',
          paste_as_text: true,
          setup: editor => {
            this.editor = editor;
            editor.on('keyup', () => this.pushContent());
            editor.on('change', () => this.pushContent());
            editor.on('init', () => {
              if (this.content) {
                this.showContent();
              }
            });
          },
        });
  }

  private showContent() {
    if (this.editor && this.content) {
      this.editor.setContent(this.content);
    }
  }

  private pushContent() {
    const content = this.editor.getContent();
    this.zone.run(() => this.pushContentInZone(content));
  }

  private pushContentInZone(content: string) {
    this.htmlString.emit(content);
    if (this.onChangelistener) {
      this.onChangelistener(content);
    }
  }

  public validate(c: AbstractControl): ValidationErrors {
    return this.validateFn(c);
  }
}
