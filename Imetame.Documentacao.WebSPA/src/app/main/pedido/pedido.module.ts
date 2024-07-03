import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { PedidoListComponent } from "./pedido-list/pedido-list.component";
import { DeParaRoutingModule } from "./pedido-routing.module";
import { SharedModule } from "app/shared/shared.module";
import { PedidoFormComponent } from "./pedido-form/pedido-form.component";
import { PedidoService } from "./pedido.service";
import { MatDialogModule } from "@angular/material/dialog";

@NgModule({
    declarations: [PedidoListComponent, PedidoFormComponent],
    imports: [CommonModule, DeParaRoutingModule, SharedModule, MatDialogModule],
    providers: [PedidoService],
})
export class PedidoModule {}
