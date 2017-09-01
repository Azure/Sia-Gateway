//Todo: figure out what else should be in this file.

---Setting Up Your Development Environment---

This Gateway is intended to be extended by Sia microservices in other code repositories.

In order to test functionality of the gateway on your local machine:
1. For each 'Service' in appsettings.Development.json:
	a. Clone the associated repository (Example: Incident is associated with the SiaManagement repository)
	b. Change the default startup from the project name to IIS Express and start
	c. You will need to have a separate visual studio instance open for each service the gateway will interact with.
	   (If you find a good way to do this without having visual studio open, please let the maintainers of this project know)
	d. Validate that the localhost port the service is running on matches its configuration in appsettings.Development.json.
		You can find IIS Express information by right clicking the IIS Express icon in your system tray.
2. Get appropriate Secrets for authentication
	a. ClientSecret is the secret that is used to authenticate to azure key vault for the gateway
	b. Crsis:ClientSecret is the secret used to authenticate to the incident API
	c. Both of these secrets can be found in the azure portal in the LiveSite-Gateway-dev instance
	d. Right click on the Sia.Gateway project and select "Manage user secrets"
	e. Paste the secrets in in the followingt format:
		{
		  "Incident:ClientSecret" :"secretfromportal",
		  "ClientSecret": "secretfromportal"
		}
3. Start the gateway


---Ticketing System Connectors---

Sia can be configured to to work with a separate ticketing system,
either by leveraging a proxy API or by direct access with a custom connector.

Data from connected ticketing systems can be used to generate events and/or present
additional information and context on the UI.

Sia will still function without a connected ticketing system, and will persist limited
ticket data in the Incident database (see Sia.Data.Incident\Models\Ticket.cs)

For additional information on use, configuration, and creation of ticketing system
connectors, see Sia.Connectors.Tickets\README.md