using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.ServiceRepositories
{
    public interface IEngagementRepository
        :IAsyncRequestHandler<GetEngagementRequest, Engagement>,
        IAsyncRequestHandler<PostEngagementRequest, Engagement>,
        IAsyncRequestHandler<PutEngagementRequest>
    {
    }

    public class EngagementRepository : IEngagementRepository
    {
        private readonly IncidentContext _context;

        public EngagementRepository(IncidentContext context)
        {
            _context = context;
        }

        public async Task<Engagement> Handle(GetEngagementRequest request)
        {
            var EngagementRecord = await _context.Engagements
                .Include(en => en.Participant)
                .FirstOrDefaultAsync(ev => ev.IncidentId == request.IncidentId && ev.Id == request.Id);
            if (EngagementRecord == null) throw new KeyNotFoundException();

            return Mapper.Map<Engagement>(EngagementRecord);
        }

        public async Task<Engagement> Handle (PostEngagementRequest request)
        {
            if (request.NewEngagement == null) throw new ArgumentNullException(nameof(request.NewEngagement));

            var dataIncident = await _context.Incidents
               .Include(cr => cr.Engagements)
                    .ThenInclude(en => en.Participant)
               .FirstOrDefaultAsync(x => x.Id == request.IncidentId);
            if (dataIncident == null) throw new KeyNotFoundException();

            var dataEngagement = Mapper.Map<Data.Incidents.Models.Engagement>(request.NewEngagement);
            dataEngagement.TimeEngaged = DateTime.UtcNow;

            dataIncident.Engagements.Add(dataEngagement);
            await _context.SaveChangesAsync();

            return Mapper.Map<Engagement>(dataEngagement);
        }

        public async Task Handle(PutEngagementRequest request)
        {
            if (request.UpdateEngagement == null) throw new ArgumentNullException(nameof(UpdateEngagement));
            var existingRecord = await _context.Engagements
                .Include(en => en.Participant)
                .FirstOrDefaultAsync(engagement => engagement.IncidentId == request.IncidentId && engagement.Id == request.EngagementId);

            var updatedModel = Mapper.Map(request.UpdateEngagement, existingRecord);

            await _context.SaveChangesAsync();
        }
    }
}
