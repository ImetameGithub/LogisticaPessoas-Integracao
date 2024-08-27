import { Injectable, Inject } from "@angular/core";
import { BehaviorSubject, Observable, ReplaySubject, Subject } from "rxjs";
import * as _ from "lodash";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { environment } from "environments/environment";
import { PaginatedResponse } from "app/models/PaginatedResponse";
import { Pedido } from "app/models/Pedido";
import { DataService } from "app/services/data.service";
import { API_URL } from "app/config/tokens";
import { map, tap } from "rxjs/operators";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Credenciadora } from "app/models/Crendenciadora";

@Injectable()
export class PedidoService implements Resolve<Pedido> {
    routeParams: any;

    private _listPedido: ReplaySubject<PaginatedResponse<Pedido>> = new ReplaySubject<PaginatedResponse<Pedido>>(1);
    private _selectPedido: ReplaySubject<Pedido> = new ReplaySubject<Pedido>(1);

    constructor(private _httpClient: HttpClient, private dataService: DataService, @Inject(API_URL) private apiUrl: string) {
        this._listPedido.next(null)
        this._selectPedido.next(null)
    }

    //#region FUNÇÕES DE ECAPSULAMENTOS
    get listPedido$(): Observable<PaginatedResponse<Pedido>> {
        return this._listPedido.asObservable();
    }
    get _selectPedido$(): Observable<Pedido> {
        return this._selectPedido.asObservable();
    }
    //#endregion

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<any> | Promise<any> | any {
        this.routeParams = route.params;

        if(this.routeParams.id == 'novo'){
            this._selectPedido.next(null)
        }
        else{
            // this.Get(this.routeParams)
            return this._httpClient.get<Pedido>(`${environment.Pedido.GetItem}/${this.routeParams.id}`).pipe(
                tap((selectPedido: Pedido) => {
                    this._selectPedido.next(selectPedido);
                })
            );
        }
      
    
    }

    //#region FUNÇÕES CRUD MATHEUS MONFREIDES - FARTEC SISTEMAS
    GetAllPaginated(page: number = 1, pageSize: number = 10, filtro: string = ''): Observable<PaginatedResponse<Pedido>> {
        const params = new HttpParams()
            .set('page', page.toString())
            .set('pageSize', pageSize.toString())
            .set('texto', filtro);
        return this._httpClient.get<PaginatedResponse<Pedido>>(environment.Pedido.GetAllPaginated, { params }).pipe(
            tap((listFeriados: PaginatedResponse<Pedido>) => {
                this._listPedido.next(listFeriados);
            })
        );
    }
    Get(id: string): Observable<Pedido> {
        return this._httpClient.get<any>(`${environment.Pedido.GetItem}/${id}`).pipe(
            tap((selectPedido: any) => {
                this._selectPedido.next(selectPedido);
            })
        );
    }
    GetAll(): Observable<Pedido[]> {
        return this._httpClient.get<Pedido[]>(environment.Pedido.GetAll);
    }
    Add(model: Pedido): Observable<Pedido> {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        model.Credenciadora = null;
        return this._httpClient.post<Pedido>(environment.Pedido.Add, model, { headers });
    }
    Update(model: Pedido): Observable<Pedido> {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._httpClient.put<Pedido>(environment.Pedido.Update, model, { headers });
    }
    Delete(id: string): Observable<Pedido> {
        return this._httpClient.delete<Pedido>(`${environment.Pedido.Delete}/${id}`)
    }
    //#endregion

    GetAllCredenciadoras(): Observable<Credenciadora[]> {
        return this._httpClient.get<Credenciadora[]>(environment.Credenciadora.GetAll);
    }

    getCredenciadoras(): Observable<any> {
        return this.dataService.getList(`${this.apiUrl}CredenciadoraDePara?pageIndex=0&pageSize=100`).pipe(
            map((response: any) => response.Data)
        );
    }

}
