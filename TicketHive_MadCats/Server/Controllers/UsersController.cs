using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketHive_MadCats.Server.Data;
using TicketHive_MadCats.Server.Models;
using TicketHive_MadCats.Server.Repos.Repos;
using TicketHive_MadCats.Shared.Models;



namespace TicketHive_MadCats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;
        private readonly IPasswordHasher<IdentityUser> passwordHasher;
        private readonly ApplicationDbContext _context;
        private readonly UserRepository userRepository;
        public UsersController(Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, ApplicationDbContext _context, IPasswordHasher<IdentityUser> passwordHasher)
        {
            this.userManager = userManager;
            this._context = _context;
            this.passwordHasher = passwordHasher;

        }


        [HttpPut("/api/users")]
        public async Task<ActionResult> UpdateUser(string userId, UpdateUserModel updateUserModel)
        {
            var currentUser = await userManager.FindByIdAsync(userId);

            if (currentUser == null)
            {
                return BadRequest("User not found.");
            }

            // Declare passwordHasher variable here
            var passwordHasher = new PasswordHasher<IdentityUser>();

            if (!string.IsNullOrEmpty(updateUserModel.NewPassword))
            {
                currentUser.PasswordHash = passwordHasher.HashPassword(currentUser, updateUserModel.NewPassword);
            }

            if (!string.IsNullOrEmpty(updateUserModel.NewCountry))
            {
                var customUser = currentUser as CustomUser;

                if (customUser != null)
                {
                    customUser.Country = updateUserModel.NewCountry;
                }
                else
                {
                    return BadRequest("User is not a CustomUser.");
                }
            }

            var result = await userManager.UpdateAsync(currentUser);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }




    }
}
