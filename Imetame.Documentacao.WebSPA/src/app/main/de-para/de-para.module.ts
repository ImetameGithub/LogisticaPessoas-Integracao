import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { DeParaListComponent } from "./de-para-list/de-para-list.component";
import { DeParaRoutingModule } from "./de-para-routing.module";
import { SharedModule } from "app/shared/shared.module";
import { DeParaFormComponent } from "./de-para-form/de-para-form.component";
import { DeParaService } from "./de-para.service";

@NgModule({
    declarations: [DeParaListComponent, DeParaFormComponent],
    imports: [CommonModule, DeParaRoutingModule, SharedModule],
    providers: [DeParaService],
})
export class DeParaModule {}
