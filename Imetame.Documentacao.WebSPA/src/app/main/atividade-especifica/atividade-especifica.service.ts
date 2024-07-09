import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { API_URL } from 'app/config/tokens';
import { AtividadeEspecifica } from 'app/models/AtividadeEspecifica';
import { PaginatedResponse } from 'app/models/PaginatedResponse';
import { DataService } from 'app/services/data.service';
import { environment } from 'environments/environment';
import { tap } from 'rxjs/operators';
import { ReplaySubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AtividadeEspecificaService implements Resolve<AtividadeEspecifica>{

  routeParams: any;

  private _listAtividadeEspecifica: ReplaySubject<PaginatedResponse<AtividadeEspecifica>> = new ReplaySubject<PaginatedResponse<AtividadeEspecifica>>(1);
  private _selectAtividadeEspecifica: ReplaySubject<AtividadeEspecifica> = new ReplaySubject<AtividadeEspecifica>(1);

  constructor(private _httpClient: HttpClient, private dataService: DataService, @Inject(API_URL) private apiUrl: string) {
      this._listAtividadeEspecifica.next(null)
      this._selectAtividadeEspecifica.next(null)
  }

  //#region FUNÇÕES DE ECAPSULAMENTOS
  get listAtividadeEspecifica$(): Observable<PaginatedResponse<AtividadeEspecifica>> {
      return this._listAtividadeEspecifica.asObservable();
  }
  get _selectAtividadeEspecifica$(): Observable<AtividadeEspecifica> {
      return this._selectAtividadeEspecifica.asObservable();
  }
  //#endregion

  resolve(
      route: ActivatedRouteSnapshot,
      state: RouterStateSnapshot
  ): Observable<any> | Promise<any> | any {
      this.routeParams = route.params;

      if(this.routeParams.id == 'novo'){
          this._selectAtividadeEspecifica.next(null)
      }
      else{
          // this.Get(this.routeParams)
          return this._httpClient.get<AtividadeEspecifica>(`${environment.AtividadeEspecifica.GetItem}/${this.routeParams.id}`).pipe(
              tap((selectAtividadeEspecifica: AtividadeEspecifica) => {
                  this._selectAtividadeEspecifica.next(selectAtividadeEspecifica);
              })
          );
      }
    
  
  }

  //#region FUNÇÕES CRUD MATHEUS MONFREIDES - FARTEC SISTEMAS
  GetAllPaginated(page: number = 1, pageSize: number = 10, filtro: string = ''): Observable<PaginatedResponse<AtividadeEspecifica>> {
      const params = new HttpParams()
          .set('page', page.toString())
          .set('pageSize', pageSize.toString())
          .set('filtro', filtro);
      return this._httpClient.get<PaginatedResponse<AtividadeEspecifica>>(environment.AtividadeEspecifica.GetAllPaginated, { params }).pipe(
          tap((listFeriados: PaginatedResponse<AtividadeEspecifica>) => {
              this._listAtividadeEspecifica.next(listFeriados);
          })
      );
  }
  Get(id: string): Observable<AtividadeEspecifica> {
      return this._httpClient.get<any>(`${environment.AtividadeEspecifica.GetItem}/${id}`).pipe(
          tap((selectAtividadeEspecifica: any) => {
              this._selectAtividadeEspecifica.next(selectAtividadeEspecifica);
          })
      );
  }
  GetAll(): Observable<AtividadeEspecifica[]> {
      return this._httpClient.get<AtividadeEspecifica[]>(environment.AtividadeEspecifica.GetAll);
  }
  Add(model: AtividadeEspecifica): Observable<AtividadeEspecifica> {
      const headers = new HttpHeaders().set('Content-Type', 'application/json');
      return this._httpClient.post<AtividadeEspecifica>(environment.AtividadeEspecifica.Add, model, { headers });
  }
  Update(model: AtividadeEspecifica): Observable<AtividadeEspecifica> {
      const headers = new HttpHeaders().set('Content-Type', 'application/json');
      return this._httpClient.put<AtividadeEspecifica>(environment.AtividadeEspecifica.Update, model, { headers });
  }
  Delete(id: string): Observable<AtividadeEspecifica> {
      return this._httpClient.delete<AtividadeEspecifica>(`${environment.AtividadeEspecifica.Delete}/${id}`)
  }
  //#endregion
}
