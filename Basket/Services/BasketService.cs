using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Models;
using CompanyProducts.Models;

namespace Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basket;
        private readonly ICustomerDetailsRepository _customerDetailsRepository;
        private readonly IProductDetailsRepository _productDetailsRepository;
        public BasketService(IBasketRepository basket, ICustomerDetailsRepository customerDetailsRepository, IProductDetailsRepository productDetailsRepository)
        {
            _basket = basket;
            _customerDetailsRepository = customerDetailsRepository;
            _productDetailsRepository = productDetailsRepository;
        }

        public void AddProductsToBasket(int customerId, int productId, int quantity)
        {
            var currentBasket = _basket.GetBasket(customerId);

            if (currentBasket == null)
            {
                currentBasket = new BasketWithGoods
                {
                    CustomerId = customerId,
                    ProductIds = new List<ProductsInBasket>()
                };
                _basket.AddToBasket(currentBasket);
            }

            var productsInBasket = currentBasket.ProductIds.FirstOrDefault(p => p.ProductId == productId);

            if (productsInBasket != null)
            {
                productsInBasket.Quantity += quantity;
            }
            else
            {
                currentBasket.ProductIds.Add(new ProductsInBasket
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }
            _basket.UpdateBasket(currentBasket.ProductIds, customerId);
        }

        public async Task<BasketResponse> GetCurrentBasketProducts(int customerId)
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

            var allProducts = products.Select(p => p.Result).ToList();
            basketResponse.Product = MapBasketResponseToProductResponse(allProducts, currentBasket.ProductIds).ToList();
            basketResponse.Customer = customerInfo;

            return basketResponse;
        }

        public void UpdateQuantityOfProductsInBasket(int customerId, int productId, int quantity)
        {
            var currentBasket = _basket.GetBasket(customerId);
            var productsInBasket = currentBasket.ProductIds.FirstOrDefault(p => p.ProductId == productId);

            if (productsInBasket != null)
            {
                productsInBasket.Quantity = quantity;
            }
            else
            {
                currentBasket.ProductIds.Add(new ProductsInBasket
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }
        }

        private IEnumerable<ProductResponse> MapBasketResponseToProductResponse(IEnumerable<Product> products, IEnumerable<ProductsInBasket> baskets)
        {
            List<ProductResponse> productResponse = new List<ProductResponse>();
            foreach (var basket in baskets)
            {
                productResponse.Add(new ProductResponse
                {
                    Quantity = basket.Quantity,
                    Details = products.FirstOrDefault(p => p.ProductId == basket.ProductId)
                });
            }

            return productResponse;
        }
    }
}
