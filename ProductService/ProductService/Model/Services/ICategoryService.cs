using ProductService.Infrastructure.Contexts;
using ProductService.Model.Entities;

namespace ProductService.Model.Services
{
    public interface ICategoryService
    {
        List<CategoryDto> GetCategories();
        void AddNewCategory(CategoryDto category);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ProductDataBaseContext context;

        public CategoryService(ProductDataBaseContext context)
        {
            this.context = context;
        }

        public void AddNewCategory(CategoryDto category)
        {
            var newCategory = new Category
            {
                Name = category.Name,
                Description = category.Description
            };
            context.Categories.Add(newCategory);
            context.SaveChanges();
        }

        public List<CategoryDto> GetCategories()
        {
            var data = context.Categories
                .OrderBy(c => c.Name)
                .Select(d => new CategoryDto
                {
                    Name = d.Name,
                    Description = d.Description
                }).ToList();
            return data;
        }
    }

    public class CategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
