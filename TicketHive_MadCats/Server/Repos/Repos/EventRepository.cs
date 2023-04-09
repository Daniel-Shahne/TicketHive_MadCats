using IdentityModel;
using Microsoft.EntityFrameworkCore;
using System;
using TicketHive_MadCats.Server.Data;
using TicketHive_MadCats.Server.Repos.RepoInterfaces;
using TicketHive_MadCats.Shared.Models;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TicketHive_MadCats.Server.Repos.Repos;

public class EventRepository : IEventRepo
{
    public readonly EventTicketDbContext _context;

    public EventRepository(EventTicketDbContext context)
    {
        _context = context;
    }

    public async Task<EventModel?> CreateEvent(EventModel model)
    {
        try
        {
            _context.Events.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }
        catch(DbUpdateException ex)
        {
            return null;
        }
    }

    /// <summary>
    /// Delete funktion som hittar event.id asynkront. om det inte är null så går den vidare och deletar,
    /// sparar och returnerar true
    /// Hittar den inte så returnerar den false
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteEventById(int id)
    {
        var eventToDelete = await _context.Events.FindAsync(id);
        if(eventToDelete != null)
        {
            _context.Events.Remove(eventToDelete);
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public async Task<List<EventModel>> GetAllEvents()
    {

        var allEvents = await _context.Events.Include(e => e.Tickets).ToListAsync();
        return allEvents;
    }

    public async Task<EventModel?> GetOneEventById(int id)
    {
        return await _context.Events.Include(e => e.Tickets).FirstOrDefaultAsync(e => e.Id == id);
    }

    //inkluderar tickets
    //public async Task<EventModel?> GetOneEventByIdWithTickets(int id)
    //{
    //    return await _context.Events.Include(e => e.Tickets).FirstOrDefaultAsync(e => e.Id == id);
    //}
}
