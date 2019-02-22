using cran.Model.Dto.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface INotificationService
    {
        Task AddPushNotificationSubscriptionAsync(NotificationSubscriptionDto subscription);

        Task SendNotificationToUserAsync(int subId);
    }
}
