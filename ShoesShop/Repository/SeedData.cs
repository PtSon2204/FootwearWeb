namespace ShoesShop.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                CategoryModel macbook = new CategoryModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is Large Product in the world", Status = 1 };
                CategoryModel pc = new CategoryModel { Name = "Pc", Slug = "pc", Description = "Pc is Large Product in the world", Status = 1 };

                BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple is Large Brand in the world", Status = 1 };
                BrandModel samsung = new BrandModel { Name = "Sumsung", Slug = "sumsung", Description = "Sumsung is Large Brand in the world", Status = 1 };

                _context.Products.AddRange(
                    new ProductModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is Best", Image = "1.jpg", Category = macbook, Price = 1233, Brand = apple },
                    new ProductModel { Name = "Pc", Slug = "pc", Description = "Pc is Best", Image = "2.jpg", Category = pc, Price = 1233, Brand = samsung }
                );

                _context.SaveChanges();
            }
        }
    }
}
