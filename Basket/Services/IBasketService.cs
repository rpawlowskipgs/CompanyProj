using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Models;

namespace Basket.Services
{
    public interface IBasketService
    {
        public void AddProductsToBasket(int customerId, int productId, int quantity);

        public Task<BasketResponse> GetCurrentBasketProducts(int customerId);

        public void UpdateQuantityOfProductsInBasket(int customerId, int productId, int quantity);
    }
}
