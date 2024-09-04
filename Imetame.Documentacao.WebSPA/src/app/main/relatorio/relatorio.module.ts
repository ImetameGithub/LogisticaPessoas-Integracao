import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SharedModule } from "app/shared/shared.module";
import { RelatorioService } from "./relatorio.service";
import { ChecklistComponent } from "./checklist/checklist/checklist.component";
import { RelatorioRoutingModule } from "./relatorio-routing.module";
import { MatDialogModule } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { ReactiveFormsModule } from "@angular/forms";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { MatOptionModule } from "@angular/material/core";
import { CustomSearchSelectComponent } from "app/shared/components/custom-select/custom-select.component";
import { MatGridListModule } from "@angular/material/grid-list";
import { MatButtonModule } from "@angular/material/button";
import { ChecklistListComponent } from './checklist/checklist/checklist-list/checklist-list.component';
import { MatTreeFlatDataSource, MatTreeFlattener, MatTreeModule } from "@angular/material/tree";

@NgModule({
    declarations: [ChecklistComponent, ChecklistListComponent],
    imports: [
        CommonModule,
        RelatorioRoutingModule,
        SharedModule,
        MatDialogModule,
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatOptionModule,
        CustomSearchSelectComponent,
        CommonModule,
        MatGridListModule,
        MatButtonModule,
        MatTreeModule
    ],
    providers: [RelatorioService],
})
export class RelatorioModule { }
