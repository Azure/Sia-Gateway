//Todo: figure out what else should be in this file.

#Setting Up Your Development Environment

This Gateway is intended to be extended by Sia microservices in other code repositories.

In order to test functionality of the gateway on your local machine:
1. For each 'Service' in appsettings.Development.json:
	1. Clone the associated repository (Example: Incident is associated with the SiaManagement repository)
	2. Change the default startup from the project name to IIS Express and start
	3. You will need to have a separate visual studio instance open for each service the gateway will interact with.
	   (If you find a good way to do this without having visual studio open, please let the maintainers of this project know)
	4. Validate that the localhost port the service is running on matches its configuration in appsettings.Development.json.
		You can find IIS Express information by right clicking the IIS Express icon in your system tray.
2. Get appropriate Secrets for authentication
	1. Right click on the Sia.Gateway project and select "Manage user secrets"
	2. Open usersecrets.template.json to use as a template for your usersecrets json file
	3. You will need these configuration values:
		1. Your AAD instance (the default is correct for the vast majority of scenarios)
		2. Your AAD tenant
		3. Your Key Vault vault name
		4. Your ApplicationInsights instrumentation key name
		5. The secret that is used to authenticate to azure key vault for the gateway (ClientSecret)
			* This secret can be found in the azure portal in the AAD App Registration for your Gateway instance
3. Start the gateway


#Ticketing System Connectors

Sia can be configured to to work with a separate ticketing system,
either by leveraging a proxy API or by direct access with a custom connector.

Data from connected ticketing systems can be used to generate events and/or present
additional information and context on the UI.

Sia will still function without a connected ticketing system, and will persist limited
ticket data in the Incident database (see Sia.Data.Incident\Models\Ticket.cs)

For additional information on use, configuration, and creation of ticketing system
connectors, see Sia.Connectors.Tickets\README.md