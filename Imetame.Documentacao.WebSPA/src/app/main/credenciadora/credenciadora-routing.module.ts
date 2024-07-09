import { Credenciadora } from './../../models/Crendenciadora';
import { NgModule, inject } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CredenciadoraListComponent } from "./credenciadora-list/credenciadora-list.component";

import { CredenciadoraFormComponent } from "./credenciadora-form/credenciadora-form.component";
import { CredenciadoraService } from "./credenciadora.service";

const routes: Routes = [
    // {
    //     path: "",
    //     component: CredenciadoraListComponent,
    //     resolve: { data: CredenciadoraService },
    // },
    {
        path: '', component: CredenciadoraListComponent,
        resolve: {
          data: ()=> inject(CredenciadoraService).GetAllPaginated(),
      }
    },
    {
        path: ":id",
        component: CredenciadoraFormComponent,
        resolve: { data: CredenciadoraService },
    },
    {
        path: "novo",
        component: CredenciadoraFormComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CredenciadoraRoutingModule {}
