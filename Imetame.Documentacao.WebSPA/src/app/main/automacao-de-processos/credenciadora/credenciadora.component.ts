import { Component, OnInit, Inject, OnDestroy, ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';

import { Subject, Observable } from 'rxjs';
import { fuseAnimations } from '@fuse/animations';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { FormControl, FormGroup, FormBuilder, Validators, UntypedFormGroup, UntypedFormBuilder } from '@angular/forms';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { takeUntil, startWith, debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import * as _ from 'lodash';

import { Title } from '@angular/platform-browser';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';
import { ShowErrosDialogComponent } from 'app/shared/components/show-erros-dialog/show-erros-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { AutomacaoDeProcessosService } from '../automacao-de-processos.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { PedidoService } from 'app/main/pedido/pedido.service';
import { CustomOptionsSelect } from 'app/shared/components/custom-select/components.types';

@Component({
    selector: 'credenciadora',
    templateUrl: './credenciadora.component.html',
    styleUrls: ['./credenciadora.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class CredenciadoraComponent implements OnInit, OnDestroy {

    private _unsubscribeAll: Subject<any>;
    filtroForm: UntypedFormGroup;
    // filtroForm: FormGroup;
    selectable = false;
    removable = true;
    separatorKeysCodes: number[] = [ENTER, COMMA];
    osCtrl = new FormControl();
    filteredOss: Observable<any[]>;
    oss: string[] = [];
    pedidos: any[] = [];
    ordemservico: any[] = []
    credenciadoras: any[] = []

    pedidosOptions: CustomOptionsSelect[] = [];
    ordemServicoOptions: CustomOptionsSelect[] = [];

    @ViewChild('osInput') osInput: ElementRef<HTMLInputElement>;
    @ViewChild('auto') matAutocomplete: MatAutocomplete;

    isBusy: boolean = false;
    constructor(
        public service: AutomacaoDeProcessosService,
        private _formBuilder: UntypedFormBuilder,
        private formBuilder: FormBuilder,
        public servicePedido: PedidoService,
        private titleService: Title,
        private _fuseProgressBarService: FuseProgressBarService,
        public dialog: MatDialog,
        public snackBar: MatSnackBar,
        private route: ActivatedRoute,
        private router: Router,
    ) {
        this._unsubscribeAll = new Subject();
        this.filteredOss = new Observable();
        // this.filtroForm = this.createForm();

        this.filtroForm = this._formBuilder.group({
            oss: [null, [Validators.required]],
            pedido: [null, [Validators.required]],
        });
    }

    createForm(): FormGroup {
        return this.formBuilder.group({
            oss: [null, [Validators.required]],
            pedido: [null, [Validators.required]],
        });
    }

    ngOnInit(): void {
        this.titleService.setTitle("Cadastro - Imetame");

        this.servicePedido.GetAll().subscribe(
            (pedidos) => {
                this.pedidos = pedidos;
                this.pedidosOptions = this.pedidos.map(item => new CustomOptionsSelect(item.Id, item.NumPedido + ' - ' + item.Unidade)) ?? [];
            },
            (error) => {
                console.error('Erro ao buscar pedidos', error);
            }
        );

        this.service.getOss('').subscribe(
            (ordensServicos: any[]) => {
                this.ordemServicoOptions = ordensServicos.map(item => new CustomOptionsSelect(item.os, item.numero + ' - ' + item.descricao)) ?? [];
            },
            (error) => {
                console.error('Erro ao buscar pedidos', error);
            }
        );

 

        
        this.filtroForm.controls.oss.valueChanges
            .pipe(
                takeUntil(this._unsubscribeAll),
                startWith(''),
                debounceTime<any>(600),
                distinctUntilChanged()
            )
            .subscribe((q: string) => {
                if (_.isObject(q)) return;

                this.filteredOss = this.service.getOss(q);
            });
        // this.osCtrl.valueChanges
        //     .pipe(
        //         takeUntil(this._unsubscribeAll),
        //         startWith(''),
        //         debounceTime<any>(600),
        //         distinctUntilChanged()
        //     )
        //     .subscribe((q: string) => {
        //         if (_.isObject(q)) return;

        //         this.filteredOss = this.service.getOss(q);
        //     });
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    displayFnResp(user: any): string {
        return user && user.numero ? user.numero + ' - ' + user.descricao : '';
    }

    selected(event: MatAutocompleteSelectedEvent): void {
        this.oss.push(event.option.value.numero);
        this.osInput.nativeElement.value = '';
        this.osCtrl.setValue(null);
        this.filtroForm.controls.oss.setValue(this.oss);
    }

    avancar(): void {
        this._fuseProgressBarService.show();
        const values = this.filtroForm.getRawValue();
        this.service.getProcessoAtivo({ idPedido: values.pedido }).then(
            (processo) => {
                if (!processo.id) {
                    this.service.cadastrarProcessamento({ IdPedido: values.pedido, Oss: values.oss.numero, OssString: '' }).then(
                        (processamento: any) => {
                            this.router.navigate([`${processamento.Id}/${values.pedido}`], { relativeTo: this.route });
                            // this.router.navigate([`cadastro-de-colaboradores/${processamento.Id}/${values.pedido}`], { relativeTo: this.route });
                            this._fuseProgressBarService.hide();
                        }
                    );              
                } else {
                    this.router.navigate([`${processo.Id}/view`], { relativeTo: this.route });
                }
            },
            (error) => this.showError(error)
        );
    }

    showError(error) {
        this._fuseProgressBarService.hide();
        let errorDialogRef = this.dialog.open(ShowErrosDialogComponent);
        errorDialogRef.componentInstance.error = error;
    }
}