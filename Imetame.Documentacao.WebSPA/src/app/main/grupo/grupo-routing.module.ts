import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GrupoListComponent } from './grupo-list/grupo-list.component';

import { GrupoFormComponent } from './grupo-form/grupo-form.component';
import { GrupoService } from './grupo.service';
import { AuthGuard } from 'app/guard/AuthGuard';

const routes: Routes = [
    { path: '', component: GrupoListComponent, resolve: { data: GrupoService } },
    {
        path: ':id',
        component: GrupoFormComponent,
        resolve: { data: GrupoService }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    
})
export class GrupoRoutingModule { }
