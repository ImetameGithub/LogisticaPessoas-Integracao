import { Component, OnInit, Input, OnChanges, SimpleChanges, forwardRef } from '@angular/core';
import { FormControl, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';

// Depending on whether rollup is used, moment needs to be imported differently.
// Since Moment.js doesn't have a default export, we normally need to import using the `* as`
// syntax. However, rollup creates a synthetic default module and we thus need to import it using
// the `default as` syntax.
import * as _moment from 'moment';
import { Moment } from 'moment';
// tslint:disable-next-line:no-duplicate-imports
// import {default as _rollupMoment, Moment} from 'moment';

// const moment = _rollupMoment || _moment;
const moment = _moment;

// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
export const MY_FORMATS = {
    parse: {
        dateInput: 'MM/YYYY',
    },
    display: {
        dateInput: 'MM/YYYY',
        monthYearLabel: 'MMM YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'MMMM YYYY',
    },
};

@Component({
  selector: 'app-mes-ano-picker',
  templateUrl: './mes-ano-picker.component.html',
    styleUrls: ['./mes-ano-picker.component.scss'],
    providers: [
        // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
        // application's root module. We provide it at the component level here, due to limitations of
        // our example generation script.
        { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },

        { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },

        { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => MesAnoPickerComponent), multi: true },
    ]
})
export class MesAnoPickerComponent implements OnInit,ControlValueAccessor {

    @Input() placeholder = '';

    propagateChange: any = () => { };
    dateControl = new FormControl();

    writeValue(value: any): void {
        this.dateControl.setValue(value);
    }
    registerOnChange(fn: any): void {
        this.propagateChange = fn;
    }
    registerOnTouched(fn: any): void {
        
    }
    setDisabledState?(isDisabled: boolean): void {
        
    }



    

  constructor() { }

    ngOnInit() {
        this.dateControl.valueChanges
            .subscribe(() => {
                this.propagateChange(this.dateControl.value);
            });
  }


    chosenYearHandler(normalizedYear: Moment) {
        if (!this.dateControl.value)
            this.dateControl.setValue(moment());
        const ctrlValue = this.dateControl.value;
        ctrlValue.year(normalizedYear.year());
        this.dateControl.setValue(ctrlValue);
    }

    chosenMonthHandler(normlizedMonth: Moment, datepicker: MatDatepicker<Moment>) {

        if (!this.dateControl.value)
            this.dateControl.setValue(moment());

        const ctrlValue = this.dateControl.value;
        ctrlValue.month(normlizedMonth.month());
        this.dateControl.setValue(ctrlValue);
        datepicker.close();
    }

}
