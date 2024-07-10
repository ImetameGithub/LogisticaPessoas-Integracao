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
import { PedidoService } from "../pedido.service";
import { DatePipe } from "@angular/common";
import { Pedido } from "app/models/Pedido";


@Component({
    selector: "pedido-form",
    templateUrl: "./pedido-form.component.html",
    styleUrls: ["./pedido-form.component.scss"],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
})
export class PedidoFormComponent implements OnInit {
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

    selectPedido: any;

    constructor(
        private titleService: Title,
        private _Pedidoservice: PedidoService,
        private _snackbar: MatSnackBar,
        private _formBuilder: UntypedFormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private _fuseProgressBarService: FuseProgressBarService,
    ) {
        this.form = new FormGroup({
            credenciadora: new FormControl('', [Validators.required]),
            numPedido: new FormControl('', [Validators.required]),
            unidade: new FormControl('', [Validators.required]),
        });
        this.titleService.setTitle("Novo - Pedido - Imetame");
    }


    ngOnInit() {

        this._Pedidoservice._selectPedido$.subscribe(
            (data: any) => {
                if (data != null) {
                    
                    this.selectPedido = data;
                    
                    this.form = this._formBuilder.group({
                        credenciadora: [data?.credenciadora, [Validators.required]],
                        numPedido: [data?.numPedido, [Validators.required]],
                        unidade: [data?.unidade, [Validators.required]],
                    });
                    this.titleService.setTitle(
                        data.credenciadora + " - Pedido - Imetame"
                    );
                }
            },
            (error) => {
                console.error('Erro ao buscar credenciadoras', error);
            }
        );
        


        this._Pedidoservice.getCredenciadoras().subscribe(
            (credenciadoras) => {
                console.log(credenciadoras)
                this.credenciadoras = credenciadoras;
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
        const model: Pedido = this.form.getRawValue();

        if (this.selectPedido) {
            this.update(model);
        } else {
            this.add(model);
        }
    }

    add(model: Pedido) {
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.blockRequisicao = true;
        this._Pedidoservice.Add(model).subscribe(
            {
                next: (response: Pedido) => {
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

    update(model: Pedido) {
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.blockRequisicao = true;
        model.id = this.selectPedido.id;
        this._Pedidoservice.Update(model).subscribe(
            {
                next: (response: Pedido) => {
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
