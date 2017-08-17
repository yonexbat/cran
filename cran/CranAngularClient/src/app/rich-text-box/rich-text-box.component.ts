import { Component,
  OnDestroy,
  AfterViewInit,
  EventEmitter,
  Input,
  Output,
  OnInit,
  NgZone, } from '@angular/core';

@Component({
  selector: 'app-rich-text-box',
  templateUrl: './rich-text-box.component.html',
  styleUrls: ['./rich-text-box.component.css']
})
export class RichTextBoxComponent implements OnInit, AfterViewInit, OnDestroy {


  @Input() elementId: string;
  @Input() public content: string;
  @Input() public required: boolean;
  @Output() htmlString = new EventEmitter<string>();
  private editor: any;

  constructor(private zone: NgZone) { }

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
        editor.on('keyup', () => this.pushContent());
        editor.on('change', () => this.pushContent());
        editor.on('init', () => {
          if (this.content) {
             this.editor.setContent(this.content);
          }
        });
      },
    });
  }

  private pushContent() {
    const content = this.editor.getContent();
    this.zone.run(() => this.pushContentInZone(content));
  }

  private pushContentInZone(content: string) {
    this.htmlString.emit(content);
  }
}
