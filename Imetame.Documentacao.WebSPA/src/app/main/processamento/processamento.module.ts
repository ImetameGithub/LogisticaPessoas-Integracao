import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ProcessamentoListComponent } from "./processamento-list/processamento-list.component";
import { ProcessamentoRoutingModule } from "./processamento-routing.module";
import { SharedModule } from "app/shared/shared.module";
import { ProcessamentoService } from "./processamento.service";

@NgModule({
    declarations: [ProcessamentoListComponent],
    imports: [CommonModule, ProcessamentoRoutingModule, SharedModule],
    providers: [ProcessamentoService],
})
export class ProcessamentoModule {}
