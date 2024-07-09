import { ChangeDetectorRef, Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormControl, UntypedFormGroup } from '@angular/forms';
import { MatDialogRef, MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router, ActivatedRoute } from '@angular/router';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { AtividadeEspecifica } from 'app/models/AtividadeEspecifica';
import { PaginatedResponse } from 'app/models/PaginatedResponse';
import { ConfirmDialogComponent } from 'app/shared/components/confirm-dialog/confirm-dialog.component';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { AtividadeEspecificaFormComponent } from '../atividade-especifica-form/atividade-especifica-form.component';
import { AtividadeEspecificaService } from '../atividade-especifica.service';
import { Title } from '@angular/platform-browser';
import { fuseAnimations } from '@fuse/animations';

@Component({
  selector: 'atividade-especifica-list',
  templateUrl: './atividade-especifica-list.component.html',
  styleUrls: ['./atividade-especifica-list.component.scss'],
  encapsulation: ViewEncapsulation.None,
  animations: fuseAnimations,
})
export class AtividadeEspecificaListComponent implements OnInit {


  //#region CONTRALADORES BOLEANOS
  blockRequisicao: boolean = false;
  //#endregion

  //#region PAGINATE e ITENS
  page: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  //#endregion 

  //#region PESQUISA E LISTAS
  AtividadeEspecificaList: AtividadeEspecifica[];
  searchInput: FormControl;
  nenhumDadoEncontrado: boolean;
  //#endregion

  //#region DATA SOURCE E FORMULARIO
  // displayedColumns = ["codigo", "descricao", "buttons"];
  displayedColumns = ["codigo","credenciadora","ultimaAtualizacao", "colaboradoresCadastrados"];
  dataSource: MatTableDataSource<AtividadeEspecifica>;
  form: UntypedFormGroup;
  dataAtual: Date = new Date()
  //#endregion

  //#region PAGINAÇÃO 
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  //#endregion
  confirmDialogRef: MatDialogRef<ConfirmDialogComponent>;
  constructor(
    public _matDialog: MatDialog,
    private _fuseConfirmationService: FuseConfirmationService,
    private _changeDetectorRef: ChangeDetectorRef,
    private _snackbar: MatSnackBar,
    public dialog: MatDialog,
    private router: Router,
    private titleService: Title,
    private route: ActivatedRoute,
    private _AtividadeEspecificaService: AtividadeEspecificaService,

  ) {

    this.searchInput = new FormControl("");
    this.dataSource = new MatTableDataSource();

    this.searchInput.valueChanges.pipe(
      debounceTime(600),
      distinctUntilChanged()
    ).subscribe((newValue) => {
      this.filtrar(newValue);
    });
  }


  ngOnInit(): void {
    this.titleService.setTitle("Atividade Específica - Imetame");
    this.loadData()
  }

  //#region FUNÇÃO DELETE - MATHEUS MONFREIDES - FARTEC SISTEMAS 

  deleteItem(ItemDelete: string,index: number) {
    this.confirmDialogRef = this.dialog.open(ConfirmDialogComponent, {
        disableClose: false,
    });

    this.confirmDialogRef.componentInstance.confirmMessage =
        "Deseja mesmo deletar o item?";

    this.confirmDialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this._AtividadeEspecificaService.Delete(ItemDelete).subscribe(
            {
              next: (response: AtividadeEspecifica) => {
                this._snackbar.open("Item excluído com sucesso", 'X', {
                  duration: 2500,
                  panelClass: 'snackbar-success',
                })
                const data = this.dataSource.data;
                data.splice(index, 1); // Remover o item na posição 'index'
                this.dataSource.data = data; // Atualizar a fonte de dados
              },
              error: (error) => {
                this._snackbar.open(error.error.erros, 'X', {
                  duration: 2500,
                  panelClass: ['mat-toolbar', 'mat-warn'],
                })
              }
            })
        }
        this.confirmDialogRef = null;
    });
}
  //#endregion

  //#region FUNÇÕES DE LOAD E ATUALIZAR PAGINA - 23/03/2024
  loadData() {
    this._AtividadeEspecificaService.listAtividadeEspecifica$.subscribe(
      (response: PaginatedResponse<AtividadeEspecifica>) => {
        this.totalCount = response.totalCount;
        this.page = response.page;
        this.pageSize = response.pageSize;
        this.dataSource.data = response.data;
        this.reassignPaginatorAndSort();
      }
    );
  }

  private reassignPaginatorAndSort() {
    if (this.paginator && this.sort) {
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    } else {
      setTimeout(() => this.reassignPaginatorAndSort(), 100);
    }
  }
  //#endregion

  //#region FUNÇÕES ABRIR MODAL DE CADASTRO/EDITAR DO FERIADO - MATHEUS MONFREIDES 23/03/2024
  abrir(item) {
    this.router.navigate([item.id], { relativeTo: this.route });
  }
  openModalEdit(itemEdit: AtividadeEspecifica) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = false;
    dialogConfig.width = '80%';
    dialogConfig.height = 'auto%';
    dialogConfig.data = { itemEdit };
    const dialogRef = this._matDialog.open(AtividadeEspecificaFormComponent, dialogConfig);

    //#region AFTER CLOSE REALIZADO DESSA MANEIRA PARA EVITAR VARIAS CONSULTAS NA API - MATHEUS MONFREIDES 04/12/2023
    dialogRef.afterClosed().subscribe((AtividadeEspecificaAtualizado: AtividadeEspecifica | null) => {
      if (AtividadeEspecificaAtualizado) {
        const index = this.dataSource.data.findIndex(m => m.Id === AtividadeEspecificaAtualizado.Id);
        if (index !== -1) {
          this.dataSource.data[index] = AtividadeEspecificaAtualizado;
          this.dataSource.data = [...this.dataSource.data];
        }
      }
    });
    //#endregion
  }
  //#endregion

  //#region FUNÇÃO FILTRAR REGISTROS - MATHEUS MONFREIDES 04/12/2023
  filtrar(value: string): void {
    this._AtividadeEspecificaService.GetAllPaginated(this.page, this.pageSize, value)
      .subscribe(
        {
          next: (response: PaginatedResponse<AtividadeEspecifica>) => {
            if (response.data.length <= 0) {
              this.nenhumDadoEncontrado = true;
            }
            this.totalCount = response.totalCount;
            this.page = response.page;
            this.pageSize = response.pageSize;

            this.AtividadeEspecificaList = response.data;
            this.dataSource = new MatTableDataSource();
            this.dataSource = new MatTableDataSource(response.data);
            this.reassignPaginatorAndSort();

          },
          error: (error) => { console.log(error.error) }
        });
  }
  //#endregion  

  //#region FUNÇÃO PAGINAÇÃO - MATHEUS MONFREIDES 01/12/2023
  onPageChange(event: PageEvent) {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.filtrar(this.searchInput.value);
  }
  //#endregion
}
