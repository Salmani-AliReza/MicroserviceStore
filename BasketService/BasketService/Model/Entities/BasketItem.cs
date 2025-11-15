namespace BasketService.Model.Entities;

public class BasketItem
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string ImageURL { get; set; }
    public Guid BasketId { get; set; }
    public Basket Basket { get; set; }

    public void SetQuantity(int quantity)
    {
        Quantity = quantity;
    }
}