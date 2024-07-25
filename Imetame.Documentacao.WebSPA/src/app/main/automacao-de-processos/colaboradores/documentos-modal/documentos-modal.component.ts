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

import { ColaboradorProtheusModel } from 'app/models/DTO/ColaboradorModel';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { StatusDocumentoObrigatoriosModel } from 'app/models/DTO/StatusDocumentoObrigatoriosModel';


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

  blockRequisicao: boolean = false;

  StatusDocumentoObrigatoriosModel: StatusDocumentoObrigatoriosModel[] = []

  constructor(
    public service: AutomacaoDeProcessosService,
    public dialogRef: MatDialogRef<DocumentosModalComponent>,
    private titleService: Title,
    private _fuseProgressBarService: FuseProgressBarService,
    private _fuseConfirmationService: FuseConfirmationService,
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
          this.getDocumentosObrigatorioStatus(this.documentos);
        },
        error: (error) => { console.log(error.error) }
      });

  }

  enviarDocsParaDestra(documento: DocumentoxColaboradorModel, index: any) {

    if(documento.SincronizadoDestra){
      this._snackbar.open("O documento já foi enviado para Destra", 'X', {
        duration: 2500,
        panelClass: 'snackbar-error',
      })
      return
    }

    if(documento.Vencer || documento.Vencido){
      this._snackbar.open("Atualize a data de vencimento e tente novamente", 'X', {
        duration: 2500,
        panelClass: 'snackbar-error',
      })
      return
    }

    this.service.EnviarDocumentoParaDestra(documento).subscribe(
      {
        next: (response: DocumentoxColaboradorModel) => {
          this._fuseProgressBarService.hide();
          this.blockRequisicao = false;
          this.documentos[index] = { ...response};
          
          this._snackbar.open("Item enviado para com sucesso", 'X', {
            duration: 2500,
            panelClass: 'snackbar-success',
          })
        },
        error: (error) => {
          this._fuseProgressBarService.hide();
          this.blockRequisicao = false;
          this._snackbar.open(error.error, 'X', {
            duration: 4000,
            panelClass: 'snackbar-error',
          })
        }
      }
    )

  }

  docDetalhes(item: DocumentoxColaboradorModel) {
    this.visualizarImagem = true;
    this.colspanDocs = 4
    this.colspanImg = 2
    this.imageSource = this.sanitizer.bypassSecurityTrustResourceUrl(`${item.Base64}`);
  }

  closeModal() {
    this.visualizarImagem = false;
  }

  getDocumentosObrigatorioStatus(documentos: DocumentoxColaboradorModel[]) {
    this.service.GetDocumentosObrigatorios(documentos).subscribe(
      {
        next: (response: StatusDocumentoObrigatoriosModel[]) => {
          this._fuseProgressBarService.hide();
          this.blockRequisicao = false;
          this.StatusDocumentoObrigatoriosModel = response;

        },
        error: (error) => {
          this._snackbar.open(error.error, 'X', {
            duration: 4000,
            panelClass: 'snackbar-error',
          });
        }
      }
    );
  }

  openDocsObrigatorio() {

    // Construir o corpo da mensagem dinamicamente
    let body = `
     <div>
         <table class="table">
           <thead>
             <tr>
               <th class="tableList">Documento Destra</th>
               <th class="tableList">Documento Protheus</th>
               <th class="tableList">Status</th>
             </tr>
           </thead>
           <tbody>`;
    this.StatusDocumentoObrigatoriosModel.forEach(item => {
      body += `
             <tr>
               <td class="tableList break-word">${item.DocDestra}</td>
               <td class="tableList break-word">${item.DocProtheus}</td>
               <td class="tableList">${item.Status}</td>
             </tr>`;
    });
    body += `
           </tbody>
         </table>
     </div>`;

    // Abrir o modal com a mensagem dinâmica
    this._fuseConfirmationService.open({
      title: "Documentos Obrigatórios",
      message: body,
      icon: {
        show: false,
        name: "warning",
        color: "primary"
      },
      actions: {
        confirm: {
          show: true,
          label: "Fechar",
          color: "primary"
        },
        cancel: {
          show: false,
          label: "Confirmar"
        }
      },
      dismissible: false
    });
  }


}
