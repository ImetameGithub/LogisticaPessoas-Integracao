import { DOCUMENT } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MAT_SORT_HEADER_INTL_PROVIDER_FACTORY } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';
import { RelatorioService } from 'app/main/relatorio/relatorio.service';
import { CheckDocumento, ChecklistModel } from 'app/models/DTO/RelatorioModel';
import { ColaboradorStatusDestra, DocumentoStatusDestra } from 'app/models/Enums/DestraEnums';
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

  documentosColumns: CheckDocumento[] = [];
  //displayedColumns = ["NOME", "ATIVIDADE"];
  displayedColumns: string[] = ['nome', 'matricula', 'cracha', 'equipe', 'funcao', 'atividade'];

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


    //#region LISTAR TODOS DOCUMENTOS
    let todosDocumentos: CheckDocumento[] = [];
    this.dataSource.data.forEach((colaborador) => {
      todosDocumentos.push(...colaborador.Documentos);
    });
    this.documentosColumns = todosDocumentos.filter((item, index, self) =>
      index === self.findIndex((t) => t.IdDestra === item.IdDestra)
    );
    //#endregion
  }

  filtrar(searchValue: string) {
    this.dataSource.data = this.todosRegistros.filter(x => x.Nome.includes(searchValue.toUpperCase()))
  }

  // Função que retorna o nome das colunas dos documentos
  getDocumentColumns() {
    return this.documentosColumns.map(x => x.nome);
  }

  // Combina as colunas fixas com as colunas dinâmicas
  getAllColumns() {
    return [...this.displayedColumns, ...this.getDocumentColumns()];
  }

  // Função para pegar o documento no índice correspondente
  getDocumento(documento: string, checklistModel: ChecklistModel) {
    //const doc: CheckDocumento = this.documentosColumns.find(x => x.nome == documento);
    const doc: CheckDocumento = checklistModel.Documentos.find(x => x.nome == documento)
    if (doc != null)
      return `Validade: \n ${doc.validade} - Destra \n ${DocumentoStatusDestra.getNameEnum(doc.Status)}`;
    else
      return `Não Relacionado`;
  }

  //#region  FORMATAÇÃO DE CAMPOS
  formatCPF(cpf: string): string {
    cpf = cpf.replace(/\D/g, '');
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  }
  getColaboradorStatusDestra(valor: number): string {
    return ColaboradorStatusDestra.getNameEnum(valor);
  }
  getStatusDestraColor(valor: number): string {
    return DocumentoStatusDestra.getColorEnum(valor);
  }
  formatarData(inputDate: string): string {
    const year = inputDate.substring(0, 4);
    const month = inputDate.substring(4, 6);
    const day = inputDate.substring(6, 8);
    // Retornar a data formatada
    return `${day}/${month}/${year}`;
  }

  // Função para calcular a largura da coluna com base no comprimento do cabeçalho
  getHeaderWidth(column: string): number {
    const baseWidth = 20; // Largura base, pode ajustar conforme necessário
    const additionalWidthPerCharacter = 8; // Pixels adicionais por caractere, ajuste conforme necessário

    // Calcular a largura mínima da coluna com base no tamanho do cabeçalho
    const calculatedWidth = baseWidth + column.length * additionalWidthPerCharacter;

    // Definir um valor mínimo de largura
    return Math.max(calculatedWidth, 100); // Definir o mínimo como 100px, ajuste conforme necessário
  }
  //#endregion

  //#region GERAR EXCEL
  gerarRelatorio() {
    this._fuseProgressBarService.show();
    const dadosExcel = this.dataSource.data;
    const workbook = new ExcelJS.Workbook();

    const worksheetChecklist = workbook.addWorksheet('Checklist');

    const estiloHeader: Partial<ExcelJS.Style> = { alignment: { horizontal: 'centerContinuous', vertical: 'middle', wrapText: true }, };

    // RECEBE A PRIMEIRA LINHA DA PLANILHA
    const rowHeader = worksheetChecklist.getRow(1);
    rowHeader.getCell(1).value = "COLABORADOR";
    rowHeader.getCell(2).value = "MATRICULA";
    rowHeader.getCell(3).value = "CRACHA";
    rowHeader.getCell(4).value = "EQUIPE";
    rowHeader.getCell(5).value = "FUNÇÃO";
    rowHeader.getCell(6).value = "ATIVIDADE";

    this.documentosColumns.forEach((documento, index) => {
      rowHeader.getCell(7 + index).value = documento.nome;
    })

    dadosExcel.forEach((colaborador, rowIndex) => {
      const row = worksheetChecklist.getRow(rowIndex + 2);
      row.getCell(1).value = colaborador.Nome;
      row.getCell(2).value = colaborador.Matricula;
      row.getCell(3).value = colaborador.Cracha;
      row.getCell(4).value = colaborador.Equipe;
      row.getCell(5).value = colaborador.Funcao;
      row.getCell(6).value = colaborador.Atividade;

      this.documentosColumns.forEach((documento, index) => {
        const documentoColaborador: CheckDocumento | undefined = colaborador.Documentos.find(x => x.IdDestra == documento.IdDestra);
        if (documentoColaborador) {
          row.getCell(7 + index).value = {
            richText: [
              {
                text: `Validade: \n ${documentoColaborador.validade} - Destra \n`,
                font: { color: { argb: '000000' } } // Cor preta
              },
              {
                text: `${DocumentoStatusDestra.getNameEnum(documentoColaborador.Status)}`,
                font: { color: { argb: this.getStatusDestraColor(documentoColaborador.Status) } } // Cor azul
              }
            ]
          };
        } else {
          row.getCell(7 + index).value = "Não Relacionado";
        }
      })

      row.eachCell(cell => {
        cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };
        cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFFFFFFF' } };
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
        cell.font = { bold: false, size: 12 }
      });
    });

    rowHeader.eachCell(cell => {
      cell.alignment = { horizontal: 'center' };
      cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFFFFFFF' } };
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.font = { bold: true, size: 14 }
    });

    // Ajuste da largura das colunas com base no conteúdo
    worksheetChecklist.columns.forEach(column => {
      let maxLength = 0;
      column.eachCell({ includeEmpty: true }, cell => {
        const columnLength = cell.value ? cell.value.toString().length : 0;
        if (columnLength > maxLength) {
          maxLength = columnLength;
        }
      });

      // Calcula o width e garante que o mínimo seja 19
      column.width = (maxLength + 2) < 19 ? 19 : maxLength + 2;
    });

    worksheetChecklist.autoFilter = "A1:E1";

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
