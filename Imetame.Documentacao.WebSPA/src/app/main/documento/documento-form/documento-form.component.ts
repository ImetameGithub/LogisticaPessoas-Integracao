import {
    Component,
    Inject,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
} from "@angular/core";
import {
    takeUntil,

} from "rxjs/operators";
import { Subject } from "rxjs";
import { fuseAnimations } from "@fuse/animations";

import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from "@angular/material/dialog";

import { ShowErrosDialogComponent } from "app/shared/components/show-erros-dialog/show-erros-dialog.component";
import { ConfirmDialogComponent } from "app/shared/components/confirm-dialog/confirm-dialog.component";

import { Title } from "@angular/platform-browser";
import {
    FormGroup,
    FormBuilder,
    Validators,
    FormControl,
    UntypedFormGroup,
    UntypedFormBuilder,
} from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { GenericValidator } from "app/utils/generic-form-validator";
import * as _ from "lodash";

import { FuseProgressBarService } from "@fuse/components/progress-bar/progress-bar.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { DocumentoService } from "../documento.service";
import { DatePipe } from "@angular/common";
import { Documento } from "app/models/Documento";


@Component({
    selector: "documento-form",
    templateUrl: "./documento-form.component.html",
    styleUrls: ["./documento-form.component.scss"],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
})
export class DocumentoFormComponent implements OnInit {
    blockRequisicao: boolean = false;
    private _unsubscribeAll: Subject<any>;
    genericValidator: GenericValidator;
    validationMessages: { [key: string]: { [key: string]: any } };
    displayMessage: { [key: string]: [string] } = {};
    errorDialogRef: MatDialogRef<ShowErrosDialogComponent>;
    pageType: string;

    confirmDialogRef: MatDialogRef<ConfirmDialogComponent>;
    documentosdestra: any[] = []

    form: UntypedFormGroup;

    item: any = {};
    isBusy: boolean = false;

    selectDocumento: any;

    constructor(
        private titleService: Title,
        private _Documentoservice: DocumentoService,
        private _snackbar: MatSnackBar,
        private _formBuilder: UntypedFormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private _fuseProgressBarService: FuseProgressBarService,
    ) {
        this.form = new FormGroup({
            Descricao: new FormControl('', [Validators.required]),
            IdDestra: new FormControl('', [Validators.required]),
            IdProtheus: new FormControl('', [Validators.required]),
        });
        this.titleService.setTitle("Novo - Documento - Imetame");
    }


    ngOnInit() {

        this._Documentoservice._selectDocumento$.subscribe(
            (data: any) => {
                if (data != null) {
                    
                    this.selectDocumento = data;
                    
                    this.form = this._formBuilder.group({
                        Descricao: [data?.Descricao, [Validators.required]],
                        IdDestra: [data?.IdDestra, [Validators.required]],
                        IdProtheus: [data?.IdProtheus, [Validators.required]],
                    });
                    this.titleService.setTitle(
                        data.Credenciadora + " - Documento - Imetame"
                    );
                }
            },
            (error) => {
                console.error('Erro ao buscar credenciadoras', error);
            }
        );
        


        this._Documentoservice.getDocumentosDestra().subscribe(
            (documentosdestra) => {
                console.log(documentosdestra)
                this.documentosdestra = documentosdestra;
            },
            (error) => {
                console.error('Erro ao buscar documentos Destra', error);
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
        const model: Documento = this.form.getRawValue();

        if (this.selectDocumento) {
            this.update(model);
        } else {
            this.add(model);
        }
    }

    add(model: Documento) {
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.blockRequisicao = true;
        this._Documentoservice.Add(model).subscribe(
            {
                next: (response: Documento) => {
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

    update(model: Documento) {
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.blockRequisicao = true;
        model.Id = this.selectDocumento.Id;
        this._Documentoservice.Update(model).subscribe(
            {
                next: (response: Documento) => {
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
