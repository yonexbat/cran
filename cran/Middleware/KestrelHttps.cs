using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace cran.Middleware
{
    public static class KestrelHttps
    {
        public static void ConfigureEndpoints(this KestrelServerOptions options, int port)
        {          
            IPAddress address = IPAddress.Loopback;
            options.Listen(address, port, listenOptions =>
            {

                HttpsConnectionAdapterOptions httpOptions = new HttpsConnectionAdapterOptions();
                httpOptions.ServerCertificate = GetHttpsCertificate();
                httpOptions.ClientCertificateMode = ClientCertificateMode.NoCertificate;               
                listenOptions.UseHttps(httpOptions);
            });
        }

        private static X509Certificate2 GetHttpsCertificate()
        {
            return CertificateUtils.CertificateFromFile(@".\certificates\https.pfx", "claude");
        }
    }
}
