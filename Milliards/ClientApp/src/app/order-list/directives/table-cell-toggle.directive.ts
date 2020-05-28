import { Directive, ElementRef, OnInit, HostListener, Renderer2 } from '@angular/core';
import { AppConstants } from 'src/app/constants/app.constants';

@Directive({
  selector: '[appTableCellToggle]'
})

export class TableCellToggleDirective implements OnInit {

  defaultClass: string = AppConstants.HasMore;

  ngOnInit(): void {
    this.renderer.addClass(this.ele.nativeElement, this.defaultClass);
  }

  constructor(private renderer: Renderer2, private ele: ElementRef) { }

  @HostListener('click') onMouseEnter() {
    this.toggle();
  }
  toggle() {
    if (this.ele.nativeElement.getAttribute('class') === AppConstants.HasMore) {
      this.ele.nativeElement.removeAttribute('class');
      this.renderer.addClass(this.ele.nativeElement, AppConstants.NoMore);
    } else {
      this.ele.nativeElement.removeAttribute('class');
      this.renderer.addClass(this.ele.nativeElement, AppConstants.HasMore);
    }
  }

}
