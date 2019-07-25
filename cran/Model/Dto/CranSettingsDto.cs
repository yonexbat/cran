using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class CranSettingsDto
    {
        public string VapiPublicKey { get; set; }
        public string VapiPrivateKey { get; set; }
        public string VapiSubject { get; set; }

        public string ConnectionString { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string DevelopmentCertStorePw { get; set; }
        public string DevelopmentCertStorePath { get; set; }

        public string RootUrl { get; set; }
    }
}
