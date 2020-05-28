import { TestBed } from '@angular/core/testing';

import { FullOrderService } from './full-order.service';
import { HttpClientModule } from '@angular/common/http';
import { AppConstants, ModeEnum } from 'src/app/constants/app.constants';
import { FullOrderList, FulOrderDetails, CarrierServiceAndTypeResp, Pallet, CarrierServiceAndType, CreatePalletResp, CarrierServiceAndTypeResponse, FullOrerListByPallet } from '../models/fullorderlist';
import { ViewFulOrder } from '../models/view-fulorder';
import { AssignRequest } from '../models/assign-request';

describe('FullOrderService', () => {
 
  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientModule],
    providers: [FullOrderService]
  }));

  
  it('should be created', () => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    expect(service).toBeTruthy();
  });


  it('Should get ful order list Regular Mode', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let config = {
      order: [{ column: 0, dir: AppConstants.Desc }],
      start: 0,
      length: AppConstants.PageLength,
      columns: AppConstants.FulOrderListColumns,
      search: { value: '' }

    }
    service.getFullOrdersList(config, ModeEnum.Regular, 0).subscribe((resp: FullOrderList) => {
      expect(resp.totalRecordsCount).toBeGreaterThan(0);
      done();
    });
  });

  it('Should get ful order list Assigning Mode', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let config = {
      order: [{ column: 0, dir: AppConstants.Desc }],
      start: 0,
      length: AppConstants.PageLength,
      columns: AppConstants.FulOrderListColumns,
      search: { value: '' }

    }
    service.getFullOrdersList(config, ModeEnum.Assigning, 0).subscribe((resp: FullOrderList) => {
      expect(resp.data[0].fulorderstatus).toBe('Entered');
      done();
    });
  });

  it('Should get ful order list Picking Mode', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let config = {
      order: [{ column: 0, dir: AppConstants.Desc }],
      start: 0,
      length: -1,
      columns: AppConstants.FulOrderListColumns,
      search: { value: '' }

    }
    service.getFullOrdersList(config, ModeEnum.Picking, 0).subscribe((resp: FullOrderList) => {
      expect(resp.data[0].fulorderstatus).toBe('Assigned');
      done();
    });
  });

  it('Should get ful order list Picking Mode with Warehosue 555555 ', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let config = {
      order: [{ column: 0, dir: AppConstants.Desc }],
      start: 0,
      length: -1,
      columns: AppConstants.FulOrderListColumns,
      search: { value: '' }

    }
    let warehouseId = 555555;//FK Logistics 
    service.getFullOrdersList(config, ModeEnum.Picking, warehouseId).subscribe((resp: FullOrderList) => {
      expect(resp.totalRecordsCount).toBe(0);
      done();
    });
  });

  it('Should Labelling Mode give error message saying UPC with Picked status not found', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let upc = '840985111991';
    service.generateLabel(upc).subscribe((resp: FulOrderDetails) => {
      expect(resp.message).toBe('UPC with Picked status not found');
      done();
    });
  });

  it('Should Return Carrier Service List with carrier ID 1001', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let carrierId: number = 1001;
    service.getCarrierServiceList(carrierId).subscribe((resp) => {
      expect(resp['carrierService'].length).toBeGreaterThan(0);
      done();
    });
  });

  it('Should Return empty Carrier Service List with carrier ID 156666', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let carrierId: number = 156666;
    service.getCarrierServiceList(carrierId).subscribe((resp) => {
      expect(resp['carrierService'].length).toBe(0);
      done();
    });
  });

  it('Should Return fulorder details with fulOrder ID 14267', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let fulOrderId: number = 14267;
    service.getFulOrderVew(fulOrderId).subscribe((resp: ViewFulOrder) => {
      expect(resp.fulOrderId).toBe(fulOrderId);
      done();
    });
  });

  it('Should not Return fulorder details with fulOrder ID 8888', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let fulOrderId: number = 8888;
    service.getFulOrderVew(fulOrderId).subscribe((resp: ViewFulOrder) => {
      expect(resp.fulOrderId).toBe(fulOrderId);
      done();
    });
  });

  it('Should not create pick list', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let fulOrderIds: Array<number> = [];
    fulOrderIds.push(8888);
    fulOrderIds.push(9999);
    service.createPickList(fulOrderIds).subscribe((resp: { status: boolean, message: string }) => {
      expect(resp.status).toBeTruthy();
      done();
    });
  });

  it('Should not Assign', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let req: AssignRequest = {
      "fulOrderId": 9999,
      "warehouseId": 9999,
      "carrierId": 9999,
      "carrierServiceId": 9999
    }
    service.assgin(req).subscribe((resp: boolean) => {
      expect(resp).toBeFalsy();
      done();
    });
  }, 20000);

  it('Should not Generate Label', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let upc = '840985111991';
    service.generateLabel(upc).subscribe((resp: FulOrderDetails) => {
      expect(resp.status).toBeFalsy();
      done();
    });
  });

  it('Should not Create Package', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let fulOrderId: number = 10495;
    service.createPackage(fulOrderId).subscribe((resp: FulOrderDetails) => {
      expect(resp.status).toBeFalsy();
      done();
    });
  });

  it('Should get CarrierAndServiceTypes', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    service.getCarrierAndServiceTypes().subscribe((resp: CarrierServiceAndTypeResp) => {
      expect(resp.status).toBeTruthy();
      done();
    });
  });

  it('Should get PalletList', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    service.getPalletList().subscribe((resp) => {
      expect(resp['data'].length).toBeGreaterThan(0);
      done();
    });
  });

  it('Should not create Pallet', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let carrierInfo: CarrierServiceAndType = {
      "carrierId": 554,
      "carrierServiceTypeId": 88,
      "carrierName": "ABC"
    }
    service.createPallet(carrierInfo).subscribe((resp: CreatePalletResp) => {
      expect(resp.status).toBeFalsy();
      done();
    });
  }, 20000);

  it('Should not Palletize', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let carrierInfo: CarrierServiceAndType = {
      "carrierId": 554,
      "carrierServiceTypeId": 88,
      "carrierName": "ABC"
    }
    service.pallatize('34343434', 8988).subscribe((resp: FulOrderDetails) => {
      expect(resp.status).toBeFalsy();
      done();
    });
  });

  it('Should Get Carrier ServiceInfo By Pallet with palletID = 3', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let palletID = 3;
    service.getCarrierServiceInfoByPallet(palletID).subscribe((resp: CarrierServiceAndTypeResponse) => {
      expect(resp.status).toBeTruthy();
      done();
    });
  });

  it('Should Get Carrier ServiceInfo By Pallet with palletID = 3', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let palletID = 3;
    let config = {
      order: [{ column: 0, dir: AppConstants.Desc }],
      start: 0,
      length: AppConstants.PageLength,
      columns: AppConstants.SrchFullOrderForExitScan,
      search: { value: '' }

    }
    service.getFullOrdersListForExitScan(config, palletID).subscribe((resp: FullOrerListByPallet) => {
      expect(resp.status).toBeTruthy();
      done();
    });
  });

  it('Should not exitScan', (done) => {
    const service: FullOrderService = TestBed.get(FullOrderService);
    let palletID = 3;
    let fulOrderIds: Array<number> = [];
    fulOrderIds.push(8888);
    fulOrderIds.push(9999);
    service.exitScan(palletID, fulOrderIds).subscribe((resp: FullOrerListByPallet) => {
      expect(resp.status).toBeFalsy();
      done();
    });
  });

});
