using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TicketHive_MadCats.Server.Models;
using TicketHive_MadCats.Shared.Statics;

namespace TicketHive_MadCats.Server.Areas.Identity.Pages.Account
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CustomUser> signInManager;

        [Required(ErrorMessage = "Username is required")]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please select a country")]
        public string? SelectedCountry { get; set; }

        public List<string> ListOfCountries = CountriesAndCodes.getListOfCountries;


        public RegisterModel(SignInManager<CustomUser> signInManager)
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
                CustomUser newUser = new()
                {
                    UserName = Username
                };

                var registerResult = await signInManager.UserManager.CreateAsync(newUser, Password!);

                if (registerResult.Succeeded)
                {

                    var signInResult = await signInManager.PasswordSignInAsync(newUser, Password!, false, false);

                    if (signInResult.Succeeded)
                    {
                        return Redirect("~/");
                    }
                }
            }
            return Page();
        }
    }
}
