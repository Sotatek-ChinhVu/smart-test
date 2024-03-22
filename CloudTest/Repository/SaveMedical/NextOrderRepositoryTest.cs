using Domain.Models.NextOrder;
using Entity.Tenant;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace CloudUnitTest.Repository.SaveMedical
{
    public class NextOrderRepositoryTest : BaseUT
    {
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
            //tenantTracking.Add(rsvkrtByomei);
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

        /*[Test]
        public void TC_013_NextOrderRepository_TestUpsertOrderInfInsertSuccess()
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
            FileItemModel fileItem = new FileItemModel(true, ListFileItems);
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
            *//*RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                IsDeleted = 0
            };*//*

            RsvkrtKarteInf rsvkrtKarte = new RsvkrtKarteInf()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                KarteKbn = 1,
                SeqNo = 1
            };

            //tenant.Add(rsvkrtMst);
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
                //tenant.RsvkrtMsts.Remove(rsvkrtMst);
                tenant.RsvkrtOdrInfs.RemoveRange(rsvkrtOdrInfs);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.SaveChanges();
            }
        }*/

        /*[Test]
        public void TC_014_NextOrderRepository_TestSaveFileNextOrderSuccess()
        {

        }*/
    }
}
