import { Component, OnInit, Inject, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FullOrderService } from '../services/full-order.service';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from 'src/app/constants/app.constants';
import { Pallet, FulFillmentOrder, FullOrerListByPallet } from '../models/fullorderlist';
import { Subject } from 'rxjs/internal/Subject';
import { DataTableDirective } from 'angular-datatables';
declare var $: any

@Component({
  selector: 'app-exit-scan-dialog',
  templateUrl: './exit-scan-dialog.component.html',
  styleUrls: ['./exit-scan-dialog.component.css']
})
export class ExitScanDialogComponent implements OnInit, AfterViewInit, OnDestroy {
  public palletId: number = 0;
  public palletList: Pallet[];
  public dtOptions: DataTables.Settings = {};
  public dtTrigger: Subject<any> = new Subject();
  public fulOrderList: FulFillmentOrder[];
  public palletErrorMsg: string = '';
  public message: string;
  public showDialog: boolean;
  public isInitialLoad: boolean;
  public isExitScanDisabled: boolean;
  public msg: string;
  public showMsg: boolean;
  @ViewChild(DataTableDirective, { static: false })
  private datatableElement: DataTableDirective;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fulOrderService: FullOrderService,
    private dialogRef: MatDialogRef<ExitScanDialogComponent>,
    private transalteService: TranslateService
  ) {
    transalteService.setDefaultLang(AppConstants.DefaultLang);
  }

  ngOnInit() {
    this.palletList = [];
    this.getPalletList();
    this.isInitialLoad = true;
    this.getFulOrdersList();
  }

  ngAfterViewInit(): void {
    this.dtTrigger.next();
  }

  ngOnDestroy() {
    this.datatableElement.dtInstance.then((instance: DataTables.Api) => {
      instance.destroy();
    });
    this.dtTrigger.unsubscribe();
  }

  /**
  * Closes dialog
  */
  public closeDialog(): void {
    this.dialogRef.close();
  }

  /**
   * Determines whether pallet change on
   * @param event 
   */
  public onPalletChange(event): void {
    this.palletErrorMsg = '';
    this.isInitialLoad = false;
    if (this.palletId == 0) {
      this.palletErrorMsg = AppConstants.SelectPalletId;
    } else {
      this.getFulOrdersList();
      this.refreshGrid();
      this.isExitScanDisabled = false;
    }

  }

  /**
   * Views fulfilnet order
   */
  public viewFulOrder(fulOrderId: number): void {
    this.dialogRef.close({ "fulOrderId": fulOrderId });
  }

  /**
   * Exits scan
   */
  public exitScan(): void {
    if (this.fulOrderList.length == 0 || this.palletId == 0) {
      this.palletErrorMsg = AppConstants.SelectPalletId;
      return;
    }
    this.fulOrderService.exitScan(this.palletId, Array.from(this.fulOrderList, x => x.fulOrderId)).subscribe(
      (resp: FullOrerListByPallet) => {
        if (resp.status) {
          this.getPalletList();
          $("#popup-table").DataTable().ajax.reload();
          this.msg = resp.message;
          this.showMsg = true;
          this.isExitScanDisabled = true;

        }
      },
      (error: Error) => {
        console.log('Error', error);
      }
    );
  }

  /**
  * Closes msg dialog
  */
  public closeMsgDialog(): void {
    this.showDialog = false;
    this.message = '';
  }


  /**
   * Closes confirm msg dialog
   */
  public closeConfirmMsgDialog(): void {
    this.showMsg = false;
    this.msg = '';
  }

  /**
   * Gets ful orders list
   */
  private getFulOrdersList(): void {
    const that = this;
    this.dtOptions = {
      paging: false,
      serverSide: true,
      processing: true,
      retrieve: true,
      language: {
        searchPlaceholder: AppConstants.SrchFullOrderForExitScan
      },
      ajax: (dataTablesParameters: any, callback) => {
        if (that.isInitialLoad) {
          callback({
            recordsTotal: 0,
            recordsFiltered: 0,
            data: []
          });
        } else {
          that.fulOrderService.getFullOrdersListForExitScan(dataTablesParameters, that.palletId).subscribe((resp: FullOrerListByPallet) => {
            if (resp.status) {
              that.fulOrderList = JSON.parse(JSON.stringify(resp.data));
            }
            that.message = resp.message;
            (that.fulOrderList.length == 0) ? that.showDialog = true : that.showDialog = false;
            callback({
              recordsTotal: that.fulOrderList.length,
              recordsFiltered: that.fulOrderList.length,
              data: []
            });
          });
        }

      },
      columns: AppConstants.FulOrderListForExitScanColumns
    }

  }

  /**
   * Gets pallet list
   */
  private getPalletList(): void {
    this.fulOrderService.getPalletList().subscribe((data) => {
      this.palletList = [];
      if (data['status']) {
        this.palletList = data[AppConstants.Data]
      } else {
        console.log('Error')
      }
    }, (error: Error) => {
      console.log("Error", error);
    });
  }

  /**
   * Refreshes grid
   */
  private refreshGrid(): void {
    this.datatableElement.dtInstance.then((instance: DataTables.Api) => {
      instance.clear().destroy();
    });
    this.dtTrigger.next();
  }
}
