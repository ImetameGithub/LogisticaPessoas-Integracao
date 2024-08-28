import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { API_URL, EXTRANET_API_URL } from 'app/config/tokens';
import { Colaborador } from 'app/models/Colaborador';
import { ColaboradorModel, ColaboradorProtheusModel } from 'app/models/DTO/ColaboradorModel';
import { PaginatedResponse } from 'app/models/PaginatedResponse';
import { DataService } from 'app/services/data.service';
import { environment } from 'environments/environment';
import { map, tap } from 'rxjs/operators';
import { ReplaySubject, Observable } from 'rxjs';
import { AtividadeEspecifica } from 'app/models/AtividadeEspecifica';
import { ColaboradorxAtividadeModel } from 'app/models/DTO/ColaboradorxAtividadeModel';
import * as _ from 'lodash';

@Injectable({
  providedIn: 'root'
})
export class ColaboradorService {

  constructor(private _httpClient: HttpClient,   @Inject(EXTRANET_API_URL) private extranetApiUrl: string,private dataService: DataService) {

  }
  //#region CRUD
  GetAll(): Observable<ColaboradorProtheusModel[]> {
    return this._httpClient.get<ColaboradorProtheusModel[]>(environment.Colaboradores.GetAll);
  }
  GetAllPaginated(page: number = 1, pageSize: number = 10, filtro: string = ''): Observable<PaginatedResponse<Colaborador>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('texto', filtro);
    return this._httpClient.get<PaginatedResponse<Colaborador>>(environment.Colaboradores.GetAllPaginated, { params });
  }
  GetColaboradores(page: number = 1, pageSize: number = 10, filtro: string = ''): Observable<PaginatedResponse<ColaboradorProtheusModel>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('texto', filtro);
    return this._httpClient.get<PaginatedResponse<ColaboradorProtheusModel>>(environment.Colaboradores.GetColaboradores, { params });
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

  Add(Colaborador: Colaborador): Observable<Colaborador> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this._httpClient.post<Colaborador>(environment.Colaboradores.Add, Colaborador, { headers });
  }
  RelacionarColaboradorxAtividade(model: ColaboradorxAtividadeModel): Observable<ColaboradorxAtividadeModel> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this._httpClient.post<ColaboradorxAtividadeModel>(environment.Colaboradores.RelacionarColaboradorxAtividade, model, { headers });
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
