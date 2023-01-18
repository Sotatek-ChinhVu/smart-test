using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PostgreDataContext;

namespace CloudUnitTest
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
                var noTrackingOptions = new DbContextOptionsBuilder<TenantNoTrackingDataContext>().UseNpgsql(_unittestDBConnectionString, buider =>
                {
                    buider.EnableRetryOnFailure(maxRetryCount: 3);
                }).LogTo(Console.WriteLine, LogLevel.Information).Options;
                var factory = new PooledDbContextFactory<TenantDataContext>(options);
                var noTrackingFactory = new PooledDbContextFactory<TenantNoTrackingDataContext>(noTrackingOptions);
                mockTenantProvider.Setup(repo => repo.GetTrackingTenantDataContext()).Returns(factory.CreateDbContext());
                mockTenantProvider.Setup(repo => repo.GetNoTrackingDataContext()).Returns(noTrackingFactory.CreateDbContext());

                return mockTenantProvider.Object;
            }
        }
    }
}
