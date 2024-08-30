import { Component, OnInit, OnDestroy, ViewEncapsulation, ViewChildren, } from "@angular/core";
import { Subject } from "rxjs";
import { fuseAnimations } from "@fuse/animations";
import { FormControl, } from "@angular/forms";
import { takeUntil, debounceTime, distinctUntilChanged, } from "rxjs/operators";
import * as _ from "lodash";
import { Title } from "@angular/platform-browser";
import { FuseProgressBarService } from "@fuse/components/progress-bar/progress-bar.service";
import { ShowErrosDialogComponent } from "app/shared/components/show-erros-dialog/show-erros-dialog.component";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { AutomacaoDeProcessosService } from "../automacao-de-processos.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { FilesDataSource } from "app/utils/files-data-source";
import { FusePerfectScrollbarDirective } from "@fuse/directives/fuse-perfect-scrollbar/fuse-perfect-scrollbar.directive";
import { ShowLogDialogComponent } from "../show-log-dialog/show-log-dialog.component";
import { DocumentoxColaboradorModel } from "app/models/DTO/DocumentoxColaboradorModel";
import { DocumentosModalComponent } from "./documentos-modal/documentos-modal.component";
import { ColaboradorModel, ColaboradorProtheusModel } from "app/models/DTO/ColaboradorModel";
import { FuseSwitchAlertService } from "@fuse/services/switch-alert";
import { ColaboradorStatusDestra } from "app/models/Enums/DestraEnums";
import { Colaborador } from "app/models/Colaborador";

@Component({
    selector: "colaboradores",
    templateUrl: "./colaboradores.component.html",
    styleUrls: ["./colaboradores.component.scss"],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
})
export class ColaboradoresComponent implements OnInit, OnDestroy {
    private _unsubscribeAll: Subject<any>;
    displayedColumns = [
        "check",
        "numCad",
        "nome",
        "funcao",
        "cracha",
        "equipe",
        "sincronizadoDestra",
        "quantidadeDocumentos",
        "statusDestra",
        "documento",
    ];
    dataSource: FilesDataSource<ColaboradorModel>;
    isBusy: boolean = false;
    isResult: boolean = false;
    blockRequisicao: boolean = false;

    allSelected: boolean = false;

    directiveScroll: FusePerfectScrollbarDirective;
    //todoMundo: boolean = false;

    searchInput: FormControl;
    searchInputEl: any;
    @ViewChildren("searchInputEl") searchInputField;

    documentos: DocumentoxColaboradorModel[]


    listStatus: ColaboradorStatusDestra[] = ColaboradorStatusDestra.values;
    constructor(
        public service: AutomacaoDeProcessosService,

        private titleService: Title,
        private _fuseProgressBarService: FuseProgressBarService,
        public dialog: MatDialog,
        private _fuseSwitchAlertService: FuseSwitchAlertService,
        private _snackbar: MatSnackBar,
    ) {
        this._unsubscribeAll = new Subject();
        this.dataSource = new FilesDataSource<any>(this.service);
        this.searchInput = new FormControl("");
    }

    ngOnInit(): void {
        this.titleService.setTitle("Cadastro - Imetame");

        this.service.onItensChanged
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((itens) => {
                this._fuseProgressBarService.hide();
                this.scrollToTop();
            });

        this.searchInput.setValue(this.service.searchText);

        this.searchInput.valueChanges
            .pipe(
                takeUntil(this._unsubscribeAll),
                debounceTime(600),
                distinctUntilChanged()
            )
            .subscribe((searchText) => {
                this.service.onSearchTextChanged.next(searchText);
            });
        this.service.onProcessamentoChanged
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((processamento) => {
                this.isResult = processamento.status === 2 || processamento.status === 3
            });
        this._fuseProgressBarService.hide();
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    toggleSelection(item): void {
        if (this.checkIsDisable(item) == true)
            return
        item.check = !item.check;
    }


    loadColaboradores(filtro: string): void {
        this.service.GetColaboradoresPorOs(filtro).then(() => {
            // A lista na tabela será atualizada automaticamente pelo `FilesDataSource`
        });
    }

    getStatusDestra(statusNum: number): string {
        return ColaboradorStatusDestra.getNameEnum(statusNum);
    }
    toggleAllSelection(): void {
        this.service.itens.forEach((item) => {
            item.check = this.allSelected;
        });
    }

    cadastrar(): void {
        if (this.isBusy) return;

        this.isBusy = true;

        let colaboradores = this.service.itens.filter(col => col.check);

        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();

        const model = {
            idProcessamento: this.service.routeParams.processamento,
            colaboradores: colaboradores,
        };

        this.service.cadastrar(model).then(
            _ => {
                const finalizados = colaboradores;

                // this.service.adicionarFinalizados(finalizados);

                this._snackbar.open("Finalizado", "OK", {
                    verticalPosition: "top",
                    duration: 2000,
                });

                this._fuseProgressBarService.hide();
                this.isBusy = false;
                this.isResult = true;

            },
            error => this.showError(error)
        );
    }

    checkIsDisable(Colaborador: Colaborador) {
        if (this.isBusy || this.isResult || Colaborador.IsAssociado)
            return true;
        else
            return false;
    }

    getDocumentosProtheus(colaborador: ColaboradorModel) {
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.service.GetDocumentosProtheus('0' + colaborador.NumCad).subscribe(
            {
                next: (response: DocumentoxColaboradorModel[]) => {
                    this._fuseProgressBarService.hide();
                    if (response.length <= 0) {
                        this._fuseSwitchAlertService.open({
                            title: 'Atenção',
                            message: 'Nenhum documento encontrado no Protheus',
                            icon: {
                                show: true,
                                name: 'error_outline',
                                color: 'error',
                            },
                            dismissible: true,
                        });
                        return
                    }
                    const dialogConfig = new MatDialogConfig();
                    dialogConfig.autoFocus = false;
                    dialogConfig.width = '95%';
                    dialogConfig.height = 'auto';
                    dialogConfig.data = {
                        _listDocumentos: response,
                        _colaborador: colaborador
                    };
                    const dialogRef = this.dialog.open(DocumentosModalComponent, dialogConfig);
                    dialogRef.afterClosed().subscribe(() => {
                        this.titleService.setTitle("Cadastro - Imetame");
                    });
                },
                error: (error) => {
                    this._fuseProgressBarService.hide();
                    this._snackbar.open('Erro ao consultar documentos', 'X', {
                        duration: 2500,
                        panelClass: 'snackbar-error',
                    })
                }
            }
        )


    }

    enviarParaColaboradorDestra() {
        let colaboradores = this.service.itens.filter(col => col.check);
        if (colaboradores.length == 0) {
            this._snackbar.open("Não há colaboradores selecionados.", 'X', {
                duration: 2500,
                panelClass: 'snackbar-success',
            });
            return;
        }
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();

        this.service.EnviarColaboradorDestra(colaboradores, this.service.routeParams.idPedido, this.service.routeParams.ordemServico).subscribe(
            {
                next: (response: ColaboradorProtheusModel) => {
                    this._fuseProgressBarService.hide();
                    this.blockRequisicao = false;
                    this._snackbar.open("Item enviado para com sucesso", 'X', {
                        duration: 2500,
                        panelClass: 'snackbar-success',
                    })
                },
                error: (error) => {
                    this._fuseProgressBarService.hide();
                    this._fuseSwitchAlertService.open({
                        title: 'Atenção',
                        message: error.error,
                        icon: {
                            show: true,
                            name: 'warning',
                            color: 'warn',
                        },
                        dismissible: true,
                    });
                }

            }
        )
    }

    enviarDocumentosParaDestra() {
        let colaboradores = this.service.itens.filter(col => col.check);
        if (colaboradores.length == 0) {
            this._snackbar.open("Não há colaboradores selecionados.", 'X', {
                duration: 2500,
                panelClass: 'snackbar-success',
            });
            return;
        }
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.service.EnviarDocsArrayDestra(colaboradores).subscribe(
            {
                next: (response: ColaboradorProtheusModel) => {
                    this._fuseProgressBarService.hide();
                    this.blockRequisicao = false;
                    this._snackbar.open("Item enviado para com sucesso", 'X', {
                        duration: 2500,
                        panelClass: 'snackbar-success',
                    });
                    this.dataSource = new FilesDataSource<any>(this.service);
                },
                error: (error) => {
                    this.blockRequisicao = false;
                    this._fuseProgressBarService.hide();
                    this._fuseSwitchAlertService.open({
                        title: 'Atenção',
                        message: error.error,
                        icon: {
                            show: true,
                            name: 'warning',
                            color: 'warn',
                        },
                        dismissible: true,
                    });
                }
            }
        )
    }

    showError(error) {
        this.isBusy = false;
        this._fuseProgressBarService.hide();
        let errorDialogRef = this.dialog.open(ShowErrosDialogComponent);
        errorDialogRef.componentInstance.error = error;
    }

    scrollToTop(speed?: number): void {
        speed = speed || 400;
        if (this.directiveScroll) {
            this.directiveScroll.update();

            setTimeout(() => {
                this.directiveScroll.scrollToTop(0, speed);
            });
        }
    }
}
