using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto.Notification;
using cran.Model.Entities;
using Microsoft.EntityFrameworkCore;
using WebPush;

namespace cran.Services
{
    public class NotificationService : CraniumService, INotificationService
    {

        

        public NotificationService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) : base(context, dbLogService, principal)
        {
        }

        public async Task AddPushNotificationSubscriptionAsync(NotificationSubscriptionDto subscriptionDto)
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

        public async Task SendNotificationToUserAsync(int subId)
        {
            WebPushClient client = new WebPushClient();
            PushSubscription sub = await GetPushSubsciption();
            VapidDetails vapiData = GetVapiData();
            string message = GetMessage();

            await client.SendNotificationAsync(sub, message, vapiData);
        }

        private string GetMessage()
        {
            return @"{""notification"": {""title"": ""Angular News"",""body"": ""Newsletter Available!""}}";
        }

        private VapidDetails GetVapiData()
        {
            VapidDetails vapidDetails = new VapidDetails();
            vapidDetails.PublicKey = "BBexMQInwvBFQtqWi9Px9FrhnzEmp0drOs4nkYGcopy_0TQjJ5jUKn7dBDTor_Ma5--Oq8rsseRl2m-dN9iyazU";
            vapidDetails.PrivateKey = "TgPuP3hErzuIjTYg_bcYCkOa0GvfGNNUbeiuQpipX3o";
            vapidDetails.Subject = "mailto:public@cladue-glauser.ch";
            return vapidDetails;
        }

        private async Task<PushSubscription> GetPushSubsciption()
        {
            NotificationSubscription sub = await _context.Notifications
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();
            PushSubscription subscription = new PushSubscription();
            subscription.P256DH = sub.P256DiffHell;
            subscription.Auth = sub.Auth;
            subscription.Endpoint = sub.Endpoint;            
            return subscription;
        }

    }
}
