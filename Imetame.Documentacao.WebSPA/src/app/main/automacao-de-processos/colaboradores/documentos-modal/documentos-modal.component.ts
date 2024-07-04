import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Title } from '@angular/platform-browser';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';
import { DocumentoxColaboradorModel } from 'app/models/DTO/DocumentoxColaboradorModel';
import { FilesDataSource } from 'app/utils/files-data-source';
import { Subject } from 'rxjs';
import { AutomacaoDeProcessosService } from '../../automacao-de-processos.service';

@Component({
  selector: 'documentos-modal',
  templateUrl: './documentos-modal.component.html',
  styleUrls: ['./documentos-modal.component.scss']
})
export class DocumentosModalComponent implements OnInit {

  documentos: DocumentoxColaboradorModel[]

  constructor(
    public service: AutomacaoDeProcessosService,
    public dialogRef: MatDialogRef<DocumentosModalComponent>,
    private titleService: Title,
    private _fuseProgressBarService: FuseProgressBarService,
    public dialog: MatDialog,
    private _snackbar: MatSnackBar,
    public snackBar: MatSnackBar
  ) {

  }

  ngOnInit(): void {
    this.titleService.setTitle("Vizualização de Documentos");

    this.service.documentoxColaborador$.subscribe(
      {
        next: (response: DocumentoxColaboradorModel[]) => {
          this.documentos = response;
        },
        error: (error) => { console.log(error.error) }
      });

  }


  closeModal() {
    this.dialogRef.close();
  }
}
