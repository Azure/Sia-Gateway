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
        public ILogger Logger { get; }
        public virtual async Task AppendDataAsync(Ticket persistedTicket)
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

        public virtual void AppendData(ICollection<Ticket> persistedTickets)
            => Task.WaitAll(
                persistedTickets
                .Select(tic => AppendDataAsync(tic))
                .ToArray()
            );

        
    }
}
