namespace BasketService.Model.Entities
{
    public class Basket
    {
        public Basket(string userId)
        {
            UserID = userId;
        }

        public Basket()
        {

        }

        public Guid Id { get; set; }
        public string UserID { get; private set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
