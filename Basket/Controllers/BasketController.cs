using System.Threading.Tasks;
using Basket.Models;
using Basket.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basket;
        private readonly IBasketService _basketService;

        public BasketController(IBasketRepository basket,
                                IBasketService basketService)
        {
            _basket = basket;
            _basketService = basketService;
        }

        [HttpPost]
        public void AddToBasket(int customerId, int productId, int quantity = 1)
        {
            _basketService.AddProductsToBasket(customerId, productId, quantity);
        }

        [HttpGet]
        public async Task<BasketResponse> GetBasket(int customerId)
        {
            return await _basketService.GetCurrentBasketProducts(customerId);
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

        [HttpPut]
        public void UpdateQuantityOfProducts(int customerId, int productId, int quantity = 1)
        {
            _basketService.UpdateQuantityOfProductsInBasket(customerId, productId, quantity);
        }
    }
}
