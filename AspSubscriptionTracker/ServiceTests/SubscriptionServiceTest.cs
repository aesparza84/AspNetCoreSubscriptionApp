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

            ApplicationDbContext context = mockContext.Object;

            //Initial Data for DbSet
            var subscriptionList = new List<Subscription>() { };
            
            //Create a Mock DbSet
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
            Guid returnedId = await subService.AddSubAsync(testSub);

            //Assert
            Assert.NotEqual(Guid.Empty, returnedId);
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
            Guid first = await subService.AddSubAsync(testSub);
            Guid second = await subService.AddSubAsync(testSubTwo);

            //Assert
            Assert.NotEqual(Guid.Empty, first);
            Assert.Equal(Guid.Empty, second);
        }

        [Fact]
        public async Task GetSubById_ValidId()
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
            Guid id = await subService.AddSubAsync(testSub);

            Subscription requestSub = await subService.FindAsync(id);

            //Assert
            Assert.NotNull(requestSub);
        }

        [Fact]
        public async Task GetSubById_InvalidId()
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
            Guid id = Guid.NewGuid();

            Subscription requestSub = await subService.FindAsync(id);

            //Assert
            Assert.Null(requestSub);
        }

        [Fact]
        public async void UpdateSusbcription_NameChange()
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
            Guid id = await subService.AddSubAsync(testSub);

            Subscription incomingSub = new Subscription()
            {
                Id = id,
                Name = "DifferentName",
                Price = 10,
                Email = "test@gmail.com",
                Category = CategoryTypeEnum.Streaming,
                RenewalType = RenewTypeEnum.Monthly,
                PurchaseDate = DateTime.Now
            };

            bool updated = subService.Update(incomingSub);

            //Assert
            Assert.True(updated);
        }

        [Fact]
        public async void UpdateSusbcription_UpdatedEmailIsntUsedWithThisSuscriptionYet()
        {
            //Arrange                  
            Subscription firstSub = new Subscription()
            {
                Name = "Test",
                Price = 10,
                Email = "test@gmail.com",
                Category = CategoryTypeEnum.Streaming,
                RenewalType = RenewTypeEnum.Monthly,
                PurchaseDate = DateTime.Now
            };

            Subscription secondSub = new Subscription()
            {
                Name = "Ooboo",
                Price = 10,
                Email = "WaWa@gmail.com",
                Category = CategoryTypeEnum.Streaming,
                RenewalType = RenewTypeEnum.Monthly,
                PurchaseDate = DateTime.Now
            };

            //Act 
            await subService.AddSubAsync(firstSub);
            Guid id = await subService.AddSubAsync(secondSub);

                //We are changing the 2nd subscription email to test@gmail.com
            Subscription incomingSub = new Subscription()
            {
                Id = id,
                Name = "Ooboo",
                Price = 10,
                Email = "test@gmail.com",
                Category = CategoryTypeEnum.Streaming,
                RenewalType = RenewTypeEnum.Monthly,
                PurchaseDate = DateTime.Now
            };

            bool updated = subService.Update(incomingSub);

            //Assert
            Assert.True(updated);
        }

        [Fact]
        public async void UpdateSusbcription_UpdatedEmailAlreadyUsedWithThisSubsctiption()
        {
            //Arrange                  
            Subscription firstSub = new Subscription()
            {
                Name = "Test",
                Price = 10,
                Email = "test@gmail.com",
                Category = CategoryTypeEnum.Streaming,
                RenewalType = RenewTypeEnum.Monthly,
                PurchaseDate = DateTime.Now
            };

            Subscription secondSub = new Subscription()
            {
                Name = "Test",
                Price = 10,
                Email = "WaWa@gmail.com",
                Category = CategoryTypeEnum.Streaming,
                RenewalType = RenewTypeEnum.Monthly,
                PurchaseDate = DateTime.Now
            };

            //Act 
            await subService.AddSubAsync(firstSub);
            Guid id = await subService.AddSubAsync(secondSub);

            //We are changing the 2nd subscription email to test@gmail.com
            Subscription incomingSub = new Subscription()
            {
                Id = id,
                Name = "Test",
                Price = 10,
                Email = "test@gmail.com",
                Category = CategoryTypeEnum.Streaming,
                RenewalType = RenewTypeEnum.Monthly,
                PurchaseDate = DateTime.Now
            };

            bool updated = subService.Update(incomingSub);

            //Assert
            Assert.False(updated);
        }
    }
}