using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TicketHive_MadCats.Server.Models;

namespace TicketHive_MadCats.Server.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Methods to login, and having some error message if user is trying with wrong login credentials 
    /// </summary>
    [BindProperties]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<CustomUser> signInManager;

        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public LoginModel(SignInManager<CustomUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var signInResult = await signInManager.PasswordSignInAsync(Username!, Password!, false, false);

                if (signInResult.Succeeded)
                {
                    return Redirect("/HomePage");
                }
            }

            return Page();
        }
    }
}
