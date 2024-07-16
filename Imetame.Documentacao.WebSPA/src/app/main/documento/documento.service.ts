import { Injectable, Inject } from "@angular/core";
import { BehaviorSubject, Observable, ReplaySubject, Subject } from "rxjs";
import * as _ from "lodash";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { environment } from "environments/environment";
import { PaginatedResponse } from "app/models/PaginatedResponse";
import { Documento } from "app/models/Documento";
import { DataService } from "app/services/data.service";
import { API_URL } from "app/config/tokens";
import { map, tap } from "rxjs/operators";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";

@Injectable()
export class DocumentoService implements Resolve<Documento> {
    routeParams: any;

    private _listDocumentosDestra: ReplaySubject<string[]> = new ReplaySubject<string[]>(1);
    private _listDocumento: ReplaySubject<PaginatedResponse<Documento>> = new ReplaySubject<PaginatedResponse<Documento>>(1);
    private _selectDocumento: ReplaySubject<Documento> = new ReplaySubject<Documento>(1);

    constructor(private _httpClient: HttpClient, private dataService: DataService, @Inject(API_URL) private apiUrl: string) {
        this._listDocumentosDestra.next(null)
        this._listDocumento.next(null)
        this._selectDocumento.next(null)
    }

    //#region FUNÇÕES DE ECAPSULAMENTOS
    get listDocumentosDestra$(): Observable<string[]> {
        return this._listDocumentosDestra.asObservable();
    }
    get listDocumento$(): Observable<PaginatedResponse<Documento>> {
        return this._listDocumento.asObservable();
    }
    get _selectDocumento$(): Observable<Documento> {
        return this._selectDocumento.asObservable();
    }
    //#endregion

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<any> | Promise<any> | any {
        this.routeParams = route.params;

        if(this.routeParams.id == 'novo'){
            this._selectDocumento.next(null)
        }
        else{
            // this.Get(this.routeParams)
            return this._httpClient.get<Documento>(`${environment.Documento.GetItem}/${this.routeParams.id}`).pipe(
                tap((selectDocumento: Documento) => {
                    this._selectDocumento.next(selectDocumento);
                })
            );
        }
      
    
    }

    //#region FUNÇÕES CRUD MATHEUS MONFREIDES - FARTEC SISTEMAS
    GetAllPaginated(page: number = 1, pageSize: number = 10, filtro: string = ''): Observable<PaginatedResponse<Documento>> {
        const params = new HttpParams()
            .set('page', page.toString())
            .set('pageSize', pageSize.toString())
            .set('filtro', filtro);
        return this._httpClient.get<PaginatedResponse<Documento>>(environment.Documento.GetAllPaginated, { params }).pipe(
            tap((listFeriados: PaginatedResponse<Documento>) => {
                this._listDocumento.next(listFeriados);
            })
        );
    }
    Get(id: string): Observable<Documento> {
        return this._httpClient.get<any>(`${environment.Documento.GetItem}/${id}`).pipe(
            tap((selectDocumento: any) => {
                this._selectDocumento.next(selectDocumento);
            })
        );
    }
    GetAll(): Observable<Documento[]> {
        return this._httpClient.get<Documento[]>(environment.Documento.GetAll);
    }
    Add(model: Documento): Observable<Documento> {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._httpClient.post<Documento>(environment.Documento.Add, model, { headers });
    }
    Update(model: Documento): Observable<Documento> {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._httpClient.put<Documento>(environment.Documento.Update, model, { headers });
    }
    Delete(id: string): Observable<Documento> {
        return this._httpClient.delete<Documento>(`${environment.Documento.Delete}/${id}`)
    }
    //#endregion

    getDocumentosDestra(): Observable<string[]> {
        return this._httpClient.get<string[]>(environment.Destra.GetDocumentosDestra).pipe(
            tap((listFeriados: string[]) => {
                this._listDocumentosDestra.next(listFeriados);
            })
        );
    }

}
