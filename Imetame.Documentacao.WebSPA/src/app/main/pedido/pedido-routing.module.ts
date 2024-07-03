import { NgModule, inject } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { PedidoListComponent } from "./pedido-list/pedido-list.component";

import { PedidoFormComponent } from "./pedido-form/pedido-form.component";
import { PedidoService } from "./pedido.service";

const routes: Routes = [
    // {
    //     path: "",
    //     component: PedidoListComponent,
    //     resolve: { data: PedidoService },
    // },
    {
        path: '', component: PedidoListComponent,
        resolve: {
          data: ()=> inject(PedidoService).GetAllPaginated(),
      }
    },
    {
        path: ":id",
        component: PedidoFormComponent,
        resolve: { data: PedidoService },
    },
    {
        path: "novo",
        component: PedidoFormComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DeParaRoutingModule {}
