import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AssignComponent } from '../assign/assign.component';
import { FulOrder } from '../models/fullorderlist';
import {  Types } from '../../product-list/models/metadata';
import { DataTableDirective } from 'angular-datatables';
import { FullOrderService } from '../services/full-order.service';
import { ProductService } from '../../product-list/services/product.service';
import { AppConstants, ModeEnum } from 'src/app/constants/app.constants';
import { TranslateService } from '@ngx-translate/core';
import { WareHouse } from '../models/meta-data-models';
import { Subject } from 'rxjs/internal/Subject';
import { LabellingDialogComponent } from '../labelling-dialog/labelling-dialog.component';
import { PalletizingDialogComponent } from '../palletizing-dialog/palletizing-dialog.component';
import { ExitScanDialogComponent } from '../exit-scan-dialog/exit-scan-dialog.component';
declare var $: any

@Component({
  selector: 'app-fulorder-list',
  templateUrl: './fulorder-list.component.html',
  styleUrls: ['./fulorder-list.component.css']
})
export class FulorderListComponent implements OnInit, AfterViewInit {
  public dtOptions: DataTables.Settings = {};
  public totalRecords: number;
  public fulOrderList: FulOrder[];
  public selectedMode: string = AppConstants.ModeRegular;
  public wareHouseList: WareHouse[];
  public types: Types;
  public isPickingMode: boolean;
  public selectedWarehouseId: number = 0;
  public masterSelected: boolean = false;
  public dtTrigger: Subject<any> = new Subject();
  public message: string = '';
  public showPickConfirmation: boolean;

  @ViewChild(DataTableDirective, { static: true })
  dtElement: DataTableDirective;
  constructor(private router: Router, private route: ActivatedRoute,
    private fulOrderService: FullOrderService,
    private productService: ProductService,
    private dialog: MatDialog,
    private transalteService: TranslateService) {
      this.router.routeReuseStrategy.shouldReuseRoute=()=>{
        return false;
      };
      this.router.events.subscribe((event)=>{
        if(event instanceof NavigationEnd) {
           this.router.navigated  = false;
           window.scrollTo(0,0);
        }
      });
    transalteService.setDefaultLang(AppConstants.DefaultLang);
    this.totalRecords = 0;
    this.fulOrderList = [];

    this.productService.getProductMetaData().subscribe((types) => {
      this.types = types;
    });
  }

  ngOnInit() {
    this.getFulOrdersList();
  }
  ngAfterViewInit(): void {
    this.dtTrigger.next();
  }
  /**
   * Opens assign dialog
   * @param fullOrder 
   */
  public openAssignDialog(fulOrder): void {
    const dialogConfig = {
      disableClose: true,
      autoFocus: true,
      width: '90%',
      data: {
        fullOrderId: fulOrder.fullOrderId,
        wareouseList: this.types.warehouseList,
        carrierList: this.types.carrierList
      }
    }
    const dialogRef = this.dialog.open(AssignComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      (data) => {
        this.refreshGrid();
      }
    );
  }

  /**
   * Views fulfilnet order
   */
  public viewFulOrder(fulOrder: FulOrder): void {
    this.router.navigate([AppConstants.FulOrderViewUrl], { queryParams: { fullOrderId: btoa((fulOrder.fullOrderId).toString()) } });

  }

  /**
   * All full orders
   * @param event 
   */
  public allFullOrders(event) {
    const checked = event.target.checked;
    this.fulOrderList.forEach(item => item['selected'] = checked);
  }

  /**
   * Determines whether all selected is
   * @param index 
   */
  public isAllSelected(index: number): void {
    let that = this;
    that.fulOrderList[index]['selected'] = !(that.fulOrderList[index]['selected']);
    for (let i = 0; i < that.fulOrderList.length; i++) {
      let item = that.fulOrderList[i];
      if (!item['selected']) {
        that.masterSelected = false;
        break;
      } else {
        that.masterSelected = true;
      }

    }
  }

  /**
   * Determines whether mode change on
   */
  public onModeChange(): void {
    this.selectedWarehouseId = 0;
    this.masterSelected = false;
    if (this.selectedMode == ModeEnum.Picking) {
      this.isPickingMode = true;
      this.refreshGrid();
    } else if (this.selectedMode == ModeEnum.Labelling) {
      this.isPickingMode = false;
      this.openLabellingDialog();
    } else if (this.selectedMode == ModeEnum.Palletizing) {
      this.isPickingMode = false;
      this.openPalletizingDialog();
    } else if (this.selectedMode == ModeEnum.ExitScan) {
      this.isPickingMode = false;
      this.openExitScanDialog();
    } else if (this.selectedMode == ModeEnum.Regular || this.selectedMode == ModeEnum.Assigning) {
      this.isPickingMode = false;
      this.refreshGrid();
    } else {
      this.isPickingMode = false;
    }

  }

  /**
   * Determines whether warehouse change on
   */
  public onWarehouseChange(): void {
    this.refreshGrid();
  }

  public createPickList(): void {
    let selectedFulOrderIds = [];
    this.fulOrderList.forEach((fulOrder: FulOrder) => {
      if (fulOrder['selected'])
        selectedFulOrderIds.push(fulOrder.fullOrderId);
    });
    if (selectedFulOrderIds.length == 0) {
      this.message = AppConstants.selectRecsForPick;
      this.showPickConfirmation = true;
      return;
    } else if (this.selectedWarehouseId == 0) {
      this.message = AppConstants.selectwarehouseForPick;
      this.showPickConfirmation = true;
      return;
    } else {
      this.fulOrderService.createPickList(selectedFulOrderIds).subscribe((data) => {
        if (data['status']) {
          this.message = data['message'];
          this.showPickConfirmation = true;
        }
        else
          console.log("Error", data['message']);
      }, (error) => {
        console.log("Error", error);
      });
    }
  }

  /**
   * Closes pick confirmation dialog
   */
  public closePickConfirmationDialog(): void {
    this.showPickConfirmation = false;
    if (this.message != AppConstants.selectRecsForPick
      && this.message != AppConstants.selectwarehouseForPick)
      this.refreshGrid();
    this.message = '';

  }

  /**
   * Opens labelling dialog
   */
  private openLabellingDialog(): void {
    const dialogConfig = {
      disableClose: true,
      autoFocus: true,
      width: '90%',
      height: '100%',
      data: {

      }
    }
    const dialogRef = this.dialog.open(LabellingDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      (data) => {
        if (data && data['fulOrderId'] > 0) {
          this.router.navigate([AppConstants.FulOrderViewUrl], { queryParams: { fullOrderId: btoa((data['fulOrderId']).toString()) } });
        } else {
          this.selectedMode = ModeEnum.Regular;
          this.refreshGrid();
        }
      }
    );
  }

  /**
  * Opens labelling dialog
  */
  private openPalletizingDialog(): void {
    const dialogConfig = {
      disableClose: true,
      autoFocus: true,
      width: '90%',
      height: '100%',
      data: {

      }
    }
    const dialogRef = this.dialog.open(PalletizingDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      (data) => {
        if (data && data['fulOrderId'] > 0) {
          this.router.navigate([AppConstants.FulOrderViewUrl], { queryParams: { fullOrderId: btoa((data['fulOrderId']).toString()) } });
        } else {
          this.selectedMode = ModeEnum.Regular;
          this.refreshGrid();
        }
      }
    );
  }

  /**
   * Opens exit scan dialog
   */
  private openExitScanDialog(): void {
    const dialogConfig = {
      disableClose: true,
      autoFocus: true,
      width: '90%',
      height: '100%',
      data: {

      }
    }
    const dialogRef = this.dialog.open(ExitScanDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(
      (data) => {
        if (data && data['fulOrderId'] > 0) {
          this.router.navigate([AppConstants.FulOrderViewUrl], { queryParams: { fullOrderId: btoa((data['fulOrderId']).toString()) } });
        } else {
          this.selectedMode = ModeEnum.Regular;
          this.refreshGrid();
        }
      }
    );
  }

  /**
   * Gets ful orders list
   */
  private getFulOrdersList(): void {
    const that = this;
    this.dtOptions = {
      pagingType: AppConstants.PagingTypeFull,
      pageLength: AppConstants.PageLength,
      serverSide: true,
      processing: true,
      retrieve: true,
      order: [[1, AppConstants.Desc]],
      language: {
        searchPlaceholder: AppConstants.SrchFullOrderList
      },
      ajax: (dataTablesParameters: any, callback) => {
        that.isPickingMode ? dataTablesParameters.length = -1 : AppConstants.PageLength
        that.fulOrderService.getFullOrdersList(dataTablesParameters, that.selectedMode, that.selectedWarehouseId).subscribe(resp => {
          that.totalRecords = resp.totalRecordsCount;
          that.fulOrderList = resp[AppConstants.Data];
          callback({
            recordsTotal: resp.totalRecordsCount,
            recordsFiltered: resp.totalRecordsCount,
            data: []
          });
        });
      },
      columns: AppConstants.FulOrderListColumns
    };

  }

  /**
   * Refreshs grid
   */
  private refreshGrid(): void {
    this.getFulOrdersList();
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
      dtInstance.clear();
      if (this.isPickingMode) {
        $('#fulOrderTable_paginate').css('display', 'none');
        dtInstance.draw(false);
      } else {
        $('#fulOrderTable_paginate').css('display', 'block');
        dtInstance.draw(true);
      }
      this.dtTrigger.next();
    });
  }

}
