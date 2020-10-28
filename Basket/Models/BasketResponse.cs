using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Services;
using CompanyProducts.Models;
using Customers.Models;

namespace Basket.Models
{
    public class BasketResponse
    {
        public Customer Customer { get; set; }

        public List<Product> Product { get; set; }    
    }
}
