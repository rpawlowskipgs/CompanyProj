using System.Collections.Generic;
using Models.Customers.Models;

namespace Basket.Models
{
    public class BasketResponse
    {
        public Customer Customer { get; set; }

        public List<ProductResponse> Product { get; set; }    
    }
}
