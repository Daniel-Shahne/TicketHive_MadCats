using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using TicketHive_MadCats.Server.Models;
using TicketHive_MadCats.Server.Repos.RepoInterfaces;
using TicketHive_MadCats.Shared.Models;
using TicketHive_MadCats.Shared.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketHive_MadCats.Server.Controllers
{
    // api/Tickets
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepo ticketRepo;
        private readonly IEventRepo eventRepo;
        private readonly UserManager<CustomUser> userManager;

        public TicketsController(ITicketRepo ticketRepo, IEventRepo eventRepo, UserManager<CustomUser> userManager)
        {
            this.ticketRepo = ticketRepo;
            this.eventRepo = eventRepo;
            this.userManager = userManager;
        }



        // api/Tickets/Ticket5
        /// <summary>
        /// Gets one TicketModel of corresponding id, if existing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Ticket{id}")]
        [Authorize]
        public async Task<ActionResult<TicketViewModel?>> GetOne(int id)
        {
            TicketModel? ticketModel = await ticketRepo.GetOneTicketById(id);
            if (ticketModel != null)
            {
                TicketViewModel ticketViewModel = new (ticketModel);
                return Ok(ticketViewModel);
            }
            else
            {
                return NotFound();
            }
        }



        // GET api/Tickets/UsernameAdmin
        /// <summary>
        /// Gets all TicketModel's with their username property matching userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("Username{userName}")]
        [Authorize]
        public async Task<ActionResult<List<TicketViewModel>>> GetUserTickets(string userName)
        {
            List<TicketModel> listOfTicketModel = await ticketRepo.GetAllTicketsByUserName(userName);
            if (listOfTicketModel.Any())
            {
                List<TicketViewModel> listOfTicketViewModel = listOfTicketModel.Select(x => new TicketViewModel(x)).ToList();
                return Ok(listOfTicketViewModel);
            }
            else
            {
                return NotFound();
            }
        }


        // POST api/<TicketsController>
        /// <summary>
        /// Attempts booking (i.e adding to database) tickets, for an user, of a certain event.
        /// </summary>
        /// <param name="bookEventTicketsModel">A BookEventTicketsModel serialized using Newtonsoft</param>
        /// <returns></returns>
        [HttpPost("book")]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] string bookEventTicketsModel)
        {
            // deserializes
            BookEventTicketsModel? model = JsonConvert.DeserializeObject<BookEventTicketsModel>(bookEventTicketsModel);
            if(model == null) return BadRequest("No valid JSON sent");

            string userName = model.Username;
            string eventName = model.Eventname;
            int quantity = model.Quantity;

            // Checks if there is a valid user for that username
            var user = await userManager.FindByNameAsync(userName);
            if (user == null) { return Unauthorized($"No user with name {userName} found"); }

            // Replaces _ in eventName with a space
            string fixedEventName = eventName.Replace("_", " ");

            // Returns NotFound if no event was found of that id
            EventModel? eventToBook = await eventRepo.GetOneEventByName(fixedEventName);
            if (eventToBook == null) { return NotFound($"No event with Id {fixedEventName} exists"); }

            // Checks if the event if avaliable to book {quantity} amount of times
            int ticketsLeft = eventToBook.MaxTickets - eventToBook.Tickets.Count;
            if (quantity > ticketsLeft) 
            {
                return Conflict($"{quantity} tickets cant be booked for event {eventToBook.Name} as it only has {ticketsLeft} tickets left");
            }

            // If no returns done so far then there is a valid user that
            // wants to book a valid event for valid quantity
            List<TicketModel> listOfTicketsToBook = new();
            for(int i = 1; i <= quantity; i++)
            {
                // Another possible solution could be to add the ticket
                // to EventModel's List<TicketModel> property. Apparently
                // EFC can handle navigation props if the foreign key is
                // written so i choose this method for simplicity
                TicketModel newTicket = new()
                {
                    Username = userName,
                    EventModelId = eventToBook.Id,
                };
                listOfTicketsToBook.Add(newTicket);
            }

            // Attempts adding tickets to database (booking)
            bool bookStatus = await ticketRepo.CreateTickets(listOfTicketsToBook);
            if (bookStatus)
            {
                return Ok();
            }
            else
            {
                return Conflict($"Database conflict for when booking {fixedEventName}");
            }
        }


        
        // DELETE api/<TicketsController>/5
        /// <summary>
        /// Deletes (unbooks) a ticket of given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            bool success = await ticketRepo.DeleteTicket(id);
            if (success) { return Ok(); }
            else return NotFound();
        }
    }
}
