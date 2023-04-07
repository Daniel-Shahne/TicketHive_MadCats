using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketHive_MadCats.Shared.Models;

namespace TicketHive_MadCats.Shared.ViewModels
{
    public class EventViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("eventType")]
        public string EventType { get; set; } = null!;

        [JsonProperty("ticketPrice")]
        public int TicketPrice { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; } = null!;

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("imageSrcs")]
        public string ImageSrcs { get; set; } = null!;

        [JsonProperty("maxTickets")]
        public int MaxTickets { get; set; }

        /// <summary>
        /// This replaces the list of TicketModel's in EventModel as the ui
        /// is only interested in how many tickets are booked, not in the
        /// details of each ticket. If the tickets of each user is to be
        /// retrieved then use TicketRepo's method for getting all tickets
        /// and convert it to TicketViewModel
        /// </summary>
        public int BookedTickets { get; set; }

        public EventViewModel(EventModel model)
        {
            Id = model.Id;
            Name = model.Name;
            EventType = model.EventType;
            TicketPrice = model.TicketPrice;
            Location = model.Location;
            Date = model.Date;
            ImageSrcs = model.ImageSrcs;
            MaxTickets = model.MaxTickets;
            BookedTickets = model.Tickets.Count;
        }

        [JsonConstructor]
        public EventViewModel()
        {
            
        }
    }
}
