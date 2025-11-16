using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Contexts;
using OrderService.Model.Entities;

namespace OrderService.Model.Services
{
    public interface IOrderService
    {
        void AddOrder(AddOrderDto Order);
        List<OrderDto> GetOrdersForUser(string UserId);
        OrderDto GetOrderById(Guid OrderId);
    }

    public class OrderService : IOrderService
    {
        private readonly OrderDataBaseContext _context;

        public OrderService(OrderDataBaseContext context)
        {
            _context = context;
        }

        public List<OrderDto> GetOrdersForUser(string UserId)
        {
            var orders = _context.Orders
                .Include(l => l.OrderLines)
                .Where(c => c.UserId == UserId)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderPlaced = o.OrderPlaced,
                    OrderPaid = o.OrderPaid,
                    OrderLines = o.OrderLines.Select(ol => new OrderLineDto
                    {
                        Id = ol.Id,
                        ProductId = ol.ProductId,
                        ProductName = ol.ProductName,
                        ProductPrice = ol.ProductPrice,
                        Quantity = ol.Quantity
                    }).ToList()
                }).ToList();

            return orders;
        }

        public OrderDto GetOrderById(Guid OrderId)
        {
            var order = _context.Orders
                .Include(l => l.OrderLines)
                .FirstOrDefault(c => c.Id == OrderId);

            if (order is null)
            {
                throw new Exception("Order Not Found...!");
            }

            var result = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderPlaced = order.OrderPlaced,
                OrderPaid = order.OrderPaid,
                OrderLines = order.OrderLines.Select(ol => new OrderLineDto
                {
                    Id = ol.Id,
                    ProductId = ol.ProductId,
                    ProductName = ol.ProductName,
                    ProductPrice = ol.ProductPrice,
                    Quantity = ol.Quantity
                }).ToList()
            };

            return result;
        }

        public void AddOrder(AddOrderDto Order)
        {
            var orderLines = new List<OrderLine>();
            foreach (var line in Order.OrderLines)
            {
                orderLines.Add(new OrderLine
                {
                    Id = Guid.NewGuid(),
                    ProductId = line.ProductId,
                    ProductName = line.ProductName,
                    ProductPrice = line.ProductPrice,
                    Quantity = line.Quantity
                });
            }
            var order = new Order(Order.UserId, orderLines);
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
    }

    public class AddOrderDto
    {
        public string UserId { get; set; }
        public List<AddOrderLineDto> OrderLines { get; set; }
    }

    public class AddOrderLineDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public bool OrderPaid { get; set; }
        public DateTime OrderPlaced { get; set; }
        public List<OrderLineDto> OrderLines { get; set; }
    }

    public class OrderLineDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}
