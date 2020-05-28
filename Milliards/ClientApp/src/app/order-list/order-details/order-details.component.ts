import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FullOrder } from '../models/FullOrderList';
import { OrderLine } from '../models/OrderLinesList';
import { OrderDetails } from '../models/OrderDetails';
import { OrderService } from '../services/order.service';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from 'src/app/constants/app.constants';
@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})
export class OrderDetailsComponent implements OnInit {

  public fullfilledOrders: Array<FullOrder> = [];
  public orderLines: Array<OrderLine> = [];
  public orderDetails: OrderDetails;

  private orderId: number;

  constructor(private orderService: OrderService,
    private router: Router,
    private transalteService: TranslateService,
    private route: ActivatedRoute) {
    transalteService.setDefaultLang(AppConstants.DefaultLang);
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.orderId = parseInt(atob(params.orderId));
    });
    if (this.orderId > 0) {
      this.getOrderLines(this.orderId);
      this.getFulfilledOrders(this.orderId);
      this.getOrderDetails(this.orderId);
    } else {
      this.router.navigate([AppConstants.OrderListUrl]);
    }
  }

  /**
     * Closes order details
     */
  public closeOrderDetails(): void {
    this.router.navigate([AppConstants.OrderListUrl]);
  }

  /**
   * Gets order details
   * @param orderId 
   */
  private getOrderDetails(orderId: number): void {
    this.orderService.getOrderDetails(orderId).subscribe((resp) => {
      this.orderDetails = resp[AppConstants.Data] ? resp[AppConstants.Data] : {};
    });
  }

  /**
   * Gets fulfilled orders
   * @param orderId 
   */
  private getFulfilledOrders(orderId: number): void {
    this.orderService.getFulfilledOrders(orderId).subscribe((resp) => {
      this.fullfilledOrders = [];
      this.fullfilledOrders = resp[AppConstants.Data];
    });
  }

  /**
   * Gets order lines
   * @param orderId 
   */
  private getOrderLines(orderId: number): void {
    this.orderService.getOrderLines(orderId).subscribe((resp) => {
      this.orderLines = [];
      this.orderLines = resp[AppConstants.Data];
    });
  }
}
