namespace OrderService.Model.Entities;

public class OrderLine
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int ProductPrice { get; set; }
    public int Quantity { get; set; }
    public Order Order { get; set; }
    public Guid OrderId { get; set; }
}