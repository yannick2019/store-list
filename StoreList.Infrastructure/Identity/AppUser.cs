using Microsoft.AspNetCore.Identity;

namespace StoreList.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
    }
}
