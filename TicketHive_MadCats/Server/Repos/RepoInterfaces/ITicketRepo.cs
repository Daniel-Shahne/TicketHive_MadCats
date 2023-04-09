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
        public Task<List<TicketModel>> GetAllTicketsByUserName(string userName);

        /// <summary>
        /// Creates new tickets. This is not necessarily an admin
        /// operation as anyone should be able to book, i.e create a ticket.
        /// Will try to add all tickets at once. If issues occurs, adds none
        /// </summary>
        /// <param name="ticketModel">The new tickets to create</param>
        /// <returns>Bool representing success of adding all entries to database</returns>
        public Task<bool> CreateTickets(List<TicketModel> ticketModel);

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
