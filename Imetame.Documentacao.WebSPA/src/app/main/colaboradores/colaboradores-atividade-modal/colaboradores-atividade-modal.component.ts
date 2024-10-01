import { ChangeDetectorRef, Component, Inject, OnInit, ViewChild, inject } from '@angular/core';
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
import { MatTableDataSource } from '@angular/material/table';
import * as _ from 'lodash';
import { takeUntil, startWith, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { AutomacaoDeProcessosService } from 'app/main/automacao-de-processos/automacao-de-processos.service';
import { PageEvent } from '@angular/material/paginator';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { MatSort, Sort, MatSortModule } from '@angular/material/sort';

@Component({
    selector: 'colaboradores-atividade-modal',
    templateUrl: './colaboradores-atividade-modal.component.html',
    styleUrls: ['./colaboradores-atividade-modal.component.scss']
})
export class ColaboradoresAtividadeModalComponent implements OnInit {
    private _liveAnnouncer = inject(LiveAnnouncer);


    displayedColumns = ["check", "NOME", "MATRICULA", "CRACHA"];
    dataSource: MatTableDataSource<ColaboradorProtheusModel>;
    checkedColaboradores: ColaboradorProtheusModel[] = [];

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

    //#region PAGINATE e ITENS
    page: number = 1;
    pageSize: number = 5;
    totalCount: number = 0;
    //#endregion 

    searchInput: FormControl;

    atividadesOptions: CustomOptionsSelect[] = [];
    perfilOptions: CustomOptionsSelect[] = [];
    equipeOptions: CustomOptionsSelect[] = [];
    OsOptions: CustomOptionsSelect[] = [];
    funcaoOptions: CustomOptionsSelect[] = [];
    disciplinaOptions: CustomOptionsSelect[] = [];

    @ViewChild(MatSort) sort: MatSort;

    constructor(
        private titleService: Title,
        @Inject(MAT_DIALOG_DATA) public _data: { _listColaboradores: ColaboradorProtheusModel[], _listAtividades: AtividadeEspecifica[] },
        private _Colaboradorservice: ColaboradorService,
        private cdr: ChangeDetectorRef,
        private _matDialogRef: MatDialogRef<ColaboradoresAtividadeModalComponent>,
        private _snackbar: MatSnackBar,
        private _formBuilder: UntypedFormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private _fuseProgressBarService: FuseProgressBarService,
    ) {
        this.searchInput = new FormControl("");

        this.searchInput.valueChanges.pipe(
            debounceTime(600),
            distinctUntilChanged()
        ).subscribe((newValue) => {
            this.filtrarColaboradores(newValue);
        });

        this.dataSource = new MatTableDataSource();
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
        });
        this.titleService.setTitle("Novo - Colaborador - Imetame");
    }

    ngAfterViewInit() {
        this.dataSource.sort = this.sort;
    }

    ngOnInit() {
        // this.getOs('');S
        this.form = new FormGroup({
            ListAtividade: new FormControl([], [Validators.required]),
        });
    }

    //#region FUNÇÕES CRUD MATHEUS MONFREIDES - FARTEC SISTEMAS
    relacionarColaboradores() {
        this._fuseProgressBarService.show();
        const modelRelacao: ColaboradorxAtividadeModel = this.form.getRawValue();
        modelRelacao.ListColaborador = this.checkedColaboradores;
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

    filtrarColaboradores(search?: string) {
        // Obtém os valores do formulário
        var filtroValues = this.formFiltro.getRawValue();
        const searchInputValue = this.searchInput.getRawValue();

        // TODO - CORRIGIR PARA PEGAR O NUMERO DE PAGINAS DISPONIVEIS
        // Se for uma busca, reiniciar a página para a primeira
        if (search != null) {
            this.page = 1;  // Redefinir para a primeira página
        }

        // Recalcular as variáveis de paginação
        const skip = (this.page - 1) * this.pageSize;
        //const take = skip + this.pageSize;
        const take = this.pageSize;

        let colaboradoresFiltrados = this._data._listColaboradores.filter(colaborador => {
            let matchPerfil = filtroValues.Perfil.length > 0 ? filtroValues.Perfil.includes(colaborador.PERFIL) : true;
            let matchEquipe = filtroValues.Equipe.length > 0 ? filtroValues.Equipe.includes(colaborador.CODIGO_EQUIPE + ' - ' + colaborador.NOME_EQUIPE) : true;
            let matchOs = filtroValues.Os.length > 0 ? filtroValues.Os.includes(colaborador.CODIGO_OS + ' - ' + colaborador.NOME_OS) : true;
            let matchFuncao = filtroValues.Funcao.length > 0 ? filtroValues.Funcao.some(funcao => funcao.includes(colaborador.CODIGO_FUNCAO + ' - ' + colaborador.NOME_FUNCAO.trim())) : true;
            let matchDisciplina = filtroValues.Disciplina.length > 0 ? filtroValues.Disciplina.includes(colaborador.CODIGO_DISCIPLINA + ' - ' + colaborador.NOME_DISCIPLINA) : true;

            return matchPerfil && matchEquipe && matchOs && matchFuncao && matchDisciplina;
        });

        if (searchInputValue != null) {
            const filteredColaboradores = colaboradoresFiltrados.filter(x => x.NOME.toUpperCase().includes(searchInputValue.toUpperCase()))
            this.totalCount = filteredColaboradores.length;
            this.dataSource.data = filteredColaboradores.slice(skip, skip + take);
        }
        else {
            this.totalCount = colaboradoresFiltrados.length;
            this.dataSource.data = colaboradoresFiltrados.slice(skip, skip + take);
        }
    }

    colaboradorIsChecked(item: ColaboradorProtheusModel): boolean {
        if (item != null) {
            return this.checkedColaboradores.some(x => x.MATRICULA == item.MATRICULA);
        } else {
            return false;
        }
    }

    checkAll(event: any) {
        if (event.checked == true) {
            this.checkedColaboradores = this._data._listColaboradores.map(colaborador => ({ ...colaborador }));;
        } else {
            this.checkedColaboradores = [];
        }
    }

    checkChange(item: ColaboradorProtheusModel) {
        if (item != null) {
            if (this.checkedColaboradores.some(x => x.MATRICULA == item.MATRICULA)) {
                const index = this.checkedColaboradores.findIndex(colaborador => colaborador.MATRICULA === item.MATRICULA);
                if (index !== -1) {
                    this.checkedColaboradores.splice(index, 1);
                }
            } else {
                this.checkedColaboradores.push(item);
            }
        }
    }

    announceSortChange(sortState: Sort) {
        if (sortState.direction) {
            this._liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
        } else {
            this._liveAnnouncer.announce('Sorting cleared');
        }
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

    //#region FUNÇÃO PAGINAÇÃO - MATHEUS MONFREIDES 01/12/2023
    onPageChange(event: PageEvent) {
        this.page = event.pageIndex + 1;
        this.pageSize = event.pageSize;
        this.filtrarColaboradores();
    }
    //#endregion
}
