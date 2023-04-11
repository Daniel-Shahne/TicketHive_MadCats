using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicketHive_MadCats.Shared.Models
{
    public class UpdateUserModel
    {
        [Key]
        public int Id { get; set; }
        
        [JsonProperty("NewPassword")]
        public string NewPassword { get; set; }
        
        [JsonProperty("NewCountry")]
        public string NewCountry { get; set; }
    }
}
