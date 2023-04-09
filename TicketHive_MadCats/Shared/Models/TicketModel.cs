using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TicketHive_MadCats.Shared.Models
{
    public class TicketModel
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }


        [JsonProperty("username")]
        public string Username { get; set; } = null!;

        // Navigation property and its id
        [JsonProperty("eventModelId")]
        public int EventModelId { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public EventModel EventModel { get; set; } = null!;
    }
}
