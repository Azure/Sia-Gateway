using Sia.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sia.Shared.Authentication
{
    public class KeyVaultCertificateRetriever
        : CertificateRetriever
    {
        private X509Certificate2 _certificate;
        private readonly string _certName;
        private readonly AzureSecretVault _certVault;

        public KeyVaultCertificateRetriever(AzureSecretVault certificateVault, string certificateName)
        {
            _certName = ThrowIf.NullOrWhiteSpace(certificateName, nameof(certificateName));
            _certVault = ThrowIf.Null(certificateVault, nameof(certificateVault));
        }

        private X509Certificate2 StoreCertificate(AzureSecretVault certificateVault, string certificateName)
        {
            var certTask = certificateVault.GetCertificate(certificateName);
            Task.WaitAll(new Task[] { certTask });
            if (certTask.IsCompleted)
            {
                _certificate = certTask.Result;
            }
            return _certificate;
        }

        public override X509Certificate2 Certificate => 
            _certificate is null 
            ? StoreCertificate(_certVault, _certName)
            : _certificate;
    }
}
;