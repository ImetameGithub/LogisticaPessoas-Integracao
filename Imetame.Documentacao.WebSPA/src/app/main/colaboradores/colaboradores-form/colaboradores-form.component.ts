import { Component, OnInit } from '@angular/core';
import { UntypedFormGroup, UntypedFormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Title } from '@angular/platform-browser';
import { Router, ActivatedRoute } from '@angular/router';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';
import { Colaborador } from 'app/models/Colaborador';
import { ConfirmDialogComponent } from 'app/shared/components/confirm-dialog/confirm-dialog.component';
import { ShowErrosDialogComponent } from 'app/shared/components/show-erros-dialog/show-erros-dialog.component';
import { GenericValidator } from 'app/utils/generic-form-validator';
import { Subject } from 'rxjs';
import { ColaboradorService } from '../colaboradores.service';
import { AtividadeEspecifica } from 'app/models/AtividadeEspecifica';
import { CustomOptionsSelect } from 'app/shared/components/components.types';
import { CustomSearchSelectComponent } from 'app/shared/components/custom-select/custom-select.component';
import { ColaboradorModel } from 'app/models/DTO/ColaboradorModel';

@Component({
  selector: 'colaboradores-form',
  templateUrl: './colaboradores-form.component.html',
  styleUrls: ['./colaboradores-form.component.scss']
})
export class ColaboradoresFormComponent implements OnInit {


  blockRequisicao: boolean = false;
    private _unsubscribeAll: Subject<any>;
    genericValidator: GenericValidator;
    validationMessages: { [key: string]: { [key: string]: any } };
    displayMessage: { [key: string]: [string] } = {};
    errorDialogRef: MatDialogRef<ShowErrosDialogComponent>;
    pageType: string;

    confirmDialogRef: MatDialogRef<ConfirmDialogComponent>;
    credenciadoras: any[] = []

    form: UntypedFormGroup;

    item: any = {};
    isBusy: boolean = false;

    selectColaborador: any;


    atividadesOptions: CustomOptionsSelect[] = [];
    colaboradoresOptions: CustomOptionsSelect[] = [];

    constructor(
        private titleService: Title,
        private _Colaboradorservice: ColaboradorService,
        private _snackbar: MatSnackBar,
        private _formBuilder: UntypedFormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private _fuseProgressBarService: FuseProgressBarService,
    ) {
        this.form = new FormGroup({
            atividades: new FormControl([], [Validators.required]),
        });
        this.titleService.setTitle("Novo - Colaboradores - Imetame");
    }


    ngOnInit() {

        this._Colaboradorservice._selectColaborador$.subscribe(
            (data: any) => {
                if (data != null) {
                    
                    this.selectColaborador = data;
                    
                    this.form = this._formBuilder.group({
                        credenciadora: [data?.credenciadora, [Validators.required]],
                        numColaborador: [data?.numColaborador, [Validators.required]],
                        unidade: [data?.unidade, [Validators.required]],
                    });
                    this.titleService.setTitle(
                        data.credenciadora + " - Colaborador - Imetame"
                    );
                }
            },
            (error) => {
                console.error('Erro ao buscar credenciadoras', error);
            }
        );
        this._Colaboradorservice.GetAllAtividades().subscribe(
            (data: AtividadeEspecifica[]) => {
                this.atividadesOptions = data.map(estrutura => new CustomOptionsSelect(estrutura.id, estrutura.codigo)) ?? [];
            },
            (error) => {
                console.error('Erro ao buscar credenciadoras', error);
            }
        );
        this._Colaboradorservice.GetAll().subscribe(
            (data: ColaboradorModel[]) => {
                this.colaboradoresOptions = data.map(estrutura => new CustomOptionsSelect(estrutura.id, estrutura.nome)) ?? [];
            },
            (error) => {
                console.error('Erro ao buscar credenciadoras', error);
            }
        );
    }


    // ngOnDestroy(): void {
    //     // Unsubscribe from all subscriptions
    //     this._unsubscribeAll.next();
    //     this._unsubscribeAll.complete();
    // }

    //#region FUNÇÕES CRUD MATHEUS MONFREIDES - FARTEC SISTEMAS
    addOrUpdate() {
        if (!this.form.valid) {
            return;
        }
        const model: Colaborador = this.form.getRawValue();

        if (this.selectColaborador) {
            this.update(model);
        } else {
            this.add(model);
        }
    }

    add(model: Colaborador) {
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.blockRequisicao = true;
        this._Colaboradorservice.Add(model).subscribe(
            {
                next: (response: Colaborador) => {
                    this._fuseProgressBarService.hide();
                    this.blockRequisicao = false;
                    this.router.navigate(["../"], { relativeTo: this.route });
                    this._snackbar.open("Item adicionado com sucesso", 'X', {
                        duration: 2500,
                        panelClass: 'snackbar-success',
                    })
                },
                error: (error) => {
                    this._fuseProgressBarService.hide();
                    this.blockRequisicao = false;
                    this._snackbar.open(error.error, 'X', {
                        duration: 2500,
                        panelClass: 'snackbar-error',
                    })
                }
            }
        )
    }

    update(model: Colaborador) {
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.blockRequisicao = true;
        model.id = this.selectColaborador.id;
        this._Colaboradorservice.Update(model).subscribe(
            {
                next: (response: Colaborador) => {
                    this.blockRequisicao = false;
                    this._fuseProgressBarService.hide();
                    this.router.navigate(["../"], { relativeTo: this.route });
                    this._snackbar.open("Item atualizado com sucesso", 'X', {
                        duration: 2500,
                        panelClass: 'snackbar-success',
                    })
                },
                error: (error) => {
                    this._fuseProgressBarService.hide();
                    this.blockRequisicao = false;
                    console.log(error)
                    this._snackbar.open(error.error, 'X', {
                        duration: 2500,
                        panelClass: 'snackbar-error',
                    })
                }
            }
        )
    }

    public myError = (
        controlName: string,
        errorName: string,
        form: FormGroup
    ) => {
        if (form.controls[controlName].touched) {
            return form.controls[controlName].hasError(errorName);
        }
        return;
    };
    //#endregion

}
