using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHive_MadCats.Shared.Models
{
	public class UpdateUserModel
	{
		[Required(ErrorMessage = "User name required / User not authenticated")]
		[JsonProperty("username")]
		public string Username { get; set; } = null!;

        [MinLength(13, ErrorMessage = "Passwords are atleast 13 characters long")]
        [JsonProperty("currentPassword")]
		public string? CurrentPassword { get; set; }

		[MinLength(13, ErrorMessage = "Password must be atleast 13 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[#£$%&!]).+$", ErrorMessage = "Password must contain at least one capital letter and one of the following symbols: # £ $ % & !")]
        [JsonProperty("password")]
		public string? Password { get; set; }

		[JsonProperty("country")]
		public string? Country { get; set; }
	}
}
