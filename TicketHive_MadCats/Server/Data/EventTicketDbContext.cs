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

        public EventTicketDbContext(DbContextOptions optionsBuilder) : base(optionsBuilder)
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
                "/images/event images/image 1.avif",
                "/images/event images/image 2.avif"
            };
            List<string> TE2LS = new()
            {
                "/images/event images/image 3.avif",
                "/images/event images/image 4.avif"
            };
            // TE1S = Test eventmodel 1 string
            string TE1S = JsonConvert.SerializeObject(TE1LS);
            string TE2S = JsonConvert.SerializeObject(TE2LS);
            
            // Seeding data for eventmodel. TicketModels have to be
            // seeded separately in another modelbuilder entity blablabla
            // and through the foreign key (ID) the relation is created
            modelBuilder.Entity<EventModel>().HasData( 
                new EventModel()
                {
                    Id = 1,
                    Name = "TestEvent1",
                    EventType = "TestEventType1",
                    TicketPrice = 100,
                    Location = "TestEvent1Location",
                    Date = DateTime.Now,
                    MaxTickets = 5,
                    ImageSrcs = TE1S
                },
                new EventModel()
                {
                    Id = 2,
                    Name = "TestEvent2",
                    EventType = "TestEventType2",
                    TicketPrice = 100,
                    Location = "TestEvent2Location",
                    Date = DateTime.Now,
                    MaxTickets = 2,
                    ImageSrcs= TE2S
                }
                );

            // Seeding ticketmodel data. EventModelId is
            // foreign key to EventModel's
            modelBuilder.Entity<TicketModel>().HasData(
                new TicketModel()
                {
                    Id = 1,
                    UserId = 1,
                    EventModelId = 1
                },
                new TicketModel()
                {
                    Id = 2,
                    UserId = 1,
                    EventModelId = 1
                },
                new TicketModel()
                {
                    Id = 3,
                    UserId = 2,
                    EventModelId = 2
                },
                new TicketModel()
                {
                    Id = 4,
                    UserId = 1,
                    EventModelId = 2
                }
                );
        }
    }
}
