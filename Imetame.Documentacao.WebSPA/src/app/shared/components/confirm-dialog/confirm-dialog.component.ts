import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';


@Component({
    selector   : 'confirm-dialog',
    templateUrl: './confirm-dialog.component.html',
    styleUrls  : ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent
{
    //mensagem: string;
    //titulo: string;
    //confirmo: string;
    //cancelar: string

    //public set confirmMessage(msg : string) {
    //    this.translate.get([msg]).subscribe(translation => {
    //        this.mensagem = translation[msg];
            
    //    });
    //}

    public confirmMessage: string;
    
    /**
     * Constructor
     *
     * @param {MatDialogRef<ConfirmDialogComponent>} dialogRef
     */
    constructor(
        public dialogRef: MatDialogRef<ConfirmDialogComponent>
    )
    {
        //this.translate.get(['APP.CONFIRME', 'APP.CONFIRMO', 'APP.CANCELAR']).subscribe(translation => {
        //    this.titulo = translation['APP.CONFIRME'];
        //    this.confirmo = translation['APP.CONFIRMO'];
        //    this.cancelar = translation['APP.CANCELAR'];
        //});
    }

}
