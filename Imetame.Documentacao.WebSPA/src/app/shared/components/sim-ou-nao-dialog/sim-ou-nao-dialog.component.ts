import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';


@Component({
    selector   : 'sim-ou-nao-dialog',
    templateUrl: './sim-ou-nao-dialog.component.html',
    styleUrls  : ['./sim-ou-nao-dialog.component.scss']
})
export class SimOuNaoDialogComponent
{
    

    public message: string;
    
    /**
     * Constructor
     *
     * @param {MatDialogRef<ConfirmDialogComponent>} dialogRef
     */
    constructor(
        public dialogRef: MatDialogRef<SimOuNaoDialogComponent>
    )
    {
        
    }

}
