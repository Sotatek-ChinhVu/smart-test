using CloudUnitTest.SampleData.TodayOdrRepository;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.MedicalCommon.FromHistory
{
    public class FromHistoryTest : BaseUT
    {
        [Test]
        public void FromHistoryTest_001()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>();
            try
            {
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                Assert.True(!result.Any());
            }
            finally
            {
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void FromHistoryTest_002()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest",1,"test")
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void FromHistoryTest_003()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest",1,"")
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void FromHistoryTest_004()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest",20,"", 0,1)
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void FromHistoryTest_005()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest",20,"", 12,1)
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void FromHistoryTest_006()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1",20,"", 12,1)
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }
        [Test]
        public void FromHistoryTest_007()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd2",20,"", 12,1)
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void FromHistoryTest_008()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd2",20,"", 0,1)
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }
        [Test]
        public void FromHistoryTest_009()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd2",20,"", 1,1)
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void FromHistoryTest_010()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd2",20,"", 2,1)
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void FromHistoryTest_011()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest",20,"", 2,1)
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            var yohoSetMsts = FromHistoryData.ReadYohoSetMst();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.AddRange(yohoSetMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.RemoveRange(yohoSetMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        #region [test CheckIsGetYakkaPrice]

        [Test]
        public void FromHistoryTest_012()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest",20,"", 2,1)
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            var yohoSetMsts = FromHistoryData.ReadYohoSetMst();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.AddRange(yohoSetMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                bool isTrue = false;
                foreach (var item in result)
                {
                    foreach (var item2 in item.OrdInfDetails)
                    {
                        if (item2.IsGetPriceInYakka)
                        {
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                            break;
                        }
                    }
                    if (isTrue == false) { break; }
                }
                Assert.True(isTrue);
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.RemoveRange(yohoSetMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void FromHistoryTest_013()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int sinDate = 20240314;
            long raiinNo = 0;
            int sainteiKbn = 0;
            int userId = 1;
            long ptId = 0;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest",20,"ItemCdTest", 2,1)
            };
            List<OrdInfModel> historyOdrInfModels = new List<OrdInfModel>()
            {
                new OrdInfModel(hpId,ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            var tenMsts = FromHistoryData.ReadTenMst();
            var kensaMsts = FromHistoryData.ReadKensaMst();
            var ipnNameMsts = FromHistoryData.ReadIpnNameMst();
            var yohoSetMsts = FromHistoryData.ReadYohoSetMst();
            var ipnKasanExcludes = FromHistoryData.ReadIpnKasanExclude();
            var ipnKasanExcludeItems = FromHistoryData.ReadIpnKasanExcludeItem();
            try
            {
                tenant.AddRange(tenMsts);
                tenant.AddRange(kensaMsts);
                tenant.AddRange(ipnNameMsts);
                tenant.AddRange(yohoSetMsts);
                tenant.AddRange(ipnKasanExcludes);
                tenant.AddRange(ipnKasanExcludeItems);
                tenant.SaveChanges();
                var result = todayOdrRepository.FromHistory(hpId, sinDate, raiinNo, sainteiKbn, userId, ptId, historyOdrInfModels);
                bool isTrue = false;
                foreach (var item in result)
                {
                    foreach (var item2 in item.OrdInfDetails)
                    {
                        if (!item2.IsGetPriceInYakka)
                        {
                            isTrue = true;
                        }
                        else
                        {
                            isTrue = false;
                            break;
                        }
                    }
                    if (isTrue == false) { break; }
                }
                Assert.True(isTrue);
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.RemoveRange(kensaMsts);
                tenant.RemoveRange(ipnNameMsts);
                tenant.RemoveRange(yohoSetMsts);
                tenant.RemoveRange(ipnKasanExcludes);
                tenant.RemoveRange(ipnKasanExcludeItems);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        #endregion [test CheckIsGetYakkaPrice]

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
