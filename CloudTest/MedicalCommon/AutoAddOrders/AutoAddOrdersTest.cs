using CloudUnitTest.SampleData.TodayOdrRepository;
using DocumentFormat.OpenXml.Bibliography;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.MedicalCommon.AutoAddOrders
{
    public class AutoAddOrdersTest : BaseUT
    {

        [Test]
        public void AutoAddOrdersTest_001()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);

            try
            {
                // Your test code here
                var hpId = 1;
                int userId = 1;
                int sinDate = 20240310;
                List<Tuple<int, int, string, int, int>> addingOdrList = new List<Tuple<int, int, string, int, int>>();
                List<Tuple<int, int, string, long>> autoAddItems = new List<Tuple<int, int, string, long>>();
                var result = todayOdrRepository.AutoAddOrders(hpId, userId, sinDate, addingOdrList, autoAddItems);
                Assert.True(!result.Any());
            }
            finally
            {
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void AutoAddOrdersTest_002()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);

            try
            {
                // Your test code here
                var hpId = 1;
                int userId = 1;
                int sinDate = 20240310;
                List<Tuple<int, int, string, int, int>> addingOdrList = new List<Tuple<int, int, string, int, int>>()
                {
                    new Tuple<int, int, string, int, int>(0,1,"test",1,1)
                };
                List<Tuple<int, int, string, long>> autoAddItems = new List<Tuple<int, int, string, long>>()
                {
                    new Tuple<int, int, string, long>(1,1,"test",1)
                };
                var result = todayOdrRepository.AutoAddOrders(hpId, userId, sinDate, addingOdrList, autoAddItems);
                Assert.True(!result.Any());
            }
            finally
            {
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void AutoAddOrdersTest_003()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);

            try
            {
                // Your test code here
                var hpId = 1;
                int userId = 1;
                int sinDate = 20240310;
                List<Tuple<int, int, string, int, int>> addingOdrList = new List<Tuple<int, int, string, int, int>>()
                {
                    new Tuple<int, int, string, int, int>(1,1,"test",1,1)
                };
                List<Tuple<int, int, string, long>> autoAddItems = new List<Tuple<int, int, string, long>>()
                {
                    new Tuple<int, int, string, long>(1,1,"test",1)
                };
                var result = todayOdrRepository.AutoAddOrders(hpId, userId, sinDate, addingOdrList, autoAddItems);
                Assert.True(result.Any());
            }
            finally
            {
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void AutoAddOrdersTest_004()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            // Your test code here
            var hpId = 1;
            int userId = 1;
            int sinDate = 20240310;
            List<Tuple<int, int, string, int, int>> addingOdrList = new List<Tuple<int, int, string, int, int>>()
                {
                    new Tuple<int, int, string, int, int>(1,1,"ItemCdTest",1,1)
                };
            List<Tuple<int, int, string, long>> autoAddItems = new List<Tuple<int, int, string, long>>()
                {
                    new Tuple<int, int, string, long>(1,1,"ItemCdTest",1)
                };
            var tenMsts = AutoAddOrdersData.ReadTenMst();

            try
            {                                             
                tenant.AddRange(tenMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.AutoAddOrders(hpId, userId, sinDate, addingOdrList, autoAddItems);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void AutoAddOrdersTest_005()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            // Your test code here
            var hpId = 1;
            int userId = 1;
            int sinDate = 20240310;
            List<Tuple<int, int, string, int, int>> addingOdrList = new List<Tuple<int, int, string, int, int>>()
                {
                    new Tuple<int, int, string, int, int>(1,1,"ItemCd1",1,1)
                };
            List<Tuple<int, int, string, long>> autoAddItems = new List<Tuple<int, int, string, long>>()
                {
                    new Tuple<int, int, string, long>(1,1,"ItemCd1",1)
                };
            var tenMsts = AutoAddOrdersData.ReadTenMst();

            try
            {
                tenant.AddRange(tenMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.AutoAddOrders(hpId, userId, sinDate, addingOdrList, autoAddItems);
                Assert.True(result.Any());
            }
            finally
            {
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        #region [Test GetKensaGaichu]
        [Test]
        public void AutoAddOrdersTest_006_GetKensaGaichu()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            // Your test code here
            var hpId = 1;
            int userId = 1;
            int sinDate = 20240310;
            List<Tuple<int, int, string, int, int>> addingOdrList = new List<Tuple<int, int, string, int, int>>()
                {
                    new Tuple<int, int, string, int, int>(1,1,"ItemCd1",1,1)
                };
            List<Tuple<int, int, string, long>> autoAddItems = new List<Tuple<int, int, string, long>>()
                {
                    new Tuple<int, int, string, long>(1,1,"",1)
                };
            var tenMsts = AutoAddOrdersData.ReadTenMst();

            try
            {
                tenant.AddRange(tenMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.AutoAddOrders(hpId, userId, sinDate, addingOdrList, autoAddItems);
                bool isTrue = false;
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
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void AutoAddOrdersTest_007_GetKensaGaichu()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            // Your test code here
            var hpId = 1;
            int userId = 1;
            int sinDate = 20240310;
            List<Tuple<int, int, string, int, int>> addingOdrList = new List<Tuple<int, int, string, int, int>>()
                {
                    new Tuple<int, int, string, int, int>(1,1,"ItemCd1",1,1)
                };
            List<Tuple<int, int, string, long>> autoAddItems = new List<Tuple<int, int, string, long>>()
                {
                    new Tuple<int, int, string, long>(1,1,"ItemCd1",1)
                };
            var tenMsts = AutoAddOrdersData.ReadTenMst();

            try
            {
                tenant.AddRange(tenMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.AutoAddOrders(hpId, userId, sinDate, addingOdrList, autoAddItems);
                bool isTrue = false;
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
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        [Test]
        public void AutoAddOrdersTest_008_GetKensaGaichu()
        {
            SetupTestEnvironment(out var systemConfRepository, out var userRepository, out var approvalinfRepository, out var todayOdrRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            // Your test code here
            var hpId = 1;
            int userId = 1;
            int sinDate = 20240310;
            List<Tuple<int, int, string, int, int>> addingOdrList = new List<Tuple<int, int, string, int, int>>()
                {
                    new Tuple<int, int, string, int, int>(1,1,"ItemCd2",1,1)
                };
            List<Tuple<int, int, string, long>> autoAddItems = new List<Tuple<int, int, string, long>>()
                {
                    new Tuple<int, int, string, long>(1,1,"ItemCd2",1)
                };
            var tenMsts = AutoAddOrdersData.ReadTenMst();

            try
            {
                tenant.AddRange(tenMsts);
                tenant.SaveChanges();
                var result = todayOdrRepository.AutoAddOrders(hpId, userId, sinDate, addingOdrList, autoAddItems);
                bool isTrue = false;
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
                tenant.RemoveRange(tenMsts);
                tenant.SaveChanges();
                CleanupResources(systemConfRepository, userRepository, approvalinfRepository, todayOdrRepository);
            }
        }

        #endregion

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
