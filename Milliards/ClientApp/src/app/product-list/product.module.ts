import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ProductMainComponent } from './product-main.component';
import { SharedModule } from '../shared/shared.module';
import { ProductComponent } from './product/product.component';
import { ProductListComponent } from './list/product-list.component';
import { AuthGuard } from '../_guards/auth-guard.guard';
import { ViewProductComponent } from './view-product/view-product.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    ProductMainComponent,
    ProductComponent,
    ProductListComponent,
    ViewProductComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule, 
    RouterModule.forChild([
      {
        path: '', component: ProductMainComponent, canActivate: [AuthGuard],
        children: [
          { path: 'list', component: ProductListComponent, canActivate: [AuthGuard] },
          { path: 'edit', component: ProductComponent, canActivateChild: [AuthGuard] },
          { path: 'add', component: ProductComponent, canActivateChild: [AuthGuard] },
          { path: 'clone', component: ProductComponent, canActivateChild: [AuthGuard] },
          { path: 'view', component: ViewProductComponent, canActivateChild: [AuthGuard] }
        ]
      }
    ])
  ]
  
})
export class ProductModule { }
