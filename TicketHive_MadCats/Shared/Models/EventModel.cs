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
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string EventType { get; set; } = null!;
        public int TicketPrice { get; set; }
        [Required]
        public string Location { get; set; } = null!;
        public DateTime Date { get; set; }

        // Contains a list of strings representing relative paths
        // to {LOCATION}
        // Is a serialized list<strings> as list of primitive data
        // types is not allowed in EFC.
        public string ImageSrcs { get; set; } = null!;

        // To compare with Tickets.Count in methods
        public int MaxTickets { get; set; }

        // Navigation properties
        public List<TicketModel> Tickets { get; set; } = new();
    }
}
