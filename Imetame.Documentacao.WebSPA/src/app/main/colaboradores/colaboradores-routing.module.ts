import { NgModule, inject } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ColaboradoresListComponent } from "./colaboradores-list/colaboradores-list.component";
import { ColaboradorService } from "./colaboradores.service";
import { ColaboradoresFormComponent } from "./colaboradores-form/colaboradores-form.component";
import { colaboradorResolver, listAllcolaboradorResolver } from "./colaboradores.resolver";
import { ColaboradoresAtividadeModalComponent } from "./colaboradores-atividade-modal/colaboradores-atividade-modal.component";



const routes: Routes = [
    {
        path: '', component: ColaboradoresListComponent,
        resolve: {
            data: listAllcolaboradorResolver,
        },
    },
    // {
    //     path: ":id",
    //     component: ColaboradoresFormComponent,
    //     resolve: { data: ColaboradorService },
    // },
    // {
    //     path: "novo",
    //     component: ColaboradoresFormComponent,
    //     resolve: {
    //         itens: listAllcolaboradorResolver,
    //     },
        
    // },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ColaboradorRoutingModule {}
