import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { TranslateModule } from '@ngx-translate/core';

import { FuseSharedModule } from '@fuse/shared.module';

import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';

import { CdkTableModule } from '@angular/cdk/table';

//import { NgxChartsModule } from '@swimlane/ngx-charts';

import { FuseWidgetModule } from '@fuse/components/widget/widget.module';
import { FuseConfirmDialogModule } from '@fuse/components';


import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { MatTooltipModule } from '@angular/material/tooltip';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialogModule } from '@angular/material/dialog';
import { MatListModule } from '@angular/material/list';
import { MatChipsModule } from '@angular/material/chips';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatRippleModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTreeModule } from '@angular/material/tree';
import { MatRadioModule } from '@angular/material/radio';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';




// Components

import { NouisliderComponent } from './components/nouislider/nouislider.component';
import { StarRatingComponent } from './components/star-rating/star-rating.component';
import { ShowErrosDialogComponent } from './components/show-erros-dialog/show-erros-dialog.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { MessageDialogComponent } from './components/message-dialog/message-dialog.component';
// Pipes
import { UppercasePipe } from './pipes/uppercase.pipe';

import { KeysPipe } from './pipes/keys.pipe';

// Directives

import { MesAnoPickerComponent } from './components/mes-ano-picker/mes-ano-picker.component';


import { ImetameDatePipe } from './pipes/imetame-date.pipe';

import { SimpleFocusDirective } from './directives/simple-focus/simple-focus.directive';
import { SimpleFocusService } from './directives/simple-focus/simple-focus.service';
import { SimOuNaoDialogComponent } from './components/sim-ou-nao-dialog/sim-ou-nao-dialog.component';


import { BlockSpecialCharDirective } from './directives/block-special-char.directive';
import { ApplicationService } from 'app/services/application.service';
// import { StatusEnumPipe } from './pipes/status-enum.pipe';











@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,

      MatButtonModule,
      MatTooltipModule,
      MatIconModule,      
      MatFormFieldModule,
      MatDialogModule,
      MatListModule,
      MatChipsModule,
      MatInputModule,
      MatPaginatorModule,
      MatRippleModule,
      MatSelectModule,
      // MatSortModule,
      MatTableModule,
      MatTabsModule,
      MatSnackBarModule,
      MatCheckboxModule,
      MatDatepickerModule,
      MatMenuModule,
      MatToolbarModule ,
      MatTreeModule,
      MatRadioModule,
      FuseSharedModule,
      TranslateModule,
      //NgxChartsModule,
      MatProgressBarModule,
      MatAutocompleteModule
        
        
        
  ],
  declarations: [
      ConfirmDialogComponent,    
      SimOuNaoDialogComponent,
      MessageDialogComponent,
      
      
      NouisliderComponent,
      StarRatingComponent,
      ShowErrosDialogComponent,
      UppercasePipe,   
      // StatusEnumPipe,
      ImetameDatePipe,
      KeysPipe,
      
      MesAnoPickerComponent,
      SimpleFocusDirective,
      BlockSpecialCharDirective,
    
    
  ],
  exports: [
    // Modules
      CommonModule,
      FormsModule,
      ReactiveFormsModule,

      
      TranslateModule,
      FuseSharedModule,
      FuseWidgetModule,
      FuseConfirmDialogModule,
      CdkTableModule,      
      //NgxChartsModule,
      //material
      MatButtonModule,
      MatTooltipModule,
      MatIconModule,      
      MatFormFieldModule,
      MatDialogModule,
      MatListModule,
      MatChipsModule,
      MatInputModule,
      MatPaginatorModule,
      MatRippleModule,
      MatSelectModule,
      // MatSortModule,
      MatTableModule,
      MatTabsModule,
      MatSnackBarModule,
      MatCheckboxModule,
      MatDatepickerModule,
      MatMenuModule,
      MatToolbarModule,
      MatTreeModule,
      MatRadioModule,
      MatProgressBarModule,
      MatAutocompleteModule,

      //do modulo
      ConfirmDialogComponent,
      SimOuNaoDialogComponent,
      MessageDialogComponent,
      
      
      NouisliderComponent,
      StarRatingComponent,
      ShowErrosDialogComponent,
      UppercasePipe,
      // StatusEnumPipe,
      ImetameDatePipe,
      KeysPipe,

      
      MesAnoPickerComponent,
      SimpleFocusDirective,
      BlockSpecialCharDirective

  ],
  entryComponents: [
      ShowErrosDialogComponent,
      ConfirmDialogComponent,
      SimOuNaoDialogComponent,
      MessageDialogComponent
      
  ],
  providers: [
    ApplicationService,
      SimpleFocusService
      
  ]

})
export class SharedModule { }
