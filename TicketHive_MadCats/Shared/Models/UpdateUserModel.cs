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

		[JsonProperty("username")]
		public string Username { get; set; } = null!;

		[Required(ErrorMessage = "Must have a password")]
		[MinLength(13, ErrorMessage = "Password must be atleast 13 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[@#£$%&!]).+$", ErrorMessage = "The {0} field must contain at least one capital letter and one of the following symbols: @ # £ $ % & !")]
        [JsonProperty("password")]
		public string Password { get; set; } = null!;

		[Required(ErrorMessage = "Must have a country selected")]
		[JsonProperty("country")]
		public string Country { get; set; } = null!;
	}
}
