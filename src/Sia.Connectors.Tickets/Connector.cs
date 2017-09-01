using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Connectors.Tickets
{
    public class Connector<TTicket>
    {
        public Client<TTicket> Client { get; }
        public Converter<TTicket> Converter { get; }

        public Connector(Client<TTicket> client, Converter<TTicket> converter)
        {
            Client = client;
            Converter = converter;
        }
    }
}
