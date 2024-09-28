import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { RelatorioService } from "./relatorio.service";
import { ChecklistComponent } from "./checklist/checklist/checklist.component";
import { ChecklistListComponent } from "./checklist/checklist/checklist-list/checklist-list.component";
import { checklistListResolver } from "./relatorio.resolver";

const routes: Routes = [
    {
        path: "",
        component: ChecklistComponent,
        resolve: { data: RelatorioService },
    }, 
    {
        path: 'checklist/:idProcesso',
        component: ChecklistListComponent,
        resolve: { data: checklistListResolver }
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RelatorioRoutingModule { }
