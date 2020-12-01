using System.Collections.Generic;
using Models.Products.Models;

namespace CompanyProducts.Services
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();

        Product GetProductById(int id);

        void AddProduct(Product product);

        void UpdateProduct(int id, Product product);

        void DeleteProduct(int id);
    }
}
