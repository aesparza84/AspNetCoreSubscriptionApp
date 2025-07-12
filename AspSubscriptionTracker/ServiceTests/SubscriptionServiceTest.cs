using AspSubscriptionTracker.Models;
using AspSubscriptionTracker.Services;
using AspSubscriptionTracker.Services.Contracts;
using AspSubscriptionTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using EntityFrameworkCoreMock;
using Moq;

namespace ServiceTests
{
    public class SubscriptionServiceTest
    {
        //https://stackoverflow.com/questions/54219742/mocking-ef-core-dbcontext-and-dbset

        private readonly ISubscriptionService subService;
        public SubscriptionServiceTest()
        {            
            
            //Mock the DBContext
            DbContextMock<ApplicationDbContext> mockContext = 
                new DbContextMock<ApplicationDbContext>(
                    new DbContextOptionsBuilder<ApplicationDbContext>().Options
                    );


            //Initial Data for DbSet
            var subscriptionList = new List<Subscription>() { };
            
            ApplicationDbContext context = mockContext.Object;
            mockContext.CreateDbSetMock(m => m.Subscriptions, subscriptionList);

            subService = new SubscriptionService(context);
        }


        [Fact]
        public async Task AddSubscription_SuccessfullAdd()
        {
            //Arrange                  
            Subscription testSub = new Subscription() 
            {
                Name = "Test",
                Price = 10,
                Email = "test@gmail.com",
                Category = CategoryTypeEnum.Streaming,
                RenewalType = RenewTypeEnum.Monthly,
                PurchaseDate = DateTime.Now
            };

            //Act 
            bool added = await subService.AddSubAsync(testSub);

            //Assert
            Assert.True(added);
        }

        [Fact]
        public async Task AddSubscription_NoDuplicates()
        {
            //Arrange                  
            Subscription testSub = new Subscription()
            {
                Name = "Test",
                Price = 10,
                Email = "test@gmail.com",
                Category = CategoryTypeEnum.Streaming,
                RenewalType = RenewTypeEnum.Monthly,
                PurchaseDate = DateTime.Now
            };

            Subscription testSubTwo = new Subscription()
            {
                Name = "Test",
                Price = 10,
                Email = "test@gmail.com",
                Category = CategoryTypeEnum.Streaming,
                RenewalType = RenewTypeEnum.Monthly,
                PurchaseDate = DateTime.Now
            };

            //Act 
            bool first = await subService.AddSubAsync(testSub);
            bool second = await subService.AddSubAsync(testSubTwo);

            //Assert
            Assert.True(first);
            Assert.False(second);
        }
    }
}