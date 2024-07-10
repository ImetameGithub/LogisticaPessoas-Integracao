import { NgModule, inject } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ColaboradoresListComponent } from "./colaboradores-list/colaboradores-list.component";
import { ColaboradorService } from "./colaboradores.service";
import { ColaboradoresFormComponent } from "./colaboradores-form/colaboradores-form.component";



const routes: Routes = [
    // {
    //     path: "",
    //     component: ColaboradoresListComponent,
    //     resolve: { data: ColaboradoresService },
    // },
    {
        path: '', component: ColaboradoresListComponent,
        resolve: {
          data: ()=> inject(ColaboradorService).GetAllPaginated(),
      }
    },
    {
        path: ":id",
        component: ColaboradoresFormComponent,
        resolve: { data: ColaboradorService },
    },
    {
        path: "novo",
        component: ColaboradoresFormComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ColaboradorRoutingModule {}
