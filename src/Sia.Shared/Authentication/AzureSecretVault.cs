using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Sia.Shared.Authentication
{
    public interface ISecretVault
    {
        Task<string> Get(string secretName);
    }
    public class AzureSecretVault : ISecretVault
    {
        private readonly string _vault;
        private readonly string _clientId;
        private readonly string _secret;

        public AzureSecretVault(IConfigurationRoot configuration,
            string vaultNameKey = "KeyVault:VaultName",
            string clientIdKey = "ClientId",
            string clientSecretKey = "ClientSecret")
        {
            _clientId = configuration[clientIdKey];
            _secret = configuration[clientSecretKey];
            _vault = String.Format(secretUriBase, configuration[vaultNameKey]);
        }

        public async Task<string> Get(string secretName)
        {
            try
            {
                var secret = await GetKeyVaultClient().GetSecretAsync(_vault + secretName).ConfigureAwait(false);
                return secret.Value;
            }
            catch (KeyVaultErrorException)
            {
                return string.Empty;
            }
        }

        public async Task<X509Certificate2> GetCertificate(string certificateName)
        {
            try
            {
                var cert = await GetKeyVaultClient().GetCertificateAsync(_vault, certificateName).ConfigureAwait(false);
                return new X509Certificate2(cert.Cer);
            }
            catch (KeyVaultErrorException)
            {
                return null;
            }
        }

        private async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(_clientId,
                        _secret);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }

        private const string secretUriBase = "https://{0}.vault.azure.net/secrets/";

        private KeyVaultClient GetKeyVaultClient() => new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));

    }

}
