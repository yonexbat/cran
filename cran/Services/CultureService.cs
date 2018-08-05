using cran.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace cran.Services
{
    public class CultureService : ICultureService
    {
        IHttpContextAccessor _httpContextAccessor;

        public CultureService(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }

        public Language GetCurrentLanguage()
        {
            IRequestCultureFeature requestCultureFeature = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
            var uiCultureInfo = requestCultureFeature.RequestCulture.Culture;          
            if (uiCultureInfo != null && uiCultureInfo.Name.EndsWith("en"))
            {
                return Language.En;
            }
            return Language.De;
        }
    }
}
