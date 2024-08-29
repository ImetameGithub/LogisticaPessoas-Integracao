import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { RelatorioService } from "./relatorio.service";
import { ChecklistComponent } from "./checklist/checklist/checklist.component";

const routes: Routes = [
    {
        path: "",
        component: ChecklistComponent,
        resolve: { data: RelatorioService },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RelatorioRoutingModule {}
