using Microsoft.AspNetCore.Identity;

namespace TicketHive_MadCats.Server.Models
{
    public class CustomUser : IdentityUser
    {
        public string? Country { get; set; }
    }
}
