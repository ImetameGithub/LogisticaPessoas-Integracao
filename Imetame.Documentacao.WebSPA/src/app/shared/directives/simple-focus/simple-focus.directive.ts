import {  OnInit, OnDestroy, AfterContentInit, Input, ElementRef, Directive } from '@angular/core';
import { SimpleFocusService } from './simple-focus.service';
import * as _ from 'lodash';

@Directive({
    selector: '[simpleFocus]'
})
export class SimpleFocusDirective implements OnInit, OnDestroy, AfterContentInit  {

    
    @Input('simpleFocus') name: string;
    @Input('autoFocus') autoFocus?: string;
    
   

    constructor(
        private _simpleFocusService: SimpleFocusService,
        private el: ElementRef
    ) { }

    ngOnInit() {
        this._simpleFocusService.register(this.name, this);
    }

    ngAfterContentInit(): void {
        if (!_.isUndefined(this.autoFocus))
            this.setFocus();
    }

    ngOnDestroy(): void {
        this._simpleFocusService.unregister(this.name);
    }

    setFocus() {
        setTimeout(() => {

            this.el.nativeElement.focus();

        }, 500)
    }
}
