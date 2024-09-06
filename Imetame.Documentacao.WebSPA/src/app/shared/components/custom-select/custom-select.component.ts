import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, OnChanges, SimpleChanges, forwardRef, EventEmitter, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { Observable, Subject } from 'rxjs';
import { debounceTime, pairwise, filter, map, startWith } from 'rxjs/operators';
import { CustomOptionsSelect } from './components.types';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'custom-search-select',
  styles: [`
   .w-full{
      width: 100% !important;
    }
    `],
  template: `
    <mat-form-field class="w-full" appearance="outline" [ngClass]="{'fuse-mat-rounded' :rounded}">
      <mat-label *ngIf="label != null">{{label}}</mat-label>
      <mat-select [formControl]="formControl" [multiple]="multiple" [disabled]="disabled" [placeholder]="placeholder" (selectionChange)="onSelectionChange()">
        <mat-option>
          <ngx-mat-select-search [noEntriesFoundLabel]="'Nenhuma opção encontrada'" [formControl]="filterControl" [placeholderLabel]="placeholder"></ngx-mat-select-search>
        </mat-option>
        <mat-option *ngIf="multiple == false" [value]="null">
          Nenhum
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
  @Input() disabled: boolean = false;
  @Input() fxFlex: string;
  @Input() formControl: FormControl = new FormControl();
  @Input() multiple: boolean = false;
  @Input() required: boolean = false;
  @Input() rounded: boolean = false;
  @Input() icon: string = null;
  @Output() selectionChange = new EventEmitter<any>();
  @Output() searchChange = new EventEmitter<string>();
  filterControl: FormControl = new FormControl();
  filteredOptions: Observable<CustomOptionsSelect[]>;


  private onChange = (_: any) => { };
  private onTouched = () => { };


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
    this.filterControl.valueChanges.pipe(
      startWith(''),
      debounceTime(600),
    ).subscribe(value => {
      this.searchChange.emit(value);
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

  // onSelectionChange() {
  //   this.onChange(this.formControl.value);
  //   this.onTouched();
  //   this.selectionChange.emit(this.formControl.value);
  // }

  onSelectionChange() {
    // Verifica se "Nenhum" está selecionado
    const currentSelection = this.formControl.value;
    // Essa validação existe para se caso o select não for multiplo não apareça nenhum erro no console log 
    if (currentSelection != null) {
      if (currentSelection.includes(null)) {
        // Se "Nenhum" está selecionado, remove todas as outras seleções e mantém apenas "Nenhum"
        this.formControl.setValue([null]);
      } else if (currentSelection.length > 1 && currentSelection.includes(null)) {
        // Se há múltiplas seleções incluindo "Nenhum", remove "Nenhum"
        this.formControl.setValue(currentSelection.filter(value => value !== null));
      }
    }

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
