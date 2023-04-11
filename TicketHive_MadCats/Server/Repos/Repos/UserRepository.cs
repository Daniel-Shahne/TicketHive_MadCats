using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketHive_MadCats.Server.Data;
using TicketHive_MadCats.Server.Models;
using TicketHive_MadCats.Server.Repos.RepoInterfaces;
using TicketHive_MadCats.Shared.Models;

namespace TicketHive_MadCats.Server.Repos.Repos
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<IdentityUser> passwordHasher;
        public UserRepository(ApplicationDbContext context, IPasswordHasher<IdentityUser> passwordHasher)
        {
            _context = context;
            this.passwordHasher = passwordHasher;

        }

        public async Task<CustomUser?> UpdateUser(UpdateUserModel updateUserModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updateUserModel.Id);
            var passwordHasher = new PasswordHasher<IdentityUser>();

            if (user == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateUserModel.NewPassword))
            {
                user.PasswordHash = passwordHasher.HashPassword(user, updateUserModel.NewPassword);
            }

            if (!string.IsNullOrEmpty(updateUserModel.NewCountry))
            {
                var customUser = user as CustomUser;

                if (customUser != null)
                {
                    customUser.Country = updateUserModel.NewCountry;
                }
                else
                {
                    return null;
                }
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

    }
}
