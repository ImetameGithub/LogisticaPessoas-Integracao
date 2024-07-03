import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ProcessamentoListComponent } from "./processamento-list/processamento-list.component";

import { ProcessamentoService } from "./processamento.service";

const routes: Routes = [
    {
        path: "",
        component: ProcessamentoListComponent,
        resolve: { data: ProcessamentoService },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProcessamentoRoutingModule {}
