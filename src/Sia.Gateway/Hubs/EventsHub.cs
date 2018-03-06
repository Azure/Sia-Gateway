﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sia.Domain;
using Sia.Gateway.Filters;
using Sia.Shared.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Hubs
{
    [Authorize()]
    public class EventsHub : Hub
    {
        public const string HubPath = "events/live";
        private readonly ConcurrentDictionary<string, IFilterByMatch<Event>> _filterLookup;
        private readonly ILogger<EventsHub> _logger;

        public EventsHub(
            ConcurrentDictionary<string, IFilterByMatch<Event>> filterLookup,
            ILoggerFactory loggerFactory
        ) : base()
        {
            _filterLookup = filterLookup;
            _logger = loggerFactory.CreateLogger<EventsHub>();
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _logger.LogInformation(
                "User {0} connected to EventsHub on connectionId {1}",
                new object[] { Context.User.Identity.Name, Context.ConnectionId }
            );
            await ClearFilter();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            if(exception == null)
            {
                _logger.LogInformation(
                    "User {0} disconnected from EventsHub on connectionId {1} without exception",
                    new object[] { Context.User.Identity.Name, Context.ConnectionId }
                );
            }
            else
            {
                _logger.LogError(
                    exception,
                    "User {0} disconnected from EventsHub on connectionId {1} with exception; see exception for details.",
                    new object[] { Context.User.Identity.Name, Context.ConnectionId }
                );
            }
        }

        public Task Send(Event ev)
            => Clients.AllExcept(
                    _filterLookup
                        .Where((kvp) => !kvp.Value.IsMatchFor(ev))
                        .Select((kvp) => kvp.Key)
                        .ToList()
                ).InvokeAsync("Send", Json(ev));

        public Task UpdateFilter(EventFilters filters)
        {
            _filterLookup.Upsert(Context.ConnectionId, filters);
            return Task.CompletedTask;
        }

        public Task<bool> ClearFilter()
        {
            var filterRemoved = _filterLookup.TryRemove(Context.ConnectionId, out var unused);
            if(filterRemoved)
            {
                _logger.LogInformation(
                    "Successfully cleared filter for connection with id {0}",
                    new object[] { Context.ConnectionId }
                );
            }
            else
            {
                _logger.LogWarning(
                    "FAILED to clear filter for connection with id {0}",
                    new object[] { Context.ConnectionId }
                );
            }
            return Task.FromResult(filterRemoved);
        }
 

        private string Json<T>(T toSerialize) => JsonConvert.SerializeObject(toSerialize, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        private IClientProxy Caller => Clients.Client(Context.ConnectionId);
    }
}
