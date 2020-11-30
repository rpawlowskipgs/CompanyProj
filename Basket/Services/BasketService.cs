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
        
        public async Task<Result<BasketResponse>> AddProductsToBasket(int customerId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return Result.Fail<BasketResponse>(Status.BadRequest);
            }

            var isCustomerFound = await FindCustomer(customerId);
            if (!isCustomerFound)
            {
                return Result.Fail<BasketResponse>(Status.NotFound);
            }

            var currentBasket = _basket.GetBasket(customerId);
            AddNewBasket(ref currentBasket, customerId);

            var productDetails = await _productDetailsRepository.GetProductAsync(productId);
            if (productDetails == null)
            {
                return Result.Fail<BasketResponse>(Status.BadRequest);
            }
            
            IncreaseOrAddProductsInBasket(currentBasket, quantity, productId);

            _basket.UpdateBasket(currentBasket.ProductIds, customerId);
            return Result.Ok<BasketResponse>(default);
        }

        public async Task<Result<BasketResponse>> GetCurrentBasketProducts(int customerId)
        {
            var currentBasket = _basket.GetBasket(customerId);

            BasketResponse basketResponse = new BasketResponse();

            if (currentBasket == null)
            {
                return Result.Fail<BasketResponse>(Status.NotFound);
            }

            var customerInfo = await _customerDetailsRepository.GetCustomer(customerId);

            List<Task<Product>> products = new List<Task<Product>>();
            foreach (var productId in currentBasket.ProductIds)
            {
                var productDetails = _productDetailsRepository.GetProductAsync(productId.ProductId);
                products.Add(productDetails);
            }
            await Task.WhenAll(products);

            var allProducts = products.Select(p => p.Result).ToList();
            basketResponse.Product = MapBasketResponseToProductResponse(allProducts, currentBasket.ProductIds).ToList();
            basketResponse.Customer = customerInfo;

            return Result.Ok(basketResponse);
        }

        public async Task<Result<BasketResponse>> UpdateQuantityOfProductsInBasket(int customerId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return Result.Fail<BasketResponse>(Status.BadRequest);
            }

            var currentBasket = _basket.GetBasket(customerId);
            if (currentBasket == null)
            {
                return Result.Fail<BasketResponse>(Status.NotFound);
            }

            var productDetails = await _productDetailsRepository.GetProductAsync(productId);
            if (productDetails == null)
            {
                return Result.Fail<BasketResponse>(Status.NotFound);
            }

            IncreaseOrAddProductsInBasket(currentBasket, quantity, productId);

            return Result.Ok<BasketResponse>(default);
        }

        public Result<BasketResponse> RemoveFromBasket(int customerId, int productId)
        {
            var isDeleted = RemoveProduct(customerId, productId);

            if (isDeleted)
            {
                return Result.Ok<BasketResponse>(default);
            }

            return Result.Fail<BasketResponse>(Status.BadRequest);
        }

        private bool RemoveProduct(int customerId, int productId)
        {
            var isDeleted = false;
            var currentBasket = _basket.GetBasket(customerId);
            if (currentBasket != null)
            {
                var productToDelete = currentBasket.ProductIds.FirstOrDefault(p => p.ProductId == productId);
                isDeleted = currentBasket.ProductIds.Remove(productToDelete);
            }

            return isDeleted;
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

        private async Task<bool> FindCustomer(int customerId)
        {
            var customerInfo = await _customerDetailsRepository.GetCustomer(customerId);
            if (customerInfo == null)
            {
                return false;
            }

            return true;
        }

        private void AddNewBasket(ref BasketWithGoods currentBasket, int customerId)
        {
            if (currentBasket == null)
            {
                currentBasket = new BasketWithGoods
                {
                    CustomerId = customerId,
                    ProductIds = new List<ProductsInBasket>()
                };
                _basket.AddToBasket(currentBasket);
            }
        }

        private void IncreaseOrAddProductsInBasket(BasketWithGoods currentBasket, int quantity, int productId)
        {
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
        }
    }
}
