import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';
import { RelatorioService } from 'app/main/relatorio/relatorio.service';
import { ChecklistModel } from 'app/models/DTO/RelatorioModel';
import { ColaboradorStatusDestra } from 'app/models/Enums/DestraEnums';
import * as ExcelJS from 'exceljs'
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'checklist-list',
  templateUrl: './checklist-list.component.html',
  styleUrls: ['./checklist-list.component.scss'],
  animations: fuseAnimations,
})
export class ChecklistListComponent implements OnInit {
  dataSource: MatTableDataSource<ChecklistModel> = new MatTableDataSource();
  searchInput: FormControl;
  displayedColumns = ["NOME", "MATRICULA", "EQUIPE", "DTADMISSAO", "OS", "NUMPEDIDO", "STATUSDESTRA", "RG", "CPF"];
  todosRegistros: ChecklistModel[];
  constructor(
    private titleService: Title,
    private _snackbar: MatSnackBar,
    private _fuseProgressBarService: FuseProgressBarService,
    private _relatorioService: RelatorioService,
    private route: ActivatedRoute,
  ) {
    this.searchInput = new FormControl("");
    this.searchInput.valueChanges.pipe(
      debounceTime(600),
      distinctUntilChanged()
    ).subscribe((newValue) => {
      this.filtrar(newValue);
    });
  }

  ngOnInit(): void {
    this.dataSource.data = this.route.snapshot.data['data'];
    this.todosRegistros = this.dataSource.data;
  }

  filtrar(searchValue: string) {
    this.dataSource.data = this.todosRegistros.filter(x => x.Nome.includes(searchValue.toUpperCase()))
  }

  //#region  FORMATAÇÃO DE CAMPOS
  formatCPF(cpf: string): string {
    cpf = cpf.replace(/\D/g, '');
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  }
  getColaboradorStatusDestra(valor: number): string {
    return ColaboradorStatusDestra.getNameEnum(valor);
  }
  formatarData(inputDate: string): string {
    const year = inputDate.substring(0, 4);
    const month = inputDate.substring(4, 6);
    const day = inputDate.substring(6, 8);
    // Retornar a data formatada
    return `${day}/${month}/${year}`;
  }
  //#endregion

  //#region GERAR EXCEL
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
      { header: 'ATIVIDADES ESPECIFICAS', key: 'Atividades', width: 38, style: estiloHeader },
    ];

    const rowHeader = worksheetChecklist.getRow(1);
    rowHeader.eachCell(cell => {
      cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFFFFFFF' } };
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.font = { bold: true, size: 14 }
    });

    worksheetChecklist.autoFilter = 'A1:K1';

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
}
