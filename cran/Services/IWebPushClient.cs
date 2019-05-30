using System.Threading.Tasks;
using WebPush;

namespace cran.Services
{
    public interface IWebPushClient {
         Task SendNotificationAsync(PushSubscription subscription, string payload, VapidDetails vapidDetails);       
    }
}