import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs/internal/Subject';
import { Product } from '../models/productList';
import { ProductService } from '../services/product.service';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from 'src/app/constants/app.constants';
declare var $: any
@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  public dtOptions: DataTables.Settings = {};
  public totalRecords: number;
  public productList: any = [];

  private dtTrigger: Subject<any> = new Subject();
  @ViewChild(DataTableDirective, { static: true })
  private dtElement: DataTableDirective;

  constructor(private router: Router,
    private transalteService: TranslateService,
    private productService: ProductService) {
    transalteService.setDefaultLang(AppConstants.DefaultLang);
  }

  ngOnInit() {
    this.getProductList();
  }

  /**
   * Navigates to Edits product
   * @param product 
   */
  public editProduct(product: Product): void {
    this.router.navigate([AppConstants.ProductEdttUrl], { queryParams: { productId: btoa((product.productId).toString()), type: btoa(AppConstants.Edit) } });
  }

  /**
   * Navigates to Adds product
   */
  public addProduct(): void {
    this.router.navigate([AppConstants.ProductAddUrl], { queryParams: { productId: btoa((0).toString()), type: btoa(AppConstants.Add) } });
  }

  /**
   * Navigates to  Clones product
   * @param product 
   */
  public cloneProduct(product: Product): void {
    this.router.navigate([AppConstants.ProductCloneUrl], { queryParams: { productId: btoa((product.productId).toString()), type: btoa(AppConstants.Clone) } });
  }

  /**
   * Navigates to Views product
   * @param product 
   */
  public viewProduct(product: Product): void {
    this.router.navigate([AppConstants.ProductviewUrl], { queryParams: { productId: btoa((product.productId).toString()), type: btoa(AppConstants.View) } });
  }

  /**
 * Gets product list
 */
  private getProductList(): void {
    let that = this;
    this.dtOptions = {
      pagingType: AppConstants.PagingTypeFull,
      pageLength: AppConstants.PageLength,
      serverSide: true,
      processing: true,
      language: {
        searchPlaceholder: AppConstants.searchProductPlaceholder
      },
      ajax: (dataTablesParameters: any, callback) => {
        that.productService.getProductList(dataTablesParameters).subscribe(resp => {
          that.totalRecords = resp.totalRecordsCount;
          that.productList = resp[AppConstants.Data]
          callback({
            recordsTotal: resp.totalRecordsCount,
            recordsFiltered: resp.totalRecordsCount,
            data: []
          });
        }, (error) => {
          console.log("Error", error)
        });
      },
      columns: AppConstants.ProductListColumns
    }
  }
}
