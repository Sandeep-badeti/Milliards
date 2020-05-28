import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FullOrderService } from '../services/full-order.service';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from 'src/app/constants/app.constants';
import { FulOrderDetails } from '../models/fullorderlist';

@Component({
  selector: 'app-labelling-dialog',
  templateUrl: './labelling-dialog.component.html',
  styleUrls: ['./labelling-dialog.component.css']
})
export class LabellingDialogComponent implements OnInit {
  public fulfilmentDetails: FulOrderDetails;
  public message: string;
  public trackingNumber: string;
  public upc: string;
  public showDialog: boolean;
  public labellingError: string;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fulOrderService: FullOrderService,
    private dialogRef: MatDialogRef<LabellingDialogComponent>,
    private transalteService: TranslateService,
  ) {

    transalteService.setDefaultLang(AppConstants.DefaultLang);
    this.fulfilmentDetails = new FulOrderDetails();
  }

  ngOnInit() {

  }

  /**
   * Determines whether upc enter on
   * @param event 
   */
  public onUpcEnter(event): void {
    this.fulOrderService.generateLabel(this.upc).subscribe((resp: FulOrderDetails) => {
      let serverResp = JSON.parse(JSON.stringify(resp));
      this.fulfilmentDetails = serverResp;
      if (serverResp.status) {
        this.trackingNumber = serverResp.data ? serverResp.data.trackingNumber : '';
      } else {
        this.labellingError = serverResp.message;
      }
    });
  }

  /**
   * Creates package
   */
  public createPackage(): void {
    this.fulOrderService.createPackage(this.fulfilmentDetails.data.fulOrderId)
      .subscribe((resp: FulOrderDetails) => {
        let serverResp = JSON.parse(JSON.stringify(resp));
        this.message = serverResp.message;
        this.fulfilmentDetails = serverResp;
        if (serverResp.status) {
          this.trackingNumber = '';
          this.upc = '';
        }
        else {
          this.fulfilmentDetails = null;
        }
        this.showDialog = true;
      });
  }

  /**
   * Fo details
   */
  public foDetails(): void {
    this.dialogRef.close({ "fulOrderId": this.fulfilmentDetails.data.fulOrderId });
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
    this.upc = '';
  }

}
