namespace Sia.Gateway.Initialization.Configuration
{
    public class KeyVaultAccessorConfiguration
    {
        public string VaultName { get; set; }
        
        public string GatewayDatabaseConnectionStringName { get; set; }
        public string GatewayRedisPasswordName { get; set; }
    }
}
