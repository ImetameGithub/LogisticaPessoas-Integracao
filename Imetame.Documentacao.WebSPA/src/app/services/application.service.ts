import { Injectable, Inject } from '@angular/core';

import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { DataService } from 'app/services/data.service';
import * as _ from 'lodash';
import { Application } from 'app/models/application.model';
import { API_URL } from 'app/config/tokens';
import { map } from 'rxjs/operators';



@Injectable()
export class ApplicationService implements Resolve<any>{

    routeParams: any;

    onSearchTextChanged: Subject<any>;
    onPageIndexChanged: Subject<any>;
    onPageSizeChanged: Subject<any>;

    onItensChanged: BehaviorSubject<any>;
    onItemChanged: BehaviorSubject<any>;
    onTotalRowsChanged: BehaviorSubject<any>;
    onDelete: BehaviorSubject<any>;
    itens: Application[];
    item: Application;
    
    searchText: string;
    pageIndex: number;
    pageSize: number;
    count: number;

    constructor(
        @Inject(API_URL) private apiUrl: string,
        private dataService: DataService
    ) {
        this.pageIndex = 0;
        this.pageSize = 25;
        this.searchText = '';


        this.onSearchTextChanged = new Subject();
        this.onPageIndexChanged = new Subject();
        this.onPageSizeChanged = new Subject();
        this.onItensChanged = new BehaviorSubject([]);
        this.onItemChanged = new BehaviorSubject([]);
        this.onTotalRowsChanged = new BehaviorSubject({});
        this.onDelete = new BehaviorSubject({});

        this.onSearchTextChanged.subscribe(searchText => {
            this.searchText = searchText;
            this.pageIndex = 0;
            this.getItens();
        });

        this.onPageIndexChanged.subscribe(pageIndex => {
            this.pageIndex = pageIndex;
            this.getItens();
        });

        this.onPageSizeChanged.subscribe(pageSize => {
            this.pageSize = pageSize;
            this.pageIndex = 0;
            this.getItens();
        });
        
    }


    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {

        this.routeParams = route.params;

        if (!_.isNull(this.routeParams.id) && !_.isUndefined(this.routeParams.id)) {
            return new Promise((resolve, reject) => {
                // Promise.all([this.getItem()]).then(() => { resolve(); }, reject);
            });

        } else {
            return new Promise((resolve, reject) => {
                Promise.all([this.getItens()]).then(() => {

                    //this.onSearchTextChanged.subscribe(searchText => {
                    //    this.searchText = searchText;
                    //    this.getItens();
                    //});

                    // resolve();
                }, reject);
            });
        }
    }


    getItem(): Promise<any> {
        return new Promise((resolve, reject) => {
            if (this.routeParams.id === 'novo') {
                this.onItemChanged.next(null);
                resolve(null);
            }
            else {
                this.dataService.get<any>(this.apiUrl +'applications/' + this.routeParams.id)
                    .subscribe((response: any) => {
                        this.item = response;
                        this.onItemChanged.next(this.item);
                        resolve(response);
                    }, reject);
            }
        });
    }

    getItens(): Promise<any> {               

        let param = { 'pageIndex': this.pageIndex , 'pageSize': this.pageSize, 'query': this.searchText };

        return new Promise((resolve, reject) => {
            this.dataService.getList(this.apiUrl + 'applications/', param)
                .subscribe((response: any) => {
                    this.itens = response.data;
                    if (this.count != response.count) {
                        this.count = response.count;
                        this.onTotalRowsChanged.next(this.count);
                    }                 
                    this.onItensChanged.next(this.itens);
                    resolve(response);
                }, reject);
        });
    }

    add(item) {
        return new Promise((resolve, reject) => {
            this.dataService.post(this.apiUrl + 'applications/', item)
                .subscribe((response: any) => {
                    resolve(response);
                    //this.onChanged.next(response);
                }, reject);
        });
    }

    save(item) {
        return new Promise((resolve, reject) => {
            this.dataService.put(this.apiUrl + 'applications/', item)
                .subscribe((response: any) => {
                    resolve(response);
                    //this.onChanged.next(response);
                }, reject);
        });
    }

    delete(item: any): Promise<any> {
        return new Promise((resolve, reject) => {
            this.dataService.delete(this.apiUrl + 'applications/' + item.id)
                .subscribe(response => {
                    if (this.itens.length == 1 && this.pageIndex > 1) {
                        this.pageIndex = this.pageIndex - 1;
                    }
                    this.getItens();
                    this.onDelete.next(response);
                    resolve(response);
                }, reject);
        });
    }

    listar(pageIndex: number, pageSize: number, searchText: string): Observable<Application[]> {

        let param = { 'pageIndex': pageIndex, 'pageSize': pageSize, 'query': searchText };


        return this.dataService.getList(this.apiUrl + 'applications/', param)
            .pipe(map((response: any) => {
                if (response)
                    return response.data || [];
                return [];
            }));
    }

    get(id: string): Observable<Application> {
        return this.dataService.get(this.apiUrl + 'applications/' + id);           
    }

    upload( files: any): Observable<any> {
        return this.dataService.uploadFile(this.apiUrl + 'applications/imagem/', files);
    }
}
