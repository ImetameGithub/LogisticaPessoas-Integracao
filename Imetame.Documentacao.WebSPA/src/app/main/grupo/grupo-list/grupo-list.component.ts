import { Component, OnDestroy, OnInit, ViewEncapsulation, ViewChild, ViewChildren, AfterViewInit } from '@angular/core';
import { takeUntil, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Subject, merge } from 'rxjs';
import { fuseAnimations } from '@fuse/animations';

import { FilesDataSource } from 'app/utils/files-data-source';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { ShowErrosDialogComponent } from 'app/shared/components/show-erros-dialog/show-erros-dialog.component';
import { ConfirmDialogComponent } from 'app/shared/components/confirm-dialog/confirm-dialog.component';

import { Title } from '@angular/platform-browser';
import { FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { FusePerfectScrollbarDirective } from '@fuse/directives/fuse-perfect-scrollbar/fuse-perfect-scrollbar.directive';
import { GrupoService } from '../grupo.service';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';
import { PermissaoService } from 'app/services/permissao.service';

@Component({
  selector: 'grupo-list',
  templateUrl: './grupo-list.component.html',
    styleUrls: ['./grupo-list.component.scss'],
   encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class GrupoListComponent implements OnInit, OnDestroy, AfterViewInit {


    

    private _unsubscribeAll: Subject<any>;
    
    displayedColumns = ['nome','descricao',  'buttons'];

    @ViewChild(FusePerfectScrollbarDirective, { static: false })
    directiveScroll: FusePerfectScrollbarDirective;

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    confirmDialogRef: MatDialogRef<ConfirmDialogComponent>;
    errorDialogRef: MatDialogRef<ShowErrosDialogComponent>;

    searchInput: FormControl;
    searchInputEl: any;
    @ViewChildren('searchInputEl') searchInputField;
    dataSource: FilesDataSource<any>;
    isBusy: boolean = false;

    constructor(
        private service: GrupoService,
        public snackBar: MatSnackBar,
        public dialog: MatDialog,
        private router: Router,
        private route: ActivatedRoute,
        private titleService: Title,
        private _fuseProgressBarService: FuseProgressBarService,
        
    ) {
        this._unsubscribeAll = new Subject();
        this.searchInput = new FormControl('');

        this.dataSource = new FilesDataSource<any>(this.service);

        
    }


    ngOnInit() {
        this.titleService.setTitle("Grupos - Imetame");
        

        this.paginator.page.pipe(takeUntil(this._unsubscribeAll)).subscribe(q => {

            if (this.paginator.pageIndex != this.service.pageIndex) {
                this.service.onPageIndexChanged.next(this.paginator.pageIndex);
            }

            if (this.paginator.pageSize != this.service.pageSize) {
                this.service.onPageSizeChanged.next(this.paginator.pageSize);
            }            
        });

        this.searchInput.setValue(this.service.searchText);

        this.searchInput.valueChanges
            .pipe(
                takeUntil(this._unsubscribeAll),
                debounceTime(600),
                distinctUntilChanged()
            )
            .subscribe(searchText => {
                this.service.onSearchTextChanged.next(searchText);
            });

        this.service.onItensChanged
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(itens => {
                this._fuseProgressBarService.hide();
                this.scrollToTop()
            });

        //em qualquer um desse eventos vai carregar novos itens
        merge(
            this.service.onSearchTextChanged,
            this.service.onPageIndexChanged,
            this.service.onPageSizeChanged,
            //this.service.onFiltroChanged,
        )
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(q => {
                this._fuseProgressBarService.setMode('indeterminate');
                this._fuseProgressBarService.show()
            });
    }

    ngAfterViewInit() {

        this.searchInputEl = this.searchInputField.first.nativeElement;
        setTimeout(() => {
            this.searchInputEl.focus();
        });
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    delete(item) {
        this.confirmDialogRef = this.dialog.open(ConfirmDialogComponent, {
            disableClose: false
        });


        this.confirmDialogRef.componentInstance.confirmMessage = 'Deseja mesmo deletar o item?';

        this.confirmDialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.service.delete(item)
                    .then(deletado => {
                        this.snackBar.open("Item deletado!", "Ok", {
                            verticalPosition: 'top',
                            duration: 2000
                        });

                    },
                        error => this.showError(error));


            }
            this.confirmDialogRef = null;
        });
    }

    abrir(item) {
        this.router.navigate([item.id], { relativeTo: this.route });
    }

    showError(error) {
        this.isBusy = false;
        this._fuseProgressBarService.hide();
        this.errorDialogRef = this.dialog.open(ShowErrosDialogComponent);
        this.errorDialogRef.componentInstance.error = error;


    }

   



    

    scrollToTop(speed?: number): void {
        speed = speed || 400;
        if (this.directiveScroll) {
            this.directiveScroll.update();

            setTimeout(() => {
                this.directiveScroll.scrollToTop(0, speed);
            });
        }
    }
}
