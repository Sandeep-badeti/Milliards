import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarrierService, Carrier, WareHouse } from '../models/meta-data-models';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProductService } from '../../product-list/services/product.service';
import { AssignRequest } from '../models/assign-request';
import { FullOrderService } from '../services/full-order.service';
import {AppConstants} from '../../constants/app.constants';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-assign',
  templateUrl: './assign.component.html',
  styleUrls: ['./assign.component.css']
})

export class AssignComponent implements OnInit {
  public wareouseList: WareHouse[] = [];
  public carrierList: Carrier[] = [];
  public carrierServiceList: CarrierService[] = [];
  public selectedWarehouse: number;
  public selectedCarrier: number;
  public selectedCarrierService: number;
  public fulOrderId: number;
  public submitted: boolean;
  public assignForm: FormGroup;
  public readonly constants = AppConstants;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fulOrderService: FullOrderService,
    private productService: ProductService,
    private dialogRef: MatDialogRef<AssignComponent>,
    private formBuilder: FormBuilder,
    private transalteService : TranslateService,
    ) {
      transalteService.setDefaultLang(AppConstants.DefaultLang);
    this.assignForm = this.formBuilder.group({
      warehouseId: ['', Validators.required],
      carrierId: ['', Validators.required],
      carrierServiceId: ['', Validators.required]
    });

  }

  ngOnInit(): void {
    this.fulOrderId = this.data.fullOrderId;
    this.wareouseList = this.data.wareouseList;
    this.carrierList = this.data.carrierList;
  }

  /**
   * Gets form controls
   */
  public get f() { return this.assignForm.controls; }

  /**
   * Gets carrier service list
   * 
   */
  public getCarrierServiceList(): void {
    if (this.selectedCarrier && this.selectedCarrier > 0) {
      this.fulOrderService.getCarrierServiceList(this.selectedCarrier)
        .subscribe((data) => {
          if (data) {
            this.carrierServiceList = [];
            this.carrierServiceList = data['carrierService'];
          } else {
            console.log("Data is not available")
          }
        }, (error) => {
          console.log("Error", error);
        });
    }
  }

  /**
   * Determines whether submit on
   * @returns  
   */
  public onSubmit(): boolean {
    event.preventDefault();
    this.submitted = true;
    if (this.assignForm.invalid) {
      return;
    }
  }

  /**
   * Assgins details
   */
  public assgin(): void {
    this.submitted = false;
    if (this.fulOrderId && this.selectedWarehouse &&
      this.selectedCarrier && this.selectedCarrierService) {
      let req: AssignRequest = {
        "fulOrderId": this.fulOrderId,
        "warehouseId": this.selectedWarehouse,
        "carrierId": this.selectedCarrier,
        "carrierServiceId": this.selectedCarrierService
      };
      this.fulOrderService.assgin(req).subscribe(
        (data) => {
          if (data) {
            this.dialogRef.close();
          } else {
            console.log("Error");
          }
        }, (error) => {
          console.log("Error", error);
        });
    }
  }
}
