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
        public Connector(TicketingClient client, ILogger logger)
        {
            Client = client;
            Logger = logger;
        }
        protected TicketingClient Client { get; }
        protected ILogger Logger { get; }
        public virtual async Task<Ticket> GetData(Ticket persistedTicket)
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
            
            return persistedTicket;
        }

        public virtual Task<IEnumerable<Ticket>> GetData(IEnumerable<Ticket> persistedTickets)
        {
            var hydrationTasks = persistedTickets
                .Select(tick => GetData(tick))
                .ToArray();
            Task.WaitAll(hydrationTasks);
            return Task.FromResult(
                hydrationTasks.Select(task => task.Result)
            );
        }
    }
}
