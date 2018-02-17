using cran.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.ViewModel
{
    public class LoginViewModel
    {
        public string ReturnUrl { get; set; }

        public string LoginInfoText { get; set; }
                
        public IList<LoginProviderDto> LoginProviders { get; set; }
    }
}
