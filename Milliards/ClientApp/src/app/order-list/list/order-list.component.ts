import { Component, OnInit, HostListener, ViewChild, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { Order } from '../models/OrderList';
import * as moment from 'moment';
import { OrderService } from '../services/order.service';
import { AppConstants } from 'src/app/constants/app.constants';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit, OnDestroy {

  public dtOptions: DataTables.Settings = {};
  public totalRecords: number;
  public orderList: any = [];
  public toDate: string = '';
  public fromDate: string = '';
  public searchText: string = "";
  public orderLines: any[] = [];
  public fullfilledOrders: any[] = [];
  public showSearch: boolean = false;
  public baseurl: string = AppConstants.baseUrl;
  public imageUrl: string;

  private dtTrigger: Subject<any> = new Subject();
  @ViewChild(DataTableDirective, { static: true })
  private dtElement: DataTableDirective;
  private fetchType: string[] = [];

  constructor(private router: Router,
    private route: ActivatedRoute,
    private transalteService: TranslateService,
    private orderService: OrderService) {
    transalteService.setDefaultLang(AppConstants.DefaultLang);
  }

  ngOnInit() {
    this.imageUrl = this.baseurl + AppConstants.UpArrow;
    this.orderLines = [];
    this.fullfilledOrders = [];
    this.getOrdertList();
  }

  /**
   * Orders details
   * @param order 
   * @param index 
   */
  public orderDetails(order: Order, index: number): void {
    this.router.navigate(['../orderDetails'], { relativeTo: this.route, queryParams: { orderId: btoa((order.orderId).toString()) } });
  }

  /**
   * Shows more
   * @param order 
   * @param index 
   */
  public showMore(order: Order, index: number): void {
    if (order[AppConstants.ShowMore]) {
      order[AppConstants.ShowMore] = false;
    } else {
      this.fetchType[index] = AppConstants.FetchType;
      this.getOrderLines(order.orderId, index);
      order[AppConstants.ShowMore] = true;
    }
  }

  /**
   * Selects type
   * @param type 
   * @param order 
   * @param index 
   */
  public selectType(type: string, order: Order, index: number): void {
    this.fetchType[index] = type;
    if (type == AppConstants.FetchType) {
      this.getOrderLines(order.orderId, index);
    } else {
      this.getFulfilledOrders(order.orderId, index);
    }
  }

  /**
   * Determines whether selected is
   * @param name 
   * @param index 
   * @returns true if selected 
   */
  public isSelected(name: string, index): boolean {
    return (this.fetchType[index] === name);
  }

  /**
   * Searchs orders by date
   */
  public searchOrdersByDate(): void {
    let config = {
      order: [{ column: 7, dir: AppConstants.Asc }],
      start: 0,
      length: AppConstants.PageLength,
      columns: AppConstants.orderListColums

    }
    this.searchOrders(config);
  }

  /**
   * Globals search orders
   */
  public globalSearchOrders(): void {
    let config = {
      order: [{ column: 7, dir: AppConstants.Desc }],
      start: 0,
      length: AppConstants.PageLength,
      columns: AppConstants.orderListColums

    }
    this.searchOrders(config);
  }

  /**
   * Searchs orders
   * @param config 
   */
  public searchOrders(config: any): void {
    this.orderList = [];
    this.totalRecords = 0;
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.clear();

      this.orderService.getOrderList(config, (this.fromDate != null && this.fromDate != '') ? moment(this.fromDate).format(AppConstants.MMDDYYYY) : '',
        (this.toDate != null && this.toDate != '') ? moment(this.toDate).format(AppConstants.MMDDYYYY) : '', this.searchText).subscribe(resp => {
          this.totalRecords = resp.totalRecordsCount;
          this.orderList = resp[AppConstants.Data];
          dtInstance.search(this.searchText).draw();
        });
      this.dtTrigger.next();

    });
  }


  /**
   * Toggles search
   */
  public toggleSearch(): void {
    if (this.showSearch) {
      this.imageUrl = this.baseurl + AppConstants.UpArrow;
    } else {
      this.imageUrl = this.baseurl + AppConstants.DownArrow;

    }
    this.showSearch = !this.showSearch;
  }

  /**
   * Gets ordert list
   */
  private getOrdertList(): void {
    const that = this;

    this.dtOptions = {
      pagingType: AppConstants.PagingTypeFull,
      pageLength: AppConstants.PageLength,
      serverSide: true,
      processing: true,
      language: {
        searchPlaceholder: AppConstants.SrchOrderListPlaceHolder
      },
      searching: false,
      order: [[7, AppConstants.Desc]],
      ajax: (dataTablesParameters: any, callback) => {

        that.orderService.getOrderList(dataTablesParameters, (that.fromDate != null && that.fromDate != '') ? moment(that.fromDate).format(AppConstants.MMDDYYYY) : '', (that.toDate != null && that.toDate != '') ? moment(that.toDate).format(AppConstants.MMDDYYYY) : '', that.searchText).subscribe(resp => {
          that.totalRecords = resp.totalRecordsCount;
          that.orderList = resp[AppConstants.Data]
          callback({
            recordsTotal: resp.totalRecordsCount,
            recordsFiltered: resp.totalRecordsCount,
            data: []
          });
        });
      },
      columns: AppConstants.orderListColums
    }
  }

  /**
   * Gets fulfilled orders
   * @param orderId 
   * @param index 
   */
  private getFulfilledOrders(orderId: number, index: number): void {

    this.orderService.getFulfilledOrders(orderId).subscribe((resp) => {
      this.fullfilledOrders[index] = [];
      this.fullfilledOrders[index] = resp[AppConstants.Data];
    });

  }

  /**
   * Gets order lines
   * @param orderId 
   * @param index 
   */
  private getOrderLines(orderId: number, index: number): void {
    this.orderService.getOrderLines(orderId).subscribe((resp) => {
      this.orderLines[index] = [];
      this.orderLines[index] = resp[AppConstants.Data];

    });
  }

  ngOnDestroy(): void {
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.clear();
      dtInstance.destroy();
    });
  }

}
