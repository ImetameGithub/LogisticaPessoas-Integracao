import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

import { FuseSharedModule } from '@fuse/shared.module';
import { Error401Component } from './error-401.component';



const routes = [
    {
        path     : '',
        component: Error401Component
    }
];

@NgModule({
    declarations: [
        Error401Component
    ],
    imports     : [
        RouterModule.forChild(routes),

        MatIconModule,

        FuseSharedModule
    ]
})
export class Error401Module
{
}
