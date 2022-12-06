using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
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
                mockTenantProvider.Setup(repo => repo.GetTrackingTenantDataContext()).Returns(new TenantDataContext(_unittestDBConnectionString));
                return mockTenantProvider.Object;
            }
        }
    }
}
