using System.Threading.Tasks;
using Basket.Models;
using Basket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Controllers
{
    [Authorize]
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
        public async Task<ActionResult> AddToBasket(int customerId, int productId, int quantity = 1)
        {
            var result = await _basketService.AddProductsToBasket(customerId, productId, quantity);
            
            return MapResult(result);
        }

        [HttpGet]
        public async Task<ActionResult<BasketResponse>> GetBasket(int customerId)
        {
            var result = await _basketService.GetCurrentBasketProducts(customerId);

            return MapResult(result);
        }

        [HttpDelete]
        public ActionResult RemoveFromBasket(int customerId, int productId)
        {
            var result = _basketService.RemoveFromBasket(customerId, productId);

            return MapResult(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateQuantityOfProducts(int customerId, int productId, int quantity = 1)
        {
            var result = await _basketService.UpdateQuantityOfProductsInBasket(customerId, productId, quantity);

            return MapResult(result);
        }
     
        private ActionResult MapResult(Result<BasketResponse> result)
        {
            switch (result.Status)
            {
                case Status.Ok:
                    return ResolveStatus(result.Value);

                case Status.BadRequest:
                    return BadRequest(result.Value);


                case Status.NotFound:
                    return NotFound(result.Value);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private ActionResult ResolveStatus(object payload)
        {
            if (payload == null)
            {
                return Ok();
            }
            return Ok(payload);
        }
    }
}
