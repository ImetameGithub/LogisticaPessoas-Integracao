import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';


@Component({
    selector   : 'message-dialog',
    templateUrl: './message-dialog.component.html',
    styleUrls  : ['./message-dialog.component.scss']
})
export class MessageDialogComponent
{
    //mensagem: string;
    //titulo: string;
    //messageo: string;
    //cancelar: string

    //public set messageMessage(msg : string) {
    //    this.translate.get([msg]).subscribe(translation => {
    //        this.mensagem = translation[msg];
            
    //    });
    //}

    public title: string;
    public message: string;
    
    /**
     * Constructor
     *
     * @param {MatDialogRef<MessageDialogComponent>} dialogRef
     */
    constructor(
        public dialogRef: MatDialogRef<MessageDialogComponent>
    )
    {
        //this.translate.get(['APP.CONFIRME', 'APP.CONFIRMO', 'APP.CANCELAR']).subscribe(translation => {
        //    this.titulo = translation['APP.CONFIRME'];
        //    this.messageo = translation['APP.CONFIRMO'];
        //    this.cancelar = translation['APP.CANCELAR'];
        //});
    }

}
