import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { API_URL } from 'app/config/tokens';
import { Colaborador } from 'app/models/Colaborador';
import { ColaboradorModel } from 'app/models/DTO/ColaboradorModel';
import { PaginatedResponse } from 'app/models/PaginatedResponse';
import { DataService } from 'app/services/data.service';
import { environment } from 'environments/environment';
import { tap } from 'rxjs/operators';
import { ReplaySubject, Observable } from 'rxjs';
import { AtividadeEspecifica } from 'app/models/AtividadeEspecifica';

@Injectable({
  providedIn: 'root'
})
export class ColaboradorService {
  routeParams: any;

  private _listColaborador: ReplaySubject<PaginatedResponse<ColaboradorModel>> = new ReplaySubject<PaginatedResponse<ColaboradorModel>>(1);
  private _selectColaborador: ReplaySubject<Colaborador> = new ReplaySubject<Colaborador>(1);
  private _listAtividades: ReplaySubject<AtividadeEspecifica[]> = new ReplaySubject<AtividadeEspecifica[]>(1);
  private _listAllColaboradores: ReplaySubject<ColaboradorModel[]> = new ReplaySubject<ColaboradorModel[]>(1);

  constructor(private _httpClient: HttpClient, private dataService: DataService, @Inject(API_URL) private apiUrl: string) {
    this._listColaborador.next(null)
    this._selectColaborador.next(null)
  }

  //#region FUNÇÕES DE ECAPSULAMENTOS
  get listColaborador$(): Observable<PaginatedResponse<ColaboradorModel>> {
    return this._listColaborador.asObservable();
  }
  get _selectColaborador$(): Observable<Colaborador> {
    return this._selectColaborador.asObservable();
  }
  get _listAtividades$(): Observable<AtividadeEspecifica[]> {
    return this._listAtividades.asObservable();
  }
  get _listAllColaboradores$(): Observable<ColaboradorModel[]> {
    return this._listAllColaboradores.asObservable();
  }
  //#endregion

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> | Promise<any> | any {
    this.routeParams = route.params;

    if (this.routeParams.id == 'novo') {
      this._selectColaborador.next(null)
    }
    else {
      // this.Get(this.routeParams)
      return this._httpClient.get<Colaborador>(`${environment.Colaboradores.GetItem}/${this.routeParams.id}`).pipe(
        tap((selectColaborador: Colaborador) => {
          this._selectColaborador.next(selectColaborador);
        })
      );
    }


  }

  //#region FUNÇÕES CRUD MATHEUS MONFREIDES - FARTEC SISTEMAS
  GetAllPaginated(page: number = 1, pageSize: number = 10, filtro: string = ''): Observable<PaginatedResponse<ColaboradorModel>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('filtro', filtro);
    return this._httpClient.get<PaginatedResponse<ColaboradorModel>>(environment.Colaboradores.GetColaboradores, { params }).pipe(
      tap((listFeriados: PaginatedResponse<ColaboradorModel>) => {
        this._listColaborador.next(listFeriados);
      })
    );
  }
  GetAllAtividades(): Observable<AtividadeEspecifica[]> {
    return this._httpClient.get<AtividadeEspecifica[]>(environment.AtividadeEspecifica.GetAll).pipe(
      tap((listAtividades: AtividadeEspecifica[]) => {
        this._listAtividades.next(listAtividades);
      })
    );
  }
  GetAll(): Observable<ColaboradorModel[]> {
    return this._httpClient.get<ColaboradorModel[]>(environment.Colaboradores.GetAll).pipe(
      tap((listAllColaboradores: ColaboradorModel[]) => {
        this._listAllColaboradores.next(listAllColaboradores);
      })
    );
  }
  Get(id: string): Observable<Colaborador> {
    return this._httpClient.get<any>(`${environment.Colaboradores.GetItem}/${id}`).pipe(
      tap((selectColaborador: any) => {
        this._selectColaborador.next(selectColaborador);
      })
    );
  }
  Add(model: Colaborador): Observable<Colaborador> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this._httpClient.post<Colaborador>(environment.Colaboradores.Add, model, { headers });
  }
  Update(model: Colaborador): Observable<Colaborador> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this._httpClient.put<Colaborador>(environment.Colaboradores.Update, model, { headers });
  }
  Delete(id: string): Observable<Colaborador> {
    return this._httpClient.delete<Colaborador>(`${environment.Colaboradores.Delete}/${id}`)
  }
  //#endregion

}
