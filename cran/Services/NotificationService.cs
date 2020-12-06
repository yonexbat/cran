using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using cran.Model.Dto.Notification;
using cran.Model.Entities;
using cran.Services.Util;
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

        private readonly ITextService _textService;
        private readonly ISecurityService _securityService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserService _userService;


        public NotificationService(ApplicationDbContext context, 
            IDbLogService dbLogService, 
            ISecurityService securityService,
            IOptions<CranSettingsDto> settingsOption,
            IWebPushClient webPushClient,
            ITextService textService,
            IUserService userService) : base(context, dbLogService, securityService)
        {
            _settings = settingsOption.Value;
            _webPushClient = webPushClient;
            _textService = textService;
            _securityService = securityService;
            _dbContext = context;
            _userService = userService;
        }

        public async Task AddPushNotificationSubscriptionAsync(NotificationSubscriptionDto subscriptionDto)
        {
            await this._dbLogService.LogMessageAsync($"adding subscription: {subscriptionDto.Endpoint}");
            if (!_dbContext.Notifications.Any(x => x.Endpoint == subscriptionDto.Endpoint
            && x.Auth == subscriptionDto.Keys.Auth && x.Active))
            {
                NotificationSubscription entity = new NotificationSubscription();
                CopyDataSubscription(subscriptionDto, entity);
                entity.User = await _userService.GetOrCreateCranUserAsync();
                await _dbContext.Notifications.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }            
        }      
        
        private void CopyDataSubscription(NotificationSubscriptionDto dto, NotificationSubscription entity)
        {
            entity.Endpoint = dto.Endpoint;
            entity.Auth = dto.Keys?.Auth;
            entity.P256DiffHell = dto.Keys?.P256dh;
            entity.ExpirationTime = dto.ExpirationTime;
            entity.AsString = dto.AsString;
        }

        private string GetMessage(NotificationDto notificationDto)
        {                            
            JObject message = new JObject();
            JObject notification = new JObject();
            message["notification"] = notification;
            notification["title"] = notificationDto.Title;
            notification["body"] = notificationDto.Text;

            
            if(!string.IsNullOrEmpty(notificationDto.ActionUrl))
            {
                JArray actions = new JArray();
                notification["actions"] = actions;
                JObject actionObj = new JObject();
                actionObj["action"] = notificationDto.Action;
                actionObj["title"] = notificationDto.Title;
                actions.Add(actionObj);

                JObject data = new JObject();
                notification["data"] = data;
                data["url"] = notificationDto.ActionUrl;
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
            NotificationSubscription sub = await _dbContext.FindAsync<NotificationSubscription>(id);
            PushSubscription subscription = new PushSubscription();
            subscription.P256DH = sub.P256DiffHell;
            subscription.Auth = sub.Auth;
            subscription.Endpoint = sub.Endpoint;            
            return subscription;
        }

        public async Task<PagedResultDto<SubscriptionShortDto>> GetAllSubscriptionsAsync(int page)
        {
            IQueryable<NotificationSubscription> query = _dbContext.Notifications.Where(x => x.Active);
            PagedResultDto<SubscriptionShortDto> result = await PagedResultUtil.ToPagedResult(query, page, MaterializeSubscriptionList);
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
            PushSubscription sub = await GetPushSubsciption(notification.SubscriptionId);
            VapidDetails vapiData = GetVapiData();
            string message = GetMessage(notification);

            try
            {
                await _webPushClient.SendNotificationAsync(sub, message, vapiData);
            } 
            catch(WebPushException exception)
            {
                if (exception.Message == "Subscription no longer valid") {
                    await DeactivateSubscription(notification.SubscriptionId);
                }
                throw;
            }
        }
        
        private async Task DeactivateSubscription(int id)
        {
            NotificationSubscription entity =  await _dbContext.Notifications.FindAsync(id);
            entity.Active = false;
            _dbContext.SaveChanges();
        }

        public async Task SendNotificationAboutQuestionAsync(int questionId, string title, string text)
        {
            IList<int> subscriptionIds = await GetPushSubscriptions(questionId);
            foreach(int id in subscriptionIds){
                NotificationDto dto = new NotificationDto()
                {
                    SubscriptionId = id,
                    Text = text,
                    Title = title,
                    Action = "optionquestion",
                    ActionTitle = "Anzeigen",
                    ActionUrl = GetQuestionUrl(questionId),
                };
                try {
                    await SendNotificationToUserAsync(dto);
                }   
                catch(WebPushException)  
                {
                    //That is ok, we want to continue
                }           
            }
        }

        private string GetQuestionUrl(int id)
        {
            return $"{_settings.RootUrl}/jsclient/viewquestion/{id}";
        }

        private async Task<IList<int>> GetPushSubscriptions(int questionId)
        {

            var userIds = _dbContext.Questions
                 .Where(x => x.Container.Questions.Any(y => y.Id == questionId))
                 .Select(x => x.User.Id)
                 .Distinct();

            return await _dbContext.Notifications
                .Where(x => userIds.Any(y => y == x.User.Id))
                .Where(x => x.Active)
                .Select(x => x.Id)
                .ToListAsync();
        }
    }
}
