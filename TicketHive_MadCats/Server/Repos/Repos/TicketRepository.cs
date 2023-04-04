using System;
using TicketHive_MadCats.Server.Data;
using TicketHive_MadCats.Server.Repos.RepoInterfaces;
using TicketHive_MadCats.Shared.Models;

namespace TicketHive_MadCats.Server.Repos.Repos
{
    public class TicketRepository : ITicketRepo
    {
        public readonly EventTicketDbContext _context;

        public TicketRepository(EventTicketDbContext context)
        {
            _context = context;
        }

        public Task<TicketModel?> CreateTicket(TicketModel ticketModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTicket(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TicketModel>> GetAllTickets()
        {
            throw new NotImplementedException();
        }

        public Task<TicketModel?> GetOneTicketById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
