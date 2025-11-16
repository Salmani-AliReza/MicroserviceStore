using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Model.Services;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var UserId = "1";
            var orders = _orderService.GetOrdersForUser(UserId);
            return Ok(orders);
        }

        [HttpGet("{OrderId}")]
        public IActionResult Get(Guid OrderId)
        {
            var order = _orderService.GetOrderById(OrderId);
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddOrderDto Order)
        {
            _orderService.AddOrder(Order);
            return Ok();
        }
    }
}
