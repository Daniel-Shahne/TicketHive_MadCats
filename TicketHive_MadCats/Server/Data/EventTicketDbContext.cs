using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TicketHive_MadCats.Shared.Models;

namespace TicketHive_MadCats.Server.Data
{
    public class EventTicketDbContext : DbContext
    {
        public EventTicketDbContext()
        {
            
        }

        public EventTicketDbContext(DbContextOptions<EventTicketDbContext> optionsBuilder) : base(optionsBuilder)
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
            // Creating list of image path strings and converting those to
            // strings as serialized objects since List<string> cannot be 
            // stored in SQL using EFC
            // TE1LS = Test Eventmodel 1 list of strings
            List<string> TE1LS = new()
            {
                "/images/event images/image-1.png",
                "/images/event images/image-2.png"
            };
            List<string> TE2LS = new()
            {
                "/images/event images/image-3.png",
                "/images/event images/image-4.png"
            };
            List<string> TE3LS = new()
            {
                "/images/event images/image-5.png",
                "/images/event images/image-6.png"
            };
            List<string> TE4LS = new()
            {
                "/images/event images/image-7.png",
                "/images/event images/image-8.png"
            };
            // TE1S = Test eventmodel 1 string
            string TE1S = JsonConvert.SerializeObject(TE1LS);
            string TE2S = JsonConvert.SerializeObject(TE2LS);
            string TE3S = JsonConvert.SerializeObject(TE3LS);
            string TE4S = JsonConvert.SerializeObject(TE4LS);


            // Seeding data for eventmodel. TicketModels have to be
            // seeded separately in another modelbuilder entity blablabla
            // and through the foreign key (ID) the relation is created
            modelBuilder.Entity<EventModel>().HasData( 
                new EventModel()
                {
                    Id = 1,
                    Name = "Rock Concert",
                    EventType = "Concert",
                    TicketPrice = 100,
                    Location = "Malmö, Sweden",
                    Date = DateTime.Now.AddMonths(7),
                    MaxTickets = 5,
                    ImageSrcs = TE1S
                },
                new EventModel()
                {
                    Id = 2,
                    Name = "Latino Concert",
                    EventType = "Concert",
                    TicketPrice = 50,
                    Location = "Stockholm, Sweden",
                    Date = DateTime.Now.AddDays(7),
                    MaxTickets = 2,
                    ImageSrcs= TE2S
                },
                new EventModel()
                {
                    Id = 3,
                    Name = "Dreamhack",
                    EventType = "Tournament",
                    TicketPrice = 5000,
                    Location = "Krakow, Poland",
                    Date = DateTime.Now.AddYears(3),
                    MaxTickets = 100,
                    ImageSrcs = TE3S
                },
                new EventModel()
                {
                    Id = 4,
                    Name = "Art Exhibition",
                    EventType = "Exhibition",
                    TicketPrice = 5,
                    Location = "Berlin, Germany",
                    Date = DateTime.Now.AddHours(5),
                    MaxTickets = 20,
                    ImageSrcs = TE4S
                }
                );

            // Seeding ticketmodel data. EventModelId is
            // foreign key to EventModel's
            modelBuilder.Entity<TicketModel>().HasData(
                new TicketModel()
                {
                    Id = 1,
                    Username = "admin",
                    EventModelId = 1
                },
                new TicketModel()
                {
                    Id = 2,
                    Username = "admin",
                    EventModelId = 1
                },
                new TicketModel()
                {
                    Id = 3,
                    Username = "admin",
                    EventModelId = 2
                },
                new TicketModel()
                {
                    Id = 4,
                    Username = "user",
                    EventModelId = 2
                }
                );
        }
    }
}
