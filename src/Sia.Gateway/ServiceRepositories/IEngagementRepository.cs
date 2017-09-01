using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.ServiceRepositories
{
    public interface IEngagementRepository
    {
        Task<Engagement> GetEngagementAsync(long incidentId, long id, AuthenticatedUserContext userContext);
        Task<Engagement> PostEngagementAsync(long incidentId, NewEngagement newEngagement, AuthenticatedUserContext userContext);
        Task PutEngagementAsync(long incidentId, long engagementId, UpdateEngagement updatedEngagement, AuthenticatedUserContext userContext);
    }

    public class EngagementRepository : IEngagementRepository
    {
        private readonly IncidentContext _context;

        public EngagementRepository(IncidentContext context)
        {
            _context = context;
        }

        public async Task<Engagement> GetEngagementAsync(long incidentId, long id, AuthenticatedUserContext userContext)
        {
            var EngagementRecord = await _context.Engagements
                .Include(en => en.Participant)
                .FirstOrDefaultAsync(ev => ev.IncidentId == incidentId && ev.Id == id);
            if (EngagementRecord == null) throw new KeyNotFoundException();

            return Mapper.Map<Engagement>(EngagementRecord);
        }

        public async Task<Engagement> PostEngagementAsync(long incidentId, NewEngagement newEngagement, AuthenticatedUserContext userContext)
        {
            if (newEngagement == null) throw new ArgumentNullException(nameof(newEngagement));

            var dataIncident = await _context.Incidents
               .Include(cr => cr.Engagements)
                    .ThenInclude(en => en.Participant)
               .FirstOrDefaultAsync(x => x.Id == incidentId);
            if (dataIncident == null) throw new KeyNotFoundException();

            var dataEngagement = Mapper.Map<Data.Incidents.Models.Engagement>(newEngagement);
            dataEngagement.TimeEngaged = DateTime.UtcNow;

            dataIncident.Engagements.Add(dataEngagement);
            await _context.SaveChangesAsync();

            return Mapper.Map<Engagement>(dataEngagement);
        }

        public async Task PutEngagementAsync(long incidentId, long engagementId, UpdateEngagement updatedEngagement, AuthenticatedUserContext userContext)
        {
            if (updatedEngagement == null) throw new ArgumentNullException(nameof(updatedEngagement));
            var existingRecord = await _context.Engagements
                .Include(en => en.Participant)
                .FirstOrDefaultAsync(engagement => engagement.IncidentId == incidentId && engagement.Id == engagementId);

            var updatedModel = Mapper.Map(updatedEngagement, existingRecord);

            await _context.SaveChangesAsync();
        }
    }
}
