﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHive_MadCats.Shared.Models
{
    public class TicketModel
    {
        [Key]
        public int Id { get; set; }

        // Referenses to an IdentityUser's Id
        // but is NOT a navigation property
        [Required]
        public int UserId { get; set; }

        // Navigation property and its id
        public int EventId { get; set; }
        public EventModel Event { get; set; } = null!;
    }
}
