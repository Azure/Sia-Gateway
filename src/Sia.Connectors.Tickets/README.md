# Ticketing System Connectors

Sia can be configured to to work with a separate ticketing system,
either by leveraging a proxy API or by direct access using a custom connector.

Data from connected ticketing systems can be used to generate events and/or
present additional information and context on the UI.

Sia will still function without a connected ticketing system, and will
persist limited ticket data in the Incident database
(see Sia.Data.Incident\Models\Ticket.cs)

# Connector Modes

## Proxy Connector

A proxy connector will connect to an endpoint given in configuration to perform
operations on tickets; the proxy could connect to a separate ticketing system,
or to a database. 

To use a proxy connector, add the URL of the proxy to use in config path
Connector:Ticket:ProxyEndpoint. If a custom connector path is also provided,
the custom connector will be used instead of the proxy connector; configure 
only one or the other (for any given environment).

Proxy Connectors use the Sia.Connectors.Tickets.TicketProxy.Ticket type, which
may not be suitable for all use cases; if a custom ticket object is required,
use a Custom Connector instead.

### Authentication

Currently, two authentication modes are supported:

* Certificate Authentication
	* To use, set config Connector:Ticket:ProxyAuthType to Certificate
	* Looks for a locally installed cert with a thumbprint given in config
	path Connector:Ticket:ProxyCertThumbprint.
	* Creates a separate HttpClient that uses proxy auth by default to connect
* No Authentication
	* Default authentication mode
	* Connects without authentication
	* Limit use of this mode. Do not use for production.

## Custom Connector

A custom connector is a class library/assembly that provides:

* an implementation of Sia.Connectors.Tickets.Client
* an implementation of Sia.Connectors.Tickets.Converter
* (Optionally) a custom type to be used as TTicket for the client and converter
* a static Initialization class that:
	* Exists in the Sia.Gateway.Initialization namespace.
	* Provides a TicketType() method that returns the Type to be used as TTicket
	* Provides an AddConnector(IServiceCollection, IHostingEnvironment, IConfigurationRoot)
	method that registers a Client, a Converter, and a Connector in the service
	collection

To use a custom connector, add the file system path of a compiled connector
assembly DLL in config path Connector:Ticket:Path.

### Authentication

Authentication is handled entirely by the custom connector assembly

## No Connector

Sia will persist limited ticket data in the Incident database 
(see Sia.Data.Incident\Models\Ticket.cs) even if no connector is used. If you
do not require Sia to interact with an existing ticketing system, or if you
want to test Sia functionality in isolation, Sia will function without a
connector registered.

To avoid using a connector, ensure there are no values in config paths 
Connector:Ticket:ProxyEndpoint and Connector:Ticket:Path