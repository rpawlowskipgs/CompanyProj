using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyProducts.Models;

namespace Basket.Services
{
    public interface IProductDetailsRepository
    {
        public Task<Product> GetProductAsync(int productId);
    }
}
