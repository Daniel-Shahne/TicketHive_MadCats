using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketHive_MadCats.Server.Repos.Repos;
using TicketHive_MadCats.Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketHive_MadCats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventRepository eventRepo;

        public EventsController(EventRepository eventRepo)
        {
            this.eventRepo = eventRepo;
        }

        
        [HttpGet]
        public async Task<List<EventModel>> GetAll()
        {
            return await eventRepo.GetAllEvents();
        }

        // GET api/<EventsController>/5
        [HttpGet("{id}")]
        public async Task<EventModel?> Get(int id)
        {
            return await eventRepo.GetOneEventById(id);
        }

        // POST api/<EventsController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<EventModel?> Post(EventModel model)
        {
            EventModel? newModel = await eventRepo.CreateEvent(model);
            return newModel;
        }

        // PUT api/<EventsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<EventsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<bool> Delete(int id)
        {
            return await eventRepo.DeleteEventById(id);
        }
    }
}
