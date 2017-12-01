using Microsoft.Extensions.Logging;
using Sia.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sia.Connectors.Tickets
{
    public class Connector
    {
        public Connector(TicketingClient client, ILoggerFactory loggerFactory)
        {
            Client = client;
            Logger = loggerFactory.CreateLogger<Connector>();
        }
        protected TicketingClient Client { get; }
        protected ILogger Logger { get; }
        public virtual async Task GetData(Ticket persistedTicket)
        {
            try
            {
                persistedTicket.Data = await Client.GetAsync(persistedTicket.OriginId);
            }
            catch (Exception ex)
            {
                Logger.LogError(
                    ex,
                    "Exception during GetAsync for ticket with Id: {0}",
                    new object[] { persistedTicket.Id }
                );
            }
        }

        public virtual void GetData(ICollection<Ticket> persistedTickets)
            => Task.WaitAll(
                persistedTickets
                .Select(tic => GetData(tic))
                .ToArray()
            );
    }
}
