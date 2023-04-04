using Microsoft.EntityFrameworkCore;
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

        public async Task<TicketModel?> CreateTicket(TicketModel ticketModel)
        {
            _context.Tickets.Add(ticketModel);
            await _context.SaveChangesAsync();

            return ticketModel;
        }

        //public async Task<List<TicketModel>> CreateTickets(int eventId, int quantity)
        //{
        //    var tickets = Enumerable.Range(0, quantity)
        //        .Select(i => new TicketModel { EventModelId = eventId })
        //        .ToList();

        //    _context.Tickets.AddRange(tickets);
        //    await _context.SaveChangesAsync();

        //    return tickets;
        //}
        
        //v1
        public async Task<bool> DeleteTicket(int id)
        {
            var ticketToDelete = await _context.Tickets.FindAsync(id);
            if (ticketToDelete != null)
            {
                _context.Tickets.Remove(ticketToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        //v2, så denna tar först in evented som ticket tillhör. Sedan hittar den ticket.id som vi valt och deletar den.
        //public async Task<bool> DeleteTicket(int eventId, int ticketId)
        //{
        //    var eventToUpdate = await _context.Events.Include(e => e.Tickets).FirstOrDefaultAsync(e => e.Id == eventId);

        //    if (eventToUpdate != null)
        //    {
        //        var ticketToDelete = eventToUpdate.Tickets.FirstOrDefault(t => t.Id == ticketId);

        //        if (ticketToDelete != null)
        //        {
        //            eventToUpdate.Tickets.Remove(ticketToDelete);

        //            await _context.SaveChangesAsync();

        //            return true;
        //        }
        //    }

        //    return false;
        //}



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
