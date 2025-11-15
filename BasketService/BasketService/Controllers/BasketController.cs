using BasketService.Model.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public IActionResult Get(string UserId)
        {
            //var basket = _basketService.GetBasket(UserId);
            var basket = _basketService.GetOrCreateBasketForUser(UserId);
            return Ok(basket);
        }

        [HttpPost]
        public IActionResult AddItemToBasket(AddItemToBasketDto request, string UserId)
        {
            var basket = _basketService.GetOrCreateBasketForUser(UserId);
            request.BasketId = basket.Id;
            _basketService.AddItemToBasket(request);
            var basketData = _basketService.GetBasket(UserId);
            return Ok();
        }

        [HttpPut]
        public IActionResult SetBasketItemQuantity(Guid ItemId, int Quantity)
        {
            _basketService.SetBasketItemQuantity(ItemId, Quantity);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Remove(Guid ItemId)
        {
            _basketService.RemoveItemFromBasket(ItemId);
            return Ok();
        }
    }
}
