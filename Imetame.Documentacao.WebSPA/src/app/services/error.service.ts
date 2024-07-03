import { Injectable, Injector, OnDestroy } from '@angular/core';
import { HttpClient, HttpResponse, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';
import { throwError } from 'rxjs';

import { filter, take, delay, first, tap, map, catchError, takeUntil } from 'rxjs/operators';
import { OAuthService } from 'angular-oauth2-oidc';

import * as _ from 'lodash';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Injectable()
export class ErrorService {

    error: any

    constructor(
        private router: Router,
        private location: Location,
        private oauthService: OAuthService, ) {

    }


    resolve(error: any): void {
        this.error = error;

        switch (error.status) {
            case 401:// Unauthorized   
                this.oauthService.initLoginFlow(this.location.path());
                break;
            case 403:// Forbidden
                this.router.navigate(['/negado']);
                break;
            case 404:// Not Found
                this.router.navigate(['/nao-encontrado']);
                break;
            default:
                this.router.navigate(['/desafio-interno']);
                break;
        }
    }


}