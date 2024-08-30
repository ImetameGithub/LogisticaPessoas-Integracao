import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChild, ViewEncapsulation, } from "@angular/core";
import { debounceTime, distinctUntilChanged, startWith, takeUntil, } from "rxjs/operators";
import { Observable, Subject } from "rxjs";
import { fuseAnimations } from "@fuse/animations";
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ShowErrosDialogComponent } from "app/shared/components/show-erros-dialog/show-erros-dialog.component";
import { ConfirmDialogComponent } from "app/shared/components/confirm-dialog/confirm-dialog.component";
import { Title } from "@angular/platform-browser";
import { FormGroup, FormBuilder, Validators, FormControl, UntypedFormGroup, UntypedFormBuilder, } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { GenericValidator } from "app/utils/generic-form-validator";
import { FuseProgressBarService } from "@fuse/components/progress-bar/progress-bar.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { DatePipe } from "@angular/common";
import { RelatorioService } from "../../relatorio.service";
import { Pedido } from "app/models/Pedido";
import { CustomOptionsSelect } from "app/shared/components/custom-select/components.types";
import { Colaborador } from "app/models/Colaborador";
import * as ExcelJS from 'exceljs'
import { ColaboradorModel, ColaboradorProtheusModel } from "app/models/DTO/ColaboradorModel";
import { ChecklistModel } from "app/models/DTO/RelatorioModel";
import * as _ from "lodash";
import { MatAutocomplete, MatAutocompleteSelectedEvent } from "@angular/material/autocomplete";
import { ColaboradorStatusDestra } from "app/models/Enums/DestraEnums";
import { MatTableDataSource } from "@angular/material/table";

@Component({
  selector: 'checklist',
  templateUrl: './checklist.component.html',
  styleUrls: ['./checklist.component.scss'],
  encapsulation: ViewEncapsulation.None,
  animations: fuseAnimations,
})
export class ChecklistComponent implements OnInit, OnDestroy {
  displayedColumns = ["NOME", "MATRICULA", "EQUIPE", "DTADMISSAO", "OS", "NUMPEDIDO", "STATUSDESTRA", "RG", "CPF"];
  isTravarPesquisa: boolean = false;
  OsOptions: CustomOptionsSelect[] = [];

  dataSource: MatTableDataSource<ChecklistModel> = new MatTableDataSource();;
  listPedidos: CustomOptionsSelect[] = [];
  ordemServicoOptions: CustomOptionsSelect[] = [];
  filteredOss: Observable<any[]>;
  oss: string[] = [];
  osCtrl = new FormControl();

  carregarDataTable: boolean = false;



  filtroForm: UntypedFormGroup;
  private _unsubscribeAll: Subject<any>;

  @ViewChild('osInput') osInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;

  constructor(
    private titleService: Title,
    private _formBuilder: UntypedFormBuilder,
    private _snackbar: MatSnackBar,
    private _fuseProgressBarService: FuseProgressBarService,
    private _relatorioService: RelatorioService,
  ) {
    this._unsubscribeAll = new Subject();
    this.filtroForm = this._formBuilder.group({
      oss: [null, [Validators.required]],
      pedido: [null, [Validators.required]],
    });
    this.titleService.setTitle("Checklist");

    this.filteredOss = new Observable();


  }

  ngOnInit(): void {
    this.getOs('')
    this._relatorioService.listPedido$.subscribe(
      (data: Pedido[]) => {
        if (data != null) {
          this.listPedidos = data.map(x => new CustomOptionsSelect(x.Id, x.NumPedido));
        }
      },
      (error) => {
        console.error('Erro ao buscar credenciadoras', error);
      }
    );
    this._relatorioService.getOss('').subscribe(
      (ordensServicos: any[]) => {
        this.ordemServicoOptions = ordensServicos.map(item => new CustomOptionsSelect(item.os, item.numero + ' - ' + item.descricao)) ?? [];
      },
      (error) => {
        console.error('Erro ao buscar pedidos', error);
      }
    );
  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next();
    this._unsubscribeAll.complete();
  }

  getOs(searchValue) {
    const codOs = this.filtroForm.get("oss").value;
    this._relatorioService.getOss(searchValue).subscribe(
      (ordensServicos: any[]) => {
        this.OsOptions = ordensServicos.map(item => new CustomOptionsSelect(item.numero, item.numero + ' - ' + item.descricao)) ?? [];
      },
      (error) => {
        console.error('Erro ao buscar pedidos', error);
      }
    );
  }

  searchOs(searchValue) {
    if (!this.isTravarPesquisa && searchValue != '')
      this._relatorioService.getOss(searchValue).subscribe(
        (ordensServicos: any[]) => {
          this.OsOptions = ordensServicos.map(item => new CustomOptionsSelect(item.numero, item.numero + ' - ' + item.descricao)) ?? [];
        },
        (error) => {
          console.error('Erro ao buscar pedidos', error);
        }
      );
    this.isTravarPesquisa = false;
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.oss.push(event.option.value.numero);
    this.osInput.nativeElement.value = '';
    this.osCtrl.setValue(null);
    this.filtroForm.controls.oss.setValue(this.oss);
  }

  displayFnResp(user: any): string {
    return user && user.numero ? user.numero + ' - ' + user.descricao : '';
  }

  //#region GERAR EXCEL
  carregarRelatorio() {
    if (!this.filtroForm.valid) {
      return;
    }
    this._fuseProgressBarService.show();
    const idPedido = this.filtroForm.get("pedido").value;
    const codOs = this.filtroForm.get("oss").value;
    this._relatorioService.GetDadosCheckList(idPedido, codOs).subscribe({

      next: (response: ChecklistModel[]) => {
        this.carregarDataTable = true;
        this.dataSource.data = response;
        this._fuseProgressBarService.hide();
      },
      error: (error) => {
        this._fuseProgressBarService.hide();
        this._snackbar.open(error.error, 'X', {
          duration: 2500,
          panelClass: 'snackbar-error',
        });
      }
    })
  }

  gerarRelatorio() {
    this._fuseProgressBarService.show();
    const dadosExcel = this.dataSource.data;
    const workbook = new ExcelJS.Workbook();

    const worksheetChecklist = workbook.addWorksheet('Checklist');

    const estiloHeader: Partial<ExcelJS.Style> = { alignment: { horizontal: 'centerContinuous', vertical: 'middle', wrapText: true }, };

    //  DEFINIÇÃO DAS COLUNAS
    worksheetChecklist.columns = [
      { header: 'NOME', key: 'Nome', width: 32, style: estiloHeader },
      { header: 'MATRICULA', key: 'Matricula', width: 20, style: estiloHeader },
      { header: 'EQUIPE', key: 'Equipe', width: 19, style: estiloHeader },
      { header: 'DATA ADMISSÃO', key: 'DataAdmissao', width: 23, style: estiloHeader },
      { header: 'ORDEM SERVIÇO', key: 'OrdemServico', width: 32, style: estiloHeader },
      { header: 'PEDIDO', key: 'Pedido', width: 15, style: estiloHeader },
      { header: 'Documentos', key: 'Documentos', width: 63, style: estiloHeader },
      { header: 'STATUS DESTRA', key: 'StatusDestra', width: 22, style: estiloHeader },
      { header: 'CPF', key: 'Cpf', width: 20, style: estiloHeader },
      { header: 'RG', key: 'Rg', width: 17, style: estiloHeader },
      //{ header: 'CTPS', key: 'Ctps', width: 32, style: estiloHeader }
      { header: 'ATIVIDADES ESPECIFICAS', key: 'Atividades', width: 38, style: estiloHeader },
    ];

    const rowHeader = worksheetChecklist.getRow(1);
    rowHeader.eachCell(cell => {
      cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFFFFFFF' } };
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.font = { bold: true, size: 14 }
    });

    // OS ATRIBUTOS DEVEM SER EQUIVALENTES AS DEFINIÇOES DAS COLUNAS
    dadosExcel.forEach((colaborador) => {
      const atividades = colaborador.Atividades.join(',');
      worksheetChecklist.addRow(
        {
          Nome: colaborador.Nome,
          Matricula: colaborador.Matricula,
          Equipe: colaborador.Equipe,
          DataAdmissao: colaborador.DataAdmissao,
          OrdemServico: colaborador.OrdemServico,
          Pedido: colaborador.NumPedido,
          Documentos: colaborador.ItensDestra.join(','),
          StatusDestra: ColaboradorStatusDestra.getNameEnum(colaborador.StatusDestra),
          Cpf: this.formatCPF(colaborador.Cpf),
          Rg: colaborador.Rg,
          //Ctps: colaborador.Ctps,
          Atividades: atividades,
        }
      );
      //worksheetChecklist.addRow(colaborador);
    });

    // Escreve o arquivo Excel
    workbook.xlsx.writeBuffer().then((buffer) => {
      // Cria um Blob a partir do buffer
      const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      // Cria um link temporário para download
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = 'Checklist.xlsx';
      // Dispara o clique no link para iniciar o download
      link.click();
      this._fuseProgressBarService.hide();
    });
  }
  //#endregion

  //#region  FORMATAÇÃO DE CAMPOS
  formatCPF(cpf: string): string {
    // Remove qualquer caractere que não seja número
    cpf = cpf.replace(/\D/g, '');
    // Aplica a máscara
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  }
  getColaboradorStatusDestra(valor: number): string {
    return ColaboradorStatusDestra.getNameEnum(valor);
  }
  //#endregion
}
