import { Injectable, Inject } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot, } from "@angular/router";
import { BehaviorSubject, Observable, ReplaySubject, Subject } from "rxjs";
import { DataService } from "app/services/data.service";
import * as _ from "lodash";
import { API_URL } from "app/config/tokens";
import { map, tap } from 'rxjs/operators';
import { Pedido } from "app/models/Pedido";
import { environment } from "environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Colaborador } from "app/models/Colaborador";
import { ColaboradorModel } from "app/models/DTO/ColaboradorModel";
import { ChecklistModel } from "app/models/DTO/RelatorioModel";

@Injectable()
export class RelatorioService implements Resolve<any> {
    routeParams: any;
    private _listPedido: ReplaySubject<Pedido[]> = new ReplaySubject<Pedido[]>(1);

    constructor(
        private _httpClient: HttpClient,
        @Inject(API_URL) private apiUrl: string,
        private dataService: DataService
    ) {

    }

    //#region FUNÇÕES DE ECAPSULAMENTOS
    get listPedido$(): Observable<Pedido[]> {
        return this._listPedido.asObservable();
    }
    // get listOs$(): Observable<[]> {
    //     return this._listPedido.asObservable();
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


    GetAll(): Observable<Pedido> {
        return this._httpClient.get<any>(`${environment.Pedido.GetAll}`).pipe(
            tap((selectPedido: any) => {
                this._listPedido.next(selectPedido);
            })
        );
    }

    GetDadosCheckList(idPedido: string): Observable<ChecklistModel[]> {
        const params = new HttpParams()
            .set('idPedido', idPedido);
        return this._httpClient.get<ChecklistModel[]>(environment.Pedido.GetDadosCheckList, { params });
    }
}
