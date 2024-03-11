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
        #region [test ConvertConversionItemToDetailModel]
        [Test]
        public void ConvertConversionItemToOrderInfModelTest_004()
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
            int sinDate = 20240202;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest", 0,"ipnNameTest")
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("ItemCdTest", new TenItemModel("ItemCdTest"));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
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
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
            }
        }
        [Test]
        public void ConvertConversionItemToOrderInfModelTest_005()
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
            int sinDate = 20240202;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 0,"ipnNameTest")
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("ItemCd1", new TenItemModel("ItemCd1"));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
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
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_006()
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
            int sinDate = 20240202;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 0,"")
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("ItemCd1", new TenItemModel("ItemCd1"));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
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
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_007()
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
            int sinDate = 20240202;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20,"", 0, 1)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("ItemCd1", new TenItemModel("ItemCd1"));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
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
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_008()
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
            int sinDate = 20240202;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20,"", 1, 1)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("ItemCd1", new TenItemModel("ItemCd1"));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
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
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_009()
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
            int sinDate = 20240202;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20,"", 2, 1)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("ItemCd1", new TenItemModel("ItemCd1"));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
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
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_010()
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
            int sinDate = 20240202;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20,"", 2, 1,3)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("ItemCd1", new TenItemModel("ItemCd1"));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
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
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_011()
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
            int sinDate = 20240202;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest", 61,"", 2, 1,3)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("ItemCdTest", new TenItemModel("ItemCdTest"));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst();
            var kensaMsts = ConvertConversionItemToDetailModelData.ReadKensaMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
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
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.SaveChanges();
            }
        }
        [Test]
        public void ConvertConversionItemToOrderInfModelTest_012()
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
            int sinDate = 20240202;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 61,"ipnNameTest", 2, 1,3)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("ItemCd1", new TenItemModel("ItemCd1"));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst();
            var kensaMsts = ConvertConversionItemToDetailModelData.ReadKensaMst();
            var ipnNameMsts = ConvertConversionItemToDetailModelData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
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
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
            }
        }
        #endregion [test ConvertConversionItemToDetailModel]
    }
}
