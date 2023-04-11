using Microsoft.AspNetCore.Identity;

namespace TicketHive_MadCats.Server.Models
{
    public class CustomUser : IdentityUser
    {
        public int Id { get; set; }
        public string? Country { get; set; }
    }
}
