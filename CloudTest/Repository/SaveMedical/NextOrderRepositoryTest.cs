using Domain.Models.NextOrder;
using Entity.Tenant;
using Helper.Constants;
using Helper.Redis;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Dynamic.Core;
using IDatabase = Microsoft.EntityFrameworkCore.Storage.IDatabase;

namespace CloudUnitTest.Repository.SaveMedical
{
    public class NextOrderRepositoryTest : BaseUT
    {
        private readonly StackExchange.Redis.IDatabase _cache;

        private readonly string baseAccessUrl = "BaseAccessUrl";

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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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

            try
            {
                //Assert
                Assert.That(result && rsvkrtMstsInsert.Any() && rsvkrtByomeisInsert.Any());
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenantTracking.RsvkrtMsts.RemoveRange(rsvkrtMstsInsert);
                tenantTracking.RsvkrtByomeis.RemoveRange(rsvkrtByomeisInsert);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_002_NextOrderRepository_TestUpdateSuccess()
        {
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

            int userId = 999;
            int hpId = 1;
            int ptId = 28032001;
            long rsvkrtNo = 1;
            int rsvkrtKbn = 1;
            int rsvDate = 1;
            string rsvName = "";
            int isDeleted = 0;
            int sortNo = 1;
            int seqNo = 1;
            int id = 1;

            List<RsvkrtByomeiModel> rsvkrtByomeis = new List<RsvkrtByomeiModel>()
            {
                new RsvkrtByomeiModel(1, hpId, ptId, rsvkrtNo, 1, "", "", 1, 1, 1, "", 1, 1, 0, new(), "", "", "", "")
            };

            RsvkrtKarteInfModel rsvkrtKarteInf = new();
            List<RsvkrtOrderInfModel> rsvkrtOrderInfs = new();
            FileItemModel fileItem = new();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
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

            foreach (var item in nextOrderModels)
            {
                item.ChangeModel("Kaito", 0);
            }

            RsvkrtByomei rsvkrtByomei = new RsvkrtByomei()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                SeqNo = seqNo,
                Id = id
            };

            RsvkrtMst rsvkrtMst = new RsvkrtMst()
            {
                HpId = hpId,
                PtId = ptId,
                RsvkrtNo = rsvkrtNo,
                RsvkrtKbn = rsvkrtKbn,
                RsvDate = rsvDate
            };

            tenant.Add(rsvkrtByomei);
            tenant.Add(rsvkrtMst);
            var rsvkrtKarteInfsInsert = new List<RsvkrtKarteInf>();

            try
            {
                //Act
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                var rsvkrtMstsInsert = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && x.PtId == rsvkrtByomeis.First().PtId && x.RsvkrtNo == rsvkrtByomeis.First().RsvkrtNo);
                rsvkrtKarteInfsInsert = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo).ToList();

                //Assert
                Assert.That(result && rsvkrtMstsInsert.Any(x => x.RsvName == "Kaito"));
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RsvkrtMsts.RemoveRange(rsvkrtMst);
                tenant.RsvkrtByomeis.RemoveRange(rsvkrtByomei);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfsInsert);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_003_NextOrderRepository_TestDeleteSuccess()
        {
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            try
            {
                //Act
                tenantTracking.SaveChanges();

                foreach (var item in nextOrderModels)
                {
                    item.ChangeModel("Kaito", 1);
                }

                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                var rsvkrtMstDelete = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo);
                var rsvkrtOdrInfDelete = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.RpNo == rpNo && x.RpEdaNo == rpEdaNo && x.Id == id);

                //Assert
                Assert.That(result && rsvkrtMstDelete.Any(x => x.IsDeleted == 1) && rsvkrtOdrInfDelete.Any(x => x.IsDeleted == 1));
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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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

            try
            {
                //Act
                tenantTracking.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);

                //Assert
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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            int seqNo = 1;

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

            try
            {
                //Act
                tenantTracking.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                var rsvkrtByomeiInserts = tenant.RsvkrtByomeis.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.SeqNo == seqNo && x.Id == id);

                //Assert
                Assert.That(result && rsvkrtByomeiInserts.Any());
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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            int seqNo = 1;

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
            var rsvkrtByomeiList = new List<RsvkrtByomei>();

            try
            {
                //Act
                tenantTracking.SaveChanges();

                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtByomeiList = tenant.RsvkrtByomeis.Where(x => x.HpId == hpId && x.Id == id && x.RsvkrtNo == rsvkrtNo && x.SeqNo == seqNo && x.PtId == ptId).ToList();
                //Assert
                Assert.That(result && rsvkrtByomeiList.Any());
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenantTracking.RsvkrtMsts.Remove(rsvkrtMst);
                tenantTracking.RsvkrtByomeis.RemoveRange(rsvkrtByomeiList);
                tenantTracking.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_007_NextOrderRepository_TestUpsertKarteInfInsertSuccess()
        {
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            int seqNo = 1;

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
            var rsvkrtKarte = new List<RsvkrtKarteInf>();

            try
            {
                //Act
                tenantTracking.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarte = tenantTracking.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == rsvkrtKbn && x.SeqNo == seqNo).ToList();

                //Assert
                Assert.That(result && rsvkrtKarte.Any());
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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();

            try
            {
                //Act
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();

                //Assert
                Assert.That(result && rsvkrtKarteInfs.Count() == 2);
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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();

            try
            {
                //Act
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenantTracking.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();

                //Assert
                Assert.That(result && rsvkrtKarteInfs.Where(x => x.IsDeleted == 1).Any());
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RsvkrtMsts.Remove(rsvkrtMst);
                tenant.RsvkrtOdrInfs.Remove(rsvkrtOdrInf);
                tenantTracking.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.SaveChanges();
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_010_NextOrderRepository_TestUpsertOrderInfUpdateSuccess()
        {
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();
            var rsvkrtOdrInfs = new List<RsvkrtOdrInf>();

            try
            {
                //Act
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
                rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();

                //Assert
                Assert.That(result && rsvkrtOdrInfs.Count() == 2);
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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();

            try
            {
                //Act
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
                var rsvkrtOdrInfs = tenantTracking.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();

                //Assert
                Assert.That(result && rsvkrtOdrInfs.Any(x => x.IsDeleted == 1));
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RsvkrtMsts.Remove(rsvkrtMst);
                tenant.RsvkrtOdrInfs.RemoveRange(rsvkrtOdrInf);
                tenant.RsvkrtKarteInfs.RemoveRange(rsvkrtKarteInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_012_NextOrderRepository_TestUpsertOrderInfInsertSuccess()
        {
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();
            var rsvkrtOdrInfs = new List<RsvkrtOdrInf>();

            try
            {
                //Act
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
                rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();

                //Assert
                Assert.That(result && rsvkrtOdrInfs.Any());
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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            int grpId = 99999999;
            int kbnCd = 99999999;
            long ptIdInput = 2803;

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
                    ptIdInput,
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
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = 99999999,
                KouiKbnId = 99999999
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = grpId
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = 99999999
            };

            RaiinListInf raiinListInf = new RaiinListInf()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = rsvDate,
                RaiinNo = 0,
                GrpId = grpId,
                KbnCd = kbnCd
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(kouiKbnMst);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);
            tenant.Add(raiinListInf);

            var rsvkrtMstData = new List<RsvkrtMst>();
            var rsvkrtOdrInfs = new List<RsvkrtOdrInf>();
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();
            var rsvkrtOdrInfDetails = new List<RsvkrtOdrInfDetail>();

            try
            {
                //Act
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
                rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
                rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == ptIdInput || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo).ToList();
                rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
                var raiinListInfs = tenant.RaiinListInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.SinDate == rsvDate && x.RaiinNo == 0 && x.GrpId == grpId && x.KbnCd == kbnCd);

                //Assert
                Assert.That(result && !raiinListInfs.Any());
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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            int grpId = 99999999;
            int kbnCd = 99999999;
            long ptIdInput = 2803;
            int seqNo = 99999999;
            int kouiKbnId = 99999999;

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
                SeqNo = seqNo
            };

            KouiKbnMst kouiKbnMst = new KouiKbnMst()
            {
                KouiKbnId = kouiKbnId,
                SortNo = seqNo,
                KouiKbn1 = 1,
                KouiKbn2 = 1,
                KouiGrpName = "Kaito"
            };

            RaiinListKoui raiinListKoui = new RaiinListKoui()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = seqNo,
                KouiKbnId = kouiKbnId
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = grpId
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = grpId
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = seqNo,
                ItemCd = "Kaito"
            };

            RaiinListInf raiinListInf = new RaiinListInf()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = rsvDate,
                RaiinNo = 0,
                GrpId = grpId,
                KbnCd = kbnCd
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(kouiKbnMst);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);
            tenant.Add(raiinListInf);

            var rsvkrtMstData = new List<RsvkrtMst>();
            var rsvkrtOdrInfs = new List<RsvkrtOdrInf>();
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();
            var rsvkrtOdrInfDetails = new List<RsvkrtOdrInfDetail>();

            try
            {
                //Act
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
                rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
                rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == ptIdInput || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo).ToList();
                rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
                var raiinListInfs = tenant.RaiinListInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.SinDate == rsvDate && x.RaiinNo == 0 && x.GrpId == grpId && x.KbnCd == kbnCd);

                //Assert
                Assert.That(result && !raiinListInfs.Any());
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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            int grpId = 99999999;
            int kbnCd = 99999999;
            long ptIdInput = 2803;
            int seqNo = 99999999;
            int kouiKbnId = 99999999;

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
                    ptIdInput,
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
                SeqNo = seqNo
            };

            KouiKbnMst kouiKbnMst = new KouiKbnMst()
            {
                KouiKbnId = kouiKbnId,
                SortNo = sortNo,
                KouiKbn1 = 1,
                KouiKbn2 = 1,
                KouiGrpName = "Kaito"
            };

            RaiinListKoui raiinListKoui = new RaiinListKoui()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = seqNo,
                KouiKbnId = kouiKbnId
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = grpId
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = seqNo,
                ItemCd = "Kaito"
            };

            RaiinListInf raiinListInf = new RaiinListInf()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = rsvDate,
                RaiinNo = 0,
                GrpId = grpId,
                KbnCd = kbnCd
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(kouiKbnMst);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);
            tenant.Add(raiinListInf);

            var rsvkrtMstData = new List<RsvkrtMst>();
            var rsvkrtOdrInfs = new List<RsvkrtOdrInf>();
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();
            var rsvkrtOdrInfDetails = new List<RsvkrtOdrInfDetail>();

            try
            {
                //Act
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
                rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
                rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == ptIdInput || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo).ToList();
                rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
                var raiinListInfs = tenant.RaiinListInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.SinDate == rsvDate && x.RaiinNo == 0 && x.GrpId == grpId && x.KbnCd == kbnCd);

                //Assert
                Assert.That(result && raiinListInfs.Count() == 1);
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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            int grpId = 99999999;
            int kbnCd = 99999999;
            long ptIdInput = 2803;
            int seqNo = 99999999;
            int kouiKbnId = 99999999;

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
                    ptIdInput,
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
                SeqNo = seqNo
            };

            KouiKbnMst kouiKbnMst = new KouiKbnMst()
            {
                KouiKbnId = kouiKbnId,
                SortNo = sortNo,
                KouiKbn1 = 1,
                KouiKbn2 = 1,
                KouiGrpName = "Kaito"
            };

            RaiinListKoui raiinListKoui = new RaiinListKoui()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = seqNo,
                KouiKbnId = kouiKbnId
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = kbnCd
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = seqNo,
                ItemCd = "Kaito"
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(kouiKbnMst);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);

            var rsvkrtMstData = new List<RsvkrtMst>();
            var rsvkrtOdrInfs = new List<RsvkrtOdrInf>();
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();
            var rsvkrtOdrInfDetails = new List<RsvkrtOdrInfDetail>();
            var raiinListInfInsert = new List<RaiinListInf>();

            try
            {
                //Act
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
                rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
                rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == ptIdInput || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo).ToList();
                rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
                raiinListInfInsert = tenant.RaiinListInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.SinDate == rsvDate && x.RaiinNo == 0 && x.GrpId == grpId && x.KbnCd == kbnCd).ToList();

                //Assert
                Assert.That(result && raiinListInfInsert.Any());
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
                tenant.RaiinListInfs.RemoveRange(raiinListInfInsert);
                tenant.SaveChanges();
            }
        }

        // Add with SinKouiKbn false
        [Test]
        public void TC_017_NextOrderRepository_TestSaveNextOrderRaiinListInf4False()
        {
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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
            int grpId = 99999999;
            int kbnCd = 99999999;
            long ptIdInput = 2803;
            int seqNo = 99999999;
            int kouiKbnId = 99999999;

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
                    ptIdInput,
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
                SeqNo = seqNo
            };

            RaiinListKoui raiinListKoui = new RaiinListKoui()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = seqNo,
                KouiKbnId = kouiKbnId
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = grpId
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = seqNo,
                ItemCd = "Kaito"
            };

            //Act
            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);

            var rsvkrtMstData = new List<RsvkrtMst>();
            var rsvkrtOdrInfs = new List<RsvkrtOdrInf>();
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();
            var rsvkrtOdrInfDetails = new List<RsvkrtOdrInfDetail>();
            var raiinListInfs = new List<RaiinListInf>();

            try
            {
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
                rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
                rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == ptIdInput || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo).ToList();
                rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
                raiinListInfs = tenant.RaiinListInfs.Where(x => x.HpId == hpId && (x.PtId == ptId || x.PtId == 2803) && x.SinDate == rsvDate && x.RaiinNo == 0 && x.GrpId == 99999999).ToList();

                //Assert
                Assert.That(result && raiinListInfs.Count() == 1);
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
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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

            //Act
            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);

            var rsvkrtMstData = new List<RsvkrtMst>();
            var rsvkrtOdrInfs = new List<RsvkrtOdrInf>();
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();
            var rsvkrtOdrInfDetails = new List<RsvkrtOdrInfDetail>();
            var raiinListInfs = new List<RaiinListInf>();

            try
            {
                tenant.SaveChanges();
                string finalKey = "GetNextOrderList28032001-1";
                _cache.StringAppend(finalKey, string.Empty);
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
                rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
                rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == 2803 || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo).ToList();
                rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
                raiinListInfs = tenant.RaiinListInfs.Where(x => x.HpId == hpId && (x.PtId == ptId || x.PtId == 2803) && x.SinDate == rsvDate && x.RaiinNo == 0 && x.GrpId == 99999999).ToList();

                //Assert
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
        public void TC_019_NextOrderRepository_TestInsertFalse()
        {
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, mockAmazonS3Options.Object);

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

            try
            {
                //Act
                tenant.SaveChanges();
                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                var rsvkrtMstsInsert = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && x.PtId == rsvkrtByomeis.First().PtId && x.RsvkrtNo == rsvkrtByomeis.First().RsvkrtNo).ToList();

                //Assert
                Assert.That(result && rsvkrtMstsInsert.Count() == 0);
            }
            finally
            {
                nextOrderRepository.ReleaseResource();
                tenant.RsvkrtMsts.Remove(rsvkrtMst);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_020_NextOrderRepository_TestUpdateSeqNoNextOrderFile_AddNewRsvkrtKarteImgInfsSuccess()
        {
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            AmazonS3Options appSettings = new AmazonS3Options() { BaseAccessUrl = baseAccessUrl };
            IOptions<AmazonS3Options> options = Options.Create(appSettings);

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, options);
            Random random = new();
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
            int grpId = 99999999;
            int kbnCd = 99999999;
            long ptIdInput = 2803;
            long ptNum = 28032001;
            int seqNo = 999999;

            List<string> ListFileItems = new List<string>()
            {
                CommonConstants.Store,
                CommonConstants.Karte
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

            FileItemModel fileItem = new FileItemModel(true, ListFileItems);
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();

            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    ptIdInput,
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

            PtInf ptInf = new PtInf()
            {
                HpId = hpId,
                PtId = ptId,
                SeqNo = seqNo,
                PtNum = ptId
            };

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
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = 99999999,
                KouiKbnId = 99999999
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = grpId
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = 99999999
            };

            RaiinListInf raiinListInf = new RaiinListInf()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = rsvDate,
                RaiinNo = 0,
                GrpId = grpId,
                KbnCd = kbnCd
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(kouiKbnMst);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);
            tenant.Add(raiinListInf);
            tenant.Add(ptInf);

            var rsvkrtMstData = new List<RsvkrtMst>();
            var rsvkrtOdrInfs = new List<RsvkrtOdrInf>();
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();
            var rsvkrtOdrInfDetails = new List<RsvkrtOdrInfDetail>();
            var raiinListInfs = new List<RaiinListInf>();
            var rsvkrtKarteImgInfs = new List<RsvkrtKarteImgInf>();

            try
            {
                //Act
                tenant.SaveChanges();
                List<string> listFileName = new() { "fileName.txt" };
                List<string> listFolders = new()
                {
                    CommonConstants.Store,
                    CommonConstants.Karte,
                    CommonConstants.NextPic
                };

                mockIAmazonS3Service.Setup(finder => finder.GetFolderUploadToPtNum(listFolders, ptNum))
               .Returns((List<string> folders, long ptNum) => $"/{CommonConstants.Store}/{CommonConstants.Karte}/{ptNum}/");

                mockIAmazonS3Service.Setup(finder => finder.GetUniqueFileNameKey(It.IsAny<string>()))
               .Returns((string fileName) => It.IsAny<string>());

                mockIAmazonS3Service.Setup(finder => finder.CopyObjectAsync(It.IsAny<string>(), It.IsAny<string>()).Result)
               .Returns((string sourceFile, string destinationFile) => false);

                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
                rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
                rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == ptIdInput || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo).ToList();
                rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
                raiinListInfs = tenant.RaiinListInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.SinDate == rsvDate && x.RaiinNo == 0 && x.GrpId == grpId && x.KbnCd == kbnCd).ToList();
                rsvkrtKarteImgInfs = tenant.RsvkrtKarteImgInfs.Where(x => x.PtId == ptId && x.RsvkrtNo == rsvkrtNo).ToList();

                //Assert
                Assert.That(result && !raiinListInfs.Any() && rsvkrtKarteImgInfs.Any());
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
                tenant.PtInfs.Remove(ptInf);
                tenant.RsvkrtKarteImgInfs.RemoveRange(rsvkrtKarteImgInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_021_NextOrderRepository_UpdateSeqNoNextOrderFile_listFileNameExitsStringEmpty_Success()
        {
            //Arrange
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            AmazonS3Options appSettings = new AmazonS3Options() { BaseAccessUrl = baseAccessUrl };
            IOptions<AmazonS3Options> options = Options.Create(appSettings);

            var nextOrderRepository = new NextOrderRepository(TenantProvider, mockIAmazonS3Service.Object, mockIConfiguration.Object, options);
            Random random = new();
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
            int grpId = 99999999;
            int kbnCd = 99999999;
            long ptIdInput = 2803;
            long ptNum = 28032001;
            int seqNo = 999999;

            List<string> ListFileItems = new List<string>()
            {
                CommonConstants.Store,
                CommonConstants.Karte,
                "BaseAccessUrl/"
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

            FileItemModel fileItem = new FileItemModel(true, ListFileItems);
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            var tenant = TenantProvider.GetTrackingTenantDataContext();

            List<NextOrderModel> nextOrderModels = new List<NextOrderModel>()
            {
                new NextOrderModel (
                    hpId,
                    ptIdInput,
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

            PtInf ptInf = new PtInf()
            {
                HpId = hpId,
                PtId = ptId,
                SeqNo = seqNo,
                PtNum = ptId
            };

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
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = 99999999,
                KouiKbnId = 99999999
            };

            RaiinListDetail raiinListDetail = new RaiinListDetail()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd
            };

            RaiinListMst raiinListMst = new RaiinListMst()
            {
                HpId = hpId,
                GrpId = grpId
            };

            RaiinListItem raiinListItem = new RaiinListItem()
            {
                HpId = hpId,
                GrpId = grpId,
                KbnCd = kbnCd,
                SeqNo = 99999999
            };

            RaiinListInf raiinListInf = new RaiinListInf()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = rsvDate,
                RaiinNo = 0,
                GrpId = grpId,
                KbnCd = kbnCd
            };

            tenant.Add(rsvkrtMst);
            tenant.Add(rsvkrtKarte);
            tenant.Add(kouiKbnMst);
            tenant.Add(raiinListKoui);
            tenant.Add(raiinListDetail);
            tenant.Add(raiinListMst);
            tenant.Add(raiinListItem);
            tenant.Add(raiinListInf);
            tenant.Add(ptInf);

            var rsvkrtMstData = new List<RsvkrtMst>();
            var rsvkrtOdrInfs = new List<RsvkrtOdrInf>();
            var rsvkrtKarteInfs = new List<RsvkrtKarteInf>();
            var rsvkrtOdrInfDetails = new List<RsvkrtOdrInfDetail>();
            var raiinListInfs = new List<RaiinListInf>();
            var rsvkrtKarteImgInfs = new List<RsvkrtKarteImgInf>();

            try
            {
                //Act
                tenant.SaveChanges();
                List<string> listFileName = new() { "fileName.txt" };
                List<string> listFolders = new()
                {
                    CommonConstants.Store,
                    CommonConstants.Karte,
                    CommonConstants.NextPic,
                    "BaseAccessUrl/"
                };

                mockIAmazonS3Service.Setup(finder => finder.GetFolderUploadToPtNum(listFolders, ptNum))
               .Returns((List<string> folders, long ptNum) => $"/{CommonConstants.Store}/{CommonConstants.Karte}/{ptNum}/");

                mockIAmazonS3Service.Setup(finder => finder.GetUniqueFileNameKey(It.IsAny<string>()))
               .Returns((string fileName) => It.IsAny<string>());

                mockIAmazonS3Service.Setup(finder => finder.CopyObjectAsync(It.IsAny<string>(), It.IsAny<string>()).Result)
               .Returns((string sourceFile, string destinationFile) => false);

                mockIAmazonS3Service.Setup(finder => finder.CopyObjectAsync(It.IsAny<string>(), It.IsAny<string>()).Result)
                .Returns((string sourceFile, string destinationFile) => true);

                var result = nextOrderRepository.Upsert(userId, hpId, ptId, nextOrderModels);
                rsvkrtKarteInfs = tenant.RsvkrtKarteInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.KarteKbn == 1).ToList();
                rsvkrtOdrInfs = tenant.RsvkrtOdrInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RsvkrtNo == rsvkrtNo && x.Id == 1).ToList();
                rsvkrtMstData = tenant.RsvkrtMsts.Where(x => x.HpId == hpId && (x.PtId == ptIdInput || x.PtId == ptId) && x.RsvkrtNo == rsvkrtNo).ToList();
                rsvkrtOdrInfDetails = tenant.RsvkrtOdrInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
                raiinListInfs = tenant.RaiinListInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.SinDate == rsvDate && x.RaiinNo == 0 && x.GrpId == grpId && x.KbnCd == kbnCd).ToList();
                rsvkrtKarteImgInfs = tenant.RsvkrtKarteImgInfs.Where(x => x.PtId == ptId && x.RsvkrtNo == rsvkrtNo).ToList();

                //Assert
                Assert.That(result && !raiinListInfs.Any() && rsvkrtKarteImgInfs.Any());
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
                tenant.PtInfs.Remove(ptInf);
                tenant.RsvkrtKarteImgInfs.RemoveRange(rsvkrtKarteImgInfs);
                tenant.SaveChanges();
            }
        }
    }
}
