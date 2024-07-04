import {
    Component,
    OnInit,
    OnDestroy,
    ViewEncapsulation,
    ViewChildren,
} from "@angular/core";

import { Subject } from "rxjs";
import { fuseAnimations } from "@fuse/animations";
import {
    FormControl,
} from "@angular/forms";
import {
    takeUntil,
    debounceTime,
    distinctUntilChanged,
} from "rxjs/operators";
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
        "status",
        "documento",
    ];
    dataSource: FilesDataSource<any>;
    isBusy: boolean = false;
    isResult: boolean = false;

    directiveScroll: FusePerfectScrollbarDirective;
    todoMundo: boolean = false;

    searchInput: FormControl;
    searchInputEl: any;
    @ViewChildren("searchInputEl") searchInputField;

    documentos: DocumentoxColaboradorModel[]

    constructor(
        public service: AutomacaoDeProcessosService,

        private titleService: Title,
        private _fuseProgressBarService: FuseProgressBarService,
        public dialog: MatDialog,
        private _snackbar: MatSnackBar,
        public snackBar: MatSnackBar
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
        
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    toggleSelection(item): void {
        if (item.check) {
            this.todoMundo = true;
        } else {
            this.todoMundo = false;
            this.service.itens.forEach((i) => {
                if (i.check) {
                    this.todoMundo = true;
                    return true;
                }
            });
        }
    }

    toggleAllSelection(): void {
        this.service.itens.forEach((item) => {
            item.check = this.todoMundo;
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

                this.snackBar.open("Finalizado", "OK", {
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

    getDocumentosProtheus(matricula: string){
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.service.GetDocumentosProtheus('0'+matricula).subscribe(
            {
                next: (response: DocumentoxColaboradorModel[]) => {
                    this._fuseProgressBarService.hide();
                   this.documentos = response;
                   const dialogConfig = new MatDialogConfig();
                   dialogConfig.autoFocus = false;
                   dialogConfig.width = '95%';
                   dialogConfig.height = 'auto';
                   const dialogRef = this.dialog.open(DocumentosModalComponent, dialogConfig);
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
