import { NgModule } from '@angular/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { FuseSharedModule } from '@fuse/shared.module';



import { MeusAppsComponent } from './meus-apps.component';


import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';


@NgModule({
    declarations: [
        MeusAppsComponent
    ],
    imports     : [
        MatDividerModule,
        MatListModule,
        MatSlideToggleModule,
        MatButtonModule,
        MatIconModule,
        FuseSharedModule,
    ],
    exports: [
        MeusAppsComponent
    ]
})
export class MeusAppsModule
{
}
