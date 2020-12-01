using Models.Products.Models;

namespace Basket.Models
{
    public class ProductResponse
    {
        public Product Details { get; set; }
        public int Quantity { get; set; }
    }
}
