import { NgModule } from '@angular/core';
import { DataTablesModule } from 'angular-datatables';
import { FormsModule } from '@angular/forms';
import { DemoMaterialModule } from '../../app/material-module';
import { MatNativeDateModule } from '@angular/material/core';
import { TextMaskModule } from 'angular2-text-mask';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import {TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [],
  imports: [
    DataTablesModule,
    FormsModule,
    DemoMaterialModule,
    MatNativeDateModule,
    TextMaskModule,
    MatTooltipModule,
    MatSnackBarModule,
    TranslateModule
  ], exports: [
    DataTablesModule,
    FormsModule,
    DemoMaterialModule,
    MatNativeDateModule,
    TextMaskModule,
    MatTooltipModule,
    MatSnackBarModule,
    TranslateModule
  ]
})
export class SharedModule { }
