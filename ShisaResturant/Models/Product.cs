using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShisaResturant.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string ImageUrl { get; set; } = "https://via.placeholder.com/150";// Path to the image of the product
        [ValidateNever]
        public Category? Category { get; set; } //Product belongs to a category
        [ValidateNever]
        public ICollection<OrderItem>? OrderItems { get; set; }  // Product can be in many order items
        [ValidateNever]
        public ICollection<ProductIngredient>? ProductIngredients { get; set; } // Product can have many ingredients
    }
}