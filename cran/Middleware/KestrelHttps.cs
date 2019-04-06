using cran.Model.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
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
                IOptions<CranSettingsDto> settings = (IOptions<CranSettingsDto>) options.ApplicationServices.GetService(typeof(IOptions<CranSettingsDto>));
                HttpsConnectionAdapterOptions httpOptions = new HttpsConnectionAdapterOptions
                {
                    ServerCertificate = GetHttpsCertificate(settings.Value.DevelopmentCertStorePath, settings.Value?.DevelopmentCertStorePw),
                    ClientCertificateMode = ClientCertificateMode.NoCertificate
                };
                listenOptions.UseHttps(httpOptions);
            });
        }

        private static X509Certificate2 GetHttpsCertificate(string path, string pw)
        {
            if(string.IsNullOrWhiteSpace(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "certificates", "httpstestcert.pfx");
            }
            return CertificateUtils.CertificateFromFile(path, pw);
        }
    }
}
