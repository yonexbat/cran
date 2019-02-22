using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto.Notification;
using cran.Model.Entities;

namespace cran.Services
{
    public class NotificationService : CraniumService, INotificationService
    {

        

        public NotificationService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) : base(context, dbLogService, principal)
        {
        }

        public async Task AddPushNotificationSubscription(NotificationSubscriptionDto subscriptionDto)
        {
            await this._dbLogService.LogMessageAsync($"adding subscription: {subscriptionDto.AsString}");
            if (!_context.Notifications.Any(x => x.AsString == subscriptionDto.AsString))
            {
                NotificationSubscription entity = new NotificationSubscription();
                CopyData(subscriptionDto, entity);
                entity.User = await GetCranUserAsync();
                await this._context.Notifications.AddAsync(entity);
                await this._context.SaveChangesAsync();
            }            
        }
    }
}
