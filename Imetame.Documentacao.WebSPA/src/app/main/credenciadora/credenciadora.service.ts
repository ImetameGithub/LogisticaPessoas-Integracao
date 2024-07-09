import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { API_URL } from 'app/config/tokens';
import { Credenciadora } from 'app/models/Crendenciadora';
import { PaginatedResponse } from 'app/models/PaginatedResponse';
import { DataService } from 'app/services/data.service';
import { environment } from 'environments/environment';
import { tap } from 'rxjs/operators';
import { ReplaySubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CredenciadoraService implements Resolve<Credenciadora> {

    routeParams: any;

    private _listCredenciadora: ReplaySubject<PaginatedResponse<Credenciadora>> = new ReplaySubject<PaginatedResponse<Credenciadora>>(1);
    private _selectCredenciadora: ReplaySubject<Credenciadora> = new ReplaySubject<Credenciadora>(1);

    constructor(private _httpClient: HttpClient, private dataService: DataService, @Inject(API_URL) private apiUrl: string) {
        this._listCredenciadora.next(null)
        this._selectCredenciadora.next(null)
    }

    //#region FUNÇÕES DE ECAPSULAMENTOS
    get listCredenciadora$(): Observable<PaginatedResponse<Credenciadora>> {
        return this._listCredenciadora.asObservable();
    }
    get _selectCredenciadora$(): Observable<Credenciadora> {
        return this._selectCredenciadora.asObservable();
    }
    //#endregion

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<any> | Promise<any> | any {
        this.routeParams = route.params;

        if(this.routeParams.id == 'novo'){
            this._selectCredenciadora.next(null)
        }
        else{
            // this.Get(this.routeParams)
            return this._httpClient.get<Credenciadora>(`${environment.Credenciadora.GetItem}/${this.routeParams.id}`).pipe(
                tap((selectCredenciadora: Credenciadora) => {
                    this._selectCredenciadora.next(selectCredenciadora);
                })
            );
        }
      
    
    }

    //#region FUNÇÕES CRUD MATHEUS MONFREIDES - FARTEC SISTEMAS
    GetAllPaginated(page: number = 1, pageSize: number = 10, filtro: string = ''): Observable<PaginatedResponse<Credenciadora>> {
        const params = new HttpParams()
            .set('page', page.toString())
            .set('pageSize', pageSize.toString())
            .set('filtro', filtro);
        return this._httpClient.get<PaginatedResponse<Credenciadora>>(environment.Credenciadora.GetAllPaginated, { params }).pipe(
            tap((listFeriados: PaginatedResponse<Credenciadora>) => {
                this._listCredenciadora.next(listFeriados);
            })
        );
    }
    Get(id: string): Observable<Credenciadora> {
        return this._httpClient.get<any>(`${environment.Credenciadora.GetItem}/${id}`).pipe(
            tap((selectCredenciadora: any) => {
                this._selectCredenciadora.next(selectCredenciadora);
            })
        );
    }
    GetAll(): Observable<Credenciadora[]> {
        return this._httpClient.get<Credenciadora[]>(environment.Credenciadora.GetAll);
    }
    Add(model: Credenciadora): Observable<Credenciadora> {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._httpClient.post<Credenciadora>(environment.Credenciadora.Add, model, { headers });
    }
    Update(model: Credenciadora): Observable<Credenciadora> {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._httpClient.put<Credenciadora>(environment.Credenciadora.Update, model, { headers });
    }
    Delete(id: string): Observable<Credenciadora> {
        return this._httpClient.delete<Credenciadora>(`${environment.Credenciadora.Delete}/${id}`)
    }
    //#endregion
}
