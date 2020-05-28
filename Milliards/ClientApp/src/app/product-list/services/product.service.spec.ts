import { TestBed } from '@angular/core/testing';

import { ProductService } from './product.service';
import { HttpClientModule } from '@angular/common/http';
import { AppConstants } from 'src/app/constants/app.constants';
import { ProductView, AddProduct } from '../models/addproduct';

describe('ProductService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientModule],
    providers: [ProductService]
  }).compileComponents());

  it('should be created', () => {
    const service: ProductService = TestBed.get(ProductService);
    expect(service).toBeTruthy();
  });

  it('should return product list', (done) => {
    const service: ProductService = TestBed.get(ProductService);
    let config = {
      order: [{ column: 0, dir: AppConstants.Desc }],
      start: 0,
      length: AppConstants.PageLength,
      columns: AppConstants.ProductListColumns,
      search: { value: "" }
    }
    service.getProductList(config).subscribe((resp) => {
      expect(resp.totalRecordsCount).toBeGreaterThan(0);
      done();
    })
  });
  it('should not return any product with given search value Boost', (done) => {
    const service: ProductService = TestBed.get(ProductService);
    let config = {
      order: [{ column: 0, dir: AppConstants.Desc }],
      start: 0,
      length: AppConstants.PageLength,
      columns: AppConstants.ProductListColumns,
      search: { value: "Boost" }
    }
    service.getProductList(config).subscribe((resp) => {
      expect(resp.totalRecordsCount).toBe(0);
      done();
    })
  });
  it('should return product detail with productId = 42 ', (done) => {
    const service: ProductService = TestBed.get(ProductService);
    let productId = 42;
    service.getProductDetails(productId).subscribe((resp) => {
      expect(resp["data"].productId).toBe(productId);
      done();
    });
  });
  it('should not return product detail with productId = 999933 ', (done) => {
    const service: ProductService = TestBed.get(ProductService);
    let productId = 999933;
    service.getProductDetails(productId).subscribe((resp) => {
      expect(resp['data']).toBeNull();
      done();
    });
  });
  it('should be able to get metadata ', (done) => {
    const service: ProductService = TestBed.get(ProductService);
    service.getProductMetaData().subscribe((resp) => {
      expect(resp).toBeDefined();
      done();
    });
  });
  it('should be able to get view info ', (done) => {
    const service: ProductService = TestBed.get(ProductService);
    let productId = 42;
    service.getProductView(productId).subscribe((resp: ProductView) => {
      expect(resp['data'].productId).toBe(productId);
      done();
    });
  });
  it('should not get any view info ', (done) => {
    const service: ProductService = TestBed.get(ProductService);
    let productId = 999933;
    service.getProductView(productId).subscribe((resp: ProductView) => {
      expect(resp['data']).toBeNull();
      done();
    });
  });
  it('should exists sku MIL-HK612-6-B', (done) => {
    const service: ProductService = TestBed.get(ProductService);
    service.validateSkuOrUpc(AppConstants.Sku, 'MIL-HK612-6-B').subscribe((resp) => {
      expect(resp.status).toBeTruthy();
      done();
    });
  });
  it('should exists UPC 840985111991', (done) => {
    const service: ProductService = TestBed.get(ProductService);
    service.validateSkuOrUpc(AppConstants.Upc, '840985111991').subscribe((resp) => {
      expect(resp.status).toBeTruthy();
      done();
    });
  });
  it('should create Product ', (done) => {
    const service: ProductService = TestBed.get(ProductService);
    let request: AddProduct = {
      "name": "TestingFrameWork4",
      "description": "Milliard 2-Inch Egg Crate Ventilated Memory Foam Mattress Topper - Full1",
      "upc": "8048794022069",
      "boxId": 1,
      "statusId": 1,
      "length": 19,
      "width": 12,
      "height": 15,
      "weight": 38,
      "mainSKU": 'TestingFrameWork4',
      "dimensionUnitId": 1,
      "weightUnitId": 6,
      "shipsAlone_FLG": false,
      "manufacturerId": 1,
      "conditionId": 1,
      "categoryId": 17,
      "colorId": 1,
      "ref1": "test",
      "ref2": "test1",
      "productSku": [{
        "productId": 0,
        "sku": "TestingFrameWork4_2",
        "statusId": 1,
        "description": "Milliard 2-Inch Egg Crate Ventilated Memory Foam Mattress Topper - Full",
        "skuTypeId": 1
      }, {
        "productId": 0,
        "sku": "TestingFrameWork_4_1",
        "statusId": 1,
        "description": "Milliard 2-Inch Egg Crate Ventilated Memory Foam Mattress Topper - Full",
        "skuTypeId": 1
      }, {
        "productId": 0,
        "sku": "TestingFrameWork4",
        "statusId": 1,
        "description": "Milliard 2-Inch Egg Crate Ventilated Memory Foam Mattress Topper - Full",
        "skuTypeId": 2
      }
      ],
      "productVersion": [{
        "productId": 0,
        "description": "Milliard 2-Inch Egg Crate Ventilated Memory Foam Mattress Topper - Full",
        "statusId": 1,
        "productVersionId": 1
      }, {
        "productId": 0,
        "description": "Milliard 2-Inch Egg Crate Ventilated Memory Foam Mattress Topper - Full_test",
        "statusId": 1,
        "productVersionId": 2
      }
      ],
      "productInventory": [{
        "productVersionId": 1,
        "warehouseId": 32012,
        "quantity": 1000,
        "statusId": 1,
        "productInventoryId": 0
      }, {
        "productVersionId": 2,
        "warehouseId": 32012,
        "quantity": 1000,
        "statusId": 1,
        "productInventoryId": 0
      }
      ],
      "productTag": [{
        "productId": 1,
        "tagId": 11024
      }
      ]
    };
    service.saveProduct(request).subscribe((resp) => {
      expect(resp['status']).toBeTruthy();
      done();
    });
  });

});
