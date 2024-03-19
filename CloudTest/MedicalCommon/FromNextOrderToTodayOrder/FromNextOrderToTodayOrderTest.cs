using Domain.Models.NextOrder;
using Domain.Models.OrdInfDetails;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.MedicalCommon.FromNextOrderToTodayOrder
{
    public class FromNextOrderToTodayOrderTest : BaseUT
    {
        [Test]
        public void FromNextOrderToTodayOrderTest_001()
        {
            int hpId = 1;
            int sinDate = 20240316;
            long raiinNo = 0;
            int userId = 1;
            List<RsvkrtOrderInfModel> rsvkrtOdrInfModels = new List<RsvkrtOrderInfModel>();
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            try
            {
                var result = todayOdrRepository.FromNextOrderToTodayOrder(hpId, sinDate, raiinNo, userId, rsvkrtOdrInfModels);
                Assert.True(!result.Any());
            }
            finally
            {
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void FromNextOrderToTodayOrderTest_002()
        {
            int hpId = 1;
            int sinDate = 20240316;
            long raiinNo = 0;
            int userId = 1;
            List<RsvKrtOrderInfDetailModel> ordInfDetailModels = new List<RsvKrtOrderInfDetailModel>()
            {
                new RsvKrtOrderInfDetailModel(hpId, "","",0)
            };
            List<RsvkrtOrderInfModel> rsvkrtOdrInfModels = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(ordInfDetailModels)
            };
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            try
            {
                var result = todayOdrRepository.FromNextOrderToTodayOrder(hpId, sinDate, raiinNo, userId, rsvkrtOdrInfModels);
                Assert.True(result.Any());
            }
            finally
            {
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }
        private void SetupTestEnvironment(out SystemConfRepository systemConfRepository, out UserRepository userRepository, out ApprovalinfRepository approvalinfRepository, out TodayOdrRepository todayOdrRepository)
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            var mockUserService = new Mock<IUserInfoService>();
            systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
            approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
            todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository, approvalinfRepository);
        }
        private void CleanupResources(SystemConfRepository systemConfRepository, UserRepository userRepository, ApprovalinfRepository approvalinfRepository, TodayOdrRepository todayOdrRepository)
        {
            systemConfRepository.ReleaseResource();
            userRepository.ReleaseResource();
            approvalinfRepository.ReleaseResource();
            todayOdrRepository.ReleaseResource();
        }
    }
}
