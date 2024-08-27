import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DomSanitizer, Title } from '@angular/platform-browser';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';
import { DocumentoxColaboradorModel, ImagemProtheus } from 'app/models/DTO/DocumentoxColaboradorModel';
import { FilesDataSource } from 'app/utils/files-data-source';
import { BehaviorSubject, Subject, interval } from 'rxjs';
import { AutomacaoDeProcessosService } from '../../automacao-de-processos.service';

import { ColaboradorModel, ColaboradorProtheusModel } from 'app/models/DTO/ColaboradorModel';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { StatusDocumentoObrigatoriosModel } from 'app/models/DTO/StatusDocumentoObrigatoriosModel';
import { Router } from '@angular/router';
import { FuseSwitchAlertService } from '@fuse/services/switch-alert/switch-alert.service';
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
import { MatProgressSpinner, MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Console } from 'console';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { CustomOptionsSelect } from 'app/shared/components/custom-select/components.types';

@Component({
  selector: 'documentos-modal',
  templateUrl: './documentos-modal.component.html',
  styleUrls: ['./documentos-modal.component.scss'],
})
export class DocumentosModalComponent implements OnInit {

  private _unsubscribeAll: Subject<any>;

  formFiltro: UntypedFormGroup;
  imageSource;
  form: UntypedFormGroup;
  todosDocumentos: DocumentoxColaboradorModel[];
  documentos: DocumentoxColaboradorModel[];
  visualizarImagem: boolean = false;
  colspanDocs: number = 6
  rowspan: number = 4
  colspanImg: number = 0



  searchInputDocumento: FormControl;

  blockRequisicao: boolean = false;

  StatusDocumentoObrigatoriosModel: StatusDocumentoObrigatoriosModel[] = []

  tipoDocumentos: CustomOptionsSelect[] = [];

  contDoc: number = 1;
  isLoading: boolean = false;

  nomeColaborador: string = "";
  listaNomeColaborador: string = "";

  constructor(
    public service: AutomacaoDeProcessosService,
    @Inject(MAT_DIALOG_DATA) public _data: { _listDocumentos: DocumentoxColaboradorModel[], _colaborador: ColaboradorModel },
    public dialogRef: MatDialogRef<DocumentosModalComponent>,
    private titleService: Title,
    private _fuseProgressBarService: FuseProgressBarService,
    private _fuseConfirmationService: FuseConfirmationService,
    private _fuseSwitchAlertService: FuseSwitchAlertService,
    public dialog: MatDialog,
    private router: Router,
    private _snackbar: MatSnackBar,
    private sanitizer: DomSanitizer,
    public snackBar: MatSnackBar,
  ) {
    this.todosDocumentos = _data._listDocumentos; // Guarda todos os documentos filtrados para não precisar fitrar novamente

    // Usando reduce para filtrar duplicatas
    this.tipoDocumentos = this.todosDocumentos.reduce((acc, x) => {
      if (!acc.some(item => item.value === x.IdTipoDocumento)) {
        acc.push(new CustomOptionsSelect(x.IdTipoDocumento, x.TipoDocumento));
      }
      return acc;
    }, [] as CustomOptionsSelect[]);


    this.documentos = _data._listDocumentos;
    const partes = this.documentos[0].NomeColaborador.split(" - ");

    this.nomeColaborador = partes[0];
    this.listaNomeColaborador = partes[1];

    this.getDocumentosObrigatorioStatus(_data._listDocumentos);
    this.form = new FormGroup({
      dtVencimento: new FormControl(new Date, [Validators.required]),
      arquivo: new FormControl('', [Validators.required]),
    });
    this.searchInputDocumento = new FormControl("");

  }

  ngOnInit(): void {
    this.titleService.setTitle("Vizualização de Documentos");

    this.searchInputDocumento.setValue(this.service.searchText);

    this.searchInputDocumento.valueChanges
      .pipe(
        takeUntil(this._unsubscribeAll),
        debounceTime(600),
        distinctUntilChanged()
      )
      .subscribe((searchText) => {
        if (searchText == "" || searchText == null) {
          this.documentos = this.todosDocumentos.map(documento => ({ ...documento }));
        } else {
          // GUARDA UMA COPIA DOS REGISTROS FILTRADOS PELO CAMPO DIGITADO
          this.documentos = this.todosDocumentos.filter(x => x.NomeColaborador.includes(searchText)).map(documento => ({ ...documento }));
        }
      });
  }

  tipoDocumentoSelected(event: any) {
    if (event == null)
      this.documentos = this.todosDocumentos.map(documento => ({ ...documento }));
    else
      this.documentos = this.todosDocumentos.filter(x => x.IdTipoDocumento.includes(event)).map(documento => ({ ...documento }));
  }

  enviarDocsParaDestra(documento: DocumentoxColaboradorModel, index: any) {

    if (documento.SincronizadoDestra) {
      this._snackbar.open("O documento já foi enviado para Destra", 'X', {
        duration: 2500,
        panelClass: 'snackbar-error',
      })
      return
    }

    if (documento.Vencer || documento.Vencido) {
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
          this.documentos[index] = { ...response };

          this._snackbar.open("Item enviado para com sucesso", 'X', {
            duration: 2500,
            panelClass: 'snackbar-success',
          })
        },
        error: (error) => {
          this._fuseProgressBarService.hide();
          this._fuseSwitchAlertService.open({
            title: 'Atenção',
            message: error.error,
            icon: {
              show: true,
              name: 'warning',
              color: 'warn',
            },
            dismissible: true,
          });
          this.blockRequisicao = false;
          // this._snackbar.open(error.error, 'X', {
          //   duration: 4000,
          //   panelClass: 'snackbar-error',
          // })
        }
      }
    )

  }

  docDetalhes(item: DocumentoxColaboradorModel) {
    this._fuseProgressBarService.show();
    this.service.GetImagemProtheus(item.Recno).subscribe(
      {
        next: (response: ImagemProtheus) => {
          this._fuseProgressBarService.hide();
          this.blockRequisicao = false;
          this.visualizarImagem = true;
          this.colspanDocs = 4
          this.colspanImg = 2
          this.imageSource = this.sanitizer.bypassSecurityTrustResourceUrl(`${response.Base64}`);
        },
        error: (error) => {
          this._fuseProgressBarService.hide();
          this._snackbar.open(error.error, 'X', {
            duration: 4000,
            panelClass: 'snackbar-error',
          });
        }
      }
    );

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
          this._fuseProgressBarService.hide();
          this._fuseSwitchAlertService.open({
            title: 'Atenção',
            message: error.error,
            icon: {
              show: true,
              name: 'warning',
              color: 'warn',
            },
            dismissible: true,
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


  enviarDocumentosParaDestra() {
    this.isLoading = true;
    //this._fuseProgressBarService.setMode("indeterminate");
    //this._fuseProgressBarService.show();
    let colaboradores: ColaboradorModel[] = [];
    colaboradores.push(this._data._colaborador)
    this.service.EnviarDocsArrayDestra(colaboradores).subscribe(
      {
        next: (response: ColaboradorProtheusModel) => {
          this.isLoading = false;
          this.blockRequisicao = false;
          // this._fuseProgressBarService.hide();
          this._snackbar.open("Item enviado para com sucesso", 'X', {
            duration: 2500,
            panelClass: 'snackbar-success',
          })
        },
        error: (error) => {
          this.isLoading = false;
          this.blockRequisicao = false;
          // this._fuseProgressBarService.hide();
          this._fuseSwitchAlertService.open({
            title: 'Atenção',
            message: error.error,
            icon: {
              show: true,
              name: 'warning',
              color: 'warn',
            },
            dismissible: true,
          });
        }
      }
    )
  }




  public openAbaAtividadeEspecifica(): void {
    // Gera a URL completa usando a rota do Angular
    const url = this.router.serializeUrl(
      this.router.createUrlTree(['colaboradores'])
    );
    // Abre a URL em uma nova aba
    window.open(url, '_blank');
  }
}
