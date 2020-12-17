import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { Basket } from '../basket';
import { BasketService } from '../basket.service';

@Component({
  selector: 'app-basket-detail',
  templateUrl: './basket-detail.component.html',
  styleUrls: ['./basket-detail.component.css']
})
export class BasketDetailComponent implements OnInit {

  basket: Basket;

  constructor( private route: ActivatedRoute, private basketService: BasketService, private location: Location) { }

  ngOnInit(): void {
    this.getBasket();
  }

  getBasket(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.basketService.getBasket(id).subscribe(basket => this.basket = basket);
  }

  goBack(): void {
    this.location.back();
  }

}
