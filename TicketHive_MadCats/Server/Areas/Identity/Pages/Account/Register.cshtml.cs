using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TicketHive_MadCats.Server.Models;
using TicketHive_MadCats.Shared.Statics;

namespace TicketHive_MadCats.Server.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Creating a register page, most of methods is about validate if there is a user, if not then error message will pop-up!
    /// </summary>
    [BindProperties]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CustomUser> signInManager;

		[Required(ErrorMessage = "Username is required")]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(13, ErrorMessage = "Password must be at least 13 characters")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[#¤$&!]).+$", ErrorMessage = "Password must contain at least one capital letter and one of the following symbols: # ¤ $ & !")]
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
            var userExists = await signInManager.UserManager.FindByNameAsync(Username) != null;
            if (userExists)
            {
                Debug.WriteLine("User already exists");
                ModelState.AddModelError(string.Empty, "A user with this username already exists.");
                Debug.WriteLine("Added error to ModelState");
                return Page();
            }

            if (ModelState.IsValid)
            {
                CustomUser newUser = new()
                {
                    UserName = Username,
                    Country = SelectedCountry
                };


                var registerResult = await signInManager.UserManager.CreateAsync(newUser, Password!);
                if (registerResult.Succeeded)
                {

                    var signInResult = await signInManager.PasswordSignInAsync(newUser, Password!, false, false);

                    if (signInResult.Succeeded)
                    {
                        return Redirect("/HomePage");
                    }
                }
            }
            return Page();
        }
    }
}
