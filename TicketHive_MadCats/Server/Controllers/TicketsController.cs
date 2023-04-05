using Microsoft.AspNetCore.Mvc;
using TicketHive_MadCats.Server.Repos.RepoInterfaces;
using TicketHive_MadCats.Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketHive_MadCats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepo ticketRepo;

        //// GET: api/<TicketsController>
        //// Ska inte finnas någon get all tickets metod
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        public TicketsController(ITicketRepo ticketRepo)
        {
            this.ticketRepo = ticketRepo;
        }




        // GET api/<TicketsController>/5
        [HttpGet("{id}")]
        public async Task<TicketModel?> Get(int id)
        {
            return await ticketRepo.GetOneTicketById(id);
        }

        // POST api/<TicketsController>
        [HttpPost]
        public async Task<TicketModel?> Post(int userId, int eventId)
        {
            TicketModel newTicket = new()
            {
                UserId = userId,
                EventModelId = eventId
            };

            return await ticketRepo.CreateTicket(newTicket);
        }

        // PUT api/<TicketsController>/5
        // Ingen update ska finnas
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<TicketsController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await ticketRepo.DeleteTicket(id);
        }
    }
}
