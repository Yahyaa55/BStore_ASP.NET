using Microsoft.AspNetCore.Identity;

namespace BStore.Models
{
    public class User : IdentityUser
    {
        public Cart Card { get; set; } = new Cart();
    }
}
