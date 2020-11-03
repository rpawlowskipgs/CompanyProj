using System.Collections.Generic;
using System.Linq;
using Basket.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Customers.Models;

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

        public bool RemoveFromBasket(int customerId, int productId)
        {
            var isDeleted = false;
            var currentBasket = GetBasket(customerId);
            if (currentBasket != null)
            {
                var productToDelete = currentBasket.ProductIds.FirstOrDefault(p => p.ProductId == productId);
                isDeleted = currentBasket.ProductIds.Remove(productToDelete);
            }

            return isDeleted;
        }
    }
}
