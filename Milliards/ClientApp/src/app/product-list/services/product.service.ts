import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ProductList } from '../models/productList';
import { Types } from '../models/metadata';
import { AddProduct, ProductView } from '../models/addproduct';
import { AppConstants } from 'src/app/constants/app.constants';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http: HttpClient) { }

  getProductList(productListQuery): Observable<ProductList> {
    var pageNo: number = (productListQuery.start / productListQuery.length) + 1;
    var column = productListQuery.columns[productListQuery.order[0].column];
    let params = new HttpParams()
      .set('pageno', pageNo + "")
      .set("recordsize", productListQuery.length)
      .set("columnsort", column.data).set("ordersort", productListQuery.order[0].dir)
      .set("searchValue", productListQuery.search.value);
    return this.http.get<ProductList>(environment.base_url + AppConstants.ProductListAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

  getProductMetaData(): Observable<Types> {
    return this.http.get<Types>(environment.base_url + AppConstants.TypesAPI)
      .pipe(map(data => {
        return data;
      }));
  }

  saveProduct(product: AddProduct) {
    return this.http.post<any>(environment.base_url + AppConstants.ProducSaveUpdateAPI, product)
      .pipe(map(data => {
        return data;
      }));
  }

  getProductDetails(productId: number) {
    let params = new HttpParams()
      .set('productId', productId + "")
    return this.http.get<AddProduct>(environment.base_url + AppConstants.ProductDetailsAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

  getProductView(productId: number): Observable<ProductView> {
    let params = new HttpParams()
      .set('productId', productId + "")
    return this.http.get<ProductView>(environment.base_url + AppConstants.ProductViewAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

  validateSkuOrUpc(type: string, name: string): Observable<any> {
    let params = new HttpParams()
      .set('type', type.toUpperCase()).set('name', name);
    return this.http.get<any>(environment.base_url + AppConstants.ProductSKUUPCValidateAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }
}
