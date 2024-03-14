using CloudUnitTest.SampleData.GetAutoAddOrders;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.MedicalCommon.GetAutoAddOrders
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
            var result = todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
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
            try
            {
                var result = todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
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
            try
            {
                var result = todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
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
        public void GetAutoAddOrders_004()
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
                new Tuple<int, int, string>(1, 1, "itemTest2")
            };
            List<Tuple<int, int, string, double, int>> currentOdrList = new List<Tuple<int, int, string, double, int>>()
            {
                new Tuple<int, int, string, double, int>(1, 1, "", 3.14, 1)
            };
            var santeiGrpDetail = GetAutoAddOrdersData.ReadSanteiGrpDetail();
            var santeiAutoOrder = GetAutoAddOrdersData.ReadSanteiAutoOrder();
            var santeiAutoOrderDetail = GetAutoAddOrdersData.ReadSanteiAutoOrderDetail();
            try
            {
                tenant.AddRange(santeiGrpDetail);
                tenant.AddRange(santeiAutoOrder);
                tenant.AddRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
                var result = todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
                Assert.True(!result.Any());
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(santeiGrpDetail);
                tenant.RemoveRange(santeiAutoOrder);
                tenant.RemoveRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
            }
        }


        [Test]
        public void GetAutoAddOrders_005()
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
                new Tuple<int, int, string>(1, 1, "itemTest")
            };
            List<Tuple<int, int, string, double, int>> currentOdrList = new List<Tuple<int, int, string, double, int>>()
            {
                new Tuple<int, int, string, double, int>(1, 1, "itemTest", 3.14, 1)
            };
            var santeiGrpDetail = GetAutoAddOrdersData.ReadSanteiGrpDetail();
            var santeiAutoOrder = GetAutoAddOrdersData.ReadSanteiAutoOrder();
            var santeiAutoOrderDetail = GetAutoAddOrdersData.ReadSanteiAutoOrderDetail();
            try
            {
                tenant.AddRange(santeiGrpDetail);
                tenant.AddRange(santeiAutoOrder);
                tenant.AddRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
                var result = todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
                Assert.True(result.Any());
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(santeiGrpDetail);
                tenant.RemoveRange(santeiAutoOrder);
                tenant.RemoveRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetAutoAddOrders_006()
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
                new Tuple<int, int, string>(1, 1, "itemTest")
            };
            List<Tuple<int, int, string, double, int>> currentOdrList = new List<Tuple<int, int, string, double, int>>()
            {
                new Tuple<int, int, string, double, int>(1, 1, "itemTest", 3.14, 0)
            };
            var santeiGrpDetail = GetAutoAddOrdersData.ReadSanteiGrpDetail();
            var santeiAutoOrder = GetAutoAddOrdersData.ReadSanteiAutoOrder();
            var santeiAutoOrderDetail = GetAutoAddOrdersData.ReadSanteiAutoOrderDetail();
            try
            {
                tenant.AddRange(santeiGrpDetail);
                tenant.AddRange(santeiAutoOrder);
                tenant.AddRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
                var result = todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
                Assert.True(!result.Any());
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(santeiGrpDetail);
                tenant.RemoveRange(santeiAutoOrder);
                tenant.RemoveRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetAutoAddOrders_007()
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
                new Tuple<int, int, string>(1, 1, "itemTest3")
            };
            List<Tuple<int, int, string, double, int>> currentOdrList = new List<Tuple<int, int, string, double, int>>()
            {
                new Tuple<int, int, string, double, int>(1, 1, "itemTest3", 3.14, 0)
            };
            var santeiGrpDetail = GetAutoAddOrdersData.ReadSanteiGrpDetail();
            var santeiAutoOrder = GetAutoAddOrdersData.ReadSanteiAutoOrder();
            var santeiAutoOrderDetail = GetAutoAddOrdersData.ReadSanteiAutoOrderDetail();
            try
            {
                tenant.AddRange(santeiGrpDetail);
                tenant.AddRange(santeiAutoOrder);
                tenant.AddRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
                var result = todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
                Assert.True(!result.Any());
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(santeiGrpDetail);
                tenant.RemoveRange(santeiAutoOrder);
                tenant.RemoveRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetAutoAddOrders_008()
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
                new Tuple<int, int, string>(1, 1, "itemTest3")
            };
            List<Tuple<int, int, string, double, int>> currentOdrList = new List<Tuple<int, int, string, double, int>>()
            {
                new Tuple<int, int, string, double, int>(1, 1, "itemTest3", 3.14, 1)
            };
            var santeiGrpDetail = GetAutoAddOrdersData.ReadSanteiGrpDetail();
            var santeiAutoOrder = GetAutoAddOrdersData.ReadSanteiAutoOrder();
            var santeiAutoOrderDetail = GetAutoAddOrdersData.ReadSanteiAutoOrderDetail();
            try
            {
                tenant.AddRange(santeiGrpDetail);
                tenant.AddRange(santeiAutoOrder);
                tenant.AddRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
                var result = todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
                Assert.True(result.Any());
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(santeiGrpDetail);
                tenant.RemoveRange(santeiAutoOrder);
                tenant.RemoveRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
            }
        }
        [Test]
        public void GetAutoAddOrders_009()
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
                new Tuple<int, int, string>(1, 1, "itemTest3")
            };
            List<Tuple<int, int, string, double, int>> currentOdrList = new List<Tuple<int, int, string, double, int>>()
            {
                new Tuple<int, int, string, double, int>(1, 1, "itemTest3", 10, 1)
            };
            var santeiGrpDetail = GetAutoAddOrdersData.ReadSanteiGrpDetail();
            var santeiAutoOrder = GetAutoAddOrdersData.ReadSanteiAutoOrder();
            var santeiAutoOrderDetail = GetAutoAddOrdersData.ReadSanteiAutoOrderDetail();
            try
            {
                tenant.AddRange(santeiGrpDetail);
                tenant.AddRange(santeiAutoOrder);
                tenant.AddRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
                var result = todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
                Assert.True(result.Any());
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(santeiGrpDetail);
                tenant.RemoveRange(santeiAutoOrder);
                tenant.RemoveRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void GetAutoAddOrders_010()
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
                new Tuple<int, int, string>(1, 1, "itemTest4")
            };
            List<Tuple<int, int, string, double, int>> currentOdrList = new List<Tuple<int, int, string, double, int>>()
            {
                new Tuple<int, int, string, double, int>(1, 1, "itemTest4", 10, 1)
            };
            var santeiGrpDetail = GetAutoAddOrdersData.ReadSanteiGrpDetail();
            var santeiAutoOrder = GetAutoAddOrdersData.ReadSanteiAutoOrder();
            var santeiAutoOrderDetail = GetAutoAddOrdersData.ReadSanteiAutoOrderDetail();
            try
            {
                tenant.AddRange(santeiGrpDetail);
                tenant.AddRange(santeiAutoOrder);
                tenant.AddRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
                var result = todayOdrRepository.GetAutoAddOrders(hpId, ptId, sinDate, addingOdrList, currentOdrList);
                Assert.True(!result.Any());
            }
            finally
            {
                systemConfRepository.ReleaseResource();
                userRepository.ReleaseResource();
                approvalinfRepository.ReleaseResource();
                todayOdrRepository.ReleaseResource();
                tenant.RemoveRange(santeiGrpDetail);
                tenant.RemoveRange(santeiAutoOrder);
                tenant.RemoveRange(santeiAutoOrderDetail);
                tenant.SaveChanges();
            }
        }
    }
}
