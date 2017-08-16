import { Component,
  OnDestroy,
  AfterViewInit,
  EventEmitter,
  Input,
  Output,
  OnInit, } from '@angular/core';

@Component({
  selector: 'app-rich-text-box',
  templateUrl: './rich-text-box.component.html',
  styleUrls: ['./rich-text-box.component.css']
})
export class RichTextBoxComponent implements OnInit, AfterViewInit, OnDestroy {


  @Input() elementId: string;
  @Input() public content: string;
  @Output() onEditorKeyup = new EventEmitter<any>();
  private editor: any;

  constructor() { }

  ngOnInit() {
  }

  ngOnDestroy(): void {
    tinymce.remove(this.editor);
  }
  ngAfterViewInit(): void {
    const id = '#' + this.elementId;

     tinymce.init({
      selector: id,
      plugins: ['link', 'paste', 'table'],
      skin_url: '/assets/skins/lightgray',
      setup: editor => {
        this.editor = editor;
        editor.on('keyup', () => {
          const content = editor.getContent();
          this.onEditorKeyup.emit(content);
        });
        editor.on('init', () => {
          if (this.content) {
             this.editor.setContent(this.content);
          }
        });
      },
    });
  }
}
