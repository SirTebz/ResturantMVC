using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShisaResturant.Models;

namespace ShisaResturant.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ProductIngredient> ProductIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure many-to-many relationship between Product and Ingredient
        modelBuilder.Entity<ProductIngredient>()
            .HasKey(pi => new { pi.ProductId, pi.IngredientId });

        modelBuilder.Entity<ProductIngredient>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.ProductIngredients)
            .HasForeignKey(pi => pi.ProductId);

        modelBuilder.Entity<ProductIngredient>()
            .HasOne(pi => pi.Ingredient)
            .WithMany(i => i.ProductIngredients)
            .HasForeignKey(pi => pi.IngredientId);

        //Seed data
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Appetizers" },
            new Category { CategoryId = 2, Name = "Main Courses" },
            new Category { CategoryId = 3, Name = "Side Dish" },
            new Category { CategoryId = 4, Name = "Desserts" },
            new Category { CategoryId = 5, Name = "Beverages" }
        );

        modelBuilder.Entity<Ingredient>().HasData(
            new Ingredient { IngredientId = 1, Name = "Chicken" },
            new Ingredient { IngredientId = 2, Name = "Beef" },
            new Ingredient { IngredientId = 3, Name = "Beef Patty" },
            new Ingredient { IngredientId = 4, Name = "Lamp" },
            new Ingredient { IngredientId = 5, Name = "Fish" },
            new Ingredient { IngredientId = 6, Name = "Pork" },
            new Ingredient { IngredientId = 7, Name = "Chips" },
            new Ingredient { IngredientId = 8, Name = "Burger Buns" },
            new Ingredient { IngredientId = 9, Name = "Veggies" },
            new Ingredient { IngredientId = 10, Name = "Salads" },
            new Ingredient { IngredientId = 11, Name = "Pap" },
            new Ingredient { IngredientId = 12, Name = "Chocolate Cake" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = 1,
                Name = "Chicken Wings",
                Description = "Spicy chicken wings served with chips.",
                Price = 5.99m,
                Stock = 50,
                CategoryId = 1
            },
            new Product
            {
                ProductId = 2,
                Name = "Beef Burger",
                Description = "Juicy beef burger with lettuce, tomato, and cheese served with chips.",
                Price = 8.99m,
                Stock = 30,
                CategoryId = 2
            },
            new Product
            {
                ProductId = 3,
                Name = "Grilled Beef",
                Description = "Rich and tender beef served with Pap or Buns and salads of choice with chips as an add-on.",
                Price = 7.50m,
                Stock = 40,
                CategoryId = 2
            },
            new Product
            {
                ProductId = 4,
                Name = "Grilled Fish",
                Description = "Grilled fish tacos with salads and chips.",
                Price = 7.99m,
                Stock = 20,
                CategoryId = 2
            },
            new Product
            {
                ProductId = 5,
                Name = "Pork Ribs",
                Description = "Tender pork ribs glazed with BBQ sauce.",
                Price = 6.99m,
                Stock = 15,
                CategoryId = 2
            },
            
            new Product
            {
                ProductId = 6,
                Name = "Chocolate Cake",
                Description = "Rich chocolate cake with a creamy frosting.",
                Price = 3.99m,
                Stock = 40,
                CategoryId = 4
            }
        );

        modelBuilder.Entity<ProductIngredient>().HasData(
            new ProductIngredient { ProductId = 1, IngredientId = 1 },
            new ProductIngredient { ProductId = 1, IngredientId = 7 },
            new ProductIngredient { ProductId = 2, IngredientId = 3 },
            new ProductIngredient { ProductId = 2, IngredientId = 7 },
            new ProductIngredient { ProductId = 2, IngredientId = 9 },
            new ProductIngredient { ProductId = 3, IngredientId = 2 },
            new ProductIngredient { ProductId = 3, IngredientId = 10 },
            new ProductIngredient { ProductId = 3, IngredientId = 11 },            
            new ProductIngredient { ProductId = 4, IngredientId = 5 },
            new ProductIngredient { ProductId = 4, IngredientId = 7 },
            new ProductIngredient { ProductId = 4, IngredientId = 10 },
            new ProductIngredient { ProductId = 5, IngredientId = 6 },
            new ProductIngredient { ProductId = 6, IngredientId = 12 }
        );
    }
}
