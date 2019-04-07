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
using Newtonsoft.Json.Linq;
using WebPush;

namespace cran.Services
{
    public class NotificationService : CraniumService, INotificationService
    {

        private CranSettingsDto _settings;
        private IWebPushClient _webPushClient;

        public NotificationService(ApplicationDbContext context, 
            IDbLogService dbLogService, 
            IPrincipal principal,
            IOptions<CranSettingsDto> settingsOption,
            IWebPushClient webPushClient) : base(context, dbLogService, principal)
        {
            this._settings = settingsOption.Value;
            this._webPushClient = webPushClient;
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

        private string GetMessage(string title, string body, int? questionId)
        {                            
            JObject message = new JObject();
            JObject notification = new JObject();
            message["notification"] = notification;
            notification["title"] = title;
            notification["body"] = body;

            
            if(questionId.HasValue)
            {
                JArray actions = new JArray();
                notification["actions"] = actions;
                JObject actionObj = new JObject();
                actionObj["action"] = "gotoquestion";
                actionObj["title"] = "Öffnen";
                actions.Add(actionObj);

                JObject data = new JObject();
                notification["data"] = data;
                data["questionid"] = questionId.Value;
            }
            
            return message.ToString();
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
            await SendNotificationToUserAsync(notification, null);
        }

         public async Task SendNotificationToUserAsync(NotificationDto notification, int? questionId)
        {            
            PushSubscription sub = await GetPushSubsciption(notification.SubscriptionId);
            VapidDetails vapiData = GetVapiData();
            string message = GetMessage(notification.Title, notification.Text, questionId);

            try
            {
                await _webPushClient.SendNotificationAsync(sub, message, vapiData);
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

        public async Task SendNotificationAboutQuestionAsync(int questionId)
        {
            IList<int> subscriptionIds = await GetPushSubscriptions(questionId);
            foreach(int id in subscriptionIds){
                NotificationDto dto = new NotificationDto()
                {
                    SubscriptionId = id,
                    Text = "Update to question",
                    Title = "Update to question",

                };
                try {
                    await SendNotificationToUserAsync(dto, questionId);
                }   
                catch(WebPushException)  
                {
                    //That is ok, we want to continue
                }           
            }
        }

        private async Task<IList<int>> GetPushSubscriptions(int questionId)
        {
          
            Question question = await _context.FindAsync<Question>(questionId);
            int userId = question.User.Id;
            
            return await _context.Notifications.Where(x => x.User.Id == userId && x.Active)
                .Select(x => x.Id).ToListAsync();
        }
    }
}
