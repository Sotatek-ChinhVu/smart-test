using CloudUnitTest.SampleData;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.MedicalCommon.ConvertConversionItemToOrderInfModel
{
    public class ConvertConversionItemToOrderInfModelTest : BaseUT
    {
        [Test]
        public void ConvertConversionItemToOrderInfModelTest_001()
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
            long raiiNo = 0;
            long ptId = 0;
            int sinDate = 02032023;
            List<OrdInfModel> odrInfItems = new();
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("", 0,"IpnTest")
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("", new TenItemModel());
            var result = todayOdrRepository.ConvertConversionItemToOrderInfModel(hpId, raiiNo, ptId, sinDate, odrInfItems, expiredItems);
            Assert.True(!result.Any());
        }

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_002()
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
            long raiiNo = 0;
            long ptId = 0;
            int sinDate = 20230202;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("", 0,"ipnNameTest")
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("", new TenItemModel());
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.ConvertConversionItemToOrderInfModel(hpId, raiiNo, ptId, sinDate, odrInfItems, expiredItems);
                Assert.True(result.Any());
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(ipnMinYakkaMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_003()
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

            var hpId = -1;
            long raiiNo = 0;
            long ptId = 0;
            int sinDate = 20230202;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("", 0,"ipnNameTest")
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("", new TenItemModel());
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.ConvertConversionItemToOrderInfModel(hpId, raiiNo, ptId, sinDate, odrInfItems, expiredItems);
                Assert.True(result.Any());
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(ipnMinYakkaMsts);
                tenant.SaveChanges();
            }
        }
    }
}
