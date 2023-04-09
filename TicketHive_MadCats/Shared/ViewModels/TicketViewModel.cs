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
    public class TicketViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("eventModelId")]
        public int EventModelId { get; set; }


        public TicketViewModel(TicketModel model)
        {
            this.Id = model.Id;
            this.Username = model.Username;
            this.EventModelId = model.EventModelId;
        }

        [JsonConstructor]
        public TicketViewModel()
        {
            
        }
    }
}
