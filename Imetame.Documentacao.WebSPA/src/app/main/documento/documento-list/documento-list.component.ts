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
  import { DocumentoService } from "../documento.service";
  import { FuseProgressBarService } from "@fuse/components/progress-bar/progress-bar.service";
  import { Documento } from "app/models/Documento";
  import { MatSort } from "@angular/material/sort";
  import { MatTableDataSource } from "@angular/material/table";
  import { FuseConfirmationService } from "@fuse/services/confirmation";
  import { PaginatedResponse } from "app/models/PaginatedResponse";
  import { DocumentoFormComponent } from "../documento-form/documento-form.component";
  
  @Component({
    selector: "documento-list",
    templateUrl: "./documento-list.component.html",
    styleUrls: ["./documento-list.component.scss"],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
  })
  export class DocumentoListComponent implements OnInit {
  
    //#region CONTRALADORES BOLEANOS
    blockRequisicao: boolean = false;
    //#endregion
  
    //#region PAGINATE e ITENS
    page: number = 1;
    pageSize: number = 10;
    totalCount: number = 0;
    //#endregion 
  
    //#region PESQUISA E LISTAS
    DocumentoList: Documento[];
    searchInput: FormControl;
    nenhumDadoEncontrado: boolean;
    //#endregion
  
    //#region DATA SOURCE E FORMULARIO
    displayedColumns = ["Descricao", "DescricaoDestra", "DescricaoProtheus", "buttons"];
    dataSource: MatTableDataSource<Documento>;
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
      private _DocumentoService: DocumentoService,
  
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
      this.titleService.setTitle("Documento - Imetame");
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
            this._DocumentoService.Delete(ItemDelete).subscribe(
              {
                next: (response: Documento) => {
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
      this._DocumentoService.listDocumento$.subscribe(
        (response: PaginatedResponse<Documento>) => {
          this.totalCount = response.TotalCount;
          this.page = response.Page;
          this.pageSize = response.PageSize;
          this.dataSource.data = response.Data;
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
      this.router.navigate([item.Id], { relativeTo: this.route });
    }
    openModalEdit(itemEdit: Documento) {
      const dialogConfig = new MatDialogConfig();
      dialogConfig.autoFocus = false;
      dialogConfig.width = '80%';
      dialogConfig.height = 'auto%';
      dialogConfig.data = { itemEdit };
      const dialogRef = this._matDialog.open(DocumentoFormComponent, dialogConfig);
  
      //#region AFTER CLOSE REALIZADO DESSA MANEIRA PARA EVITAR VARIAS CONSULTAS NA API - MATHEUS MONFREIDES 04/12/2023
      dialogRef.afterClosed().subscribe((DocumentoAtualizado: Documento | null) => {
        if (DocumentoAtualizado) {
          const index = this.dataSource.data.findIndex(m => m.Id === DocumentoAtualizado.Id);
          if (index !== -1) {
            this.dataSource.data[index] = DocumentoAtualizado;
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
  
      this._DocumentoService.GetAllPaginated(this.page, this.pageSize, value)
        .subscribe(
          {
            next: (response: PaginatedResponse<Documento>) => {
              if (response.Data.length <= 0) {
                this.nenhumDadoEncontrado = true;
              }
              this.totalCount = response.TotalCount;
              this.page = response.Page;
              this.pageSize = response.PageSize;
  
              this.DocumentoList = response.Data;
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
  