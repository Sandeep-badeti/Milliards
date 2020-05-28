import { TestBed } from '@angular/core/testing';

import { OrderService } from './order.service';
import { HttpClientModule } from '@angular/common/http';
import { OrderLineList, OrderLine } from '../models/OrderLinesList';
import { FullOrderList } from '../models/FullOrderList';
import { OrderDetails } from '../models/OrderDetails';
import { AppConstants } from 'src/app/constants/app.constants';
import { OrderList } from '../models/OrderList';

describe('OrderService', () => {

  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientModule],
    providers: [OrderService]
  }));

  it('should be created', () => {
    const service: OrderService = TestBed.get(OrderService);
    expect(service).toBeTruthy();
  });

  it('Should return order lines with orderId 30941', (done) => {
    const service: OrderService = TestBed.get(OrderService);
    let orderId: number = 30941;
    service.getOrderLines(orderId).subscribe((resp: OrderLineList) => {
      expect(resp.data.length).toBeGreaterThan(0);
      done();
    })
  });

  it('Should not return any order lines with orderId 893894838', (done) => {
    const service: OrderService = TestBed.get(OrderService);
    let orderId: number = 893894838;
    service.getOrderLines(orderId).subscribe((resp: OrderLineList) => {
      expect(resp.data.length).toBe(0);
      done();
    })

  });

  it('Should  return fulorder details with orderId 30941', (done) => {
    const service: OrderService = TestBed.get(OrderService);
    let orderId: number = 30941;
    service.getFulfilledOrders(orderId).subscribe((resp: FullOrderList) => {
      expect(resp.data.length).toBeGreaterThan(0);
      done();
    });
  });
  it('Should not  return any fulorder details with orderId 30941000', (done) => {
    const service: OrderService = TestBed.get(OrderService);
    let orderId: number = 30941000;
    service.getFulfilledOrders(orderId).subscribe((resp: FullOrderList) => {
      expect(resp.data.length).toBe(0);
      done();
    });
  });
  it('Should return order details with orderId 30941', (done) => {
    const service: OrderService = TestBed.get(OrderService);
    let orderId: number = 30941;
    service.getOrderDetails(orderId).subscribe((resp: OrderDetails) => {
      expect(resp).toBeDefined();
      done();
    });
  });

  it('Should not  return any order details with orderId 30941000', (done) => {
    const service: OrderService = TestBed.get(OrderService);
    let orderId: number = 30941000;
    service.getOrderDetails(orderId).subscribe((resp: OrderDetails) => {
      expect(resp['data']).toBeNull();
      done();
    });

  });

  it('Should return the order list', (done) => {
    const service: OrderService = TestBed.get(OrderService);
    let config = {
      order: [{ column: 7, dir: AppConstants.Desc }],
      start: 0,
      length: AppConstants.PageLength,
      columns: AppConstants.orderListColums

    }
    service.getOrderList(config, '','', '').subscribe((resp: OrderList) => {
      expect(resp.totalRecordsCount).toBeGreaterThan(0);
      done();
    });

  });

  it('Should not return the order list', (done) => {
    const service: OrderService = TestBed.get(OrderService);
    let config = {
      order: [{ column: 7, dir: AppConstants.Desc }],
      start: 0,
      length: AppConstants.PageLength,
      columns: AppConstants.orderListColums

    }
    service.getOrderList(config, '','', 'This is worng search').subscribe((resp: OrderList) => {
      expect(resp.totalRecordsCount).toBe(0);
      done();
    });

  });


});
