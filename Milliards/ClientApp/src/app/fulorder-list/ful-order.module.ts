import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FulorderMainComponent } from './fulorder-main.component';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../_guards/auth-guard.guard';
import { FulorderListComponent } from './list/fulorder-list.component';
import { AssignComponent } from './assign/assign.component';
import { ViewFulorderComponent } from './view-fulorder/view-fulorder.component';
import { LabellingDialogComponent } from './labelling-dialog/labelling-dialog.component';
import { PalletizingDialogComponent } from './palletizing-dialog/palletizing-dialog.component';
import { ExitScanDialogComponent } from './exit-scan-dialog/exit-scan-dialog.component';

@NgModule({
  declarations: [
    FulorderMainComponent,
    FulorderListComponent,
    AssignComponent,
    ViewFulorderComponent,
    LabellingDialogComponent,
    PalletizingDialogComponent,
    ExitScanDialogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      {
        path: '', component: FulorderMainComponent, canActivate: [AuthGuard],
        children: [
          { path: 'list', component: FulorderListComponent, canActivate: [AuthGuard] },
          { path: 'view', component: ViewFulorderComponent, canActivate: [AuthGuard] }

        ]
      }
    ])
  ],
  entryComponents: [AssignComponent, LabellingDialogComponent, PalletizingDialogComponent, ExitScanDialogComponent]
})
export class FulOrderModule { }
