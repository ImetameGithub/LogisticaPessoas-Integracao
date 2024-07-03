import { Component, OnInit, Inject, OnDestroy, ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';

import { Subject, Observable } from 'rxjs';
import { fuseAnimations } from '@fuse/animations';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { takeUntil, startWith, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import * as _ from 'lodash';

import { Title } from '@angular/platform-browser';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';
import { ShowErrosDialogComponent } from 'app/shared/components/show-erros-dialog/show-erros-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { AutomacaoDeProcessosService } from '../automacao-de-processos.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { PedidoService } from 'app/main/pedido/pedido.service';

@Component({
    selector: 'credenciadora',
    templateUrl: './credenciadora.component.html',
    styleUrls: ['./credenciadora.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class CredenciadoraComponent implements OnInit, OnDestroy {

    private _unsubscribeAll: Subject<any>;
    filtroForm: FormGroup;
    selectable = false;
    removable = true;
    separatorKeysCodes: number[] = [ENTER, COMMA];
    osCtrl = new FormControl();
    filteredOss: Observable<any[]>;
    oss: string[] = [];
    pedidos: any[] = [];
    credenciadoras: any[] = []


    @ViewChild('osInput') osInput: ElementRef<HTMLInputElement>;
    @ViewChild('auto') matAutocomplete: MatAutocomplete;

    isBusy: boolean = false;

    constructor(
        public service: AutomacaoDeProcessosService,
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
        this.filtroForm = this.createForm();
        this.osCtrl.valueChanges
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

    }

    createForm(): FormGroup {

        return this.formBuilder.group({
            oss: [null, [Validators.required]],
            pedido: [null, [Validators.required]],
        });
    }

    ngOnInit(): void {
        this.titleService.setTitle("Cadastro - Imetame");

        // this.service.getPedidos().subscribe(
        //     (pedidos) => {
        //         this.pedidos = pedidos;
        //     },
        //     (error) => {
        //         console.error('Erro ao buscar pedidos', error);
        //     }
        // );
        this.servicePedido.GetAll().subscribe(
            (pedidos) => {
                this.pedidos = pedidos;
            },
            (error) => {
                console.error('Erro ao buscar pedidos', error);
            }
        );
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }


    add(event: MatChipInputEvent): void {

    }

    remove(os: string): void {
        const index = this.oss.indexOf(os);

        if (index >= 0) {
            this.oss.splice(index, 1);
        }
        this.filtroForm.controls.oss.setValue(this.oss);
    }

    selected(event: MatAutocompleteSelectedEvent): void {
        this.oss.push(event.option.value.numero);
        this.osInput.nativeElement.value = '';
        this.osCtrl.setValue(null);
        this.filtroForm.controls.oss.setValue(this.oss);
    }

    avancar(): void {
        const values = this.filtroForm.getRawValue()
        this.service.getProcessoAtivo({ idPedido: values.pedido }).then(
            (processo) => {
                if (!processo.id) {
                    this.service.cadastrarProcessamento({IdPedido: values.pedido,Oss: values.oss,OssString: '' }).then(
                        (processo) => {
                            this.router.navigate([`${processo.id}`], { relativeTo: this.route });
                        }
                    )
                }else{
                    this.router.navigate([`${processo.id}/view`], { relativeTo: this.route });
                }
            },
            (error) => this.showError(error)
        );

    }

    showError(error) {
        this.isBusy = false;
        this._fuseProgressBarService.hide();
        let errorDialogRef = this.dialog.open(ShowErrosDialogComponent);
        errorDialogRef.componentInstance.error = error;


    }
    

}
