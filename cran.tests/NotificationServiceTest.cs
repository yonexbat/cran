
using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services;
using cran.tests.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using cran.Model.Dto.Notification;
using Moq;
using WebPush;

namespace cran.tests
{

    public class NotificationServiceTest {
        
        private void SetUpTestingContext(TestingContext testingContext)
        {
            testingContext.AddPrincipalMock();
            testingContext.AddBinaryServiceMock();
            testingContext.AddMockLogService();
            testingContext.AddCranAppSettings();
            testingContext.AddInMemoryDb();   

            Mock<IWebPushClient> webPushClientMock =  new Mock<IWebPushClient>(MockBehavior.Loose);
            webPushClientMock.Setup(x => x.SendNotificationAsync(It.IsAny<PushSubscription>(), 
                    It.IsAny<string>(), It.IsAny<VapidDetails>()))
                .Returns(Task.CompletedTask);
            
            testingContext.DependencyMap[typeof(IWebPushClient)] = webPushClientMock.Object;
        }

        [Fact]
        public async Task GetAllSubscriptionsAsync()
        {
            
            //Setup
            TestingContext testingContext = new TestingContext();
            SetUpTestingContext(testingContext);
            await AddSampleSubscriptions(testingContext);    
            INotificationService notificationService = testingContext.GetService<NotificationService>();        

            //Act            
            PagedResultDto<SubscriptionShortDto> result =  await notificationService.GetAllSubscriptionsAsync(0);
            
            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal("testendpoint", result.Data[0].Endpoint);
        }

        [Fact]
        public async Task TestSendNotificationAsync(){
            
            //Setup
            TestingContext testingContext = new TestingContext();
            SetUpTestingContext(testingContext);
            await AddSampleSubscriptions(testingContext); 
           
            Mock<IWebPushClient> webPushClientMock =  new Mock<IWebPushClient>(MockBehavior.Loose);


            string messageSent = string.Empty;
            PushSubscription psSent = null;
            VapidDetails vapiDetSent = null;

            webPushClientMock.Setup(x => x.SendNotificationAsync(It.IsAny<PushSubscription>(), 
                It.IsAny<string>(), It.IsAny<VapidDetails>()))
                .Callback<PushSubscription, string, VapidDetails>((ps, message, vapiDet) => {                                               
                    messageSent = message;
                    psSent = ps;
                    vapiDetSent = vapiDet;

                })
                .Returns(Task.CompletedTask);

            testingContext.DependencyMap[typeof(IWebPushClient)] = webPushClientMock.Object;
            INotificationService notificationService = testingContext.GetService<NotificationService>();

            NotificationDto dto = new NotificationDto();
            dto.SubscriptionId = testingContext.GetSimple<ApplicationDbContext>().Notifications.First().Id;
            dto.Text = "TestText";
            dto.Title = "TestTitle";

            
            

            //Act
            await notificationService.SendNotificationToUserAsync(dto);


            //Assert
            webPushClientMock.Verify(x => x.SendNotificationAsync(It.IsAny<PushSubscription>(), 
                It.IsAny<string>(), It.IsAny<VapidDetails>()), Times.Once());

        }

    

        private async Task AddSampleSubscriptions(TestingContext testingContext)
        {
            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();
            IPrincipal principal =  testingContext.GetSimple<IPrincipal>();
            
            CranUser user = await dbContext.CranUsers.FirstOrDefaultAsync(x => x.UserId == principal.Identity.Name);

           
            dbContext.Notifications.Add( new NotificationSubscription()
            {
                 Active = true,
                 Endpoint = "testendpoint",
                 User = user,
            });

            
            dbContext.Notifications.Add( new NotificationSubscription()
            {
                 Active = false,
                 Endpoint = "testendpoint",
                 User = user,
            });

            await dbContext.SaveChangesAsync();
        }
    }

}