using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using cran.Model.Dto.Notification;
using cran.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebPush;

namespace cran.Services
{
    public class NotificationService : CraniumService, INotificationService
    {

        private CranSettingsDto _settings;

        public NotificationService(ApplicationDbContext context, 
            IDbLogService dbLogService, 
            IPrincipal principal,
            IOptions<CranSettingsDto> settingsOption) : base(context, dbLogService, principal)
        {
            this._settings = settingsOption.Value;
        }

        public async Task AddPushNotificationSubscriptionAsync(NotificationSubscriptionDto subscriptionDto)
        {
            await this._dbLogService.LogMessageAsync($"adding subscription: {subscriptionDto.Endpoint}");
            if (!_context.Notifications.Any(x => x.Endpoint == subscriptionDto.Endpoint
            && x.Auth == subscriptionDto.Keys.Auth && x.Active))
            {
                NotificationSubscription entity = new NotificationSubscription();
                CopyData(subscriptionDto, entity);
                entity.User = await GetCranUserAsync();
                await this._context.Notifications.AddAsync(entity);
                await this._context.SaveChangesAsync();
            }            
        }        

        private string GetMessage(string title, string body)
        {
            return $"{{\"notification\": {{\"title\": \"{title}\",\"body\": \"{body}\"}}}}";
        }

        private VapidDetails GetVapiData()
        {
            VapidDetails vapidDetails = new VapidDetails();
            vapidDetails.PublicKey = this._settings.VapiPublicKey;
            vapidDetails.PrivateKey = this._settings.VapiPrivateKey;
            vapidDetails.Subject = this._settings.VapiSubject;
            return vapidDetails;
        }

        private async Task<PushSubscription> GetPushSubsciption(int id)
        {
            NotificationSubscription sub = await _context.FindAsync<NotificationSubscription>(id);
            PushSubscription subscription = new PushSubscription();
            subscription.P256DH = sub.P256DiffHell;
            subscription.Auth = sub.Auth;
            subscription.Endpoint = sub.Endpoint;            
            return subscription;
        }

        public async Task<PagedResultDto<SubscriptionShortDto>> GetAllSubscriptionsAsync(int page)
        {
            IQueryable<NotificationSubscription> query = _context.Notifications.Where(x => x.Active);
            PagedResultDto<SubscriptionShortDto> result = await ToPagedResult(query, page, MaterializeSubscriptionList);
            return result;
        }

        private async Task<IList<SubscriptionShortDto>> MaterializeSubscriptionList(IQueryable<NotificationSubscription> query)
        {
            IList<SubscriptionShortDto> result = await query.Select(x => new SubscriptionShortDto
            {
                Id = x.Id,
                Endpoint = x.Endpoint,
                UserId = x.User.UserId,
            }).ToListAsync();
            return result;
        }

        public async Task SendNotificationToUserAsync(NotificationDto notification)
        {
            WebPushClient client = new WebPushClient();
            PushSubscription sub = await GetPushSubsciption(notification.SubscriptionId);
            VapidDetails vapiData = GetVapiData();
            string message = GetMessage(notification.Title, notification.Text);

            try
            {
                await client.SendNotificationAsync(sub, message, vapiData);
            } 
            catch(WebPushException exception)
            {
                if (exception.Message == "Subscription no longer valid") {
                    await DeactivateSubscription(notification.SubscriptionId);
                }
                throw exception;
            }
        }
        
        private async Task DeactivateSubscription(int id)
        {
            NotificationSubscription entity =  await this._context.Notifications.FindAsync(id);
            entity.Active = false;
            _context.SaveChanges();
        }
    }
}
