import { Component, OnInit } from '@angular/core';
import { Basket } from '../basket';
import { BasketService } from '../basket.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent implements OnInit {

  baskets: Basket[] = [];

  constructor(private basketService: BasketService) { }

  ngOnInit() {
    this.getBaskets();
  }

  getBaskets(): void {
    this.basketService.getBaskets().subscribe(baskets => this.baskets = baskets.slice(1, 5));
  }

}
