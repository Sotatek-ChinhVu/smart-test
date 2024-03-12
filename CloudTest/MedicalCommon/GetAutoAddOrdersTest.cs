using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.AccessControl;

namespace CloudUnitTest.MedicalCommon
{
    public class GetAutoAddOrdersTest : BaseUT
    {
        [Test]
        public void GetAutoAddOrders_001()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            var mockUserService = new Mock<IUserInfoService>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
            ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository, approvalinfRepository);

            var hpId = 1;
            long ptId = 0;
            int sinDate = 20230303;
            List<Tuple<int, int, string>> addingOdrList = new List<Tuple<int, int, string>>();
            List<Tuple<int, int, string, double, int>> currentOdrList = new List<Tuple<int, int, string, double, int>>();
            var result= todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
            try
            {
                // Assert
                Assert.True(!result.Any());
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
            }
        }

        [Test]
        public void GetAutoAddOrders_002()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            var mockUserService = new Mock<IUserInfoService>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
            ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository, approvalinfRepository);

            var hpId = 1;
            long ptId = 0;
            int sinDate = 20230303;
            List<Tuple<int, int, string>> addingOdrList = new List<Tuple<int, int, string>>()
            {
                new Tuple<int, int, string>(1, 1, "")
            };
            List<Tuple<int, int, string, double, int>> currentOdrList = new List<Tuple<int, int, string, double, int>>()
            {
                new Tuple<int, int, string, double, int>(1, 1, "", 3.14, 1)
            };
            todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
        }

        [Test]
        public void GetAutoAddOrders_003()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            var mockUserService = new Mock<IUserInfoService>();
            SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
            ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository, approvalinfRepository);

            var hpId = 1;
            long ptId = 0;
            int sinDate = 20230303;
            List<Tuple<int, int, string>> addingOdrList = new List<Tuple<int, int, string>>()
            {
                new Tuple<int, int, string>(1, 1, "test")
            };
            List<Tuple<int, int, string, double, int>> currentOdrList = new List<Tuple<int, int, string, double, int>>()
            {
                new Tuple<int, int, string, double, int>(1, 1, "", 3.14, 1)
            };
            todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
        }
    }
}
