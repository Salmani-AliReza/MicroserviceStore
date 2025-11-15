using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Contexts;
using ProductService.Model.Entities;

namespace ProductService.Model.Services
{
    public interface IProductService
    {
        List<ProductDto> GetProductList();
        ProductDto GetProductById(Guid id);
        void AddNewProduct(AddNewProductDto product);
    }

    public class ProductService : IProductService
    {
        private readonly ProductDataBaseContext context;
        public ProductService(ProductDataBaseContext context)
        {
            this.context = context;
        }

        public List<ProductDto> GetProductList()
        {
            var data = context.Products
                .Include(c => c.Category)
                .OrderByDescending(p => p.ID)
                .Select(d => new ProductDto
                {
                    ID = d.ID,
                    Name = d.Name,
                    Description = d.Description,
                    Image = d.Image,
                    Price = d.Price,
                    ProductCategory = new ProductCategoryDto
                    {
                        ID = d.Category.ID,
                        CategoryName = d.Category.Name
                    }
                }).ToList();
            return data;
        }

        public ProductDto GetProductById(Guid id)
        {
            var data = context.Products
                .Include(c => c.Category)
                .Where(p => p.ID == id)
                .Select(d => new ProductDto
                {
                    ID = d.ID,
                    Name = d.Name,
                    Description = d.Description,
                    Image = d.Image,
                    Price = d.Price,
                    ProductCategory = new ProductCategoryDto
                    {
                        ID = d.Category.ID,
                        CategoryName = d.Category.Name
                    }
                }).FirstOrDefault();
            if (data is null)
                throw new Exception("Product not found...!");

            return data;
        }

        public void AddNewProduct(AddNewProductDto product)
        {
            var category = context.Categories.Find(product.CategoryID);
            if (category is null)
                throw new Exception("Category not found...!");

            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Price = product.Price,
                Category = category
            };
            context.Products.Add(newProduct);
            context.SaveChanges();
        }
    }

    public class ProductDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public ProductCategoryDto ProductCategory { get; set; }
    }

    public class ProductCategoryDto
    {
        public Guid ID { get; set; }
        public string CategoryName { get; set; }
    }

    public class AddNewProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public Guid CategoryID { get; set; }
    }
}
