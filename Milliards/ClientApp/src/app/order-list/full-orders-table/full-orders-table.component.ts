import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from 'src/app/constants/app.constants';
@Component({
  selector: 'app-full-orders-table',
  templateUrl: './full-orders-table.component.html',
  styleUrls: ['./full-orders-table.component.css']
})
export class FullOrdersTableComponent implements OnInit, OnChanges {

  ngOnChanges(changes: SimpleChanges): void {
    this.fullfilledOrders = this.fullOrdersList;
  }

  @Input()
  fullOrdersList: any[];
  fullfilledOrders: any[] = [];
  constructor(private transalteService : TranslateService) {
    transalteService.setDefaultLang(AppConstants.DefaultLang);
   }

  ngOnInit() {
    this.fullfilledOrders = this.fullOrdersList;
  }
  

}
