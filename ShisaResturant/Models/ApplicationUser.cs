using Microsoft.AspNetCore.Identity;

namespace ShisaResturant.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order> Orders { get; set; } 
    }
}
