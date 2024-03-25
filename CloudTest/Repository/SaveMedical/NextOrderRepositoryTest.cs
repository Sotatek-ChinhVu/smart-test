using Domain.Models.NextOrder;
using Entity.Tenant;
using Helper.Redis;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using IDatabase = Microsoft.EntityFrameworkCore.Storage.IDatabase;

namespace CloudUnitTest.Repository.SaveMedical
{
    public class NextOrderRepositoryTest : BaseUT
    {
        private readonly StackExchange.Redis.IDatabase _cache;

        public NextOrderRepositoryTest()
        {
            string connection = string.Concat("10.2.15.78", ":", "6379");
            if (RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        [Test]
        public void TC_001_NextOrderRepository_TestInsertSuccess()
        {
            //Setup Data Test
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };
            RsvkrtKarteInfModel rsvkrtKarteInf = new();
            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new();
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    rsvkrtByomeis,
                    rsvkrtKarteInf,
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtMstsInsert = tenantTracking.RsvkrtMsts.Where(x => x.HpId == hpId && x.PtId == rsvkrtByomeis.First().PtId && x.RsvkrtNo == rsvkrtByomeis.First().RsvkrtNo).ToList();
            var rsvkrtByomeisInsert = tenantTracking.RsvkrtByomeis.Where(x => x.HpId == hpId && x.PtId == rsvkrtByomeis.First().PtId && x.RsvkrtNo == rsvkrtByomeis.First().RsvkrtNo && x.Id == rsvkrtByomeis.First().Id && x.SeqNo == rsvkrtByomeis.First().SeqNo).ToList();
            var rsvkrtKarteInfsInsert = tenantTracking.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo).ToList();

            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenantTracking.RsvkrtMsts.RemoveRange(rsvkrtMstsInsert);
                tenantTracking.RsvkrtByomeis.RemoveRange(rsvkrtByomeisInsert);
                tenantTracking.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfsInsert);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_002_NextOrderRepository_TestUpdateSuccess()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };
            RsvkrtKarteInfModel rsvkrtKarteInf = new();
            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new();
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    rsvkrtByomeis,
                    rsvkrtKarteInf,
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtMstList = tenantTracking.RsvkrtMsts.FirstOrDefault(item => item.HpId == hpId && item.PtId == ptId && item.RsvkrtNo == nextOrderModels.First().RsvkrtNo);

            foreach (var item in nextOrderModels)
            {
                item.ChangeModel("Kaito", 0);
            }

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);

            var rsvkrtMstsInsert = tenantTracking.RsvkrtMsts.Where(x => x.HpId == hpId && x.PtId == rsvkrtByomeis.First().PtId && x.RsvkrtNo == rsvkrtByomeis.First().RsvkrtNo).ToList();
            var rsvkrtByomeisInsert = tenantTracking.RsvkrtByomeis.Where(x => x.HpId == hpId && x.PtId == rsvkrtByomeis.First().PtId && x.RsvkrtNo == rsvkrtByomeis.First().RsvkrtNo && x.Id == rsvkrtByomeis.First().Id && x.SeqNo == rsvkrtByomeis.First().SeqNo).ToList();
            var rsvkrtKarteInfsInsert = tenantTracking.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo).ToList();

            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenantTracking.RsvkrtMsts.RemoveRange(rsvkrtMstsInsert);
                tenantTracking.RsvkrtByomeis.RemoveRange(rsvkrtByomeisInsert);
                tenantTracking.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfsInsert);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_003_NextOrderRepository_TestDeleteSuccess()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;
            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };
            RsvkrtKarteInfModel rsvkrtKarteInf = new();
            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    new(),
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetNoTrackingDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    rsvkrtByomeis,
                    rsvkrtKarteInf,
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtByomei rsvkrtByomei = new RsvkrtByomei()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                SeqNo = 1,
                Id = 1,
                IsDeleted = 0
            };

            RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                RpNo = rpNo,
                RpEdaNo = rpEdaNo,
                Id = 1
            };

            tenantTracking.Add(rsvkrtOdrInf);
            tenantTracking.Add(rsvkrtMst);
            tenantTracking.Add(rsvkrtByomei);
            tenantTracking.SaveChanges();

            foreach (var item in nextOrderModels)
            {
                item.ChangeModel("Kaito", 1);
            }

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenantTracking.RsvkrtMsts.Remove(rsvkrtMst);
                tenantTracking.RsvkrtByomeis.Remove(rsvkrtByomei);
                tenantTracking.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_004_NextOrderRepository_TestUpsertByomeiDeleteSuccess()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;
            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 1, new(), "", "", "", "")
            };
            RsvkrtKarteInfModel rsvkrtKarteInf = new();
            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    new(),
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetNoTrackingDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    rsvkrtByomeis,
                    new(),
                    new(),
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                RpNo = rpNo,
                Id = 1
            };

            RsvkrtByomei rsvkrtByomei = new RsvkrtByomei()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                SeqNo = 1,
                Id = 1,
                IsDeleted = 0
            };

            tenantTracking.Add(rsvkrtOdrInf);
            tenantTracking.Add(rsvkrtMst);
            tenantTracking.Add(rsvkrtByomei);
            tenantTracking.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenantTracking.RsvkrtMsts.Remove(rsvkrtMst);
                tenantTracking.RsvkrtByomeis.Remove(rsvkrtByomei);
                tenantTracking.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_005_NextOrderRepository_TestUpsertByomeiUpdateSuccess()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;
            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };
            RsvkrtKarteInfModel rsvkrtKarteInf = new();
            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    new(),
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetNoTrackingDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    rsvkrtByomeis,
                    new(),
                    new(),
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                RpNo = rpNo,
                Id = 1
            };

            RsvkrtByomei rsvkrtByomei = new RsvkrtByomei()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                SeqNo = 1,
                Id = 1,
                IsDeleted = 0
            };

            tenantTracking.Add(rsvkrtOdrInf);
            tenantTracking.Add(rsvkrtMst);
            tenantTracking.Add(rsvkrtByomei);
            tenantTracking.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenantTracking.RsvkrtMsts.Remove(rsvkrtMst);
                tenantTracking.RsvkrtByomeis.Remove(rsvkrtByomei);
                tenantTracking.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_006_NextOrderRepository_TestUpsertByomeiInsertSuccess()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;
            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };
            RsvkrtKarteInfModel rsvkrtKarteInf = new();
            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    new(),
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetNoTrackingDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    rsvkrtByomeis,
                    new(),
                    new(),
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                RpNo = rpNo,
                Id = 1
            };

            tenantTracking.Add(rsvkrtOdrInf);
            tenantTracking.Add(rsvkrtMst);
            tenantTracking.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenantTracking.RsvkrtMsts.Remove(rsvkrtMst);
                tenantTracking.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_007_NextOrderRepository_TestUpsertKarteInfInsertSuccess()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 0);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    new(),
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetNoTrackingDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    rsvkrtKarteInf,
                    new(),
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                RpNo = rpNo,
                Id = 1
            };

            tenantTracking.Add(rsvkrtOdrInf);
            tenantTracking.Add(rsvkrtMst);
            tenantTracking.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarte = tenantTracking.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo);
            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenantTracking.RsvkrtMsts.Remove(rsvkrtMst);
                tenantTracking.RsvkrtKarteInfs.RemoveRange(rsvkrtKarte);
                tenantTracking.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_008_NextOrderRepository_TestUpsertKarteInfUpdateSuccess()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 0);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    new(),
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    rsvkrtKarteInf,
                    new(),
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                RpNo = rpNo,
                Id = 1
            };

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            tenant.Add(rsvkrtOdrInf);
            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();

            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RsvkrtMsts.Remove(rsvkrtMst);
                tenant.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_009_NextOrderRepository_TestUpsertKarteInfDeleteSuccess()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 1);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    new(),
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    rsvkrtKarteInf,
                    new(),
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                RpNo = rpNo,
                Id = 1
            };

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            tenant.Add(rsvkrtOdrInf);
            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();

            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RsvkrtMsts.Remove(rsvkrtMst);
                tenant.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_010_NextOrderRepository_TestUpsertOrderInfUpdateSuccess()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 1);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    new(),
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    new(),
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                RpNo = rpNo,
                Id = 1
            };

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            tenant.Add(rsvkrtOdrInf);
            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
            var rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RsvkrtMsts.Remove(rsvkrtMst);
                tenant.RsvkrtOdrInfs.RemoveRange(rsvkrtOdrInfs);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_011_NextOrderRepository_TestUpsertOrderInfDeleteSuccess()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 1);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    1,
                    0,
                    new(),
                    0,
                    "",
                    new(),
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    new(),
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtOdrInf rsvkrtOdrInf = new RsvkrtOdrInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                RpNo = rpNo,
                Id = 1
            };

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            tenant.Add(rsvkrtOdrInf);
            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
            var rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RsvkrtMsts.Remove(rsvkrtMst);
                tenant.RsvkrtOdrInfs.RemoveRange(rsvkrtOdrInfs);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_012_NextOrderRepository_TestUpsertOrderInfInsertSuccess()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 1);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    new(),
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    new(),
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
            var rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();

            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RsvkrtMsts.Remove(rsvkrtMst);
                tenant.RsvkrtOdrInfs.RemoveRange(rsvkrtOdrInfs);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.SaveChanges();
            }
        }

        //Delete with SinKouiKbn
        [Test]
        public void TC_013_NextOrderRepository_TestSaveNextOrderRaiinListInf0Success()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            long ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;
            List<string> ListFileItems = new List<string>()
            {
                "Kaito0",
                "Kaito1",
                "Kaito2"
            };

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            List<RsvKrtOrderInfDetailModel> rsvKrtOrderInfDetailModels = new List<RsvKrtOrderInfDetailModel>()
            {
                new RsvKrtOrderInfDetailModel(
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    rsvDate,
                    1,
                    1,
                    "Kaito",
                    "Kaito",
                    1,
                    "",
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    "",
                    1,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    1,
                    "",
                    1,
                    1,
                    true,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    new(),
                    1,
                    1,
                    "",
                    "",
                    1,
                    true
                    )
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 1);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    1,
                    0,
                    new(),
                    0,
                    "",
                    rsvKrtOrderInfDetailModels,
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new FileItemModel(false, ListFileItems);
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    new(),
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            KouiKbnMst kouiKbnMst = new KouiKbnMst()
            {
                KouiKbnId = 99999999,
                SortNo = 99999999,
                KouiKbn1 = 1,
                KouiKbn2 = 1,
                KouiGrpName = "Kaito"
            };

            RaiinListKoui raiinListKoui = new RaiinListKoui()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999,
                KouiKbnId = 99999999
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = 99999999
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999
            };

            RaiinListInf raiinListInf = new RaiinListInf()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = rsvDate,
                RaiinNo = 0,
                GrpId = 99999999,
                KbnCd = 99999999
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(kouiKbnMst);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);
            tenant.Add(raiinListInf);
            tenant.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
            var rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
            var rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == 2803 || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo);
            var rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId);

            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.KouiKbnMsts.Remove(kouiKbnMst);
                tenant.RaiinListMsts.Remove(raiinListMst);
                tenant.RaiinListDetails.Remove(raiinListDetail);
                tenant.RaiinListKouis.Remove(raiinListKoui);
                tenant.RaiinListItems.Remove(raiinListItem);
                tenant.RsvkrtMsts.RemoveRange(rsvkrtMstData);
                tenant.RsvkrtOdrInfs.RemoveRange(rsvkrtOdrInfs);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.RsvkrtOdrInfDetails.RemoveRange(rsvkrtOdrInfDetails);
                tenant.SaveChanges();
            }
        }

        // Delete with ItemCd
        [Test]
        public void TC_014_NextOrderRepository_TestSaveNextOrderRaiinListInf1Success()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            long ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;
            List<string> ListFileItems = new List<string>()
            {
                "Kaito0",
                "Kaito1",
                "Kaito2"
            };

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            List<RsvKrtOrderInfDetailModel> rsvKrtOrderInfDetailModels = new List<RsvKrtOrderInfDetailModel>()
            {
                new RsvKrtOrderInfDetailModel(
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    rsvDate,
                    1,
                    1,
                    "Kaito",
                    "Kaito",
                    1,
                    "",
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    "",
                    1,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    1,
                    "",
                    1,
                    1,
                    true,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    new(),
                    1,
                    1,
                    "",
                    "",
                    1,
                    true
                    )
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 1);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    1,
                    0,
                    new(),
                    0,
                    "",
                    rsvKrtOrderInfDetailModels,
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new FileItemModel(false, ListFileItems);
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    new(),
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            KouiKbnMst kouiKbnMst = new KouiKbnMst()
            {
                KouiKbnId = 99999999,
                SortNo = 99999999,
                KouiKbn1 = 1,
                KouiKbn2 = 1,
                KouiGrpName = "Kaito"
            };

            RaiinListKoui raiinListKoui = new RaiinListKoui()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999,
                KouiKbnId = 99999999
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = 99999999
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999,
                ItemCd = "Kaito"
            };

            RaiinListInf raiinListInf = new RaiinListInf()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = rsvDate,
                RaiinNo = 0,
                GrpId = 99999999,
                KbnCd = 99999999
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(kouiKbnMst);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);
            tenant.Add(raiinListInf);
            tenant.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
            var rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
            var rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == 2803 || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo);
            var rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId);

            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.KouiKbnMsts.Remove(kouiKbnMst);
                tenant.RaiinListMsts.Remove(raiinListMst);
                tenant.RaiinListDetails.Remove(raiinListDetail);
                tenant.RaiinListKouis.Remove(raiinListKoui);
                tenant.RaiinListItems.Remove(raiinListItem);
                tenant.RsvkrtMsts.RemoveRange(rsvkrtMstData);
                tenant.RsvkrtOdrInfs.RemoveRange(rsvkrtOdrInfs);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.RsvkrtOdrInfDetails.RemoveRange(rsvkrtOdrInfDetails);
                tenant.SaveChanges();
            }
        }

        // Update with ItemCd and SinKouiKbn
        [Test]
        public void TC_015_NextOrderRepository_TestSaveNextOrderRaiinListInf2Success()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            long ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;
            List<string> ListFileItems = new List<string>()
            {
                "Kaito0",
                "Kaito1",
                "Kaito2"
            };

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            List<RsvKrtOrderInfDetailModel> rsvKrtOrderInfDetailModels = new List<RsvKrtOrderInfDetailModel>()
            {
                new RsvKrtOrderInfDetailModel(
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    rsvDate,
                    1,
                    1,
                    "Kaito",
                    "Kaito",
                    1,
                    "",
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    "",
                    1,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    1,
                    "",
                    1,
                    1,
                    true,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    new(),
                    1,
                    1,
                    "",
                    "",
                    1,
                    true
                    )
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 1);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    rsvKrtOrderInfDetailModels,
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new FileItemModel(false, ListFileItems);
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    new(),
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            KouiKbnMst kouiKbnMst = new KouiKbnMst()
            {
                KouiKbnId = 99999999,
                SortNo = 99999999,
                KouiKbn1 = 1,
                KouiKbn2 = 1,
                KouiGrpName = "Kaito"
            };

            RaiinListKoui raiinListKoui = new RaiinListKoui()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999,
                KouiKbnId = 99999999
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = 99999999
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999,
                ItemCd = "Kaito"
            };

            RaiinListInf raiinListInf = new RaiinListInf()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = rsvDate,
                RaiinNo = 0,
                GrpId = 99999999,
                KbnCd = 99999999
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(kouiKbnMst);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);
            tenant.Add(raiinListInf);
            tenant.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
            var rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
            var rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == 2803 || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo);
            var rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId);

            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.KouiKbnMsts.Remove(kouiKbnMst);
                tenant.RaiinListMsts.Remove(raiinListMst);
                tenant.RaiinListDetails.Remove(raiinListDetail);
                tenant.RaiinListKouis.Remove(raiinListKoui);
                tenant.RaiinListItems.Remove(raiinListItem);
                tenant.RaiinListInfs.Remove(raiinListInf);
                tenant.RsvkrtMsts.RemoveRange(rsvkrtMstData);
                tenant.RsvkrtOdrInfs.RemoveRange(rsvkrtOdrInfs);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.RsvkrtOdrInfDetails.RemoveRange(rsvkrtOdrInfDetails);
                tenant.SaveChanges();
            }
        }

        // Add with ItemCd and SinKouiKbn
        [Test]
        public void TC_016_NextOrderRepository_TestSaveNextOrderRaiinListInf3Success()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            long ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;
            List<string> ListFileItems = new List<string>()
            {
                "Kaito0",
                "Kaito1",
                "Kaito2"
            };

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            List<RsvKrtOrderInfDetailModel> rsvKrtOrderInfDetailModels = new List<RsvKrtOrderInfDetailModel>()
            {
                new RsvKrtOrderInfDetailModel(
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    rsvDate,
                    1,
                    1,
                    "Kaito",
                    "Kaito",
                    1,
                    "",
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    "",
                    1,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    1,
                    "",
                    1,
                    1,
                    true,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    new(),
                    1,
                    1,
                    "",
                    "",
                    1,
                    true
                    )
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 1);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    rsvKrtOrderInfDetailModels,
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new FileItemModel(false, ListFileItems);
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    new(),
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            KouiKbnMst kouiKbnMst = new KouiKbnMst()
            {
                KouiKbnId = 99999999,
                SortNo = 99999999,
                KouiKbn1 = 1,
                KouiKbn2 = 1,
                KouiGrpName = "Kaito"
            };

            RaiinListKoui raiinListKoui = new RaiinListKoui()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999,
                KouiKbnId = 99999999
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = 99999999
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999,
                ItemCd = "Kaito"
            };
   
            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(kouiKbnMst);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);
            tenant.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
            var rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
            var rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == 2803 || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo);
            var rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId);

            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.KouiKbnMsts.Remove(kouiKbnMst);
                tenant.RaiinListMsts.Remove(raiinListMst);
                tenant.RaiinListDetails.Remove(raiinListDetail);
                tenant.RaiinListKouis.Remove(raiinListKoui);
                tenant.RaiinListItems.Remove(raiinListItem);
                tenant.RsvkrtMsts.RemoveRange(rsvkrtMstData);
                tenant.RsvkrtOdrInfs.RemoveRange(rsvkrtOdrInfs);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.RsvkrtOdrInfDetails.RemoveRange(rsvkrtOdrInfDetails);
                tenant.SaveChanges();
            }
        }

        // Add with SinKouiKbn false
        [Test]
        public void TC_017_NextOrderRepository_TestSaveNextOrderRaiinListInf4False()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            long ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;
            List<string> ListFileItems = new List<string>()
            {
                "Kaito0",
                "Kaito1",
                "Kaito2"
            };

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            List<RsvKrtOrderInfDetailModel> rsvKrtOrderInfDetailModels = new List<RsvKrtOrderInfDetailModel>()
            {
                new RsvKrtOrderInfDetailModel(
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    rsvDate,
                    1,
                    1,
                    "Kaito",
                    "Kaito",
                    1,
                    "",
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    "",
                    1,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    1,
                    "",
                    1,
                    1,
                    true,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    new(),
                    1,
                    1,
                    "",
                    "",
                    1,
                    true
                    )
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 1);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    rsvKrtOrderInfDetailModels,
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new FileItemModel(false, ListFileItems);
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    new(),
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            RaiinListKoui raiinListKoui = new RaiinListKoui()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999,
                KouiKbnId = 99999999
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = 99999999
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999,
                ItemCd = "Kaito"
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);
            tenant.SaveChanges();

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
            var rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
            var rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == 2803 || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo);
            var rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId);
            var raiinListInfs = tenant.RaiinListInfs.Where(x => x.HpId == hpId && (x.PtId == ptId || x.PtId == 2803) && x.SinDate == rsvDate && x.RaiinNo == 0 && x.GrpId == 99999999);

            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RaiinListMsts.Remove(raiinListMst);
                tenant.RaiinListDetails.Remove(raiinListDetail);
                tenant.RaiinListKouis.Remove(raiinListKoui);
                tenant.RaiinListItems.Remove(raiinListItem);
                tenant.RaiinListInfs.RemoveRange(raiinListInfs);
                tenant.RsvkrtMsts.RemoveRange(rsvkrtMstData);
                tenant.RsvkrtOdrInfs.RemoveRange(rsvkrtOdrInfs);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.RsvkrtOdrInfDetails.RemoveRange(rsvkrtOdrInfDetails);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_018_NextOrderRepository_TestKeyDelete()
        {
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            long ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            long rpNo = 1;
            long rpEdaNo = 1;
            long id = 1;
            List<string> ListFileItems = new List<string>()
            {
                "Kaito0",
                "Kaito1",
                "Kaito2"
            };

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            List<RsvKrtOrderInfDetailModel> rsvKrtOrderInfDetailModels = new List<RsvKrtOrderInfDetailModel>()
            {
                new RsvKrtOrderInfDetailModel(
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    rsvDate,
                    1,
                    1,
                    "Kaito",
                    "Kaito",
                    1,
                    "",
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    "",
                    1,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    1,
                    "",
                    1,
                    1,
                    true,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    "",
                    new(),
                    1,
                    1,
                    "",
                    "",
                    1,
                    true
                    )
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new RsvkrtKarteInfModel(hpId, ptId, rsvDate, rsvkrtNo, 1, "KarteInf1", "KarteInf2", 1);

            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new List<RsvkrtOrderInfModel>()
            {
                new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvDate,
                    rsvkrtNo,
                    rpNo,
                    rpEdaNo,
                    id,
                    0,
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new(),
                    0,
                    "",
                    rsvKrtOrderInfDetailModels,
                    new(),
                    "",
                    ""
                    )
            };
            FileItemModel fileItem = new FileItemModel(false, ListFileItems);
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    2803,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    new(),
                    new(),
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            //Act
            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            RaiinListKoui raiinListKoui = new RaiinListKoui()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999,
                KouiKbnId = 99999999
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = 99999999
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = 99999999,
                KbnCd = 99999999,
                SeqNo = 99999999,
                ItemCd = "Kaito"
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);
            tenant.SaveChanges();
            string finalKey = "GetNextOrderList28032001-1";
            _cache.StringAppend(finalKey, string.Empty);

            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
            var rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
            var rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == 2803 || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo);
            var rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId);
            var raiinListInfs = tenant.RaiinListInfs.Where(x => x.HpId == hpId && (x.PtId == ptId || x.PtId == 2803) && x.SinDate == rsvDate && x.RaiinNo == 0 && x.GrpId == 99999999);

            //
            try
            {
                Assert.That(!_cache.KeyExists(finalKey));
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RaiinListMsts.Remove(raiinListMst);
                tenant.RaiinListDetails.Remove(raiinListDetail);
                tenant.RaiinListKouis.Remove(raiinListKoui);
                tenant.RaiinListItems.Remove(raiinListItem);
                tenant.RaiinListInfs.RemoveRange(raiinListInfs);
                tenant.RsvkrtMsts.RemoveRange(rsvkrtMstData);
                tenant.RsvkrtOdrInfs.RemoveRange(rsvkrtOdrInfs);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.RsvkrtOdrInfDetails.RemoveRange(rsvkrtOdrInfDetails);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_019_NextOrderRepository_TestInsertSuccess()
        {
            //Setup Data Test
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            //Mock data
            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };
            RsvkrtKarteInfModel rsvkrtKarteInf = new();
            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new();
            FileItemModel fileItem = new();
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    ptId,
                    rsvkrtNo,
                    rsvkrtKbn,
                    rsvDate,
                    rsvName,
                    isDeleted,
                    sortNo,
                    rsvkrtByomeis,
                    rsvkrtKarteInf,
                    rsvkrtOrderInfs,
                    fileItem
                    ),
                new NextOrderModel (
                    hpId,
                    ptId +1,
                    rsvkrtNo +1,
                    rsvkrtKbn +1,
                    rsvDate +1,
                    rsvName,
                    1,
                    sortNo + 1,
                    rsvkrtByomeis,
                    rsvkrtKarteInf,
                    rsvkrtOrderInfs,
                    fileItem
                    )
            };

            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo + 1,
                RsvkrtKbn = 0,
                RsvDate = rsvDate,
            };

            tenant.Add(rsvkrtMst);
            tenant.SaveChanges();

            //Act
            var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
            var rsvkrtMstsInsert = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && x.PtId == rsvkrtByomeis.First().PtId && x.RsvkrtNo == rsvkrtByomeis.First().RsvkrtNo).ToList();
            var rsvkrtByomeisInsert = tenant.RsvkrtByomeis.Where(x => x.HpId == hpId && x.PtId == rsvkrtByomeis.First().PtId && x.RsvkrtNo == rsvkrtByomeis.First().RsvkrtNo && x.Id == rsvkrtByomeis.First().Id && x.SeqNo == rsvkrtByomeis.First().SeqNo).ToList();
            var rsvkrtKarteInfsInsert = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo).ToList();

            //
            try
            {
                Assert.That(result);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RsvkrtMsts.RemoveRange(rsvkrtMstsInsert);
                tenant.RsvkrtMsts.Remove(rsvkrtMst);
                tenant.RsvkrtByomeis.RemoveRange(rsvkrtByomeisInsert);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfsInsert);
                tenant.SaveChanges();
            }
        }
    }
}
