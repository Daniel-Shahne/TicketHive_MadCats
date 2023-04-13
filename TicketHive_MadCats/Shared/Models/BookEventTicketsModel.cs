using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHive_MadCats.Shared.Models
{
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
