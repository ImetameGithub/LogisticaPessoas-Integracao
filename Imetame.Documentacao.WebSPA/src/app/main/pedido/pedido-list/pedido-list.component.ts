import {
  Component,
  OnDestroy,
  OnInit,
  ViewEncapsulation,
  ViewChild,
  ViewChildren,
  AfterViewInit,
  ChangeDetectorRef,
} from "@angular/core";
import { takeUntil, debounceTime, distinctUntilChanged } from "rxjs/operators";
import { Subject, merge } from "rxjs";
import { fuseAnimations } from "@fuse/animations";

import { FilesDataSource } from "app/utils/files-data-source";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { MatDialogRef, MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";

import { ShowErrosDialogComponent } from "app/shared/components/show-erros-dialog/show-erros-dialog.component";
import { ConfirmDialogComponent } from "app/shared/components/confirm-dialog/confirm-dialog.component";

import { Title } from "@angular/platform-browser";
import { FormControl, UntypedFormGroup } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";

import { FusePerfectScrollbarDirective } from "@fuse/directives/fuse-perfect-scrollbar/fuse-perfect-scrollbar.directive";
import { PedidoService } from "../pedido.service";
import { FuseProgressBarService } from "@fuse/components/progress-bar/progress-bar.service";
import { Pedido } from "app/models/Pedido";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { FuseConfirmationService } from "@fuse/services/confirmation";
import { PaginatedResponse } from "app/models/PaginatedResponse";
import { PedidoFormComponent } from "../pedido-form/pedido-form.component";

@Component({
  selector: "pedido-list",
  templateUrl: "./pedido-list.component.html",
  styleUrls: ["./pedido-list.component.scss"],
  encapsulation: ViewEncapsulation.None,
  animations: fuseAnimations,
})
export class PedidoListComponent implements OnInit {

  //#region CONTRALADORES BOLEANOS
  blockRequisicao: boolean = false;
  //#endregion

  //#region PAGINATE e ITENS
  page: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  //#endregion 

  //#region PESQUISA E LISTAS
  PedidoList: Pedido[];
  searchInput: FormControl;
  nenhumDadoEncontrado: boolean;
  //#endregion

  //#region DATA SOURCE E FORMULARIO
  displayedColumns = ["credenciadora", "unidade", "numPedido", "buttons"];
  dataSource: MatTableDataSource<Pedido>;
  form: UntypedFormGroup;
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
    private titleService: Title,
    private router: Router,
    private route: ActivatedRoute,
    private _PedidoService: PedidoService,

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
    this.titleService.setTitle("Pedido - Imetame");
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
          this._PedidoService.Delete(ItemDelete).subscribe(
            {
              next: (response: Pedido) => {
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
    this._PedidoService.listPedido$.subscribe(
      (response: PaginatedResponse<Pedido>) => {
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
  openModalEdit(itemEdit: Pedido) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = false;
    dialogConfig.width = '80%';
    dialogConfig.height = 'auto%';
    dialogConfig.data = { itemEdit };
    const dialogRef = this._matDialog.open(PedidoFormComponent, dialogConfig);

    //#region AFTER CLOSE REALIZADO DESSA MANEIRA PARA EVITAR VARIAS CONSULTAS NA API - MATHEUS MONFREIDES 04/12/2023
    dialogRef.afterClosed().subscribe((PedidoAtualizado: Pedido | null) => {
      if (PedidoAtualizado) {
        const index = this.dataSource.data.findIndex(m => m.id === PedidoAtualizado.id);
        if (index !== -1) {
          this.dataSource.data[index] = PedidoAtualizado;
          this.dataSource.data = [...this.dataSource.data];
        }
      }
    });
    //#endregion
  }
  //#endregion

  //#region FUNÇÃO FILTRAR REGISTROS - MATHEUS MONFREIDES 04/12/2023
  filtrar(value: string): void {
    // if (value == '' || value == "") {
    //   this.loadData();
    //   return;
    // }

    // if (this.paginator.pageIndex !== 0) {
    //   this.paginator.firstPage();
    // }

    this._PedidoService.GetAllPaginated(this.page, this.pageSize, value)
      .subscribe(
        {
          next: (response: PaginatedResponse<Pedido>) => {
            if (response.data.length <= 0) {
              this.nenhumDadoEncontrado = true;
            }
            this.totalCount = response.totalCount;
            this.page = response.page;
            this.pageSize = response.pageSize;

            this.PedidoList = response.data;
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
