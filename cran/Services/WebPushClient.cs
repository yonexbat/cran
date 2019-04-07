using System.Threading.Tasks;
using WebPush;

namespace cran.Services
{
    public class WebPushClient : IWebPushClient
    {
        public async Task SendNotificationAsync(PushSubscription subscription, string payload, VapidDetails vapidDetails)
        {
            WebPush.WebPushClient client = new WebPush.WebPushClient();
            await client.SendNotificationAsync(subscription, payload, vapidDetails);
        }
    }
}