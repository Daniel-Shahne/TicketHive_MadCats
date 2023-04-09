﻿using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketHive_MadCats.Server.Models;
using TicketHive_MadCats.Server.Repos.RepoInterfaces;
using TicketHive_MadCats.Shared.Models;
using TicketHive_MadCats.Shared.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketHive_MadCats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepo ticketRepo;
        private readonly IEventRepo eventRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public TicketsController(ITicketRepo ticketRepo, IEventRepo eventRepo, UserManager<ApplicationUser> userManager)
        {
            this.ticketRepo = ticketRepo;
            this.eventRepo = eventRepo;
            this.userManager = userManager;
        }




        // GET api/<TicketsController>/5
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



        // GET api/Tickets/5Tickets
        [HttpGet("User{id}")]
        [Authorize]
        public async Task<ActionResult<List<TicketViewModel>>> GetUserTickets(int id)
        {
            List<TicketModel> listOfTicketModel = await ticketRepo.GetAllTicketsByUserId(id);
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
        [HttpPost("{userName}books{eventId}times{quantity}")]
        [Authorize]
        public async Task<ActionResult> Post(string userName, int eventId, int quantity)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null) { return Unauthorized($"No user with name {userName} found"); }

            // TODO REMOVE THIS, ONLY FOR TESTING GETTING USERID
            int userId = 0;
            return Conflict();

            // Returns NotFound if no event was found of that id
            EventModel? eventToBook = await eventRepo.GetOneEventById(eventId);
            if (eventToBook == null) { return NotFound($"No event with Id {eventId} exists"); }

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
                    UserId = userId,
                    EventModelId = eventId,
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
                return Conflict("Could not book tickets");
            }
        }



        // DELETE api/<TicketsController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<bool> Delete(int id)
        {
            return await ticketRepo.DeleteTicket(id);
        }
    }
}
