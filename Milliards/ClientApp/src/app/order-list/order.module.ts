import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderMainComponent } from './order-main.component';
import { OrderListComponent } from './list/order-list.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../_guards/auth-guard.guard';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { OrderLineTableComponent } from './order-line-table/order-line-table.component';
import { FullOrdersTableComponent } from './full-orders-table/full-orders-table.component';
import { TableCellToggleDirective } from './directives/table-cell-toggle.directive';

@NgModule({
  declarations: [
    OrderMainComponent,
    OrderListComponent,
    OrderDetailsComponent,
    TableCellToggleDirective,
    OrderLineTableComponent,
    FullOrdersTableComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {
        path: '', component: OrderMainComponent, canActivate: [AuthGuard],
        children: [
          { path: 'list', component: OrderListComponent, canActivate: [AuthGuard] },
          { path: 'orderDetails', component: OrderDetailsComponent, canActivate: [AuthGuard] },
        ]
      }
    ])
  ]
})
export class OrderModule { }
