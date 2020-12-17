import { Injectable } from '@angular/core';
import { Basket } from './basket';
import { BASKETS } from './mock-basket';
import { Observable, of } from 'rxjs';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  constructor(private messageService: MessageService) { }

  getBaskets(): Observable<Basket[]> {
    this.messageService.add('It is fetched from Basket service');
    return of(BASKETS);
  }

  getBasket(id: number): Observable<Basket> {
    this.messageService.add(`Basket service fetched basket ID = ${id}`);
    return of(BASKETS.find(basket => basket.basketId === id));
  }
}
