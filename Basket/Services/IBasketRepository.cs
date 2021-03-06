﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Models;

namespace Basket.Services
{
    public interface IBasketRepository
    {
        public void AddToBasket(BasketWithGoods basket);

        public BasketWithGoods GetBasket(int customerId);

        public void UpdateBasket(List<ProductsInBasket> products, int customerId);
    }
}
