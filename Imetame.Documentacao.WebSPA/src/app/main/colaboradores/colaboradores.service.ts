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

  constructor(private _httpClient: HttpClient,) {

  }
  //#region CRUD
  GetAll(): Observable<ColaboradorModel[]> {
    return this._httpClient.get<ColaboradorModel[]>(environment.Colaboradores.GetAll);
  }
  GetAllPaginated(page: number = 1, pageSize: number = 10, filtro: string = ''): Observable<PaginatedResponse<ColaboradorModel>> {
    const params = new HttpParams()     
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('filtro', filtro);
    return this._httpClient.get<PaginatedResponse<ColaboradorModel>>(environment.Colaboradores.GetAllPaginated, { params });
  }
  GetColaboradores(page: number = 1, pageSize: number = 10, filtro: string = ''): Observable<PaginatedResponse<ColaboradorModel>> {
    const params = new HttpParams()     
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('filtro', filtro);
    return this._httpClient.get<PaginatedResponse<ColaboradorModel>>(environment.Colaboradores.GetColaboradores, { params });
  }

  Add(Colaborador: Colaborador): Observable<Colaborador> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this._httpClient.post<Colaborador>(environment.Colaboradores.Add, Colaborador, { headers });
  }
  Update(Colaborador: Colaborador): Observable<Colaborador> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this._httpClient.put<Colaborador>(environment.Colaboradores.Update, Colaborador, { headers });
  }
  Delete(id: string): Observable<Colaborador> {
    return this._httpClient.delete<Colaborador>(`${environment.Colaboradores.Delete}/${id}`)
}
  //#endregion

}
