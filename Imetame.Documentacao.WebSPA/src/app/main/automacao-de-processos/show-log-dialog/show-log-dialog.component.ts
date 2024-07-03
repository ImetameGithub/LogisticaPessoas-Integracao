import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'show-log-dialog',
    templateUrl: './show-log-dialog.component.html',
    styleUrls: ['./show-log-dialog.component.scss']
})
export class ShowLogDialogComponent  {

    public logs:any = {};
 

    constructor(public dialogRef: MatDialogRef<ShowLogDialogComponent>) { }

 

}
