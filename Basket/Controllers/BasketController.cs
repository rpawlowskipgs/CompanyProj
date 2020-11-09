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
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost]
        public ActionResult AddToBasket(int customerId, int productId, int quantity = 1)
        {
            bool isAdded =_basketService.AddProductsToBasket(customerId, productId, quantity);
            if (isAdded)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<BasketResponse>> GetBasket(int customerId)
        {
            var currentBasketProducts = await _basketService.GetCurrentBasketProducts(customerId);

            if (currentBasketProducts == null)
            {
                return NotFound();
            }

            return currentBasketProducts;
        }

        [HttpDelete]
        public ActionResult RemoveFromBasket(int customerId, int productId)
        {
            var isProductDeleted = _basketService.RemoveFromBasket(customerId, productId);

            if (isProductDeleted)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPut]
        public ActionResult UpdateQuantityOfProducts(int customerId, int productId, int quantity = 1)
        {
            if (quantity <= 0)
            {
                return BadRequest();
            }
            _basketService.UpdateQuantityOfProductsInBasket(customerId, productId, quantity);
            return Ok();
        }
    }
}
