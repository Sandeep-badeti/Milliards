import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProductView } from '../models/addproduct';
import { ProductService } from '../services/product.service';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from 'src/app/constants/app.constants';

@Component({
  selector: 'app-view-product',
  templateUrl: './view-product.component.html',
  styleUrls: ['./view-product.component.css']
})
export class ViewProductComponent implements OnInit {

  public productDetails: ProductView;
  public tags: string = '';

  private productId: number;

  constructor(private productSservice: ProductService,
    private router: Router,
    private transalteService: TranslateService,
    private route: ActivatedRoute) {
    transalteService.setDefaultLang(AppConstants.DefaultLang);
  }

  ngOnInit() {
    this.productDetails = new ProductView();
    this.route.queryParams.subscribe(params => {
      this.productId = parseInt(atob(params.productId));
    });
    if (this.productId > 0) {
      this.getProductDetails(this.productId)
    } else {
      this.router.navigate([AppConstants.ProductListUrl]);
    }
  }

  /**
   *  Navigates to Edits product
   */
  public editProduct(): void {
    this.router.navigate([AppConstants.ProductEdttUrl], { queryParams: { productId: btoa((this.productDetails.productId).toString()), type: btoa(AppConstants.Edit) } });
  }

  /**
   * Navigates to Clones product
   */
  public cloneProduct(): void {
    this.router.navigate([AppConstants.ProductCloneUrl], { queryParams: { productId: btoa((this.productDetails.productId).toString()), type: btoa(AppConstants.Clone) } });
  }

  /**
   * Gets product details
   * @param productId 
   */
  private getProductDetails(productId: number): void {
    this.productSservice.getProductView(productId).subscribe((resp) => {
      this.productDetails = resp[AppConstants.Data];
      let tag = '';
      if (this.productDetails.productTag)
        this.productDetails.productTag.forEach((tagObj) => {
          if (tagObj.tagName)
            tag += tagObj.tagName + ',';
        })
      this.tags = tag.substr(0, tag.length - 1);
    });
  }
}
