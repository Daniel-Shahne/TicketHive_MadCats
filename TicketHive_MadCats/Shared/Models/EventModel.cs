using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TicketHive_MadCats.Shared.Models
{
    public class EventModel
    {
        // Self properties
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [Required]
        [JsonProperty("eventType")]
        public string EventType { get; set; } = null!;

        [JsonProperty("ticketPrice")]
        public int TicketPrice { get; set; }

        [Required]
        [JsonProperty("location")]
        public string Location { get; set; } = null!;

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        // Contains a list of strings representing relative paths
        // to {LOCATION}
        // Is a serialized list<strings> as list of primitive data
        // types is not allowed in EFC.
        [JsonProperty("imageSrcs")]
        public string ImageSrcs { get; set; } = null!;

        // To compare with Tickets.Count in methods
        [JsonProperty("maxTickets")]
        public int MaxTickets { get; set; }

        // Navigation properties
        [JsonProperty("tickets")]
        public List<TicketModel> Tickets { get; set; }
    }
}
