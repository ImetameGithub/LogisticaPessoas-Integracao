import { Component, OnInit, Inject, ViewEncapsulation, OnDestroy } from '@angular/core';
import { IDENTITY_API_URL } from 'app/config/tokens';
import { DataService } from 'app/services/data.service';
import { Observable, Subject } from 'rxjs';
import { fuseAnimations } from '@fuse/animations';
import { OAuthService } from 'angular-oauth2-oidc';
import { takeUntil, filter } from 'rxjs/operators';

@Component({
  selector: 'meus-apps',
  templateUrl: './meus-apps.component.html',
    styleUrls: ['./meus-apps.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class MeusAppsComponent implements OnInit, OnDestroy {

    meusApp: Observable<any>;
    private _unsubscribeAll: Subject<any>;
    

    constructor(
        @Inject(IDENTITY_API_URL) private apiUrl: string,
        private dataService: DataService,
        private oauthService: OAuthService
    )
    {
        this._unsubscribeAll = new Subject();
        this.meusApp = this.dataService.getList(this.apiUrl + 'userinfo/aplicativos');
    }

    ngOnInit() {

        this.oauthService.events
            .pipe(takeUntil(this._unsubscribeAll),filter(e => e.type === 'token_received'))
            .subscribe(_ => {
                this.meusApp = this.dataService.getList(this.apiUrl + 'userinfo/aplicativos');
            });
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

}
