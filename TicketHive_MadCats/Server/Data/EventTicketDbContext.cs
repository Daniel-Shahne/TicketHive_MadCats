using Microsoft.EntityFrameworkCore;
using TicketHive_MadCats.Shared.Models;

namespace TicketHive_MadCats.Server.Data
{
    public class EventTicketDbContext : DbContext
    {
        public EventTicketDbContext()
        {
            
        }

        public EventTicketDbContext(DbContextOptionsBuilder optionsBuilder) : base(optionsBuilder)
        {
            
        }

        public DbSet<EventModel> Events { get; set; }
        public DbSet<TicketModel> Tickets { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EventTicketDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data
        }
    }
}
