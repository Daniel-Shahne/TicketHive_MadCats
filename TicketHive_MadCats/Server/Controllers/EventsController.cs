using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketHive_MadCats.Server.Repos.RepoInterfaces;
using TicketHive_MadCats.Server.Repos.Repos;
using TicketHive_MadCats.Shared.Models;
using TicketHive_MadCats.Shared.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketHive_MadCats.Server.Controllers
{
    // api/Events
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
        /// <summary>
        /// Gets all EventModels in database
        /// </summary>
        /// <returns>All EventModels projected into EventViewModels</returns>
        [HttpGet]
        public async Task<ActionResult<List<EventViewModel>>> GetAll()
        {
            List<EventModel> listOfModels = await eventRepo.GetAllEvents();
            List<EventViewModel> listOfViewModels = listOfModels.Select(model => new EventViewModel(model)).ToList();

            if(!listOfViewModels.Any())
            {
                return NotFound();
            }
            return Ok(listOfViewModels);
        }

        // api/Events/3
        // GET api/<EventsController>/5
        /// <summary>
        /// Gets one EventModel from database with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The found EventModel as an EventViewModel</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventViewModel?>> Get(int id)
        {
            EventModel? model = await eventRepo.GetOneEventById(id);
            if(model != null)
            {
                EventViewModel viewModel = new(model);
                return Ok(viewModel);
            }
            else return NotFound();
        }

        // POST api/Events
        /// <summary>
        /// Creates a new EventModel entry in database
        /// </summary>
        /// <param name="newtonsoftModel">An EventModel serialized with Newtonsoft. Should be in request body</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]string newtonsoftModel)
        {
            EventModel? model = JsonConvert.DeserializeObject<EventModel>(newtonsoftModel);
            if(model is null) return NotFound("Could not instantiate an EventModel");
            EventModel? newModel = await eventRepo.CreateEvent(model);
            if(newModel != null)
            {
                return Ok(newModel.Id);
            }
            else
            {
                return Conflict("Could not create a database entry for the event");
            }
        }

        // DELETE api/<EventsController>/5
        /// <summary>
        /// Deletes an EventModel entry from database with corresponding id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
