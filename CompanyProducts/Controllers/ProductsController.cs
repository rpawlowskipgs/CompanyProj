using System.Collections.Generic;
using CompanyProducts.Models;
using CompanyProducts.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyProducts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _productRepository.GetProducts();
        }

        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return _productRepository.GetProductById(id);
        }

        [HttpPost]
        public void Post([FromBody] Product product)
        {
            _productRepository.AddProduct(product);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Product product)
        {
            _productRepository.UpdateProduct(id, product);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _productRepository.DeleteProduct(id);
        }
    }
}
