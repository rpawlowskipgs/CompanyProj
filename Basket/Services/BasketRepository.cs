using System.Collections.Generic;
using System.Linq;
using Basket.Models;

namespace Basket.Services
{
    public class BasketRepository : IBasketRepository
    {
        private List<BasketWithGoods> _basket = new List<BasketWithGoods>();

        public void AddToBasket(BasketWithGoods basket)
        {
            _basket.Add(basket);
        }

        public void UpdateBasket(List<ProductsInBasket> products, int customerId)
        {
            var basketForCustomer = _basket.FirstOrDefault(c => c.CustomerId == customerId);
            if (basketForCustomer != null)
            {
                basketForCustomer.ProductIds = products;
            }
        }

        public BasketWithGoods GetBasket(int customerId)
        {
            return _basket.FirstOrDefault(b => b.CustomerId == customerId);
        }
    }
}
