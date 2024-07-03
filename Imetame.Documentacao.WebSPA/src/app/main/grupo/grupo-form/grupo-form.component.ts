import { Component, OnDestroy, OnInit, ViewEncapsulation, ViewChild, ElementRef, ViewChildren } from '@angular/core';
import { takeUntil, debounceTime, distinctUntilChanged, startWith } from 'rxjs/operators';
import { Subject, Observable, merge } from 'rxjs';
import { fuseAnimations } from '@fuse/animations';


import { MatPaginator } from '@angular/material/paginator';
import { MatDialogRef,  MatDialog} from '@angular/material/dialog';




import { ShowErrosDialogComponent } from 'app/shared/components/show-erros-dialog/show-erros-dialog.component';
import { ConfirmDialogComponent } from 'app/shared/components/confirm-dialog/confirm-dialog.component';

import { Title } from '@angular/platform-browser';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { GenericValidator } from 'app/utils/generic-form-validator';
import * as _ from 'lodash';



import { Location } from '@angular/common';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GrupoService } from '../grupo.service';

import { PermissaoService } from 'app/services/permissao.service';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';



@Component({
  selector: 'grupo-form',
  templateUrl: './grupo-form.component.html',
    styleUrls: ['./grupo-form.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class GrupoFormComponent implements OnInit, OnDestroy {

    

    private _unsubscribeAll: Subject<any>;
    detailForm: FormGroup;
    genericValidator: GenericValidator;
    validationMessages: { [key: string]: { [key: string]: any } };
    displayMessage: { [key: string]: [string] } = {};
    errorDialogRef: MatDialogRef<ShowErrosDialogComponent>;    
    pageType: string;

    confirmDialogRef: MatDialogRef<ConfirmDialogComponent>;
        
    item: any ;
    portas: any[] ;
    colaboradores: any[];

    isBusy: boolean = false;

    portaCtrl = new FormControl();
    portasList: Observable<any[]>;    
    @ViewChild('portaInput') portaInput: ElementRef<HTMLInputElement>;

    colaboradorCtrl = new FormControl();
    colaboradoresList: Observable<any[]>;
    @ViewChild('colaboradorInput') colaboradorInput: ElementRef<HTMLInputElement>;

    constructor(
        public service: GrupoService,
        private router: Router,
        private route: ActivatedRoute,
        public snackBar: MatSnackBar,
        public dialog: MatDialog,
        private titleService: Title,
        private formBuilder: FormBuilder,
        private location: Location,
        private _fuseProgressBarService: FuseProgressBarService,
        private _permissaoService: PermissaoService
    ) {
        this._unsubscribeAll = new Subject();

        this.item = {        };
        

        this.validationMessages = {
            nome: {
                required: {
                    key: 'Campo obrigatório',
                    param: {}
                },                
                maxlength: {
                    key: 'Tamanho máximo de 255 caracteres'
                }
            },
            descricao: {
                required: {
                    key: 'Campo obrigatório',
                    param: {}
                },
                maxlength: {
                    key: 'Tamanho máximo de 500 caracteres'
                }
            }
        };

        this.genericValidator = new GenericValidator(this.validationMessages);

               
        
        this.pageType = 'edit';
    }

    
    ngOnInit() {

        


            this.service.onItemChanged
                .pipe(takeUntil(this._unsubscribeAll))
                .subscribe((item:any) => {

                    if (item) {
                        this.titleService.setTitle(item.nome + " - Grupo - Imetame");
                        this.item = item;                        
                        this.pageType = 'edit';
                        this.detailForm = this.createForm();
                    }
                    else {
                        this.titleService.setTitle("Novo - Grupo - Imetame");
                        this.pageType = 'new';
                        this.item = { portas:[], colaboradores:[] };
                        this.detailForm = this.createForm();
                    }                    

                    this.detailForm.valueChanges
                        .pipe(takeUntil(this._unsubscribeAll))
                        .subscribe(() => {
                            this.displayMessage = this.genericValidator.processMessages(this.detailForm);
                        });

                });

        this.service.onPortasChanged
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((portas: any[]) => {

                if (portas) {
                    this.portas = portas;
                } else {
                    this.portas = [];
                }
            });

        this.service.onColaboradoresChanged
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((colaboradores: any[]) => {

                if (colaboradores) {
                    this.colaboradores = colaboradores;
                } else {
                    this.colaboradores = [];
                }
            });

       


        this.portaCtrl.valueChanges
            .pipe(
                takeUntil(this._unsubscribeAll),
                startWith(''),
                debounceTime(300),
                distinctUntilChanged()
            )
            .subscribe((nome: string) => {
                this.portasList = this.service.listarPorta(0, 10, nome);
            });

        this.colaboradorCtrl.valueChanges
            .pipe(
                takeUntil(this._unsubscribeAll),
                startWith(''),
                debounceTime(300),
                distinctUntilChanged()
            )
            .subscribe((nome: string) => {
                this.colaboradoresList = this.service.listarColaborador(0, 10, nome);
            });


    }

    createForm() {
        
        return this.formBuilder.group({
            id: [this.item.id],
            nome: [this.item.nome, [Validators.required, Validators.maxLength(255)]],
            descricao: [this.item.descricao, [Validators.required, Validators.maxLength(500)]]
            


        });
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    saveOrAdd() {
        if (this.isBusy) return;
        this.isBusy = true;

            
        this._fuseProgressBarService.setMode('indeterminate');
        this._fuseProgressBarService.show()

        this.item = this.detailForm.getRawValue();
        

        if (this.pageType === 'new') {

            this.service.add(this.item).then(salvo => {
                this.snackBar.open("Salvo", "OK", {
                    verticalPosition: 'top',
                    duration: 2000
                });
                this._fuseProgressBarService.hide();
                this.isBusy = false;
                this.location.go(this.location.path().split('/novo')[0] + '/' + this.item.id);
                //this.router.navigate(['../'], { relativeTo: this.route });

            },
                error => this.showError(error));

        } else if (this.pageType === 'edit') {

            this.service.save(this.item).then(salvo => {
                this.snackBar.open("Salvo", "OK", {
                    verticalPosition: 'top',
                    duration: 2000
                });
                this._fuseProgressBarService.hide();
                this.isBusy = false;
                this.router.navigate(['../'], { relativeTo: this.route });
            },
                error => this.showError(error));

        }


    }

    selectedPorta(event: MatAutocompleteSelectedEvent): void {        

        this._fuseProgressBarService.show();

        this.service.addPorta(this.item.id,event.option.value).then(salvo => {
            this.snackBar.open("Adicionado", "OK", {
                verticalPosition: 'top',
                duration: 2000
            });

            this._fuseProgressBarService.hide();
            this.portas.push(event.option.value);
        },
            error => this.showError(error));

        this.portaInput.nativeElement.value = '';
        this.portaCtrl.setValue(null);
    }

   
    deletePorta(porta: any): void {

        if (this.isBusy)
            return;
        this.isBusy = true;
        
        this.confirmDialogRef = this.dialog.open(ConfirmDialogComponent, {
            disableClose: false
        });


        this.confirmDialogRef.componentInstance.confirmMessage = 'Deseja mesmo remover a porta?';

        this.confirmDialogRef.afterClosed().subscribe(result => {
            if (result) {

                this._fuseProgressBarService.show();
                this._fuseProgressBarService.setMode('indeterminate');;

                this.service.deletePorta(this.item, porta)
                    .then(deletado => {
                        this._fuseProgressBarService.hide();
                        this.isBusy = false;
                        this.snackBar.open("Porta removida!", "Ok", {
                            verticalPosition: 'top',
                            duration: 2000
                        });

                        _.remove(this.portas, function (p) {
                            return p == porta;
                        });

                    },
                        error => this.showError(error));


            }
            this.confirmDialogRef = null;
        });

        
    }


    selectedColaborador(event: MatAutocompleteSelectedEvent): void {

        this._fuseProgressBarService.show();

        this.service.addColaborador(this.item.id, event.option.value).then(salvo => {
            this.snackBar.open("Adicionado", "OK", {
                verticalPosition: 'top',
                duration: 2000
            });

            this._fuseProgressBarService.hide();
            this.colaboradores.push(event.option.value);
        },
            error => this.showError(error));

        this.colaboradorInput.nativeElement.value = '';
        this.colaboradorCtrl.setValue(null);
    }


    deleteColaborador(colaborador: any): void {

        if (this.isBusy)
            return;
        this.isBusy = true;

        this.confirmDialogRef = this.dialog.open(ConfirmDialogComponent, {
            disableClose: false
        });


        this.confirmDialogRef.componentInstance.confirmMessage = 'Deseja mesmo remover o colaborador?';

        this.confirmDialogRef.afterClosed().subscribe(result => {
            if (result) {

                this._fuseProgressBarService.show();
                this._fuseProgressBarService.setMode('indeterminate');;

                this.service.deleteColaborador(this.item, colaborador)
                    .then(deletado => {
                        this._fuseProgressBarService.hide();
                        this.isBusy = false;
                        this.snackBar.open("Porta removida!", "Ok", {
                            verticalPosition: 'top',
                            duration: 2000
                        });

                        _.remove(this.colaboradores, function (p) {
                            return p == colaborador;
                        });

                    },
                        error => this.showError(error));


            }
            this.confirmDialogRef = null;
        });


    }
   

    showError(error) {
        this._fuseProgressBarService.hide();
        this.isBusy = false;
        this.errorDialogRef = this.dialog.open(ShowErrosDialogComponent);
        this.errorDialogRef.componentInstance.error = error;


    }
}


