using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Models;

namespace Basket.Services
{
    public interface IBasketService
    {
        public Task<Result<BasketResponse>> AddProductsToBasket(int customerId, int productId, int quantity);

        public Task<Result<BasketResponse>> GetCurrentBasketProducts(int customerId);

        public Task<Result<BasketResponse>> UpdateQuantityOfProductsInBasket(int customerId, int productId, int quantity);

        public Result<BasketResponse> RemoveFromBasket(int customerId, int productId);
    }
}
