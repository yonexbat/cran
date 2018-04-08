using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace cran.Middleware
{
    public class CertificateUtils
    {
        public static bool ValidateCientCertificate(X509Certificate2 clientCertificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
        {
            CertificateUtils certificateVerifier = new CertificateUtils();
            return certificateVerifier.AddCaFromFile(@".\certificates\iampublic.cer")
                .VerifyCertificate(clientCertificate);
        }

        public static X509Certificate2 CertificateFromFile(string pathToCertificateFile)
        {
            X509Certificate2 certificate = new X509Certificate2(pathToCertificateFile);
            return certificate;
        }

        public static X509Certificate2 CertificateFromFile(string pathToCertificateFile, string pwd)
        {
            X509Certificate2 certificate = new X509Certificate2(pathToCertificateFile, pwd);
            return certificate;
        }


        IList<X509Certificate2> caList = new List<X509Certificate2>();

        public CertificateUtils AddCa(X509Certificate2 certificate)
        {
            this.caList.Add(certificate);
            return this;
        }

        public CertificateUtils AddCaFromFile(string pathToCertificateFile)
        {
            X509Certificate2 certificate = CertificateFromFile(pathToCertificateFile);
            caList.Add(certificate);
            return this;
        }

        public bool VerifyCertificate(X509Certificate2 clientCertificate)
        {
            X509Chain chain = new X509Chain();
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
            chain.ChainPolicy.VerificationTime = DateTime.Now;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 0);

            foreach (var ca in caList)
            {
                chain.ChainPolicy.ExtraStore.Add(ca);
            }

            bool valid = chain.Build(clientCertificate);
            if (!valid)
            {
                string[] errors = chain.ChainStatus
                 .Select(x => String.Format("{0} ({1})", x.StatusInformation.Trim(), x.Status))
                 .ToArray();

                string certificateErrorsString = "Unknown errors.";

                if (errors != null && errors.Length > 0)
                {
                    certificateErrorsString = String.Join(", ", errors);
                }
                return false;
            }

            valid = chain.ChainElements
                .Cast<X509ChainElement>()
                .Any(x => caList.Any(y => x.Certificate.Thumbprint == y.Thumbprint));

            return valid;
        }
    }
}
