import { Component, OnInit } from '@angular/core';
import { UntypedFormGroup, UntypedFormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Title } from '@angular/platform-browser';
import { Router, ActivatedRoute } from '@angular/router';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';
import { Credenciadora } from 'app/models/Crendenciadora';
import { ConfirmDialogComponent } from 'app/shared/components/confirm-dialog/confirm-dialog.component';
import { ShowErrosDialogComponent } from 'app/shared/components/show-erros-dialog/show-erros-dialog.component';
import { GenericValidator } from 'app/utils/generic-form-validator';
import { Subject } from 'rxjs';
import { CredenciadoraService } from '../credenciadora.service';

@Component({
  selector: 'credenciadora-form',
  templateUrl: './credenciadora-form.component.html',
  styleUrls: ['./credenciadora-form.component.scss']
})
export class CredenciadoraFormComponent implements OnInit {

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

    selectCredenciadora: any;

    constructor(
        private titleService: Title,
        private _Credenciadoraservice: CredenciadoraService,
        private _snackbar: MatSnackBar,
        private _formBuilder: UntypedFormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private _fuseProgressBarService: FuseProgressBarService,
    ) {
        this.form = new FormGroup({
            descricao: new FormControl('', [Validators.required]),            
        });
        this.titleService.setTitle("Novo - Credenciadora - Imetame");
    }


    ngOnInit() {

        this._Credenciadoraservice._selectCredenciadora$.subscribe(
            (data: any) => {
                if (data != null) {
                    
                    this.selectCredenciadora = data;
                    
                    this.form = this._formBuilder.group({
                        descricao: [data?.Descricao, [Validators.required]],                        
                    });
                    this.titleService.setTitle(
                        data.Descricao + " - Credenciadora - Imetame"
                    );
                }
            },
            (error) => {
                console.error('Erro ao buscar credenciadoras', error);
            }
        );
        


        // this._Credenciadoraservice.getCredenciadoras().subscribe(
        //     (credenciadoras) => {
        //         console.log(credenciadoras)
        //         this.credenciadoras = credenciadoras;
        //     },
        //     (error) => {
        //         console.error('Erro ao buscar credenciadoras', error);
        //     }
        // );
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
        const model: Credenciadora = this.form.getRawValue();

        if (this.selectCredenciadora) {
            this.update(model);
        } else {
            this.add(model);
        }
    }

    add(model: Credenciadora) {
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.blockRequisicao = true;
        this._Credenciadoraservice.Add(model).subscribe(
            {
                next: (response: Credenciadora) => {
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

    update(model: Credenciadora) {
        this._fuseProgressBarService.setMode("indeterminate");
        this._fuseProgressBarService.show();
        this.blockRequisicao = true;
        model.Id = this.selectCredenciadora.Id;
        this._Credenciadoraservice.Update(model).subscribe(
            {
                next: (response: Credenciadora) => {
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
