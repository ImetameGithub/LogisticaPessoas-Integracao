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

@Component({
  selector: 'checklist',
  templateUrl: './checklist.component.html',
  styleUrls: ['./checklist.component.scss'],
  encapsulation: ViewEncapsulation.None,
  animations: fuseAnimations,
})
export class ChecklistComponent implements OnInit, OnDestroy {
  isTravarPesquisa: boolean = false;
  OsOptions: CustomOptionsSelect[] = [];

  listPedidos: CustomOptionsSelect[] = [];
  ordemServicoOptions: CustomOptionsSelect[] = [];
  filteredOss: Observable<any[]>;
  oss: string[] = [];
  osCtrl = new FormControl();

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
  gerarRelatorio() {
    if (!this.filtroForm.valid) {
      return;
    }
    const idPedido = this.filtroForm.get("pedido").value;
    const codOs = this.filtroForm.get("oss").value;
    this._relatorioService.GetDadosCheckList(idPedido, codOs).subscribe({
      next: (response: ChecklistModel[]) => {
        const dadosExcel = response;
        const workbook = new ExcelJS.Workbook();

        const worksheetChecklist = workbook.addWorksheet('Checklist');

        const estiloHeader: Partial<ExcelJS.Style> = { alignment: { horizontal: 'centerContinuous', vertical: 'middle', wrapText: true }, };

        //  DEFINIÇÃO DAS COLUNAS
        worksheetChecklist.columns = [
          { header: 'NOME', key: 'nome', width: 12, style: estiloHeader },
          { header: 'CPF', key: 'cpf', width: 32, style: estiloHeader },
          { header: 'ATIVIDADES ESPECIFICAS', key: 'atividade', width: 40, style: estiloHeader },
        ];

        const rowHeader = worksheetChecklist.getRow(1);
        rowHeader.eachCell(cell => {
          cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFFFFFFF' } };
          cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
          cell.font = { bold: true, size: 14 }
        });

        // USAR dadosExcel.ListaMateriais.forEach(material => worksheetChecklist.addRow(material)); PARA QUANDO TODOS DADOS NECESSARIOS FOREM RETORNADOS 
        // OS ATRIBUTOS DEVEM SER EQUIVALENTES AS DEFINIÇOES DAS COLUNAS
        dadosExcel.forEach((colaborador) => {
          const atividades = colaborador.Atividades.join(',');
          worksheetChecklist.addRow({ nome: colaborador.Nome, cpf: this.formatCPF(colaborador.Cpf), atividade: atividades });
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
        });
      },
      error: (error) => {
        this._snackbar.open("Erro ao gerar Excel.", 'X', {
          duration: 2500,
          panelClass: 'snackbar-error',
        });
      }
    })
  }

  formatCPF(cpf: string): string {
    // Remove qualquer caractere que não seja número
    cpf = cpf.replace(/\D/g, '');
    // Aplica a máscara
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  }
  //#endregion
}
