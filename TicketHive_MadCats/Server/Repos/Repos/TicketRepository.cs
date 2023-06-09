﻿using Microsoft.EntityFrameworkCore;
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

        // Apparently EFC only saves all the values if no issues occured,
        // otherwise none are added.
        public async Task<bool> CreateTickets(List<TicketModel> ticketModel)
        {
            try
            {
                _context.Tickets.AddRange(ticketModel);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(DbUpdateException ex)
            {
                return false;
            }
        }

        public async Task<List<TicketModel>> GetAllTicketsByUserName(string userName)
        {
            return await _context.Tickets.Where(t => t.Username == userName)
                                         .Include(t => t.EventModel)
                                         .ToListAsync();
        }

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

        public async Task<TicketModel?> GetOneTicketById(int id)
        {
            return await _context.Tickets.Include(t => t.EventModel).FirstOrDefaultAsync(t => t.Id == id);
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


        //public async Task<List<TicketModel>> CreateTickets(int eventId, int quantity)
        //{
        //    var tickets = Enumerable.Range(0, quantity)
        //        .Select(i => new TicketModel { EventModelId = eventId })
        //        .ToList();

        //    _context.Tickets.AddRange(tickets);
        //    await _context.SaveChangesAsync();

        //    return tickets;
        //}


    }
}
