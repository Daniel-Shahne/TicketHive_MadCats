using TicketHive_MadCats.Shared.Models;

namespace TicketHive_MadCats.Server.Repos.RepoInterfaces
{
    public interface ITicketRepo
    {
        // IMPORTANT: NONE OF THESE SHOULD BE ALTERED WITHOUT
        // COMMUNICATING WITH THE REST OF THE GROUP

        /// <summary>
        /// Searches the database for a ticket with the corresponding id
        /// </summary>
        /// <param name="id">Id of the ticket to look for</param>
        /// <returns>If a ticket is found its returned, null returned otherwise</returns>
        public Task<TicketModel?> GetOneTicketById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns all ticketes</returns>
        public Task<List<TicketModel>> GetAllTickets();

        /// <summary>
        /// Creates a new ticket. This is not necessarily an admin
        /// operation as anyone should be able to book, i.e create a ticket
        /// </summary>
        /// <param name="ticketModel">The new ticket to create</param>
        /// <returns>Returns the created ticket, per convention. Returns null if failed to create the ticket</returns>
        public Task<TicketModel?> CreateTicket(TicketModel ticketModel);

        /// <summary>
        /// Deletes a ticket of given Id. This is not necessarily an admin
        /// operation, however its important that the API checks an user is
        /// only attempting to delete their OWN TICKET as this method
        /// has no built in control
        /// </summary>
        /// <param name="id">Id of the ticket to delete</param>
        /// <returns>A bool representing success of ticket deletion</returns>
        public Task<bool> DeleteTicket(int id);
    }
}
