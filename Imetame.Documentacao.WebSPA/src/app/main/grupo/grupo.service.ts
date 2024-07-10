import { Injectable, Inject } from '@angular/core';

import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { DataService } from 'app/services/data.service';
import * as _ from 'lodash';
import { API_URL } from 'app/config/tokens';
import { map } from 'rxjs/operators';




@Injectable()
export class GrupoService implements Resolve<any>{
    
    

    routeParams: any;

    onSearchTextChanged: Subject<any>;
    onPageIndexChanged: Subject<any>;
    onPageSizeChanged: Subject<any>;

    onItensChanged: BehaviorSubject<any>;
    onItemChanged: BehaviorSubject<any>;
    onPortasChanged: BehaviorSubject<any>;
    onColaboradoresChanged: BehaviorSubject<any>;
    onTotalRowsChanged: BehaviorSubject<any>;
    //onDelete: BehaviorSubject<any>;
    itens: any[];
    item: any;
    portas: any[];
    colaboradores: any[];


    searchText: string;
    pageIndex: number;
    pageSize: number;
    count: number;
    

    constructor(
        @Inject(API_URL) private apiUrl: string,
        private dataService: DataService,
        
    ) {
        this.pageIndex = 0;
        this.pageSize = 25;
        this.searchText = '';


        this.onSearchTextChanged = new Subject();
        this.onPageIndexChanged = new Subject();
        this.onPageSizeChanged = new Subject();
        this.onItensChanged = new BehaviorSubject([]);
        this.onItemChanged = new BehaviorSubject([]);
        this.onPortasChanged = new BehaviorSubject([]);
        this.onItemChanged = new BehaviorSubject([]);
        this.onColaboradoresChanged = new BehaviorSubject({});
        this.onTotalRowsChanged = new BehaviorSubject({});

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
                Promise.all([this.getItem(),this.getPortas(), this.GetColaboradoresPorOs()]).then(() => { resolve(); }, reject);
            });

        } else {
            return new Promise((resolve, reject) => {
                Promise.all([this.getItens()]).then(() => {

                    //this.onSearchTextChanged.subscribe(searchText => {
                    //    this.searchText = searchText;
                    //    this.getItens();
                    //});

                    resolve();
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
                this.dataService.get<any>(this.apiUrl +'grupos/' + this.routeParams.id)
                    .subscribe((response: any) => {
                        this.item = response;
                        this.onItemChanged.next(this.item);
                        resolve(response);
                    }, reject);
            }
        });
    }

    getPortas(): Promise<any> {
        return new Promise((resolve, reject) => {
            if (this.routeParams.id === 'novo') {
                this.onItemChanged.next(null);
                resolve(null);
            }
            else {
                this.dataService.get<any>(this.apiUrl + 'grupos/' + this.routeParams.id+"/portas")
                    .subscribe((response: any) => {
                        this.portas = response;
                        this.onPortasChanged.next(this.portas);
                        resolve(response);
                    }, reject);
            }
        });
    }

    GetColaboradoresPorOs(): Promise<any> {
        return new Promise((resolve, reject) => {
            if (this.routeParams.id === 'novo') {
                this.onItemChanged.next(null);
                resolve(null);
            }
            else {
                this.dataService.get<any>(this.apiUrl + 'grupos/' + this.routeParams.id + "/colaboradores")
                    .subscribe((response: any) => {
                        this.colaboradores = response;
                        this.onColaboradoresChanged.next(this.colaboradores);
                        resolve(response);
                    }, reject);
            }
        });
    }
   
    getItens(): Promise<any> {               

        this.onItensChanged.next([]);
        

        let param = { 'pageIndex': this.pageIndex , 'pageSize': this.pageSize, 'query': this.searchText };

        return new Promise((resolve, reject) => {
            this.dataService.getList(this.apiUrl + 'grupos', param)
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
            this.dataService.post(this.apiUrl + 'grupos/', item)
                .subscribe((response: any) => {
                    this.onItemChanged.next(response);
                    resolve(response);                    
                }, reject);
        });
    }

   

    save(item) {
        return new Promise((resolve, reject) => {
            this.dataService.put(this.apiUrl + 'grupos/', item)
                .subscribe((response: any) => {
                    resolve(response);
                    //this.onChanged.next(response);
                }, reject);
        });
    }

    


    delete(item: any): Promise<any> {
        return new Promise((resolve, reject) => {
            this.dataService.delete(this.apiUrl + 'grupos/' + item.id)
                .subscribe(response => {
                    if (this.itens.length == 1 && this.pageIndex > 1) {
                        this.pageIndex = this.pageIndex - 1;
                    }
                    this.getItens();
                    //this.onDelete.next(response);
                    resolve(response);
                }, reject);
        });
    }

    deletePorta(grupo: any, porta: any): Promise<any> {
        return new Promise((resolve, reject) => {
            this.dataService.delete(this.apiUrl + 'grupos/' + grupo.id + '/portas/' + porta.id)
                .subscribe(response => {                    
                    resolve(response);
                }, reject);
        });
    }

    deleteColaborador(grupo: any, colaborador: any): Promise<any> {
        return new Promise((resolve, reject) => {
            this.dataService.delete(this.apiUrl + `grupos/${grupo.id}/colaboradores/${colaborador.numEmp}/${colaborador.tipCol}/${colaborador.numCad}`)
                .subscribe(response => {
                    resolve(response);
                }, reject);
        });
    }

    listarPorta(pageIndex: number, pageSize: number, searchText: string): Observable<any[]> {

        let param = { 'pageIndex': pageIndex, 'pageSize': pageSize, 'query': searchText };


        return this.dataService.getList(this.apiUrl + 'portas/', param)
            .pipe(map((response: any) => {
                if (response)
                    return response.data || [];
                return [];
            }));
    }

    listarColaborador(pageIndex: number, pageSize: number, searchText: string): Observable<any[]> {

        let param = { 'pageIndex': pageIndex, 'pageSize': pageSize, 'query': searchText };


        return this.dataService.getList(this.apiUrl + 'colaboradores', param)
            .pipe(map((response: any) => {
                if (response)
                    return response.data || [];
                return [];
            }));
    }

    addPorta(id: number, porta: any) {
        return new Promise((resolve, reject) => {
            this.dataService.post(this.apiUrl + 'grupos/' + id + '/portas', porta)
                .subscribe((response: any) => {
                    //this.onItemChanged.next(response);
                    resolve(response);
                }, reject);
        });
    }

    addColaborador(id: number, colaborador: any) {
        return new Promise((resolve, reject) => {
            this.dataService.post(this.apiUrl + 'grupos/' + id + '/colaboradores', colaborador)
                .subscribe((response: any) => {
                    //this.onItemChanged.next(response);
                    resolve(response);
                }, reject);
        });
    }
   
}
