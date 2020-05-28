import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgHttpLoaderModule } from 'ng-http-loader';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { SharedModule } from './shared/shared.module';
import { DatePipe } from '@angular/common';
import { AppInterceptorService } from './app-interceptor.service';
import { AuthGuard } from './_guards/auth-guard.guard';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgHttpLoaderModule.forRoot(),
    ReactiveFormsModule,
    SharedModule,
    BrowserAnimationsModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    RouterModule.forRoot([
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: HomeComponent },
      { path: 'products', loadChildren: './product-list/product.module#ProductModule', canLoad: [AuthGuard] },
      { path: 'orders', loadChildren: './order-list/order.module#OrderModule', canLoad: [AuthGuard] },
      { path: 'fulorders', loadChildren: './fulorder-list/ful-order.module#FulOrderModule', canLoad: [AuthGuard] }
    ])
  ],
  providers: [DatePipe, { provide: HTTP_INTERCEPTORS, useClass: AppInterceptorService, multi: true }],
  exports: [NavMenuComponent],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
