using Microsoft.AspNetCore.Identity;

namespace AdvanceChat.Data
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
