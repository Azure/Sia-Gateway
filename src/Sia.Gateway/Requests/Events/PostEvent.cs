using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Core.Authentication;
using Sia.Core.Exceptions;
using Sia.Core.Requests;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sia.Core.Validation;
using Sia.Gateway.Hubs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;

namespace Sia.Gateway.Requests
{
    public class PostEventRequest : AuthenticatedRequest<Event>
    {
        public PostEventRequest(
            long incidentId,
            NewEvent newEvent,
            AuthenticatedUserContext userContext,
            Lazy<string> lazyToken,
            int? port
        ) : base(userContext)
        {
            IncidentId = incidentId;
            NewEvent = ThrowIf.Null(newEvent, nameof(newEvent));
            LazyToken = ThrowIf.Null(lazyToken, nameof(lazyToken));
            Port = port;
        }
        public NewEvent NewEvent { get; }
        public long IncidentId { get; }
        public Lazy<string> LazyToken { get; }
        public int? Port { get; }
    }

    public class HubConnectionInfo
    {
        public int Port { get; set; }
    }

    public class PostEventHandler : IncidentContextHandler<PostEventRequest, Event>
    {
        private ILogger Logger { get; }
        private HubConnectionBuilder EventHubConnectionBuilder { get; }
        public PostEventHandler(
            IncidentContext context,
            HubConnectionBuilder hubConnectionBuilder,
            ILoggerFactory loggerFactory
        ) : base(context)
        {
            EventHubConnectionBuilder = hubConnectionBuilder;
            Logger = loggerFactory.CreateLogger<IncidentContextHandler<PostEventRequest, Event>>();
        }
        public override async Task<Event> Handle(PostEventRequest request, CancellationToken cancellationToken)
        {
            var dataIncident = await _context
                                   .Incidents
                                   .Include(cr => cr.Events)
                                   .FirstOrDefaultAsync(x => x.Id == request.IncidentId, cancellationToken)
                                   .ConfigureAwait(continueOnCapturedContext: false);
            if (dataIncident == null) throw new NotFoundException($"Found no incident with id {request.IncidentId}.");

            var dataEvent = Mapper.Map<Data.Incidents.Models.Event>(request.NewEvent);

            dataIncident.Events.Add(dataEvent);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            var result = Mapper.Map<Event>(dataEvent);

            await SendEventToSubscribers(request, result, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            return result;
        }

        private async Task SendEventToSubscribers(PostEventRequest request, Event result, CancellationToken cancellationToken)
        {
            var url = $"http://localhost:{request.Port}{EventsHub.HubPath}";
            try
            {
                var eventHubConnection = EventHubConnectionBuilder
                    .WithAccessToken(() => request.LazyToken.Value)
                    .WithUrl(url)
                    .Build();
                await eventHubConnection.StartAsync().ConfigureAwait(continueOnCapturedContext: false);
                await eventHubConnection.SendAsync("Send", result, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                await eventHubConnection.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Encountered exception when attempting to send posted event to SignalR subscribers, url: {url}");
            }
        }
    }
}
