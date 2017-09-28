using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sia.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Hubs
{
    public class EventsHub : Hub
    {
        public const string HubPath = "events/live";
        public EventsHub() : base()
        {

        }

        public Task Send(Event ev)
        {
            return Clients.All.InvokeAsync("Send", JsonConvert.SerializeObject(ev, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
