import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, UntypedFormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DomSanitizer, Title } from '@angular/platform-browser';
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
  imageSource;
  form: UntypedFormGroup;
  documentos: DocumentoxColaboradorModel[]
  visualizarImagem: boolean = false;
  colspanDocs: number = 6
  rowspan: number = 4
  colspanImg: number = 0
  constructor(
    public service: AutomacaoDeProcessosService,
    public dialogRef: MatDialogRef<DocumentosModalComponent>,
    private titleService: Title,
    private _fuseProgressBarService: FuseProgressBarService,
    public dialog: MatDialog,
    private _snackbar: MatSnackBar,
    private sanitizer: DomSanitizer,
    public snackBar: MatSnackBar
  ) {
    this.form = new FormGroup({
      dtVencimento: new FormControl(new Date, [Validators.required]),
      arquivo: new FormControl('', [Validators.required]),
  });
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

  docDetalhes(item: DocumentoxColaboradorModel) {
    this.visualizarImagem = true;
    this.colspanDocs = 4
    this.colspanImg = 2
    this.imageSource = this.sanitizer.bypassSecurityTrustResourceUrl(`${item.base64}`);
  }

  closeModal() {
    // this.dialogRef.close();
    this.colspanDocs = 6
    this.colspanImg = 0
  }
}
