import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-main',
  templateUrl: './order-main.component.html',
  styleUrls: ['./order-main.component.css']
})
export class OrderMainComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

}
