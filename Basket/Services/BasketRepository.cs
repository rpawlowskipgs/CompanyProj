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

        public void UpdateBasket(ProductsInBasket prods, int customerId, int productId)
        {
            var itemToUpdate = _basket.FirstOrDefault(c => c.CustomerId == customerId).ProductIds
                .FirstOrDefault(p => p.ProductId == productId);
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = prods.Quantity;
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
