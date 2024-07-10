import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, OnChanges, SimpleChanges, forwardRef, EventEmitter, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CustomOptionsSelect } from '../components.types';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'custom-search-select',
  template: `
    <mat-form-field class="w-full" appearance="outline" [ngClass]="{'fuse-mat-rounded' :rounded}">
      <mat-label *ngIf="label != null">{{label}}</mat-label>
      <mat-select [formControl]="formControl" [multiple]="multiple" [placeholder]="placeholder" (selectionChange)="onSelectionChange()">
        <mat-option>
          <ngx-mat-select-search [formControl]="filterControl" [placeholderLabel]="placeholder"></ngx-mat-select-search>
        </mat-option>
        <mat-option [value]="null">
            Selecione
        </mat-option>
        <mat-option *ngFor="let option of filteredOptions | async" [value]="option.value">
          {{option.display}}
        </mat-option>
      </mat-select>
      <mat-icon class="icon-size-5" *ngIf="icon != null" matPrefix [svgIcon]="icon"></mat-icon>
      <mat-error *ngIf="formControl.hasError('required') && formControl.touched">
    Este campo é obrigatório
  </mat-error>
    </mat-form-field>
  `,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => CustomSearchSelectComponent),
    multi: true
  }],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
    NgxMatSelectSearchModule
  ]
})
export class CustomSearchSelectComponent implements OnInit, OnChanges, ControlValueAccessor {
  @Input() options: CustomOptionsSelect[] = [];
  @Input() placeholder: string = 'Pesquisar...';
  @Input() label: string;
  @Input() fxFlex: string;
  @Input() formControl: FormControl = new FormControl();
  @Input() multiple: boolean = false;
  @Input() required: boolean = false;
  @Input() rounded: boolean = false;
  @Input() icon: string = null;
  @Output() selectionChange = new EventEmitter<any>();
  filterControl: FormControl = new FormControl();
  filteredOptions: Observable<CustomOptionsSelect[]>;

  private onChange = (_: any) => {};
  private onTouched = () => {};

  ngOnInit() {
    this.filteredOptions = this.filterControl.valueChanges.pipe(
      startWith(''),
      map(search => this.filterOptions(search))
    );
    if (this.required) {
        this.formControl.setValidators(Validators.required);
      }
    this.formControl.valueChanges.subscribe(value => {
      this.onChange(value);
      this.selectionChange.emit(value);
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.options) {
      this.filteredOptions = this.filterControl.valueChanges.pipe(
        startWith(''),
        map(search => this.filterOptions(search))
      );
    }
  }

  writeValue(value: any): void {
    this.formControl.setValue(value, { emitEvent: false });
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    isDisabled ? this.formControl.disable() : this.formControl.enable();
  }

  onSelectionChange() {
    this.onChange(this.formControl.value);
    this.onTouched();
    this.selectionChange.emit(this.formControl.value);
  }

  private filterOptions(search: string): CustomOptionsSelect[] {
    if (!search) {
      return this.options.slice();
    }
    return this.options.filter(option => option.display.toLowerCase().includes(search.toLowerCase()));
  }
}
