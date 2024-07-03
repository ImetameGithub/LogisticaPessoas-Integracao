import { Injectable, Injector } from '@angular/core';
import { HttpClient, HttpResponse, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';
import { throwError } from 'rxjs';

import { filter, take, delay, first, tap, map, catchError } from 'rxjs/operators';




@Injectable()
export class DataService {

    // Define the internal Subject we'll use to push the command count
   // public pendingCommandsSubject = new Subject<number>();
    //public pendingCommandCount = 0;

    // Provide the *public* Observable that clients can subscribe to
    //public pendingCommands$: Observable<number>;
   

    constructor(
        public http: HttpClient
    ) {
   
      //  this.pendingCommands$ = this.pendingCommandsSubject.asObservable();
    }

    public getImage(url: string): Observable<any> {
        return Observable.create((observer: any) => {
            const req = new XMLHttpRequest();
            req.open('get', url);
            req.onreadystatechange = function () {
                if (req.readyState === 4 && req.status === 200) {
                    observer.next(req.response);
                    observer.complete();
                }
            };

            // req.setRequestHeader('Authorization', `Bearer ${this.inj.get(AccountService).accessToken}`);
            req.send();
        });
    }
    public getFile(url: string, filename: string): Observable<any> {
        return this.http.get(url, { responseType: 'blob' })
            .pipe(map(res => {
                console.log('start download:', res);
                return {
                    filename: filename,
                    data: res//.blob()
                };
            }));

    }

    public downloadFile(url: string, params?: any, empresa?: string, filial?: string): Observable<any> {
        return this.http.get(url, { params: this.buildUrlSearchParams(params), responseType: 'blob', headers: this.buildHeader(empresa, filial)  })
            .pipe(map(resp => {                
                return resp;
            }));

    }

    public uploadFile<T>(url: string, files: any): Observable<any> {
        const formData = new FormData();
        for (let file of files)
            formData.append(file.name, file);

        let response = this.http.post<T>(url, formData)
            .pipe(map(this.extractData), catchError(this.serviceError));
        return response;


    }

    public get<T>(url: string, params?: any, empresa?: string, filial?: string): Observable<T> {
        // return this.http.get<T>(url, { params: this.buildUrlSearchParams(params) });

        let response = this.http.get<T>(url, { params: this.buildUrlSearchParams(params), headers: this.buildHeader(empresa, filial)})
            .pipe(map(this.extractData), catchError(this.serviceError));

        return response;
    }

    public getList<T>(url: string, params?: any, empresa?:string, filial?:string): Observable<T> {
        // return this.http.get<T>(url, { params: this.buildUrlSearchParams(params) });

        let response = this.http.get<T>(url, { params: this.buildUrlSearchParams(params), headers: this.buildHeader(empresa, filial) })
            .pipe(catchError(this.serviceError));

        return response;
    }

    public getFull<T>(url: string): Observable<HttpResponse<T>> {
        return this.http.get<T>(url, { observe: 'response' });
    }

    public post<T>(url: string, data?: any,  empresa?: string, filial?: string): Observable<T> {
        let response = this.http.post<T>(url, data, {  headers: this.buildHeader(empresa, filial)})
            .pipe(map(this.extractData), catchError(this.serviceError));
        return response;
    }

    public put<T>(url: string, data?: any, empresa?: string, filial?: string): Observable<T> {
        let response = this.http.put<T>(url, data, {  headers: this.buildHeader(empresa, filial) })
            .pipe(map(this.extractData), catchError(this.serviceError));
        return response;
    }



    public delete<T>(url: string, empresa?: string, filial?: string): Observable<T> {
        let response = this.http.delete<T>(url, {  headers: this.buildHeader(empresa, filial) })
            .pipe(map(this.extractData), catchError(this.serviceError));
        return response;
    }

    private buildUrlSearchParams(params: any): HttpParams {
        let searchParams = new HttpParams();
        for (const key in params) {
            if (params.hasOwnProperty(key)) {
                searchParams = searchParams.append(key, params[key]);
            }
        }
        return searchParams;
    }

    private buildHeader(empresa?: string, filial?: string): HttpHeaders {
        let header = new HttpHeaders();
        if (empresa) {
            header = header.append('x-empresa', empresa);

            if (filial) {
                header = header.append('x-filial', filial);
            }
        }
        return header;
    }

    private extractData(response: any) {
        if (response && response.data) 
            return response.data || {};
        return response || {};
    }

    private serviceError(error: Response | any) {
        
        if (error && error.error )
            return throwError(error.error);
        return throwError(error);

    }



}
