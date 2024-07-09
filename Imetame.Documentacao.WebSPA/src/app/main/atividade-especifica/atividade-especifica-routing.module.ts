import { NgModule, inject } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AtividadeEspecificaListComponent } from "./atividade-especifica-list/atividade-especifica-list.component";

import { AtividadeEspecificaFormComponent } from "./atividade-especifica-form/atividade-especifica-form.component";
import { AtividadeEspecificaService } from "./atividade-especifica.service";

const routes: Routes = [
    // {
    //     path: "",
    //     component: AtividadeEspecificaListComponent,
    //     resolve: { data: AtividadeEspecificaService },
    // },
    {
        path: '', component: AtividadeEspecificaListComponent,
        resolve: {
          data: ()=> inject(AtividadeEspecificaService).GetAllPaginated(),
      }
    },
    {
        path: ":id",
        component: AtividadeEspecificaFormComponent,
        resolve: { data: AtividadeEspecificaService },
    },
    {
        path: "novo",
        component: AtividadeEspecificaFormComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AtividadeEspecificaRoutingModule {}
