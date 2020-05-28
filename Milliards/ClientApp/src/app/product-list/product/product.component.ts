import { Component, OnInit, NgZone, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { Types, ProductTag } from '../models/metadata';
import { FormBuilder, FormGroup, Validators, FormArray } from "@angular/forms";
import { AddProduct, ProductSKU, ProductVersion, ProductInventory } from '../models/addproduct';
import { ProductService } from '../services/product.service';
import { AppConstants } from 'src/app/constants/app.constants';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  public types: Types;
  public submitted: boolean;
  public prdoductInfo: AddProduct;
  public productTagSelected: any;
  public message: string = "";
  public showProduct: boolean = false;
  public mainSku: string = '';
  public mainVirtualDuplicated: Array<boolean> = [];
  public dupliateSkuExist: Array<boolean> = [];
  public upcExist: boolean;
  public showErrorPopup: boolean = false;
  public errorMessage: string;
  public productForm: FormGroup;
  public type: string = '';

  
  private productId: number = 0;
  private versionId: number = 1;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private zone: NgZone,
    private transalteService : TranslateService, 
    private productService: ProductService) {
    this.types = new Types();
    this.prdoductInfo = new AddProduct();
    transalteService.setDefaultLang(AppConstants.DefaultLang);
    this.productForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      mainSku: ['', Validators.required],
      upc: ['', Validators.required],
      boxId: ['', Validators.required],
      statusId: ['', Validators.required],
      length: ['', [Validators.required, Validators.min(1)]],
      width: ['', [Validators.required, Validators.min(1)]],
      height: ['', [Validators.required, Validators.min(1)]],
      weight: ['', [Validators.required, Validators.min(1)]],
      dimensionUnitId: ['', Validators.required],
      weightUnitId: ['', Validators.required],
      shipsaloneFlg: ['', Validators.required],
      conditionId: ['', Validators.required],
      colorId: ['', Validators.required],
      manufacturerId: ['', Validators.required],
      categoryId: ['', Validators.required],
      productSku: this.formBuilder.array([]),
      productVersion: this.formBuilder.array([]),
      productInventory: this.formBuilder.array([])

    });
    window.scrollTo(0, 0);
  }

  ngOnInit() {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0)
    });

    this.route.queryParams.subscribe(params => {
      this.productId = parseInt(atob(this.route.snapshot.queryParams.productId));
      this.prdoductInfo.productId = this.productId;
      this.type = atob(this.route.snapshot.queryParams.type);
    });

    this.productService.getProductMetaData().subscribe((types) => {
      this.types = types;
    });
    this.loadDefaults();

  }


  /**
   * Adds sku row
   */
  public addSKURow(): void {
    let newSKU = new ProductSKU();
    newSKU.skuTypeId = 1;
    newSKU.productId = this.productId;
    if (!this.prdoductInfo.productSku)
      this.prdoductInfo.productSku = [];
    this.prdoductInfo.productSku.push(newSKU);
    this.productSkuArray.push(this.getProductPoductSkus());
  }

  /**
   * Gets product inventory array
   */
  public get productInventoryArray(): FormArray {
    return <FormArray>this.productForm.get('productInventory');
  }

  /**
   * Gets product sku array
   */
  public get productSkuArray(): FormArray {
    return <FormArray>this.productForm.get('productSku');
  }

  /**
   * Gets product version array
   */
  public get productVersionArray(): FormArray {
    return <FormArray>this.productForm.get('productVersion');
  }

  /**
   * Deletes skurow
   * @param index 
   */
  public deleteSKURow(index: number): void {
    this.prdoductInfo.productSku.splice(index, 1);
    this.productSkuArray.removeAt(index);
  }

  /**
   * Determines whether new skurow is
   * @param i 
   * @returns boolean 
   */
  public isNewSKURow(i: number): boolean {
    return this.prdoductInfo.productSku.length - 1 == i;
  }

  /**
   * Adds version row
   */
  public addVersionRow(): void {
    let newVersion = new ProductVersion();
    newVersion.statusId = 1;
    newVersion.productId = this.productId;
    newVersion.productVersionId = ++this.versionId;
    this.prdoductInfo.productVersion.push(newVersion);
    let newInventory = new ProductInventory();
    newInventory.productVersionId = this.versionId;
    this.prdoductInfo.productInventory.push(newInventory);
    this.productVersionArray.push(this.getProductVersion());
    this.productInventoryArray.push(this.getProductInventory());
  }

  /**
   * Deletes version row
   * @param index 
   */
  public deleteVersionRow(index: number): void {
    this.prdoductInfo.productVersion.splice(index, 1);
    this.productVersionArray.removeAt(index);
    this.prdoductInfo.productInventory.splice(index, 1);
    this.productInventoryArray.removeAt(index);
    this.versionId--;

  }

  /**
   * Determines whether new version row is
   * @param index 
   * @returns true if new version row 
   */
  public isNewVersionRow(index: number): boolean {
    return this.prdoductInfo.productVersion.length - 1 == index;
  }

  /**
   * Gets form controls
   */
  public get f() { return this.productForm.controls; }

  /**
   * Determines whether submit on
   * @returns  
   */
  public onSubmit(): boolean {
    event.preventDefault();
    this.submitted = true;
    if (this.productForm.invalid) {
      window.scrollTo(0, 0);
      return;
    }
  }

  /**
   * Determines whether ships alone is
   * @param value 
   */
  public isShipsAlone(value: any): void {
    this.prdoductInfo.shipsAlone_FLG = (value.currentTarget.checked);
  }

  /**
   * Gets selected tags
   * @param data 
   */
  public getSelectedTags(data: Array<number>): void {
    this.prdoductInfo.productTag = [];
    let vaues = data || [];
    vaues.forEach((tagId, index) => {
      this.prdoductInfo.productTag.push({ "productId": this.productId, "tagId": tagId })
    });
  }

  /**
   * Saves product component
   *  
   */
  public save(): void {
    this.submitted = false;
    if (this.productForm.invalid || this.mainVirtualDuplicated.includes(true) || this.dupliateSkuExist.includes(true) || this.upcExist) {
      return;
    } else {
      this.prdoductInfo.productSku.push(
        { "productId": this.prdoductInfo.productId, 
        "sku": this.mainSku, "statusId": 1, 
        "description": "",
         "skuTypeId": AppConstants.MainSKUTypeId 
      });
      this.productService.saveProduct(this.prdoductInfo).subscribe(data => {
        if (data) {
          if (data["productID"] > 0 && data["status"] == true) {
            this.message = data.message;
            this.showProduct = true;
            this.prdoductInfo.productId = data["productID"];
            this.getProductDetailsById();
          } else if (data["status"] == false) {
            console.log(data['message']);
            this.errorMessage = 'Error occurred during ' + this.type + ' Product.'
            this.showErrorPopup = true;
          }
        }
      }, (err) => {
        // Remove Main SKU if error
        this.prdoductInfo.productSku.forEach(sku => {
          if (sku.skuTypeId == AppConstants.MainSKUTypeId)
            this.prdoductInfo.productSku.splice(this.prdoductInfo.productSku.indexOf(sku), 1);
        });
        console.log('Error in creating product', err);
      });
    }
  }


  /**
   * Closes product dialog
   */
  public closeProduct(): void {
    this.type = AppConstants.Edit;
    this.message = '';
    this.showProduct = false;
    this.submitted = false;
  }

  /**
   * Closes error dialog
   */
  public closeError(): void {
    this.errorMessage = '';
    this.showErrorPopup = false;
    this.submitted = false;
  }

  /**
   * Cancels product component
   */
  public cancel(): void {
    this.router.navigate([AppConstants.ProductListUrl]);
  }


  /**
   * Validates upc
   * @param event 
   * @returns boolean 
   */
  public validateUpc(event): boolean {
    if (this.prdoductInfo.upc) {
      this.productService.validateSkuOrUpc(AppConstants.Upc, this.prdoductInfo.upc).subscribe((data) => {
        if (this.type == AppConstants.Edit && this.productId == data['productID'] && data['status']) {
          this.upcExist = false;
        } else if (this.type == AppConstants.Edit && this.productId != data['productID'] && data['status']) {
          this.upcExist = true;
          event.target.focus();
        } else if (data['status'] && (this.type == AppConstants.Add || this.type == AppConstants.Clone)) {
          this.upcExist = true;
          event.target.focus();
          return false;
        } else {
          this.upcExist = false;
          return true;
        }
      });
    } else {
      this.upcExist = false;
      return true;
    }
  }

  /**
   * Validates sku
   * @param event 
   * @param index 
   * @returns  
   */
  public validateSKU(event, index: number): boolean {
    event.preventDefault();
    // Check with Main SKU
    if (this.prdoductInfo.productSku && this.prdoductInfo.productSku.some(sku => sku.sku === this.mainSku) && this.mainSku) {
      this.mainVirtualDuplicated[index] = true;
      event.target.focus();
      return false;
    } else {
      this.mainVirtualDuplicated[index] = false;
    }
    if (index != AppConstants.MainSKUIndex && this.prdoductInfo.productSku.length > 1) { // Check Virtual SKUs duplicated
      this.mainVirtualDuplicated[index] = false;
      let skuArr = this.prdoductInfo.productSku.map(function (item) { return item.sku });
      let isDuplicate = skuArr.some(function (item, idx) {
        if (item)
          return skuArr.indexOf(item) != idx
      });
      if (isDuplicate) {
        this.dupliateSkuExist[index] = true;
        event.target.focus();
        return false;

      } else {
        this.mainVirtualDuplicated[index] = false;
        this.dupliateSkuExist[index] = false;
        return this.checkServerSide(event, index);
      }
    }
    return this.checkServerSide(event, index);
  }
  /**
   * Checks server side duplicate skus
   * @param event 
   * @param index 
   */
  checkServerSide(event, index: number): boolean {
    this.mainVirtualDuplicated[index] = false;
    this.dupliateSkuExist[index] = false;
    let skuToBeValdated = index == AppConstants.MainSKUIndex ? this.mainSku :
      this.prdoductInfo.productSku[index].sku;
    if (skuToBeValdated && skuToBeValdated.trim().length > 0) {
      this.productService.validateSkuOrUpc(AppConstants.Sku, skuToBeValdated)
        .subscribe((res) => {
          if (this.type == AppConstants.Edit && res['status'] && this.productId == res['productID']) {
            this.dupliateSkuExist[index] = false;
            return true;
          } if (this.type == AppConstants.Edit && res['status'] && this.productId != res['productID']) {
            this.dupliateSkuExist[index] = true;
            event.target.focus();
            return false;
          } else if (res['status'] && (this.type == AppConstants.Add || this.type == AppConstants.Clone)) {
            this.dupliateSkuExist[index] = true;
            event.target.focus();
            return false;
          } else {
            this.dupliateSkuExist[index] = false;
            return true;
          }
        });
    } else {
      this.dupliateSkuExist[index] = false;
      return true;
    }
  }

  /**
   * Loads defaults
   */
  private loadDefaults(): void {

    if (this.productId == 0) {
      this.prdoductInfo.productSku = [];
      this.prdoductInfo.productVersion = []
      this.prdoductInfo.productInventory = [];
      let sku = new ProductSKU();
      sku.skuTypeId = 1
      this.prdoductInfo.productSku.push(sku);
      let version = new ProductVersion();
      version.description = AppConstants.DefaultDescription;
      version.statusId = 1;
      version.productVersionId = this.versionId;
      this.prdoductInfo.productVersion.push(version);
      this.productVersionArray.push(this.getProductVersion());
      let inventory = new ProductInventory();
      inventory.productVersionId = this.versionId;
      this.prdoductInfo.productInventory.push(inventory);
      this.productSkuArray.push(this.getProductPoductSkus());
      this.productInventoryArray.push(this.getProductInventory());
    } else if (this.productId > 0) {
      this.getProductDetailsById();
    }

  }
  /**
   * Gets product poduct skus
   * @returns  Poduct Skus FormGroup
   */
  private getProductPoductSkus(): FormGroup {
    return this.formBuilder.group({
      productId: [],
      sku: [null, Validators.required],
      statusId: [null, Validators.required],
      skuTypeId: [null, Validators.required],
      description: []
    });
  }

  /**
   * Gets product version
   * @returns product version FormGroup
   */
  private getProductVersion(): FormGroup {
    return this.formBuilder.group({
      productId: [],
      description: [],
      statusId: [],
      productVersionId: []
    });
  }

  /**
   * Gets product inventory
   * @returns product inventory FormGroup
   */
  private getProductInventory(): FormGroup {
    return this.formBuilder.group({
      productVersionId: [],
      productInventoryId: [],
      warehouseId: [null, Validators.required],
      quantity: [null, Validators.required],
      statusId: [null, Validators.required]
    });
  }
  /**
   * Gets product details by id
   */
  private getProductDetailsById(): void {
    this.productService.getProductDetails(this.prdoductInfo.productId).subscribe(resp => {
      this.prdoductInfo = resp[AppConstants.Data];
      this.refreshData();
    });
  }

  /**
   * Refreshs data
   */
  private refreshData(): void {
    let that = this;
    if (this.prdoductInfo.productTag) {
      this.productTagSelected = [];
      this.prdoductInfo.productTag.forEach(tag => {
        this.productTagSelected.push(tag.tagId);
      });
    }
    if (this.type ===  AppConstants.Clone) {
      this.prdoductInfo.upc = '';
      this.mainSku = '';
      if (this.prdoductInfo.productSku) {
        this.prdoductInfo.productSku.forEach((element, index) => {
          element.productId = that.productId;
          element.description = '';
          element.sku = '';
          element.statusId = undefined;
          this.prdoductInfo.productSku[index] = element;
          (<FormArray>that.productForm.get('productSku')).removeAt(index);
          this.productSkuArray.push(that.getProductPoductSkus());
        });
      }
      if (this.prdoductInfo.productVersion) {
        this.versionId = 0;
        this.prdoductInfo.productVersion.forEach((version, index) => {
          (<FormArray>that.productForm.get('productInventory')).removeAt(index);
          (<FormArray>that.productForm.get('productVersion')).removeAt(index);
          this.productVersionArray.push(that.getProductVersion());
          version.productVersionId = ++this.versionId;
          version.description = '';
          that.prdoductInfo.productVersion[index] = version;
          if (this.prdoductInfo.productInventory) {
            this.prdoductInfo.productInventory[index].productVersionId = this.versionId;
            this.prdoductInfo.productInventory[index].quantity = undefined;
            this.prdoductInfo.productInventory[index].statusId = undefined;
            this.prdoductInfo.productInventory[index].warehouseId = undefined;
            this.productInventoryArray.push(that.getProductInventory())
          }
        });
      }
      this.prdoductInfo.productId = 0;
    }
    if (this.type === AppConstants.Edit) {
      this.mainSku = this.prdoductInfo.mainSKU;
      this.prdoductInfo.productSku.forEach((element, index) => {
        (<FormArray>that.productForm.get('productSku')).removeAt(index);
        this.productSkuArray.push(this.getProductPoductSkus());
      });
      if (this.prdoductInfo.productVersion) {
        this.versionId = 0;
        this.prdoductInfo.productVersion.forEach((version, index) => {
          (<FormArray>that.productForm.get('productInventory')).removeAt(index);
          (<FormArray>that.productForm.get('productVersion')).removeAt(index);
          version.productVersionId = ++this.versionId;
          this.productVersionArray.push(this.getProductVersion());
          if (this.prdoductInfo.productInventory) {
            this.prdoductInfo.productInventory[index].productVersionId = this.versionId;
          }
          this.productInventoryArray.push(this.getProductInventory())
        });
      }
    }
  }
}
