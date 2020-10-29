using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Basket.Models;
using Basket.Services;
using CompanyProducts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basket;
        private readonly IProductDetailsRepository _productDetailsRepository;
        private readonly ICustomerDetailsRepository _customerDetailsRepository;

        public BasketController(IProductDetailsRepository productDetailsRepository,
                                ICustomerDetailsRepository customerDetailsRepository,
                                IBasketRepository basket)
        {
            _basket = basket;
            _productDetailsRepository = productDetailsRepository;
            _customerDetailsRepository = customerDetailsRepository;
        }

        [HttpPost]
        public void AddToBasket(int customerId, int productId, int quantity = 1)
        {
            var currentBasket = _basket.GetBasket(customerId);

            if (currentBasket == null)
            {
                currentBasket = new BasketWithGoods
                {
                    CustomerId = customerId,
                    ProductIds = new List<ProductsInBasket>
                    {
                        new ProductsInBasket
                        {
                            ProductId = productId,
                            Quantity = 1
                        }
                    }
                };
                _basket.AddToBasket(currentBasket);
            }
            else if (currentBasket.CustomerId == customerId && currentBasket.ProductIds.Select(i => i.ProductId).Contains(productId))
            {
                var currentItems = currentBasket.ProductIds.Select(i => i.Quantity).FirstOrDefault();

                var prods = new ProductsInBasket
                {
                        Quantity = currentItems + quantity
                };
                int number = currentItems + quantity;
                _basket.UpdateBasket(prods, customerId, productId);
            }
        }

        [HttpGet]
        public async Task<BasketResponse> GetBasket(int customerId)
        {
            var currentBasket = _basket.GetBasket(customerId);

            BasketResponse basketResponse = new BasketResponse();

            var customerInfo = await _customerDetailsRepository.GetCustomer(customerId);

            List<Task<Product>> products = new List<Task<Product>>();
            foreach (var productId in currentBasket.ProductIds)
            {
                var productDetails = _productDetailsRepository.GetProduct(productId.ProductId);
                products.Add(productDetails);
            }
            await Task.WhenAll(products);

            basketResponse.Product = products.Select(p => p.Result).ToList();
            basketResponse.Customer = customerInfo;
            return basketResponse;
        }

        [HttpDelete]
        public StatusCodeResult RemoveFromBasket(int customerId, int productId)
        {
            var isProductDeleted = _basket.RemoveFromBasket(customerId, productId);

            if (isProductDeleted)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
