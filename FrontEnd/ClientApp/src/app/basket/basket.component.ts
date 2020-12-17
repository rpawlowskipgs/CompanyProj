import { Component, OnInit } from '@angular/core';

import { Basket } from '../basket';
import { BasketService } from '../basket.service';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {

  baskets: Basket[];

  constructor(private basketService: BasketService, private messageService: MessageService) { }

  ngOnInit() {
    this.getBaskets();
  }

  getBaskets(): void {
    this.basketService.getBaskets().subscribe(baskets => this.baskets = baskets);
  }

}
