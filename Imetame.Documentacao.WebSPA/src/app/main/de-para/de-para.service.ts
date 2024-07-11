import { Injectable, Inject } from "@angular/core";

import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from "@angular/router";
import { BehaviorSubject, Observable, Subject } from "rxjs";
import { DataService } from "app/services/data.service";
import * as _ from "lodash";
import { API_URL } from "app/config/tokens";
import { map } from "rxjs/operators";

@Injectable()
export class DeParaService implements Resolve<any> {
    routeParams: any;

    onSearchTextChanged: Subject<any>;
    onPageIndexChanged: Subject<any>;
    onPageSizeChanged: Subject<any>;

    onItensChanged: BehaviorSubject<any>;
    onItemChanged: BehaviorSubject<any>;
    onTotalRowsChanged: BehaviorSubject<any>;

    onHitoricosChanged: BehaviorSubject<any>;
    onHistoricoTotalRowsChanged: BehaviorSubject<any>;
    onFiltroChanged: Subject<any>;
    onHistoricoPageIndexChanged: Subject<any>;
    onHistoricoPageSizeChanged: Subject<any>;

    itens: any[];
    item: any;
    historicos: any[];

    searchText: string;
    pageIndex: number;
    pageSize: number;
    count: number;

    historicoPageIndex: number;
    historicoPageSize: number;
    historicoCount: number;
    historicoFiltro: any;

    constructor(
        @Inject(API_URL) private apiUrl: string,
        private dataService: DataService
    ) {
        this.pageIndex = 0;
        this.pageSize = 25;
        this.searchText = "";

        this.historicoPageIndex = 0;
        this.historicoPageSize = 25;

        this.historicoFiltro = {
            localId: null,
            dataInicio: null,
            dataFim: null,
        };

        this.onSearchTextChanged = new Subject();
        this.onPageIndexChanged = new Subject();
        this.onPageSizeChanged = new Subject();
        this.onItensChanged = new BehaviorSubject([]);
        this.onItemChanged = new BehaviorSubject([]);
        this.onTotalRowsChanged = new BehaviorSubject({});

        this.onHitoricosChanged = new BehaviorSubject([]);
        this.onHistoricoPageIndexChanged = new Subject();
        this.onHistoricoPageSizeChanged = new Subject();
        this.onFiltroChanged = new Subject();

        this.onSearchTextChanged.subscribe((searchText) => {
            this.searchText = searchText;
            this.pageIndex = 0;
            this.getItens();
        });

        this.onPageIndexChanged.subscribe((pageIndex) => {
            this.pageIndex = pageIndex;
            this.getItens();
        });

        this.onPageSizeChanged.subscribe((pageSize) => {
            this.pageSize = pageSize;
            this.pageIndex = 0;
            this.getItens();
        });

        this.onFiltroChanged.subscribe((filtro) => {
            this.historicoFiltro = filtro;
            this.historicoPageIndex = 0;
        });

        this.onHistoricoPageIndexChanged.subscribe((pageIndex) => {
            this.historicoPageIndex = pageIndex;
        });

        this.onHistoricoPageSizeChanged.subscribe((pageSize) => {
            this.historicoPageSize = pageSize;
            this.pageIndex = 0;
        });
    }

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<any> | Promise<any> | any {
        this.routeParams = route.params;

        if (
            !_.isNull(this.routeParams.id) &&
            !_.isUndefined(this.routeParams.id)
        ) {
            return new Promise((resolve, reject) => {
                Promise.all([this.getItem()]).then(() => {
                    resolve(null);
                }, reject);
            });
        } else {
            return new Promise((resolve, reject) => {
                Promise.all([this.getItens()]).then(() => {
                    //this.onSearchTextChanged.subscribe(searchText => {
                    //    this.searchText = searchText;
                    //    this.getItens();
                    //});

                    resolve(null);
                }, reject);
            });
        }
    }

    getItem(): Promise<any> {
        return new Promise((resolve, reject) => {
            if (this.routeParams.id === "novo") {
                this.onItemChanged.next(null);
                resolve(null);
            } else {
                this.dataService
                    .get<any>(
                        this.apiUrl +
                            "CredenciadoraDePara/" +
                            this.routeParams.id
                    )
                    .subscribe((response: any) => {
                        this.item = response;
                        this.onItemChanged.next(this.item);
                        resolve(response);
                    }, reject);
            }
        });
    }

    getItens(): Promise<any> {
        this.onItensChanged.next([]);

        let param = {
            pageIndex: this.pageIndex,
            pageSize: this.pageSize,
            query: this.searchText,
        };

        return new Promise((resolve, reject) => {
            this.dataService
                .getList(this.apiUrl + "CredenciadoraDePara", param)
                .subscribe((response: any) => {
                    this.itens = response.Data;
                    if (this.count != response.Count) {
                        this.count = response.Count;
                        this.onTotalRowsChanged.next(this.count);
                    }
                    this.onItensChanged.next(this.itens);
                    resolve(response);
                }, reject);
        });
    }

    add(item) {
        return new Promise((resolve, reject) => {
            this.dataService
                .post(this.apiUrl + "CredenciadoraDePara/", item)
                .subscribe((response: any) => {
                    this.onItemChanged.next(response);
                    resolve(response);
                }, reject);
        });
    }

    save(item) {
        return new Promise((resolve, reject) => {
            this.dataService
                .put(this.apiUrl + "CredenciadoraDePara/", item)
                .subscribe((response: any) => {
                    resolve(response);
                    //this.onChanged.next(response);
                }, reject);
        });
    }

    delete(item: any): Promise<any> {
        return new Promise((resolve, reject) => {
            this.dataService
                .delete(this.apiUrl + "CredenciadoraDePara/" + item.Id)
                .subscribe((response) => {
                    if (this.itens.length == 1 && this.pageIndex > 1) {
                        this.pageIndex = this.pageIndex - 1;
                    }
                    this.getItens();
                    //this.onDelete.next(response);
                    resolve(response);
                }, reject);
        });
    }
}
