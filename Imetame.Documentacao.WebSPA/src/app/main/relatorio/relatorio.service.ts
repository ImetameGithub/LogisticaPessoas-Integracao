import { Injectable, Inject } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot, } from "@angular/router";
import { BehaviorSubject, Observable, ReplaySubject, Subject, forkJoin } from "rxjs";
import { DataService } from "app/services/data.service";
import * as _ from "lodash";
import { API_URL, EXTRANET_API_URL } from "app/config/tokens";
import { map, tap } from 'rxjs/operators';
import { Pedido } from "app/models/Pedido";
import { environment } from "environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Colaborador } from "app/models/Colaborador";
import { ColaboradorModel, ColaboradorProtheusModel } from "app/models/DTO/ColaboradorModel";
import { ChecklistModel } from "app/models/DTO/RelatorioModel";

@Injectable()
export class RelatorioService implements Resolve<any> {
    routeParams: any;
    private _listPedido: ReplaySubject<Pedido[]> = new ReplaySubject<Pedido[]>(1);
    //private _listColaboradores: ReplaySubject<ColaboradorProtheusModel[]> = new ReplaySubject<ColaboradorProtheusModel[]>(1);

    constructor(
        @Inject(EXTRANET_API_URL) private extranetApiUrl: string,
        private _httpClient: HttpClient,
        @Inject(API_URL) private apiUrl: string,
        private dataService: DataService
    ) {

    }

    //#region FUNÇÕES DE ECAPSULAMENTOS
    get listPedido$(): Observable<Pedido[]> {
        return this._listPedido.asObservable();
    }
    // get listColaboradores$(): Observable<ColaboradorProtheusModel[]> {
    //     return this._listColaboradores.asObservable();
    // }
    //#endregion

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<any> | Promise<any> | any {
        this.routeParams = route.params;

        return this._httpClient.get<Pedido[]>(`${environment.Pedido.GetAll}`).pipe(
            tap((pedidos: Pedido[]) => {
                this._listPedido.next(pedidos);
            })
        );
    }

    GetAllColaboradoresProtheus(): Observable<ColaboradorProtheusModel[]> {
        return this._httpClient.get<ColaboradorProtheusModel[]>(environment.Colaboradores.GetAll);
    }

    GetOrdemServico(): Observable<any[]> {
        let param = { 'pageIndex': 0, 'pageSize': 10 };
        param['query'] = '';

        const params = new HttpParams()
            .set('pageIndex', 0)
            .set('pageSize', 0)
            .set('query', '');
        return this.dataService.getList(this.extranetApiUrl + 'oss', param)
    }

    getOss(searchText: string): Observable<any[]> {
        let param = { 'pageIndex': 0, 'pageSize': 10 };
        if (!_.isNull(searchText) && !_.isUndefined(searchText))
            param['query'] = searchText;

        return this.dataService.getList(this.extranetApiUrl + 'oss', param)
            .pipe(map((response: any) => {
                //this._fuseProgressBarService.hide();
                if (response)
                    return response || [];
                return [];
            }));
    }

    GetAll(): Observable<Pedido> {
        return this._httpClient.get<any>(`${environment.Pedido.GetAll}`).pipe(
            tap((selectPedido: any) => {
                this._listPedido.next(selectPedido);
            })
        );
    }

    GetDadosCheckList(idPedido: string, codOs: string): Observable<ChecklistModel[]> {
        const params = new HttpParams()
            .set('idPedido', idPedido)
            .set('codOs', codOs);
        return this._httpClient.get<ChecklistModel[]>(environment.Pedido.GetDadosCheckList, { params });
    }
}
