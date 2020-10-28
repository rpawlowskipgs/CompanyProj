using System.Collections.Generic;

namespace Basket.Models
{
    public class BasketWithGoods
    {
        public int BasketId { get; set; }

        public int CustomerId { get; set; }

        public List<ProductsInBasket> ProductIds { get; set; } = new List<ProductsInBasket>();
        
    }
}
