import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-order-line-table',
  templateUrl: './order-line-table.component.html',
  styleUrls: ['./order-line-table.component.css']
})

export class OrderLineTableComponent implements OnInit, OnChanges {
  ngAfterViewInit(): void {
    this.orderLines = this.orderLinesList;
  }

  @Input()
  orderLinesList: any[];
  orderLines: any[] = [];
  constructor() { }

  ngOnInit() {
    this.orderLines = this.orderLinesList;
  }
  
  ngOnChanges(changes: SimpleChanges): void {
    this.orderLines = this.orderLinesList;
  }

}
