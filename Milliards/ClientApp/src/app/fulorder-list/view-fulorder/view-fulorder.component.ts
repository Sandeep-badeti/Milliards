import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from 'src/app/constants/app.constants';
import { FullOrderService } from '../services/full-order.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-view-fulorder',
  templateUrl: './view-fulorder.component.html',
  styleUrls: ['./view-fulorder.component.css']
})
export class ViewFulorderComponent implements OnInit {

  fulOrderDetails: any = {}

  private fulOrderId: number;

  constructor(private fullOrderService: FullOrderService,
    private router: Router,
    private transalteService: TranslateService,
    private route: ActivatedRoute) {
    transalteService.setDefaultLang(AppConstants.DefaultLang);
  }

  ngOnInit() {
    this.fulOrderDetails = {};
    this.route.queryParams.subscribe(params => {

      this.fulOrderId = parseInt(atob(params.fullOrderId));
    });
    if (this.fulOrderId > 0) {
      this.getFulOrderVew(this.fulOrderId);
    } else {
      this.router.navigate([AppConstants.FulOrderListUrl]);
    }
  }
  /**
   * Closes ful order details
   */
  public closeFulOrderDetails(): void {
    this.router.navigate([AppConstants.FulOrderListUrl]);
  }

  /**
   * Gets ful order vew
   * @param fulOrderId 
   */
  private getFulOrderVew(fulOrderId: number): void {
    this.fullOrderService.getFulOrderVew(fulOrderId).subscribe((resp) => {
      this.fulOrderDetails = resp;
    });
  }
}
