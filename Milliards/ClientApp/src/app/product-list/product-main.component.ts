import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-product-main',
  templateUrl: './product-main.component.html'
})
export class ProductMainComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

}
