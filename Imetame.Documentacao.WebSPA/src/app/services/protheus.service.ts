import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProtheusService {

  constructor(private _httpClient: HttpClient) { }

  //#region FUNÇÃO PARA BUSCAR ORDENS DE SERVIÇO DO PROTHEUS - MATHEUS MONFREIDES 05/12/2023
  getOs() {
    return this._httpClient.get(`${environment.Protheus.GetOs}`);
  }
  //#endregion
}
