import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { DeParaListComponent } from "./de-para-list/de-para-list.component";

import { DeParaFormComponent } from "./de-para-form/de-para-form.component";
import { DeParaService } from "./de-para.service";
import { AuthGuard } from "app/guard/AuthGuard";

const routes: Routes = [
    {
        path: "",
        component: DeParaListComponent,
        resolve: { data: DeParaService },
    },
    {
        path: ":id",
        component: DeParaFormComponent,
        resolve: { data: DeParaService },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DeParaRoutingModule {}
