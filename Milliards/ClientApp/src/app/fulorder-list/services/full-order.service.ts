import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { FullOrderList, FulOrderDetails, Pallet, CarrierServiceAndTypeResp, CarrierServiceAndType, CreatePalletResp, FullOrerListByPallet, CarrierServiceAndTypeResponse } from '../models/fullorderlist';
import { CarrierService } from '../models/meta-data-models';
import { AssignRequest } from '../models/assign-request';
import { AppConstants } from 'src/app/constants/app.constants';
import { ViewFulOrder } from '../models/view-fulorder';

@Injectable({
  providedIn: 'root'
})
export class FullOrderService {

  constructor(private http: HttpClient) { }

  getFullOrdersList(fulOrderListQuery, mode: string, warehouseId: number) {
    var pageNo: number = (fulOrderListQuery.start / fulOrderListQuery.length) + 1;
    var column = fulOrderListQuery.columns[fulOrderListQuery.order[0].column];
    let params = new HttpParams()
      .set('pageno', pageNo + "")
      .set("recordsize", fulOrderListQuery.length)
      .set("columnsort", column.data).set("ordersort", fulOrderListQuery.order[0].dir)
      .set("searchValue", fulOrderListQuery.search.value)
      .set("warehouseId", '' + warehouseId)
      .set("mode", mode);
    return this.http.get<FullOrderList>(environment.base_url + AppConstants.FulOrderListAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

  assgin(request: AssignRequest): Observable<boolean> {
    return this.http.put<boolean>(environment.base_url + AppConstants.AssignAPI, request)
      .pipe(map(data => {
        return data;
      }));
  }

  getCarrierServiceList(carrierId: number): Observable<CarrierService[]> {
    let params = new HttpParams()
      .set('carrierId', carrierId + "");
    return this.http.get<CarrierService[]>(environment.base_url + AppConstants.CarrierServiceAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

  getFulOrderVew(fulOrderId: number): Observable<ViewFulOrder> {
    let params = new HttpParams()
      .set('fulOrderId', fulOrderId + "")
    return this.http.get<ViewFulOrder>(environment.base_url + AppConstants.FulOrderViewAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

  createPickList(fulOrderIds: Array<number>): Observable<{ status: boolean, message: string }> {
    return this.http.post<{ status: boolean, message: string }>(environment.base_url + AppConstants.CreatePickListAPI, fulOrderIds)
      .pipe(map(data => {
        return data;
      }));
  }

  generateLabel(upc: string): Observable<FulOrderDetails> {
    return this.http.post<FulOrderDetails>(environment.base_url + AppConstants.CreateLabelAPI, {"upc": upc})
      .pipe(map(data => {
        return data;
      }));
  }

  createPackage(fulOrderId: number): Observable<FulOrderDetails> {
    return this.http.put<FulOrderDetails>(environment.base_url + AppConstants.CreatePackageAPI, {"fullOrderId": fulOrderId})
      .pipe(map(data => {
        return data;
      }));
  }

  getCarrierAndServiceTypes(): Observable<CarrierServiceAndTypeResp> {
    return this.http.get<CarrierServiceAndTypeResp>(environment.base_url + AppConstants.GetCarrierServiceAndTypesAPI)
      .pipe(map(data => {
        return data;
      }));
  }

  getPalletList(): Observable<Pallet[]> {
    return this.http.get<Pallet[]>(environment.base_url + AppConstants.GetPlalletListAPI)
      .pipe(map(data => {
        return data;
      }));
  }

  createPallet(carrierInfo: CarrierServiceAndType) {
    return this.http.post<CreatePalletResp>(environment.base_url + AppConstants.CreatePlalletAPI, carrierInfo)
    .pipe(map(data => {
      return data;
    }));
  }

  pallatize(trackingNumber: string,palletId:number): Observable<FulOrderDetails> {
    return this.http.post<FulOrderDetails>(environment.base_url + AppConstants.PalletizeAPI,
       {trackingNumber:trackingNumber,palletId:palletId})
    .pipe(map(data => {
      return data;
    }));

  }
  
  getFullOrdersListForExitScan(fulOrderListQuery, palletId: number):Observable<FullOrerListByPallet> {
  
    var column = fulOrderListQuery.columns[fulOrderListQuery.order[0].column];
    let params = new HttpParams()
      .set("columnsort", column.data).set("ordersort", fulOrderListQuery.order[0].dir)
      .set("searchValue", fulOrderListQuery.search.value)
      .set("palletId", '' + palletId) 
    return this.http.get<FullOrerListByPallet>(environment.base_url + AppConstants.FulOrderListByPalletAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

  exitScan(palletId: number, fulOrderIds: Array<number>) : Observable<FullOrerListByPallet> {
     return this.http.post<FullOrerListByPallet>(environment.base_url + AppConstants.ExitScanAPI,
       {palletId:palletId,fulOrderIdList:fulOrderIds})
    .pipe(map(data => {
      return data;
    }));
  }

  getCarrierServiceInfoByPallet(palletId:number):Observable<CarrierServiceAndTypeResponse> {
    let params = new HttpParams()
      .set("palletId", ""+palletId);
    return this.http.get<CarrierServiceAndTypeResponse>(environment.base_url + AppConstants.GetCarrierServiceInfoByPalletAPI, { params: params })
      .pipe(map(data => {
        return data;
      }));
  }

}
