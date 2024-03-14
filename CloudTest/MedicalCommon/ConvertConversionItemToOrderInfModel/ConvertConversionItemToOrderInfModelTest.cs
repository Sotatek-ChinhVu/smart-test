using CloudUnitTest.SampleData;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Helper.Constants;
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
            var sufix = "004";
            var itemCd = "ItemCd" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 0,"ipnNameTest")
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
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
            var sufix = "005";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 0,"ipnNameTest")
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
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
            var sufix = "006";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 0,"")
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
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
            var sufix = "007";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 20,"", 0, 1)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
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
            var sufix = "008";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 20,"", 1, 1)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
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
            var sufix = "009";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 20,"", 2, 1)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
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
            var sufix = "010";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 20,"", 2, 1,3)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
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
            var sufix = "011";
            var itemCd = "ItemCd" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 61,"", 2, 1,3)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
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
            var sufix = "012";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 61,"ipnNameTest", 2, 1,3)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
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

        #region [test GetKensaGaichu]
        [Test]
        public void ConvertConversionItemToOrderInfModelTest_013()
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
            var sufix = "013";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("", 0,"ipnNameTest", 2, 1,3)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("", new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
            var kensaMsts = ConvertConversionItemToDetailModelData.ReadKensaMst();
            var ipnNameMsts = ConvertConversionItemToDetailModelData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                bool isTrue = true;
                var result = todayOdrRepository.ConvertConversionItemToOrderInfModel(hpId, raiiNo, ptId, sinDate, odrInfItems, expiredItems);
                foreach (var item in result)
                {
                    foreach (var item2 in item.OrdInfDetails)
                    {
                        if (item2.KensaGaichu == KensaGaichuTextConst.NONE)
                        {
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                            break;
                        }
                    }
                }
                Assert.True(isTrue);
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

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_014()
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
            var sufix = "014";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 61,"ipnNameTest", 2, 1,3)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
            var kensaMsts = ConvertConversionItemToDetailModelData.ReadKensaMst();
            var ipnNameMsts = ConvertConversionItemToDetailModelData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                bool isTrue = false;
                var result = todayOdrRepository.ConvertConversionItemToOrderInfModel(hpId, raiiNo, ptId, sinDate, odrInfItems, expiredItems);
                foreach (var item in result)
                {
                    foreach (var item2 in item.OrdInfDetails)
                    {
                        if (item2.KensaGaichu == KensaGaichuTextConst.NONE)
                        {
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                            break;
                        }
                    }
                }
                Assert.True(isTrue);
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

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_015()
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
            var sufix = "015";
            var itemCd = "ItemCd2" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 61,"ipnNameTest", 2, 1,3, 1)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
            var kensaMsts = ConvertConversionItemToDetailModelData.ReadKensaMst();
            var ipnNameMsts = ConvertConversionItemToDetailModelData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                bool isTrue = false;
                var result = todayOdrRepository.ConvertConversionItemToOrderInfModel(hpId, raiiNo, ptId, sinDate, odrInfItems, expiredItems);
                foreach (var item in result)
                {
                    foreach (var item2 in item.OrdInfDetails)
                    {
                        if (item2.KensaGaichu == KensaGaichuTextConst.GAICHU_NOT_SET)
                        {
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                            break;
                        }
                    }
                }
                Assert.True(isTrue);
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

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_016()
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
            var sufix = "016";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 61,"ipnNameTest", 2, 1,3, 1)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
            //var kensaMsts = ConvertConversionItemToDetailModelData.ReadKensaMst();
            var ipnNameMsts = ConvertConversionItemToDetailModelData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
                //tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                bool isTrue = false;
                var result = todayOdrRepository.ConvertConversionItemToOrderInfModel(hpId, raiiNo, ptId, sinDate, odrInfItems, expiredItems);
                foreach (var item in result)
                {
                    foreach (var item2 in item.OrdInfDetails)
                    {
                        if (item2.KensaGaichu == KensaGaichuTextConst.GAICHU_NONE)
                        {
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                            break;
                        }
                    }
                }
                Assert.True(isTrue);
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(ipnMinYakkaMsts);
                tenant.RemoveRange(tenMsts);
                //tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_017()
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
            var sufix = "017";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(itemCd, 60,"ipnNameTest", 2, 1,3, 1)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,0, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add(itemCd, new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
            //var kensaMsts = ConvertConversionItemToDetailModelData.ReadKensaMst();
            var ipnNameMsts = ConvertConversionItemToDetailModelData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
                //tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                bool isTrue = false;
                var result = todayOdrRepository.ConvertConversionItemToOrderInfModel(hpId, raiiNo, ptId, sinDate, odrInfItems, expiredItems);
                foreach (var item in result)
                {
                    foreach (var item2 in item.OrdInfDetails)
                    {
                        if (item2.KensaGaichu == KensaGaichuTextConst.NONE)
                        {
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                            break;
                        }
                    }
                }
                Assert.True(isTrue);
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(ipnMinYakkaMsts);
                tenant.RemoveRange(tenMsts);
                //tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_018()
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
            var sufix = "018";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("", 60,"ipnNameTest", 2, 1,3, 1, "test")
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,28, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("", new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
            //var kensaMsts = ConvertConversionItemToDetailModelData.ReadKensaMst();
            var ipnNameMsts = ConvertConversionItemToDetailModelData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
                //tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                bool isTrue = false;
                var result = todayOdrRepository.ConvertConversionItemToOrderInfModel(hpId, raiiNo, ptId, sinDate, odrInfItems, expiredItems);
                foreach (var item in result)
                {
                    foreach (var item2 in item.OrdInfDetails)
                    {
                        if (item2.KensaGaichu == KensaGaichuTextConst.IS_DISPLAY_RECE_ON)
                        {
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                            break;
                        }
                    }
                }
                Assert.True(isTrue);
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(ipnMinYakkaMsts);
                tenant.RemoveRange(tenMsts);
                //tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void ConvertConversionItemToOrderInfModelTest_019()
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
            var sufix = "019";
            var itemCd = "ItemCd1" + sufix;

            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("", 60,"ipnNameTest", 2, 1,3, 1, "test",1)
            };
            List<OrdInfModel> odrInfItems = new List<OrdInfModel>()
            {
                new OrdInfModel(0,27, ordInfDetailModels)
            };
            Dictionary<string, TenItemModel> expiredItems = new Dictionary<string, TenItemModel>();
            expiredItems.Add("", new TenItemModel(itemCd));
            var ipnMinYakkaMsts = ConvertConversionItemToOrderInfModelData.ReadIpnMinYakkaMst();
            var tenMsts = ConvertConversionItemToDetailModelData.ReadTenMst(sufix);
            //var kensaMsts = ConvertConversionItemToDetailModelData.ReadKensaMst();
            var ipnNameMsts = ConvertConversionItemToDetailModelData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(ipnMinYakkaMsts);
                tenant.AddRange(tenMsts);
                //tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                bool isTrue = false;
                var result = todayOdrRepository.ConvertConversionItemToOrderInfModel(hpId, raiiNo, ptId, sinDate, odrInfItems, expiredItems);
                foreach (var item in result)
                {
                    foreach (var item2 in item.OrdInfDetails)
                    {
                        if (item2.KensaGaichu == KensaGaichuTextConst.IS_DISPLAY_RECE_OFF)
                        {
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                            break;
                        }
                    }
                }
                Assert.True(isTrue);
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(ipnMinYakkaMsts);
                tenant.RemoveRange(tenMsts);
                //tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
            }
        }
        #endregion [test GetKensaGaichu]
    }
}
