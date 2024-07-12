import { ChangeDetectorRef, Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormControl, UntypedFormGroup } from '@angular/forms';
import { MatDialogRef, MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { Router, ActivatedRoute } from '@angular/router';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { Colaborador } from 'app/models/Colaborador';
import { PaginatedResponse } from 'app/models/PaginatedResponse';
import { ConfirmDialogComponent } from 'app/shared/components/confirm-dialog/confirm-dialog.component';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ColaboradorService } from '../colaboradores.service';
import { ColaboradorModel, ColaboradorProtheusModel } from 'app/models/DTO/ColaboradorModel';
import { ColaboradoresFormComponent } from '../colaboradores-form/colaboradores-form.component';
import { fuseAnimations } from '@fuse/animations';
import { ColaboradoresAtividadeModalComponent } from '../colaboradores-atividade-modal/colaboradores-atividade-modal.component';
import { AtividadeEspecifica } from 'app/models/AtividadeEspecifica';
import { ColaboradoresFiltroComponent } from '../colaboradores-filtro/colaboradores-filtro.component';

@Component({
  selector: 'colaboradores-list',
  templateUrl: './colaboradores-list.component.html',
  styleUrls: ['./colaboradores-list.component.scss'],
  encapsulation: ViewEncapsulation.None,
  animations: fuseAnimations,
})
export class ColaboradoresListComponent implements OnInit {


  //#region CONTRALADORES BOLEANOS
  blockRequisicao: boolean = false;
  //#endregion

  //#region PAGINATE e ITENS
  page: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  //#endregion 

  //#region PESQUISA E LISTAS
  ColaboradorList: Colaborador[];
  searchInput: FormControl;
  nenhumDadoEncontrado: boolean;
  //#endregion

  //#region DATA SOURCE E FORMULARIO
  displayedColumns = ["cadastro", "cracha", "nome", "empresa", "mudaFuncao", "atividadeEspecifica", "buttons"];
  dataSource: MatTableDataSource<Colaborador>;
  form: UntypedFormGroup;
  //#endregion

  _listAllColaboradores: ColaboradorProtheusModel[];
  _listAtividades: AtividadeEspecifica[];

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
    private titleService: Title,
    private router: Router,
    private route: ActivatedRoute,
    private _ColaboradorService: ColaboradorService,

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
    const Colaborador: PaginatedResponse<Colaborador> = this.route.snapshot.data['data'].colaboradorPaginated;
    this.totalCount = Colaborador.TotalCount;
    this.page = Colaborador.Page;
    this.pageSize = Colaborador.PageSize;
    this.dataSource.data = Colaborador.Data;
    this.reassignPaginatorAndSort();
    this.titleService.setTitle("Colaborador - Imetame");
    // this.loadData()
  }

  //#region FUNÇÃO DELETE - MATHEUS MONFREIDES - FARTEC SISTEMAS 

  deleteItem(ItemDelete: string, index: number) {
    this.confirmDialogRef = this.dialog.open(ConfirmDialogComponent, {
      disableClose: false,
    });

    this.confirmDialogRef.componentInstance.confirmMessage =
      "Deseja mesmo deletar o item?";

    this.confirmDialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this._ColaboradorService.Delete(ItemDelete).subscribe(
          {
            next: (response: Colaborador) => {
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

  abrirModal() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = false;
    dialogConfig.width = '80%';
    dialogConfig.height = 'auto%';
    dialogConfig.data = {
      _listColaboradores: this.route.snapshot.data['data'].listAllColaborador,
      _listAtividades: this.route.snapshot.data['data'].listAllAtividades
    };
    const dialogRef = this._matDialog.open(ColaboradoresAtividadeModalComponent, dialogConfig);

    //#region AFTER CLOSE REALIZADO DESSA MANEIRA PARA EVITAR VARIAS CONSULTAS NA API - MATHEUS MONFREIDES 04/12/2023
    // dialogRef.afterClosed().subscribe((ColaboradorAtualizado: ColaboradorModel | null) => {
    //   if (ColaboradorAtualizado) {
    //     const index = this.dataSource.data.findIndex(m => m.Id === ColaboradorAtualizado.Id);
    //     if (index !== -1) {
    //       this.dataSource.data[index] = ColaboradorAtualizado;
    //       this.dataSource.data = [...this.dataSource.data];
    //     }
    //   }
    // });
    //#endregion
  }
  abrirModalFiltro() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = false;
    dialogConfig.width = '80%';
    dialogConfig.height = 'auto%';
    dialogConfig.data = {     
    };
    const dialogRef = this._matDialog.open(ColaboradoresFiltroComponent, dialogConfig);

    //#region AFTER CLOSE REALIZADO DESSA MANEIRA PARA EVITAR VARIAS CONSULTAS NA API - MATHEUS MONFREIDES 04/12/2023
    // dialogRef.afterClosed().subscribe((ColaboradorAtualizado: ColaboradorModel | null) => {
    //   if (ColaboradorAtualizado) {
    //     const index = this.dataSource.data.findIndex(m => m.Id === ColaboradorAtualizado.Id);
    //     if (index !== -1) {
    //       this.dataSource.data[index] = ColaboradorAtualizado;
    //       this.dataSource.data = [...this.dataSource.data];
    //     }
    //   }
    // });
    //#endregion
  }
  //#endregion

  //#region FUNÇÃO FILTRAR REGISTROS - MATHEUS MONFREIDES 04/12/2023
  filtrar(value: string): void {
    this._ColaboradorService.GetAllPaginated(this.page, this.pageSize, value)
      .subscribe(
        {
          next: (response: PaginatedResponse<Colaborador>) => {
            if (response.Data.length <= 0) {
              this.nenhumDadoEncontrado = true;
            }
            this.totalCount = response.TotalCount;
            this.page = response.Page;
            this.pageSize = response.PageSize;

            this.ColaboradorList = response.Data;
            this.dataSource = new MatTableDataSource();
            this.dataSource = new MatTableDataSource(response.Data);
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
