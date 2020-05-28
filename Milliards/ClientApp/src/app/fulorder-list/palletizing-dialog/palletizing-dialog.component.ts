import { Component, OnInit, Inject } from '@angular/core';
import { FulOrderDetails, CarrierServiceAndType, CarrierServiceAndTypeResp, CreatePalletResp, CarrierServiceAndTypeResponse } from '../models/fullorderlist';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FullOrderService } from '../services/full-order.service';
import { AppConstants } from 'src/app/constants/app.constants';
import { TranslateService } from '@ngx-translate/core';
import { error } from 'protractor';

@Component({
  selector: 'app-palletizing-dialog',
  templateUrl: './palletizing-dialog.component.html',
  styleUrls: ['./palletizing-dialog.component.css']
})
export class PalletizingDialogComponent implements OnInit {
  public fulfilmentDetails: FulOrderDetails;
  public message: string;
  public trackingNumber: string;
  public showDialog: boolean;
  public palletError: string;
  public palletList: [];
  public carrierObj: string = '';
  public palletId: number = 0;
  public carrierObjError: string;
  public trackingNumError: string;
  public carrierAndServiceType: CarrierServiceAndType[];
  public disableCreatePallet: boolean;
  public isSelected: boolean;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    private fulOrderService: FullOrderService,
    private dialogRef: MatDialogRef<PalletizingDialogComponent>,
    private transalteService: TranslateService) {
    transalteService.setDefaultLang(AppConstants.DefaultLang);
    this.fulfilmentDetails = new FulOrderDetails();
  }

  ngOnInit() {
    this.getPalletList();
    this.getCarrierAndServiceTypes();
  }

  /**
  * Fo details
  */
  public foDetails(): void {
    this.dialogRef.close({ "fulOrderId": this.fulfilmentDetails.data.fulOrderId });
  }

  /**
   * Gets carrier service info by pallet
   */
  public getCarrierServiceInfoByPallet(): void {
    if (this.palletId == 0) {
      this.disableCreatePallet = false;
      this.carrierObj = '';
      this.carrierObjError = '';
    } else {

      this.fulOrderService.getCarrierServiceInfoByPallet(this.palletId)
        .subscribe((resp: CarrierServiceAndTypeResponse) => {
          if (resp.status) {
            this.isSelected = this.carrierAndServiceType.indexOf(resp.data) >= 0;
            this.carrierObj = resp.data.carrierName;
            this.disableCreatePallet = true;
          }
        }, (error: Error) => {
          this.disableCreatePallet = false;
          console.log("Error", error);
        });
    }

  }

  /**
   * Creates pallet
   */
  public createPallet(): void {
    this.resetValues();
    this.trackingNumber = '';
    let obj;
    for(let i=0;i<this.carrierAndServiceType.length;i++) {
      if (this.carrierAndServiceType[i].carrierName == this.carrierObj) {
        obj = this.carrierAndServiceType[i];
        break;
      }
    }
    if (obj) {
      this.fulOrderService.createPallet(obj).subscribe((resp: CreatePalletResp) => {
        if (resp.status) {
          this.getPalletList();
          this.palletId = resp.palletId;
        } else {
          console.log('Error');
        }
        this.message = resp.message;
        this.showDialog = true;
      }, (error: Error) => {
        console.log("Error", error);
      });
    } else {
      this.carrierObjError = AppConstants.SelectCarrierAndService;
    }
  }

  /**
   * Palletizes palletizing dialog component
   * @param event 
   * @returns palletize 
   */
  public palletize(event): void {
    this.resetValues();
    if (this.palletId == 0) {
      this.palletError = AppConstants.SelectPalletId;
      return;
    } else if (!this.trackingNumber || this.trackingNumber.trim().length == 0) {
      this.trackingNumError = AppConstants.EnterTrackingNumber;
      return;
    } else {
      this.fulOrderService.pallatize(this.trackingNumber, this.palletId).subscribe((resp: FulOrderDetails) => {
        let serverResp = JSON.parse(JSON.stringify(resp));
        this.message = serverResp.message;
        this.fulfilmentDetails = serverResp;
        if (serverResp.status) {
          this.resetValues();
        }
        else {
          this.fulfilmentDetails = null;
        }
        this.trackingNumber = '';
        this.showDialog = true;
      });
    }
  }


  /**
   * Closes dialog
   */
  public closeDialog(): void {
    this.dialogRef.close();
  }

  /**
   * Closes msg dialog
   */
  public closeMsgDialog(): void {
    this.showDialog = false;
    this.trackingNumber = '';
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
   * Gets carrier and service types
   */
  private getCarrierAndServiceTypes(): void {
    this.fulOrderService.getCarrierAndServiceTypes().subscribe((data: CarrierServiceAndTypeResp) => {
      this.carrierAndServiceType = [];
      if (data.status) {
        this.carrierAndServiceType = data[AppConstants.Data]
      } else {
        console.log('Error')
      }
    }, (error: Error) => {
      console.log("Error", error);
    });
  }

  /**
   * Resets values
   */
  private resetValues() {
    this.palletError = '';
    this.trackingNumError = '';
    this.carrierObjError = '';
  }

}
