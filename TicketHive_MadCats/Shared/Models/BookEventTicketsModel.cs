using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHive_MadCats.Shared.Models
{
	/// <summary>
	/// Used for booking tickets to an event by calling tickets API. 
	/// Needs to be filled in for
	/// properties Username, Eventname, Quantity.
	/// </summary>
	public class BookEventTicketsModel
	{
		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("eventname")]
		public string Eventname { get; set; }

		[JsonProperty("quantity")]
		public int Quantity { get; set; }
	}
}
