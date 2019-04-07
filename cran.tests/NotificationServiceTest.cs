
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
    

        [Fact]
        public async Task TestGetAllSubscriptionsAsync()
        {
            
            //Setup
            TestingContext testingContext = new TestingContext();
            SetUpTestingContext(testingContext);
            await AddSampleSubscriptions(testingContext, null);    
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
            await AddSampleSubscriptions(testingContext, null); 
           
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
            Assert.Matches("TestText", messageSent);
        }

        [Fact]
        public async Task TestAddPushNotificationSubscriptionAsync()
        {
            //Setup
            TestingContext testingContext = new TestingContext();
            SetUpTestingContext(testingContext);

            INotificationService notificationService = testingContext.GetService<NotificationService>();                       
            NotificationSubscriptionDto dto = new NotificationSubscriptionDto()
            {
                Endpoint = "Test",
                Keys = new KeyDto(){
                    Auth = "TestAut",
                    P256dh = "testp256",
                },
                ExpirationTime = DateTime.Today.AddYears(3),
            };
            
            //Act
            await notificationService.AddPushNotificationSubscriptionAsync(dto); 

            //Assert
            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();
            NotificationSubscription sub = await dbContext.Notifications.LastAsync();
            
            Assert.Equal(dto.Endpoint, sub.Endpoint);
            Assert.True(sub.Id > 0);
        }

        [Fact]
        public async Task TestSendNotificationAboutQuestionAsync(){
            
            TestingContext testingContext = new TestingContext();
            SetUpTestingContext(testingContext);
            

            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();
            Question first = await dbContext.Questions.LastAsync();
            await AddSampleSubscriptions(testingContext, first.User);
       

            string messageSent = string.Empty;
            PushSubscription psSent = null;
            VapidDetails vapiDetSent = null;

            Mock<IWebPushClient> webPushClientMock =  new Mock<IWebPushClient>(MockBehavior.Loose);

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
            
            // Act
            await notificationService.SendNotificationAboutQuestionAsync(first.Id);

            //Assert
            webPushClientMock.Verify(x => x.SendNotificationAsync(It.IsAny<PushSubscription>(), 
                It.IsAny<string>(), It.IsAny<VapidDetails>()), Times.Once());

        }
    
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
        private async Task AddSampleSubscriptions(TestingContext testingContext, CranUser user)
        {
            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();
            IPrincipal principal =  testingContext.GetSimple<IPrincipal>();
            
            if(user == null)
            {
                user = await dbContext.CranUsers.FirstOrDefaultAsync(x => x.UserId == principal.Identity.Name);
            }

           
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