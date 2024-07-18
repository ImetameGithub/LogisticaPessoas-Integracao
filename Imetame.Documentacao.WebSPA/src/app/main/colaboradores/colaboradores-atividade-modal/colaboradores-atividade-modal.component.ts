import { Component, Inject, OnInit } from '@angular/core';
import { UntypedFormGroup, UntypedFormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
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
import { ColaboradorModel, ColaboradorProtheusModel } from 'app/models/DTO/ColaboradorModel';
import { AtividadeEspecifica } from 'app/models/AtividadeEspecifica';
import { CustomOptionsSelect } from 'app/shared/components/custom-select/components.types';
import { ColaboradorxAtividadeModel } from 'app/models/DTO/ColaboradorxAtividadeModel';

@Component({
    selector: 'colaboradores-atividade-modal',
    templateUrl: './colaboradores-atividade-modal.component.html',
    styleUrls: ['./colaboradores-atividade-modal.component.scss']
})
export class ColaboradoresAtividadeModalComponent implements OnInit {

    blockRequisicao: boolean = false;
    private _unsubscribeAll: Subject<any>;
    genericValidator: GenericValidator;
    validationMessages: { [key: string]: { [key: string]: any } };
    displayMessage: { [key: string]: [string] } = {};
    errorDialogRef: MatDialogRef<ShowErrosDialogComponent>;
    pageType: string;

    confirmDialogRef: MatDialogRef<ConfirmDialogComponent>;
    credenciadoras: any[] = []

    listPerfil: string[]
    listEquipe: string[]
    listOs: string[]
    listFuncao: string[]
    listDisciplina: string[]

    listColaboradorAdd: Colaborador[];

    form: UntypedFormGroup;
    formFiltro: UntypedFormGroup;

    item: any = {};
    isBusy: boolean = false;

    selectColaborador: any;

    atividadesOptions: CustomOptionsSelect[] = [];
    colaboradoresOptions: CustomOptionsSelect[] = [];
    perfilOptions: CustomOptionsSelect[] = [];
    equipeOptions: CustomOptionsSelect[] = [];
    OsOptions: CustomOptionsSelect[] = [];
    funcaoOptions: CustomOptionsSelect[] = [];
    disciplinaOptions: CustomOptionsSelect[] = [];

    constructor(
        private titleService: Title,
        @Inject(MAT_DIALOG_DATA) public _data: { _listColaboradores: ColaboradorProtheusModel[], _listAtividades: AtividadeEspecifica[] },
        private _Colaboradorservice: ColaboradorService,
        private _matDialogRef: MatDialogRef<ColaboradoresAtividadeModalComponent>,
        private _snackbar: MatSnackBar,
        private _formBuilder: UntypedFormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private _fuseProgressBarService: FuseProgressBarService,
    ) {
        this.listPerfil = Array.from(new Set(_data._listColaboradores.map(m => m.PERFIL)));
        this.listEquipe = Array.from(new Set(_data._listColaboradores.map(m => m.CODIGO_EQUIPE + ' - ' + m.NOME_EQUIPE)));
        this.listOs = Array.from(new Set(_data._listColaboradores.map(m => m.CODIGO_OS + ' - ' + m.NOME_OS)));
        this.listFuncao = Array.from(new Set(_data._listColaboradores.map(m => m.CODIGO_FUNCAO + ' - ' + m.NOME_FUNCAO)));
        this.listDisciplina = Array.from(new Set(_data._listColaboradores.map(m => m.CODIGO_DISCIPLINA + ' - ' + m.NOME_DISCIPLINA)));

        this.atividadesOptions = _data._listAtividades.map(item => new CustomOptionsSelect(item.Id, item.Codigo)) ?? [];
        this.perfilOptions = this.listPerfil.map(item => new CustomOptionsSelect(item, item)) ?? [];
        this.equipeOptions = this.listEquipe.map(item => new CustomOptionsSelect(item, item)) ?? [];
        this.OsOptions = this.listOs.map(item => new CustomOptionsSelect(item, item)) ?? [];
        this.funcaoOptions = this.listFuncao.map(item => new CustomOptionsSelect(item, item)) ?? [];
        this.disciplinaOptions = this.listDisciplina.map(item => new CustomOptionsSelect(item, item)) ?? [];

        this.formFiltro = new FormGroup({
            Perfil: new FormControl([]),
            Equipe: new FormControl([]),
            Os: new FormControl([]),
            Funcao: new FormControl([]),
            Disciplina: new FormControl([]),
        });
        this.form = new FormGroup({
            ListAtividade: new FormControl([], [Validators.required]),
            ListColaborador: new FormControl([], [Validators.required]),
        });
        this.titleService.setTitle("Novo - Colaborador - Imetame");
    }


    ngOnInit() {
        this.form = new FormGroup({
            ListAtividade: new FormControl([], [Validators.required]),
            ListColaborador: new FormControl([], [Validators.required]),
        });
    }

    //#region FUNÇÕES CRUD MATHEUS MONFREIDES - FARTEC SISTEMAS

    relacionarColaboradores() {
        const modelRelacao: ColaboradorxAtividadeModel = this.form.getRawValue();

        this._Colaboradorservice.RelacionarColaboradorxAtividade(modelRelacao).subscribe(
            {
                next: (response: ColaboradorxAtividadeModel) => {
                    this._fuseProgressBarService.hide();
                    this.blockRequisicao = false;
                    this.closeDiag(response);
                    this._snackbar.open("Relação realizada com sucesso", 'X', {
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

    filtrarColaboradores() {
        // Obtém os valores do formulário
        var filtroValues = this.formFiltro.getRawValue();

        // Filtra a lista de colaboradores de acordo com os valores do formulário
        let colaboradoresFiltrados = this._data._listColaboradores.filter(colaborador => {
            let matchPerfil = filtroValues.Perfil.length > 0 ? filtroValues.Perfil.includes(colaborador.PERFIL) : true;
            let matchEquipe = filtroValues.Equipe.length > 0 ? filtroValues.Equipe.includes(colaborador.CODIGO_EQUIPE + ' - ' + colaborador.NOME_EQUIPE) : true;
            let matchOs = filtroValues.Os.length > 0 ? filtroValues.Os.includes(colaborador.CODIGO_OS + ' - ' + colaborador.NOME_OS) : true;
            let matchFuncao = filtroValues.Funcao.length > 0 ? filtroValues.Funcao.includes(colaborador.CODIGO_FUNCAO + ' - ' + colaborador.NOME_FUNCAO) : true;
            let matchDisciplina = filtroValues.Funcao.length > 0 ? filtroValues.Funcao.includes(colaborador.CODIGO_DISCIPLINA + ' - ' + colaborador.NOME_DISCIPLINA) : true;

            return matchPerfil && matchEquipe && matchOs && matchFuncao && matchDisciplina;
        });

        // Mapeia os colaboradores filtrados para as opções do select
        this.colaboradoresOptions = colaboradoresFiltrados.map(item => new CustomOptionsSelect(item, item.NOME)) ?? [];

        this._snackbar.open("Filtro aplicado com sucesso", 'X', {
            duration: 2500,
            panelClass: 'snackbar-success',
        })
    }

    update(model: Colaborador) {
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.blockRequisicao = true;
        model.Id = this.selectColaborador.id;
        this._Colaboradorservice.Update(model).subscribe(
            {
                next: (response: Colaborador) => {
                    this.blockRequisicao = false;
                    this._fuseProgressBarService.hide();
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


    closeDiag(newItem?: ColaboradorxAtividadeModel): void {
        this._matDialogRef.close(newItem);        
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