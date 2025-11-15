namespace ProductService.Model.Entities
{
    public class Product
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public Guid CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}
