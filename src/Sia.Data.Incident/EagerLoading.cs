using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents.Models;
using System.Linq;

namespace Sia.Data.Incidents
{
    public static class EagerLoading
    {
        public static IQueryable<Incident> WithEagerLoading(this DbSet<Incident> table)
        {
            return table
                .Include(cr => cr.PrimaryTicket)
                .Include(cr => cr.Tickets)
                .Include(cr => cr.Engagements)
                    .ThenInclude(en => en.Participant);
        }
    }
}
