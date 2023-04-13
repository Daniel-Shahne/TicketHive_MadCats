using TicketHive_MadCats.Shared.Models;

namespace TicketHive_MadCats.Server.Repos.RepoInterfaces
{
    public interface IEventRepo
    {
        // IMPORTANT: NONE OF THESE SHOULD BE ALTERED WITHOUT
        // COMMUNICATING WITH THE REST OF THE GROUP
        // Remember to include .Include() in any LINQ's
        // so that the List<TicketModel> wont be null

        /// <summary>
        /// Searches the EventTicket database for an event
        /// with the given Id
        /// </summary>
        /// <param name="id">Id of the EventModel to get</param>
        /// <returns>The EventModel if found, null otherwise</returns>
        public Task<EventModel?> GetOneEventById(int id);

        /// <summary>
        /// Searches the EventTicket database for an event of
        /// given name
        /// </summary>
        /// <param name="eventName">Name of the event to search for</param>
        /// <returns>The event as an Eventmodel if found, null otherwise</returns>
        public Task<EventModel?> GetOneEventByName(string eventName);

        /// <summary>
        /// Gets all events
        /// </summary>
        /// <returns></returns>
        public Task<List<EventModel>> GetAllEvents();

        /// <summary>
        /// Deletes an event with a given Id. This is an admin
        /// only operation and so this method should only be called
        /// after authorization by server API
        /// </summary>
        /// <param name="id">Id of the EventModel to delete</param>
        /// <returns>The updated EventModel, per convention</returns>
        public Task<bool> DeleteEventById(int id);

        //// STÅR INGET OM UPDATE I UPPGIFTEN
        ///// <summary>
        ///// Updates an EventModel. Should ONLY be called if
        ///// authorization is done and valid by the server API
        ///// </summary>
        ///// <param name="model">The EventModel to update</param>
        ///// <returns>The updated EventModel, per convention</returns>
        //public Task<EventModel> UpdateEvent(EventModel model);

        /// <summary>
        /// Creates a new event. This is an admin only operation
        /// and so this method should only be called
        /// after authorization by server API
        /// </summary>
        /// <param name="model">The EventModel to be added to database</param>
        /// <returns>Returns the newly created EventModel, per convention, if successfull. Null otherwise</returns>
        public Task<EventModel?> CreateEvent(EventModel model);
    }
}