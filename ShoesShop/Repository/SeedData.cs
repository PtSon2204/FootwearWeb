using Microsoft.AspNetCore.Identity;

namespace ShoesShop.Repository
{
    public class SeedData
    {

        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                CategoryModel c1 = new CategoryModel
                {
                    Name = "Boots",
                    Slug = "boots",
                    Description = "Boots is brand in the world",
                    Status = 1
                };

                CategoryModel c2 = new CategoryModel
                {
                    Name = "Sneakers",
                    Slug = "sneakers",
                    Description = "Sneakers is brand in the world",
                    Status = 1
                };

                CategoryModel c3 = new CategoryModel
                {
                    Name = "Men's Sports Shoes",
                    Slug = "Men''s-Sports-Shoes",
                    Description = "Lightweight, breathable sports shoes designed for running, training, and daily athletic activities.",
                    Status = 1
                };

                CategoryModel c4 = new CategoryModel
                {
                    Name = "Men's Leather Shoes",
                    Slug = "Men's-Leather-Shoes",
                    Description = "High-quality leather shoes for men, perfect for formal events, office wear, and business attire.",
                    Status = 1
                };

                CategoryModel c5 = new CategoryModel
                {
                    Name = "Men's Sandals",
                    Slug = "Men's-Sandals",
                    Description = "Comfortable men''s sandals designed for daily use, beachwear, and casual outings.",
                    Status = 1
                };

                CategoryModel c6 = new CategoryModel
                {
                    Name = "Women's Sandals",
                    Slug = "Women's-Sandals",
                    Description = "Stylish and comfortable women''s sandals suitable for summer and outdoor activities.",
                    Status = 1
                };

                CategoryModel c7 = new CategoryModel
                {
                    Name = "High Heels",
                    Slug = "High-Heels",
                    Description = "Elegant women’s high-heel shoes designed for parties, work, and special occasions.",
                    Status = 1
                };

                CategoryModel c8 = new CategoryModel
                {
                    Name = "Women's Slippers",
                    Slug = "Women's-Slippers",
                    Description = "Comfortable and lightweight slippers for women, perfect for daily home use.",
                    Status = 1
                };

                BrandModel b1 = new BrandModel
                {
                    Name = "Mlb",
                    Description = "Mlb is Large Brand in the world",
                    Slug = "Mlb",
                    Status = 1
                };

                BrandModel b2 = new BrandModel
                {
                    Name = "Slipper Crop",
                    Description = "Crop is Large Brand in the world",
                    Slug = "Slipper-crop",
                    Status = 1
                };

                BrandModel b3 = new BrandModel
                {
                    Name = "Nike",
                    Description = "Nike is shoes most famous",
                    Slug = "Nike",
                    Status = 1
                };

                BrandModel b4 = new BrandModel
                {
                    Name = "Louis Vuitton",
                    Description = "Louis Vuitton is an fashion famous about shoes, slipper,....",
                    Slug = "Louis-Vuitton",
                    Status = 1
                };

                BrandModel b5 = new BrandModel
                {
                    Name = "Adidas",
                    Description = "Combine sport and fashion with signature products featuring the three stripes.",
                    Slug = "Adidas",
                    Status = 1
                };

                BrandModel b6 = new BrandModel
                {
                    Name = "Puma",
                    Description = "A youthful shoe brand that collaborates with many celebrities.",
                    Slug = "Puma",
                    Status = 1
                };

                BrandModel b7 = new BrandModel
                {
                    Name = "Vans",
                    Description = "The original brand of skateboarding, associated with street style.",
                    Slug = "Vans",
                    Status = 1
                };

                BrandModel b8 = new BrandModel
                {
                    Name = "Biti's",
                    Description = "National brand with many durable and diverse lines of sandals (Hunter, sandals).",
                    Slug = "Biti's",
                    Status = 0
                };

                BrandModel b9 = new BrandModel
                {
                    Name = "Ananas",
                    Description = "Popular among young people with unique, dynamic sneakers and sandals.",
                    Slug = "Ananas",
                    Status = 1
                };

                BrandModel b10 = new BrandModel
                {
                    Name = "Timberland",
                    Description = "Famous for its waterproof Yellow Boots, strong, durable Work boots, suitable for all environments.",
                    Slug = "Timberland",
                    Status = 1
                };

                BrandModel b11 = new BrandModel
                {
                    Name = "Grenson",
                    Description = "High quality, British handmade shoe brand.",
                    Slug = "Grenson",
                    Status = 1
                };

                BrandModel b12 = new BrandModel
                {
                    Name = "Dr. Martens",
                    Description = "Symbol of rebellion and personality with distinctive design and durable rubber sole.",
                    Slug = "Dr.-Martens",
                    Status = 1
                };

                _context.Products.AddRange(
                    new ProductModel
                    {
                        Name = "Mlb Boston",
                        Slug = "mlb-boston",
                        Description = "MaMlbcbook is the best ",
                        Image = "1.jpg",
                        Category = c1,
                        Brand = b1,
                        Price = 20000,
                        CapitalPrice = 2900,
                        Quantity = 100
                    },

                    new ProductModel
                    {
                        Name = "pc",
                        Slug = "pc",
                        Description = "pc is the best ",
                        Image = "1.jpg",
                        Category = c1,
                        Brand = b2,
                        Price = 30000,
                        CapitalPrice = 2900,
                        Quantity = 200
                    }
                );
                _context.SaveChanges();
            }

            if (!_context.Contacts.Any())
            {
                ContactModel contact = new ContactModel
                {
                    Id = 1,
                    Name = "SA2 Sakura",
                    Description = "<p>Chi nh&aacute;nh H&agrave; &ETH;&ocirc;ng</p>\r\n",
                    Phone = "123456789",
                    Email = "admin@gmail.com",
                    Map = "!1m18!1m12!1m3!1d3724.7176252129993!2d105.7369453!3d21.0039533!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x313453790a5bf21f%3A0xb30c61db4effc90e!2sSA2%20Sakura!5e0!3m2!1svi!2s!4v1765424254836!5m2!1svi!2s",
                    LogoImg = "17ea7b79-d6fc-4e4e-8303-b84d8b8b2799_360_F_908560397_HTk6hkRQQzUCGDRnf1MHfiX3q0IJyjAD.jpg"
                };
                // Add the contact to the database context
                _context.Contacts.Add(contact);
                _context.SaveChanges();
            }

            if (!_context.Roles.Any())
            {
                // Create roles
                var roles = new List<IdentityRole>
                    {
                        new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                        new IdentityRole { Name = "User", NormalizedName = "USER" },
                    };

                foreach (var role in roles)
                {
                    _context.Roles.Add(role);
                }

                _context.SaveChanges();
            }

            if (!_context.Users.Any())
            {
                // Create a new user
                var user = new AppUserModel
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true, // Skip email confirmation
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@GMAIL.COM",

                    PasswordHash = new PasswordHasher<AppUserModel>().HashPassword(null, "Admin@123"), // Securely hash the password
                    SecurityStamp = Guid.NewGuid().ToString(), // Generate a unique security stamp
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                // Add user to the context
                _context.Users.Add(user);
                // Ensure the role exists in the database
                var roleName = "Admin";
                var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
                if (role == null)
                {
                    role = new IdentityRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    };
                    _context.Roles.Add(role);
                    _context.SaveChanges();
                }
                // Assign the user to the role
                var userRole = new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = role.Id
                };
                _context.UserRoles.Add(userRole);
                // Save changes to the database
                _context.SaveChanges();

                Console.WriteLine("User and role assignment seeded successfully!");
            }
            else
            {
                Console.WriteLine("Users already exist in the database.");
            }
        }
    }

}
