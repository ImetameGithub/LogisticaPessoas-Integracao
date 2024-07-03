import {
    Component,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
    ViewChild,
    ElementRef,
    ViewChildren,
} from "@angular/core";
import {
    takeUntil,
    debounceTime,
    distinctUntilChanged,
    startWith,
} from "rxjs/operators";
import { Subject, Observable, merge } from "rxjs";
import { fuseAnimations } from "@fuse/animations";

import { MatPaginator } from "@angular/material/paginator";
import { MatDialogRef, MatDialog } from "@angular/material/dialog";

import { ShowErrosDialogComponent } from "app/shared/components/show-erros-dialog/show-erros-dialog.component";
import { ConfirmDialogComponent } from "app/shared/components/confirm-dialog/confirm-dialog.component";

import { Title } from "@angular/platform-browser";
import {
    FormControl,
    FormGroup,
    FormBuilder,
    Validators,
} from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { GenericValidator } from "app/utils/generic-form-validator";
import * as _ from "lodash";

import { Location } from "@angular/common";
import { FuseProgressBarService } from "@fuse/components/progress-bar/progress-bar.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { DeParaService } from "../de-para.service";

import { PermissaoService } from "app/services/permissao.service";

@Component({
    selector: "de-para-form",
    templateUrl: "./de-para-form.component.html",
    styleUrls: ["./de-para-form.component.scss"],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
})
export class DeParaFormComponent implements OnInit, OnDestroy {
    private _unsubscribeAll: Subject<any>;
    detailForm: FormGroup = this.createForm();
    genericValidator: GenericValidator;
    validationMessages: { [key: string]: { [key: string]: any } };
    displayMessage: { [key: string]: [string] } = {};
    errorDialogRef: MatDialogRef<ShowErrosDialogComponent>;
    pageType: string;

    confirmDialogRef: MatDialogRef<ConfirmDialogComponent>;

    item: any = {};
    isBusy: boolean = false;

    constructor(
        public service: DeParaService,
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

        this.validationMessages = {
            credenciadora: {
                required: {
                    key: "Campo obrigatório",
                    param: {},
                },
                maxlength: {
                    key: "Tamanho máximo de 255 caracteres",
                },
            },
            de: {
                required: {
                    key: "Campo obrigatório",
                    param: {},
                },
                maxlength: {
                    key: "Tamanho máximo de 255 caracteres",
                },
            },
            Para: {
                required: {
                    key: "Campo obrigatório",
                    param: {},
                },
                maxlength: {
                    key: "Tamanho máximo de 255 caracteres",
                },
            },
        };

        this.genericValidator = new GenericValidator(this.validationMessages);

        this.pageType = "edit";
    }

    ngOnInit() {
        this.service.onItemChanged
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((item: any) => {
                if (item) {
                    this.titleService.setTitle(
                        item.credenciadora + " - De Para - Imetame"
                    );

                    this.item = item;
                    this.pageType = "edit";
                    this.detailForm = this.createForm();
                } else {
                    this.titleService.setTitle("Novo - De Para - Imetame");
                    this.pageType = "new";
                    this.detailForm = this.createForm();
                }

                this.detailForm.valueChanges
                    .pipe(takeUntil(this._unsubscribeAll))
                    .subscribe(() => {
                        this.displayMessage =
                            this.genericValidator.processMessages(
                                this.detailForm
                            );
                    });
            });
    }

    createForm() {
        return this.formBuilder.group({
            id: [this.item?.id],

            credenciadora: [
                this.item?.credenciadora,
                [Validators.required, Validators.maxLength(255)],
            ],
            de: [
                this.item?.de,
                [Validators.required, Validators.maxLength(255)],
            ],
            para: [
                this.item?.para,
                [Validators.required, Validators.maxLength(255)],
            ],
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

        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();

        this.item = this.detailForm.getRawValue();

        if (this.pageType === "new") {
            this.service.add(this.item).then(
                (salvo) => {
                    this.snackBar.open("Salvo", "OK", {
                        verticalPosition: "top",
                        duration: 2000,
                    });
                    this._fuseProgressBarService.hide();
                    this.isBusy = false;
                    //this.location.go(this.location.path().split('/novo')[0] + '/' + this.item.id);
                    this.router.navigate(["../"], { relativeTo: this.route });
                },
                (error) => this.showError(error)
            );
        } else if (this.pageType === "edit") {
            this.service.save(this.item).then(
                (salvo) => {
                    this.snackBar.open("Salvo", "OK", {
                        verticalPosition: "top",
                        duration: 2000,
                    });
                    this._fuseProgressBarService.hide();
                    this.isBusy = false;
                    this.router.navigate(["../"], { relativeTo: this.route });
                },
                (error) => this.showError(error)
            );
        }
    }

    showError(error) {
        this._fuseProgressBarService.hide();
        this.isBusy = false;
        this.errorDialogRef = this.dialog.open(ShowErrosDialogComponent);
        this.errorDialogRef.componentInstance.error = error;
    }
}
