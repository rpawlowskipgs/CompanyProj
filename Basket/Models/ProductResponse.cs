using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyProducts.Models;

namespace Basket.Models
{
    public class ProductResponse
    {
        public Product Details { get; set; }
        public int Quantity { get; set; }
    }
}
