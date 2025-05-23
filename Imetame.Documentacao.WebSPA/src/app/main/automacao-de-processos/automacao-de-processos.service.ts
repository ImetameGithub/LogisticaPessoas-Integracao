import { Injectable, Inject } from '@angular/core';

import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { BehaviorSubject, Observable, ReplaySubject, Subject } from 'rxjs';
import { DataService } from 'app/services/data.service';
import * as _ from 'lodash';
import { API_URL, EXTRANET_API_URL } from 'app/config/tokens';
import { map, tap } from 'rxjs/operators';
import { Finalizado } from './finalizados/finalizados.component';
import { environment } from 'environments/environment';
import { Pedido } from 'app/models/Pedido';
import { HttpClient, HttpEvent, HttpHeaders, HttpParams } from '@angular/common/http';
import { ColaboradorModel, DocumentoStatus } from 'app/models/DTO/ColaboradorModel';
import { DocumentoxColaboradorModel, ImagemProtheus } from 'app/models/DTO/DocumentoxColaboradorModel';
import { PaginatedResponse } from 'app/models/PaginatedResponse';
import { OAuthService } from 'angular-oauth2-oidc';
import { Log } from './logs/logs.component';



@Injectable()
export class AutomacaoDeProcessosService implements Resolve<any> {


    private _finalizadosSource = new BehaviorSubject<any>([]);
    private _documentoxColaborador: ReplaySubject<DocumentoxColaboradorModel[]> = new ReplaySubject<DocumentoxColaboradorModel[]>(1);
    private _logSource = new BehaviorSubject<Log[]>([]);

    finalizados$ = this._finalizadosSource.asObservable();
    logs$ = this._logSource.asObservable();


    private _logsProcessamentoSource = new BehaviorSubject<Log[]>([]);
    logsProcessamento$ = this._logsProcessamentoSource.asObservable();

    private _logsFinalizado = new BehaviorSubject<any[]>([]);
    logsFinalizados$ = this._logsFinalizado.asObservable();

    routeParams: any;
    user: any;

    onProcessamentoChanged: BehaviorSubject<any>;
    onItensChanged: BehaviorSubject<any>;
    onSearchTextChanged: Subject<any>;
    itens: ColaboradorModel[];
    searchText: string;
    processamento: any;
    private intervaloDeLogs: any;
    private intervaloDeFinalizados: any;

    constructor(
        @Inject(EXTRANET_API_URL) private extranetApiUrl: string,
        @Inject(API_URL) private apiUrl: string,
        private dataService: DataService,
        private _httpClient: HttpClient,
        private oauthService: OAuthService,


    ) {
        this._documentoxColaborador.next(null);
        this.onProcessamentoChanged = new BehaviorSubject({});
        this.onItensChanged = new BehaviorSubject([]);
        this.onSearchTextChanged = new Subject();

        this.onSearchTextChanged.subscribe(searchText => {
            this.searchText = searchText.toUpperCase();

            this.onItensChanged.next(this.itens.filter((col) => col.Nome.toUpperCase().includes(this.searchText)));
        });

        this.user = this.oauthService.getIdentityClaims();

    }

    get documentoxColaborador$(): Observable<DocumentoxColaboradorModel[]> {
        return this._documentoxColaborador.asObservable();
    }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {

        this.routeParams = route.params;
        this.searchText = null;

        return new Promise((resolve, reject) => {
            Promise.all([this.GetColaboradoresPorOs()]).then(() => { resolve(null); }, reject);
        });
    }

    // getColaboradorPorOs(texto: string = ""): {
    //     //Guid idProcessamento, CancellationToken cancellationToken, string texto = ""
    //     return this._httpClient.get<ImagemProtheus>(`${environment.Colaboradores.GetImagemProtheus}/${recno}`)
    // }

    limparFinalizados() {
        this._finalizadosSource.next([]);
        if (this.intervaloDeFinalizados) {
            clearInterval(this.intervaloDeFinalizados);
            this.intervaloDeFinalizados = null;
        }
    }

    resetarLogsProcessamento() {
        this._logsProcessamentoSource.next([]);
        if (this.intervaloDeLogs) {
            clearInterval(this.intervaloDeLogs);
            this.intervaloDeLogs = null;
        }
    }

    // GetColaboradoresPorOs(): Promise<any> {
    //     this.onItensChanged.next([]);
    //     //let param = { 'pageIndex': this.pageIndex, 'pageSize': this.pageSize, 'query': this.searchText };

    //     return new Promise((resolve, reject) => {
    //         this.dataService.getList(this.apiUrl + `colaboradores/${this.routeParams.processamento}`, {})
    //             .subscribe((response: any) => {
    //                 this.itens = response;
    //                 this.onItensChanged.next(this.itens);
    //                 resolve(response);
    //             }, reject);
    //     });
    // }

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

    GetOrdemServico(): Observable<any[]> {
        let param = { 'pageIndex': 0, 'pageSize': 10 };
        param['query'] = '';

        const params = new HttpParams()
            .set('pageIndex', 0)
            .set('pageSize', 0)
            .set('query', '');
        return this.dataService.getList(this.extranetApiUrl + 'oss', param)
    }




    cadastrar(item: any): Promise<any> {
        return this.dataService.post(this.apiUrl + 'Cadastros/', item).toPromise();
    }


    GetDocumentosProtheus(matricula: string, docsStatusList: DocumentoStatus[] = null): Observable<any> {
        const docsStatusListJson = encodeURIComponent(JSON.stringify(docsStatusList));
        return this._httpClient.get<any>(
            `${environment.Colaboradores.GetDocumentosProtheus}/${matricula}/${docsStatusListJson}`
        );
    }
    
    // GetDocumentosProtheus(matricula: string, docsStatusList: DocumentoStatus[] = null): Observable<DocumentoxColaboradorModel[]> {
    //     return this._httpClient.get<DocumentoxColaboradorModel[]>(`${environment.Colaboradores.GetDocumentosProtheus}/${matricula}/${docsStatusList}`)
    // }
    
    

    GetImagemProtheus(recno: string): Observable<ImagemProtheus> {
        return this._httpClient.get<ImagemProtheus>(`${environment.Colaboradores.GetImagemProtheus}/${recno}`)
    }


    // EnviarColaboradorDestra(Colaborador: any, IdPedido: string): Observable<any> {
    //     const headers = new HttpHeaders().set('Content-Type', 'application/json');        
    //     return this._httpClient.post<any>(environment.Colaboradores.EnviarColaboradorDestra, Colaborador, { headers });
    // }

    EnviarColaboradorDestra(Colaborador: any, IdPedido: string,ordemServico: string): Observable<any> {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        const body = {
            listColaboradores: Colaborador,
            IdPedido: IdPedido,
            OrdemServico: ordemServico,
            MatriculaUsuario: this.user["numcad"]
        };
        return this._httpClient.post<any>(environment.Colaboradores.EnviarColaboradorDestra, body, { headers });
    }

    // EnviarDocsArrayDestra(Colaborador: any): Observable<any> {
    //     const headers = new HttpHeaders().set('Content-Type', 'application/json');
    //     return this._httpClient.post<any>(environment.Colaboradores.EnviarDocsArrayDestra, Colaborador, { headers });
    // }


    EnviarDocsArrayDestra(Colaborador: any): Observable<any> {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._httpClient.post<any>(environment.Colaboradores.EnviarDocsArrayDestra, Colaborador, { headers });
    }

    EnviarDocumentoParaDestra(Documento: DocumentoxColaboradorModel): Observable<any> {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._httpClient.post<any>(environment.Colaboradores.EnviarDocumentoParaDestra, Documento, { headers });
    }

    GetDocumentosObrigatorios(Documentos: DocumentoxColaboradorModel[]): Observable<any> {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._httpClient.post<DocumentoxColaboradorModel[]>(environment.Colaboradores.GetDocumentosObrigatorios, Documentos);
    }

    getProgress(): Observable<any> {
        return this._httpClient.get(environment.Colaboradores.GetProgress);
    }


    GetColaboradoresPorOs(filtro: string = ''): Promise<ColaboradorModel> {
        const params = new HttpParams()
            .set('texto', filtro);
            return new Promise((resolve, reject) => {
            this.dataService.getList(`${environment.Colaboradores.GetColaboradoresPorOs}/${this.routeParams.processamento}/${this.routeParams.ordemServico}`, { params })
                .subscribe((response: any) => {
                    this.itens = response;
                    this.onItensChanged.next(this.itens);
                    resolve(response);
                }, reject);
        });
    }

    adicionarFinalizados(novosFinalizados: Finalizado[]) {
        const finalizadosAtuais = this._finalizadosSource.getValue();
        const finalizadosAtualizados = [...finalizadosAtuais, ...novosFinalizados];
        this._finalizadosSource.next(finalizadosAtualizados);
    }

    getProcessoAtivo(params: any): Promise<any> {
        return new Promise((resolve, reject) => {
            this.dataService.get(this.apiUrl + `processamento/ativo`, params)
                .subscribe((response: any) => {
                    this.processamento = response;
                    this.onProcessamentoChanged.next(this.processamento);
                    resolve(response);
                }, reject);
        });
    }


    cadastrarProcessamento(item: any): Promise<any> {
        return this.dataService.post<any>(this.apiUrl + 'processamento/', item).toPromise();
    }

    // cadastrarProcessamento(item: any): Observable<any> {
    //     const headers = new HttpHeaders().set('Content-Type', 'application/json');
    //     return this._httpClient.post<any>(environment.Processamento.Criar , item, { headers });
    // }


    getProcesso(id: any): Promise<any> {
        return new Promise((resolve, reject) => {
            this.dataService.get(this.apiUrl + `processamento/${id}`)
                .subscribe((response: any) => {
                    this.processamento = response;
                    this.onProcessamentoChanged.next(this.processamento);
                    resolve(response);
                }, reject);
        });
    }

    getLogsProcessamento(idProcessamento: string, pageIndex: number = 0, pageSize: number = 50): Promise<any> {
        return new Promise((resolve, reject) => {
            const url = `${this.apiUrl}Processamento/${idProcessamento}/logs?pageIndex=${pageIndex}&pageSize=${pageSize}`;
            this.dataService.getList(url, {})
                .subscribe((response: any) => {
                    this._logsProcessamentoSource.next(response);
                    resolve(response);
                }, reject);
        });
    }

    iniciarObservacaoLogsProcessamento(idProcessamento: string, pageIndex: number = 0, pageSize: number = 50) {
        const carregarLogs = () => {
            this.getLogsProcessamento(idProcessamento, pageIndex, pageSize).then((logs) => {

                const logsAtuais = this._logsProcessamentoSource.getValue();
                const todosLogs = [...logsAtuais, ...logs.data];
                this._logsProcessamentoSource.next(todosLogs);

            }).catch((error) => {
                console.error("Erro ao atualizar logs de processamento:", error);
            });
        };

        carregarLogs();

        this.intervaloDeLogs = setInterval(carregarLogs, 5000)
    }

    // iniciarObservacaoFinalizados(idProcessamento: string, pageIndex: number = 0, pageSize: number = 50) {
    //     const carregarFinalizados = () => {
    //         this.buscarEAdicionarResultados(idProcessamento, pageIndex, pageSize).then((finalizados: any) => {
    //             const finalizadosAtuais = this._finalizadosSource.getValue();

    //             const novosFinalizados = finalizados.data.filter((novoFinalizado: any) => {
    //                 return !finalizadosAtuais.some((atual: any) => atual.id === novoFinalizado.id);
    //             });

    //             if (novosFinalizados.length > 0) {
    //                 const todosFinalizados = [...finalizadosAtuais, ...novosFinalizados];
    //                 this._finalizadosSource.next(todosFinalizados);
    //             }

    //         }).catch((error) => {
    //             console.error("Erro ao atualizar:", error);
    //         });
    //     };

    //     carregarFinalizados();
    //     this.intervaloDeFinalizados = setInterval(carregarFinalizados, 5000);
    // }



    buscarEAdicionarResultados(idProcessamento: string, pageIndex: number = 0, pageSize: number = 50): Promise<void> {
        return new Promise((resolve, reject) => {
            this.dataService.getList(`${this.apiUrl}Processamento/${idProcessamento}/resultados?pageIndex=${pageIndex}&pageSize=${pageSize}`, {})
                .subscribe((response: any) => {
                    const finalizadosAtuais = this._finalizadosSource.getValue();

                    const novosFinalizados = response.data.filter((novoFinalizado: any) => {
                        return !finalizadosAtuais.some((atual: any) => atual.id === novoFinalizado.id);
                    });

                    if (novosFinalizados.length > 0) {
                        const todosFinalizados = [...finalizadosAtuais, ...novosFinalizados];
                        this._finalizadosSource.next(todosFinalizados);
                    }
                    resolve();
                }, error => reject(error));
        });
    }


    // getPedidos(): Observable<any> {
    //     return this.dataService.getList(`${this.apiUrl}Pedido?pageIndex=0&pageSize=50`).pipe(
    //         map((response: any) => response.data)
    //     );
    // }
    getPedidos(): Observable<Pedido[]> {
        return this._httpClient.get<Pedido[]>(environment.Pedido.GetAll);
    }

    getCredenciadoras(): Observable<any> {
        return this.dataService.getList(`${this.apiUrl}CredenciadoraDePara?pageIndex=0&pageSize=100`).pipe(
            map((response: any) => response.data)
        );
    }


}
