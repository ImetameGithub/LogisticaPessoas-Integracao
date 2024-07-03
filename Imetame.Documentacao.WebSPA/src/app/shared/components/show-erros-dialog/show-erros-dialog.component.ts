import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-show-erros-dialog',
  templateUrl: './show-erros-dialog.component.html',
  styleUrls: ['./show-erros-dialog.component.scss']
})
export class ShowErrosDialogComponent  {

    public error:any = {};
 

  constructor(public dialogRef: MatDialogRef<ShowErrosDialogComponent>) { }

 

}
