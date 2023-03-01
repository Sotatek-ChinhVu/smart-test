using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PostgreDataContext;

namespace CalculateUnitTest
{
    public class BaseUT
    {
        private string _unittestDBConnectionString;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("unittest.env.json", false, true)
                .Build();

            _unittestDBConnectionString = config["UnitTestDB"];
        }

        protected ITenantProvider TenantProvider
        {
            get
            {
                var mockTenantProvider = new Mock<ITenantProvider>();

                var options = new DbContextOptionsBuilder<TenantDataContext>().UseNpgsql(_unittestDBConnectionString, buider =>
                {
                    buider.EnableRetryOnFailure(maxRetryCount: 3);
                }).LogTo(Console.WriteLine, LogLevel.Information).Options;
                var factory = new PooledDbContextFactory<TenantDataContext>(options);
                mockTenantProvider.Setup(repo => repo.GetTrackingTenantDataContext()).Returns(factory.CreateDbContext());
                
                return mockTenantProvider.Object;
            }
        }
    }
}
