using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TicketHive_MadCats.Server.Repos.RepoInterfaces;
using TicketHive_MadCats.Server.Repos.Repos;
using TicketHive_MadCats.Shared.Models;
using TicketHive_MadCats.Shared.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketHive_MadCats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    // [EnableCors("AllowAll")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepo eventRepo;

        public EventsController(IEventRepo eventRepo)
        {
            this.eventRepo = eventRepo;
        }

        // base/api/Events
        [HttpGet]
        public async Task<ActionResult<List<EventViewModel>>> GetAll()
        {
            List<EventModel> listOfModels = await eventRepo.GetAllEvents();
            List<EventViewModel> listOfViewModels = listOfModels.Select(model => new EventViewModel(model)).ToList();

            if(!listOfViewModels.Any())
            {
                return BadRequest();
            }
            return Ok(listOfViewModels);
        }

        // GET api/<EventsController>/5
        [HttpGet("{id}")]
        public async Task<EventViewModel?> Get(int id)
        {
            EventModel? model = await eventRepo.GetOneEventById(id);
            if(model != null)
            {
                EventViewModel viewModel = new(model);
                return viewModel;
            }
            else return null;
        }

        // POST api/<EventsController>
        [HttpPost]
        public async Task<ActionResult<EventModel?>> Post(EventModel model)
        {
            EventModel? newModel = await eventRepo.CreateEvent(model);
            if(newModel != null)
            {
                return Ok(newModel);
            }
            else
            {
                return BadRequest(null);
            }
        }

        // PUT api/<EventsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<EventsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool success = await eventRepo.DeleteEventById(id);
            if(success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
