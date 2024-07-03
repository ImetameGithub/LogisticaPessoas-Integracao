import { Injectable, Injector, OnDestroy } from '@angular/core';
import { HttpClient, HttpResponse, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';
import { throwError } from 'rxjs';

import { filter, take, delay, first, tap, map, catchError, takeUntil } from 'rxjs/operators';
import { OAuthService } from 'angular-oauth2-oidc';

import * as _ from 'lodash';


@Injectable()
export class PermissaoService implements OnDestroy{

    private _unsubscribeAll: Subject<any>;
    user: any;
   

    constructor(
        private oauthService: OAuthService
    ) {
        this._unsubscribeAll = new Subject();

        this.user = this.oauthService.getIdentityClaims();

        this.oauthService.events
            .pipe(takeUntil(this._unsubscribeAll), filter(e => e.type === 'user_profile_loaded'))
            .subscribe(e => {

                this.user = this.oauthService.getIdentityClaims();
                
            });


        this.oauthService.events
            .pipe(filter(e => e.type === 'token_received'))
            .subscribe(_ => {
                this.user = this.oauthService.getIdentityClaims();
            });
    }

    /**
    * On destroy
    */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }


    hasClaim(claim: any): boolean {
        return this.hasPermission(claim.claimType, claim.claimValue)
    }


    hasPermission(claimType: string, claimValue:string): boolean {
        
            let user = this.oauthService.getIdentityClaims();
        if (user[claimType]) {
            if (_.isArray(user[claimType])) {
                return _.includes(user[claimType], claimValue);

                }
            return user[claimType] == claimValue;

            }
            return false
        
    }


}
