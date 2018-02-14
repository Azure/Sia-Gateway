﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Shared.Authentication;
using Sia.Shared.Exceptions;
using Sia.Shared.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests.Events
{
    public class GetEventRequest : AuthenticatedRequest<Event>
    {
        public GetEventRequest(long id, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            Id = id;
        }
        public long Id { get; }
    }

    public class GetEventHandler
        : IncidentContextHandler<GetChildEventRequest, Event>
    {
        public GetEventHandler(IncidentContext context)
            : base(context)
        {

        }
        public override async Task<Event> Handle(GetChildEventRequest request, CancellationToken cancellationToken)
        {
            var eventRecord = await _context
                                    .Events
                                    .FirstOrDefaultAsync(ev => ev.Id == request.Id, cancellationToken);
            if (eventRecord == null) throw new NotFoundException($"Could not find event with id {request.Id} associated with incident {request.IncidentId}");

            return Mapper.Map<Event>(eventRecord);
        }
    }
}
