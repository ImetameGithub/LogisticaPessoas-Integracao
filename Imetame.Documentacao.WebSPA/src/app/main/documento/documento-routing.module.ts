import { NgModule, inject } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { DocumentoListComponent } from "./documento-list/documento-list.component";

import { DocumentoFormComponent } from "./documento-form/documento-form.component";
import { DocumentoService } from "./documento.service";

const routes: Routes = [
    // {
    //     path: "",
    //     component: DocumentoListComponent,
    //     resolve: { data: DocumentoService },
    // },
    {
        path: '', component: DocumentoListComponent,
        resolve: {
          data: ()=> inject(DocumentoService).GetAllPaginated(),
      }
    },
    {
        path: ":id",
        component: DocumentoFormComponent,
        resolve: { data: DocumentoService },
    },
    {
        path: "novo",
        component: DocumentoFormComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DeParaRoutingModule {}
