import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { OrderList } from '../models/OrderList';
import { OrderLineList } from '../models/OrderLinesList';
import { FullOrderList } from '../models/FullOrderList';
import { OrderDetails } from '../models/OrderDetails';
import { AppConstants } from 'src/app/constants/app.constants';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient) { }

  getOrderList(orderListQuery: any, fromDate: string, toDate: string, searchText: string): Observable<OrderList> {
    
    let pageNo: number = (orderListQuery.start / orderListQuery.length) + 1;
    let column = orderListQuery.columns[orderListQuery.order[0].column];
    let params = new HttpParams()
      .set('pageno', pageNo + "")
      .set("recordsize", orderListQuery.length)
      .set("columnsort", column.data).set("ordersort", orderListQuery.order[0].dir)
      .set("searchValue", searchText).set("fromDate", fromDate).set("toDate", toDate);
    return this.http.get<OrderList>(environment.base_url + AppConstants.OrderListAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

  getOrderLines(orderId: number): Observable<OrderLineList> {
    let params = new HttpParams()
      .set('orderId', orderId + "");
    return this.http.get<OrderLineList>(environment.base_url + AppConstants.OrderLinesAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

  getFulfilledOrders(orderId: number): Observable<FullOrderList> {
    let params = new HttpParams()
      .set('orderId', orderId + "");
    return this.http.get<FullOrderList>(environment.base_url + AppConstants.FulOrderAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

  getOrderDetails(orderId: number): Observable<OrderDetails> {
    let params = new HttpParams().set('orderId', orderId + "");
    return this.http.get<OrderDetails>(environment.base_url + AppConstants.OrderDetailsAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

}
