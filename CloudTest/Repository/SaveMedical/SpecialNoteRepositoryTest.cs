using Domain.Models.SpecialNote.SummaryInf;
using Entity.Tenant;
using Infrastructure.Repositories.SpecialNote;
using Microsoft.Extensions.Configuration;
using Moq;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.PtCmtInf;
using Domain.Models.SpecialNote;
using Helper.Constants;
using Helper.Redis;
using StackExchange.Redis;

namespace CloudUnitTest.Repository.SaveMedical;

public class SpecialNoteRepositoryTest : BaseUT
{
    private readonly IDatabase _cache;

    public SpecialNoteRepositoryTest()
    {
        string connection = string.Concat("10.2.15.78", ":", "6379");
        if (RedisConnectorHelper.RedisHost != connection)
        {
            RedisConnectorHelper.RedisHost = connection;
        }
        _cache = RedisConnectorHelper.Connection.GetDatabase();
    }


    #region IsInvalidInputId
    [Test]
    public void TC_001_IsInvalidInputId_TestSuccess()
    {
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(9999, 9999999);
        long ptId = random.NextInt64(99999, 999999999);

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = specialNoteRepository.IsInvalidInputId(hpId, ptId);

            // Assert

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_002_IsInvalidInputId_TestHpInfFalse()
    {
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = specialNoteRepository.IsInvalidInputId(random.Next(999999, 9999999), ptId);

            // Assert

            Assert.True(result == false);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_003_IsInvalidInputId_TestPtInfFalse()
    {
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = specialNoteRepository.IsInvalidInputId(hpId, random.NextInt64(9999999999, 99999999999));

            // Assert

            Assert.True(result == false);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion IsInvalidInputId

    #region SaveSpecialNote
    [Test]
    public void TC_004_SaveSpecialNote_TestSuccess()
    {
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);
        var mockISpecialNoteRepository = new Mock<ISpecialNoteRepository>();
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, null, null, userId);

            // Assert
            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_005_SaveSpecialNote_TestFalse()
    {
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);
        var mockISpecialNoteRepository = new Mock<ISpecialNoteRepository>();
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = specialNoteRepository.SaveSpecialNote(hpId, random.NextInt64(9999, 99999999), sinDate, null, null, null, userId);

            // Assert

            Assert.True(!result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion

    #region SaveSummaryInf
    [Test]
    public void TC_006_SaveSummaryInf_TestCreateSuccess()
    {
        var tenant = TenantProvider.GetTrackingTenantDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        SummaryInfModel summaryInfModel = new SummaryInfModel(
                                              random.Next(999, 99999),
                                              hpId,
                                              ptId,
                                              random.Next(999, 99999),
                                              "textSumaryInf",
                                              "rTextSumaryInf",
                                              DateTime.UtcNow,
                                              DateTime.UtcNow);

        SummaryInf? summaryInf = null;

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.SummaryInfGetList + "_" + summaryInfModel.HpId + "_" + summaryInfModel.PtId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, summaryInfModel, null, null, userId);

            // Assert
            summaryInf = tenant.SummaryInfs.FirstOrDefault(item => item.HpId == summaryInfModel.HpId
                                                                   && item.PtId == summaryInfModel.PtId
                                                                   && item.SeqNo == summaryInfModel.SeqNo
                                                                   && item.Text == summaryInfModel.Text
                                                                   && item.CreateId == userId
                                                                   && item.UpdateId == userId);
            result = result
                && summaryInf != null
                && summaryInf.HpId == summaryInfModel.HpId
                && summaryInf.PtId == summaryInfModel.PtId
                && summaryInf.SeqNo == summaryInfModel.SeqNo
                && summaryInf.Text == summaryInfModel.Text
                && summaryInf.UpdateId == userId
                && summaryInf.CreateId == userId;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (summaryInf != null)
            {
                tenant.SummaryInfs.Remove(summaryInf);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_007_SaveSummaryInf_TestUpdateSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        SummaryInfModel summaryInfModel = new SummaryInfModel(
                                              random.Next(999, 99999),
                                              hpId,
                                              ptId,
                                              random.Next(999999, 99999999),
                                              "textSumaryInf",
                                              "rTextSumaryInf",
                                              DateTime.UtcNow,
                                              DateTime.UtcNow);

        SummaryInf? summaryInf = new SummaryInf()
        {
            Id = summaryInfModel.Id,
            HpId = summaryInfModel.HpId,
            PtId = summaryInfModel.PtId,
            SeqNo = summaryInfModel.SeqNo,
        };
        tenant.SummaryInfs.Add(summaryInf);

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.SummaryInfGetList + "_" + summaryInfModel.HpId + "_" + summaryInfModel.PtId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);

        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, summaryInfModel, null, null, userId);

            // Assert
            var summaryInfAfter = tenant.SummaryInfs.FirstOrDefault(item => item.Id == summaryInfModel.Id
                                                                            && item.HpId == summaryInfModel.HpId);

            result = result
                && summaryInfAfter != null
                && summaryInfAfter.Id == summaryInfModel.Id
                && summaryInfAfter.HpId == summaryInfModel.HpId
                && summaryInfAfter.PtId == summaryInfModel.PtId
                && summaryInfAfter.SeqNo == summaryInfModel.SeqNo
                && summaryInfAfter.Text == summaryInfModel.Text
                && summaryInfAfter.UpdateId == userId;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (summaryInf != null)
            {
                tenant.SummaryInfs.Remove(summaryInf);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SaveSummaryInf

    #region SaveImportantNote
    #region SaveAlrgyFoodItems
    [Test]
    public void TC_008_SaveImportantNote_TestSaveAlrgyFoodItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        var ptAlrgyFoodModel = new PtAlrgyFoodModel(hpId, ptId, 0, random.Next(999, 99999), "alrgyKbnPtAlrgyFood", 20220202, 20220202, "cmtPtAlrgyFood", 0, "foodNamePtAlrgyFood");
        PtAlrgyFood? ptAlrgyFood = null;

        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { ptAlrgyFoodModel },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.AlrgyFoodGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            ptAlrgyFood = tenant.PtAlrgyFoods.FirstOrDefault(item => item.HpId == ptAlrgyFoodModel.HpId
                                                                     && item.PtId == ptAlrgyFoodModel.PtId
                                                                     && item.SortNo == ptAlrgyFoodModel.SortNo
                                                                     && item.AlrgyKbn == ptAlrgyFoodModel.AlrgyKbn
                                                                     && item.StartDate == ptAlrgyFoodModel.StartDate
                                                                     && item.EndDate == ptAlrgyFoodModel.EndDate
                                                                     && item.Cmt == ptAlrgyFoodModel.Cmt
                                                                     && item.IsDeleted == 0);

            result = result
                && ptAlrgyFood != null
                && ptAlrgyFood.HpId == ptAlrgyFoodModel.HpId
                && ptAlrgyFood.PtId == ptAlrgyFoodModel.PtId
                && ptAlrgyFood.SortNo == ptAlrgyFoodModel.SortNo
                && ptAlrgyFood.AlrgyKbn == ptAlrgyFoodModel.AlrgyKbn
                && ptAlrgyFood.StartDate == ptAlrgyFoodModel.StartDate
                && ptAlrgyFood.EndDate == ptAlrgyFoodModel.EndDate
                && ptAlrgyFood.Cmt == ptAlrgyFoodModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            if (_cache.KeyExists(finalKey))
            {
                _cache.KeyDelete(finalKey);
            }

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptAlrgyFood != null)
            {
                tenant.PtAlrgyFoods.Remove(ptAlrgyFood);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_009_SaveImportantNote_TestUpdateAlrgyFoodItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        var ptAlrgyFoodModel = new PtAlrgyFoodModel(hpId, ptId, random.Next(999, 99999), random.Next(999, 99999), "alrgyKbnPtAlrgyFood", 20220202, 20220202, "cmtPtAlrgyFood", 0, "foodNamePtAlrgyFood");
        PtAlrgyFood? ptAlrgyFood = new PtAlrgyFood
        {
            HpId = ptAlrgyFoodModel.HpId,
            PtId = ptAlrgyFoodModel.PtId,
            SeqNo = ptAlrgyFoodModel.SeqNo,
        };

        tenant.PtAlrgyFoods.Add(ptAlrgyFood);
        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { ptAlrgyFoodModel },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.AlrgyFoodGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            var ptAlrgyFoodAfter = tenant.PtAlrgyFoods.FirstOrDefault(item => item.HpId == ptAlrgyFoodModel.HpId
                                                                              && item.PtId == ptAlrgyFoodModel.PtId
                                                                              && item.SeqNo == ptAlrgyFoodModel.SeqNo
                                                                              && item.SortNo == ptAlrgyFoodModel.SortNo
                                                                              && item.AlrgyKbn == ptAlrgyFoodModel.AlrgyKbn
                                                                              && item.StartDate == ptAlrgyFoodModel.StartDate
                                                                              && item.EndDate == ptAlrgyFoodModel.EndDate
                                                                              && item.Cmt == ptAlrgyFoodModel.Cmt
                                                                              && item.IsDeleted == ptAlrgyFoodModel.IsDeleted);

            result = result
                && ptAlrgyFoodAfter != null
                && ptAlrgyFoodAfter.HpId == ptAlrgyFoodModel.HpId
                && ptAlrgyFoodAfter.PtId == ptAlrgyFoodModel.PtId
                && ptAlrgyFoodAfter.SeqNo == ptAlrgyFoodModel.SeqNo
                && ptAlrgyFoodAfter.SortNo == ptAlrgyFoodModel.SortNo
                && ptAlrgyFoodAfter.AlrgyKbn == ptAlrgyFoodModel.AlrgyKbn
                && ptAlrgyFoodAfter.StartDate == ptAlrgyFoodModel.StartDate
                && ptAlrgyFoodAfter.EndDate == ptAlrgyFoodModel.EndDate
                && ptAlrgyFoodAfter.Cmt == ptAlrgyFoodModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptAlrgyFood != null)
            {
                tenant.PtAlrgyFoods.Remove(ptAlrgyFood);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SaveAlrgyFoodItems

    #region SaveElseItems
    [Test]
    public void TC_010_SaveImportantNote_TestSaveElseItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        var ptAlrgyElseModel = new PtAlrgyElseModel(hpId, ptId, 0, random.Next(999, 99999), "alrgyNamePtAlrgyElse", 20220202, 20220202, "cmtPtAlrgyElse", 0);
        PtAlrgyElse? ptAlrgyElse = null;

        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { ptAlrgyElseModel },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.AlrgyElseGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            ptAlrgyElse = tenant.PtAlrgyElses.FirstOrDefault(item => item.HpId == ptAlrgyElseModel.HpId
                                                                     && item.PtId == ptAlrgyElseModel.PtId
                                                                     && item.SortNo == ptAlrgyElseModel.SortNo
                                                                     && item.AlrgyName == ptAlrgyElseModel.AlrgyName
                                                                     && item.StartDate == ptAlrgyElseModel.StartDate
                                                                     && item.EndDate == ptAlrgyElseModel.EndDate
                                                                     && item.Cmt == ptAlrgyElseModel.Cmt
                                                                     && item.IsDeleted == 0);

            result = result
                && ptAlrgyElse != null
                && ptAlrgyElse.HpId == ptAlrgyElseModel.HpId
                && ptAlrgyElse.PtId == ptAlrgyElseModel.PtId
                && ptAlrgyElse.SortNo == ptAlrgyElseModel.SortNo
                && ptAlrgyElse.AlrgyName == ptAlrgyElseModel.AlrgyName
                && ptAlrgyElse.StartDate == ptAlrgyElseModel.StartDate
                && ptAlrgyElse.EndDate == ptAlrgyElseModel.EndDate
                && ptAlrgyElse.Cmt == ptAlrgyElseModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptAlrgyElse != null)
            {
                tenant.PtAlrgyElses.Remove(ptAlrgyElse);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_011_SaveImportantNote_TestUpdateElseItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        var ptAlrgyElseModel = new PtAlrgyElseModel(hpId, ptId, random.Next(999, 99999), random.Next(999, 99999), "alrgyNamePtAlrgyElse", 20220202, 20220202, "cmtPtAlrgyElse", 0);
        PtAlrgyElse? ptAlrgyElse = new PtAlrgyElse()
        {
            HpId = ptAlrgyElseModel.HpId,
            PtId = ptAlrgyElseModel.PtId,
            SeqNo = ptAlrgyElseModel.SeqNo
        };
        tenant.PtAlrgyElses.Add(ptAlrgyElse);

        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { ptAlrgyElseModel },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.AlrgyElseGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            var ptAlrgyElseAfter = tenant.PtAlrgyElses.FirstOrDefault(item => item.HpId == ptAlrgyElseModel.HpId
                                                                      && item.PtId == ptAlrgyElseModel.PtId
                                                                      && item.SeqNo == ptAlrgyElseModel.SeqNo
                                                                      && item.SortNo == ptAlrgyElseModel.SortNo
                                                                      && item.AlrgyName == ptAlrgyElseModel.AlrgyName
                                                                      && item.StartDate == ptAlrgyElseModel.StartDate
                                                                      && item.EndDate == ptAlrgyElseModel.EndDate
                                                                      && item.Cmt == ptAlrgyElseModel.Cmt
                                                                      && item.IsDeleted == ptAlrgyElseModel.IsDeleted);

            result = result
                && ptAlrgyElseAfter != null
                && ptAlrgyElseAfter.HpId == ptAlrgyElseModel.HpId
                && ptAlrgyElseAfter.PtId == ptAlrgyElseModel.PtId
                && ptAlrgyElseAfter.SortNo == ptAlrgyElseModel.SortNo
                && ptAlrgyElseAfter.SeqNo == ptAlrgyElseModel.SeqNo
                && ptAlrgyElseAfter.AlrgyName == ptAlrgyElseModel.AlrgyName
                && ptAlrgyElseAfter.StartDate == ptAlrgyElseModel.StartDate
                && ptAlrgyElseAfter.EndDate == ptAlrgyElseModel.EndDate
                && ptAlrgyElseAfter.Cmt == ptAlrgyElseModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptAlrgyElse != null)
            {
                tenant.PtAlrgyElses.Remove(ptAlrgyElse);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SaveElseItems

    #region SaveDrugItems
    [Test]
    public void TC_012_SaveImportantNote_TestSaveDrugItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        var ptAlrgyDrugModel = new PtAlrgyDrugModel(hpId, ptId, 0, random.Next(999, 99999), "itemCdUT", " drugNamePtAlrgyDrug", 20220202, 20220202, "cmtPtAlrgyDrug", 0);
        PtAlrgyDrug? ptAlrgyDrug = null;

        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { ptAlrgyDrugModel },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.PtAlrgyDrugGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            ptAlrgyDrug = tenant.PtAlrgyDrugs.FirstOrDefault(item => item.HpId == ptAlrgyDrugModel.HpId
                                                                     && item.PtId == ptAlrgyDrugModel.PtId
                                                                     && item.SortNo == ptAlrgyDrugModel.SortNo
                                                                     && item.ItemCd == ptAlrgyDrugModel.ItemCd
                                                                     && item.DrugName == ptAlrgyDrugModel.DrugName
                                                                     && item.StartDate == ptAlrgyDrugModel.StartDate
                                                                     && item.EndDate == ptAlrgyDrugModel.EndDate
                                                                     && item.Cmt == ptAlrgyDrugModel.Cmt
                                                                     && item.IsDeleted == 0);

            result = result
                && ptAlrgyDrug != null
                && ptAlrgyDrug.HpId == ptAlrgyDrugModel.HpId
                && ptAlrgyDrug.PtId == ptAlrgyDrugModel.PtId
                && ptAlrgyDrug.SortNo == ptAlrgyDrugModel.SortNo
                && ptAlrgyDrug.ItemCd == ptAlrgyDrugModel.ItemCd
                && ptAlrgyDrug.DrugName == ptAlrgyDrugModel.DrugName
                && ptAlrgyDrug.StartDate == ptAlrgyDrugModel.StartDate
                && ptAlrgyDrug.EndDate == ptAlrgyDrugModel.EndDate
                && ptAlrgyDrug.Cmt == ptAlrgyDrugModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptAlrgyDrug != null)
            {
                tenant.PtAlrgyDrugs.Remove(ptAlrgyDrug);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_013_SaveImportantNote_TestUpdateDrugItemsSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        var ptAlrgyDrugModel = new PtAlrgyDrugModel(hpId, ptId, random.Next(999, 99999), random.Next(999, 99999), "itemCdUT", " drugNamePtAlrgyDrug", 20220202, 20220202, "cmtPtAlrgyDrug", 0);
        PtAlrgyDrug? ptAlrgyDrug = new PtAlrgyDrug()
        {
            HpId = ptAlrgyDrugModel.HpId,
            PtId = ptAlrgyDrugModel.PtId,
            SeqNo = ptAlrgyDrugModel.SeqNo
        };
        tenant.PtAlrgyDrugs.Add(ptAlrgyDrug);

        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { ptAlrgyDrugModel },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.PtAlrgyDrugGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            var ptAlrgyDrugAfter = tenant.PtAlrgyDrugs.FirstOrDefault(item => item.HpId == ptAlrgyDrugModel.HpId
                                                                              && item.PtId == ptAlrgyDrugModel.PtId
                                                                              && item.SortNo == ptAlrgyDrugModel.SortNo
                                                                              && item.SeqNo == ptAlrgyDrugModel.SeqNo
                                                                              && item.ItemCd == ptAlrgyDrugModel.ItemCd
                                                                              && item.DrugName == ptAlrgyDrugModel.DrugName
                                                                              && item.StartDate == ptAlrgyDrugModel.StartDate
                                                                              && item.EndDate == ptAlrgyDrugModel.EndDate
                                                                              && item.Cmt == ptAlrgyDrugModel.Cmt
                                                                              && item.IsDeleted == ptAlrgyDrugModel.IsDeleted);

            result = result
                && ptAlrgyDrugAfter != null
                && ptAlrgyDrugAfter.HpId == ptAlrgyDrugModel.HpId
                && ptAlrgyDrugAfter.PtId == ptAlrgyDrugModel.PtId
                && ptAlrgyDrugAfter.SortNo == ptAlrgyDrugModel.SortNo
                && ptAlrgyDrugAfter.SeqNo == ptAlrgyDrugModel.SeqNo
                && ptAlrgyDrugAfter.ItemCd == ptAlrgyDrugModel.ItemCd
                && ptAlrgyDrugAfter.DrugName == ptAlrgyDrugModel.DrugName
                && ptAlrgyDrugAfter.StartDate == ptAlrgyDrugModel.StartDate
                && ptAlrgyDrugAfter.EndDate == ptAlrgyDrugModel.EndDate
                && ptAlrgyDrugAfter.Cmt == ptAlrgyDrugModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptAlrgyDrug != null)
            {
                tenant.PtAlrgyDrugs.Remove(ptAlrgyDrug);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SaveElseItems

    #region SaveKioRekiItems
    [Test]
    public void TC_014_SaveImportantNote_TestSaveKioRekiItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };
        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);
        var ptKioRekiModel = new PtKioRekiModel(hpId, ptId, 0, random.Next(999, 99999), "byCdUT", "byotaiCdUT", "byomeiPtKioReki", 20220202, "cmtPtKioReki", 0);
        PtKioReki? ptKioReki = null;
        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { ptKioRekiModel },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.KioRekiGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            ptKioReki = tenant.PtKioRekis.FirstOrDefault(item => item.HpId == ptKioRekiModel.HpId
                                                                 && item.PtId == ptKioRekiModel.PtId
                                                                 && item.SortNo == ptKioRekiModel.SortNo
                                                                 && item.ByomeiCd == ptKioRekiModel.ByomeiCd
                                                                 && item.ByotaiCd == ptKioRekiModel.ByotaiCd
                                                                 && item.Byomei == ptKioRekiModel.Byomei
                                                                 && item.StartDate == ptKioRekiModel.StartDate
                                                                 && item.Cmt == ptKioRekiModel.Cmt
                                                                 && item.IsDeleted == 0);

            result = result
                     && ptKioReki != null
                     && ptKioReki.HpId == ptKioRekiModel.HpId
                     && ptKioReki.PtId == ptKioRekiModel.PtId
                     && ptKioReki.SortNo == ptKioRekiModel.SortNo
                     && ptKioReki.ByomeiCd == ptKioRekiModel.ByomeiCd
                     && ptKioReki.ByotaiCd == ptKioRekiModel.ByotaiCd
                     && ptKioReki.Byomei == ptKioRekiModel.Byomei
                     && ptKioReki.StartDate == ptKioRekiModel.StartDate
                     && ptKioReki.Cmt == ptKioRekiModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptKioReki != null)
            {
                tenant.PtKioRekis.Remove(ptKioReki);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_015_SaveImportantNote_TestUpdateKioRekiItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        var ptKioRekiModel = new PtKioRekiModel(hpId, ptId, random.Next(999, 99999), random.Next(999, 99999), "byCdUT", "byotaiCdUT", "byomeiPtKioReki", 20220202, "cmtPtKioReki", 0);
        PtKioReki? ptKioReki = new PtKioReki()
        {
            HpId = ptKioRekiModel.HpId,
            PtId = ptKioRekiModel.PtId,
            SeqNo = ptKioRekiModel.SeqNo
        };
        tenant.PtKioRekis.Add(ptKioReki);

        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { ptKioRekiModel },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.KioRekiGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            var ptKioRekiAfter = tenant.PtKioRekis.FirstOrDefault(item => item.HpId == ptKioRekiModel.HpId
                                                                          && item.PtId == ptKioRekiModel.PtId
                                                                          && item.SortNo == ptKioRekiModel.SortNo
                                                                          && item.SeqNo == ptKioRekiModel.SeqNo
                                                                          && item.ByomeiCd == ptKioRekiModel.ByomeiCd
                                                                          && item.ByotaiCd == ptKioRekiModel.ByotaiCd
                                                                          && item.Byomei == ptKioRekiModel.Byomei
                                                                          && item.StartDate == ptKioRekiModel.StartDate
                                                                          && item.Cmt == ptKioRekiModel.Cmt
                                                                          && item.IsDeleted == ptKioRekiModel.IsDeleted);

            result = result
                && ptKioRekiAfter != null
                && ptKioRekiAfter.HpId == ptKioRekiModel.HpId
                && ptKioRekiAfter.PtId == ptKioRekiModel.PtId
                && ptKioRekiAfter.SortNo == ptKioRekiModel.SortNo
                && ptKioRekiAfter.SeqNo == ptKioRekiModel.SeqNo
                && ptKioRekiAfter.ByomeiCd == ptKioRekiModel.ByomeiCd
                && ptKioRekiAfter.ByotaiCd == ptKioRekiModel.ByotaiCd
                && ptKioRekiAfter.Byomei == ptKioRekiModel.Byomei
                && ptKioRekiAfter.StartDate == ptKioRekiModel.StartDate
                && ptKioRekiAfter.Cmt == ptKioRekiModel.Cmt
                && ptKioRekiAfter.IsDeleted == ptKioRekiModel.IsDeleted;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptKioReki != null)
            {
                tenant.PtKioRekis.Remove(ptKioReki);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SaveElseItems

    #region SaveInfectionsItems
    [Test]
    public void TC_016_SaveImportantNote_TestSaveInfectionsItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };
        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);
        var ptInfectionModel = new PtInfectionModel(hpId, ptId, 0, random.Next(999, 99999), "byCdUT", "byotaiCdPtInfection", "byomeiPtInfection", 20220202, "cmtPtInfection", 0);
        PtInfection? ptInfection = null;
        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { ptInfectionModel },
                                                    new() { },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.InfectionGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            ptInfection = tenant.PtInfection.FirstOrDefault(item => item.HpId == ptInfectionModel.HpId
                                                                    && item.PtId == ptInfectionModel.PtId
                                                                    && item.SortNo == ptInfectionModel.SortNo
                                                                    && item.ByomeiCd == ptInfectionModel.ByomeiCd
                                                                    && item.ByotaiCd == ptInfectionModel.ByotaiCd
                                                                    && item.Byomei == ptInfectionModel.Byomei
                                                                    && item.StartDate == ptInfectionModel.StartDate
                                                                    && item.Cmt == ptInfectionModel.Cmt
                                                                    && item.IsDeleted == 0);

            result = result
                     && ptInfection != null
                     && ptInfection.HpId == ptInfectionModel.HpId
                     && ptInfection.PtId == ptInfectionModel.PtId
                     && ptInfection.SortNo == ptInfectionModel.SortNo
                     && ptInfection.ByomeiCd == ptInfectionModel.ByomeiCd
                     && ptInfection.ByotaiCd == ptInfectionModel.ByotaiCd
                     && ptInfection.Byomei == ptInfectionModel.Byomei
                     && ptInfection.StartDate == ptInfectionModel.StartDate
                     && ptInfection.Cmt == ptInfectionModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptInfection != null)
            {
                tenant.PtInfection.Remove(ptInfection);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_017_SaveImportantNote_TestUpdateInfectionsItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        var ptInfectionModel = new PtInfectionModel(hpId, ptId, random.Next(999, 99999), random.Next(999, 99999), "byCdUT", "byotaiCdPtInfection", "byomeiPtInfection", 20220202, "cmtPtInfection", 0);
        PtInfection? ptInfection = new PtInfection()
        {
            HpId = ptInfectionModel.HpId,
            PtId = ptInfectionModel.PtId,
            SeqNo = ptInfectionModel.SeqNo
        };
        tenant.PtInfection.Add(ptInfection);

        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { ptInfectionModel },
                                                    new() { },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.InfectionGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            var ptInfectionAfter = tenant.PtInfection.FirstOrDefault(item => item.HpId == ptInfectionModel.HpId
                                                                    && item.PtId == ptInfectionModel.PtId
                                                                    && item.SortNo == ptInfectionModel.SortNo
                                                                    && item.SeqNo == ptInfectionModel.SeqNo
                                                                    && item.ByomeiCd == ptInfectionModel.ByomeiCd
                                                                    && item.ByotaiCd == ptInfectionModel.ByotaiCd
                                                                    && item.Byomei == ptInfectionModel.Byomei
                                                                    && item.StartDate == ptInfectionModel.StartDate
                                                                    && item.Cmt == ptInfectionModel.Cmt
                                                                    && item.IsDeleted == ptInfectionModel.IsDeleted);

            result = result
                     && ptInfectionAfter != null
                     && ptInfectionAfter.HpId == ptInfectionModel.HpId
                     && ptInfectionAfter.PtId == ptInfectionModel.PtId
                     && ptInfectionAfter.SortNo == ptInfectionModel.SortNo
                     && ptInfectionAfter.SeqNo == ptInfectionModel.SeqNo
                     && ptInfectionAfter.ByomeiCd == ptInfectionModel.ByomeiCd
                     && ptInfectionAfter.ByotaiCd == ptInfectionModel.ByotaiCd
                     && ptInfectionAfter.Byomei == ptInfectionModel.Byomei
                     && ptInfectionAfter.StartDate == ptInfectionModel.StartDate
                     && ptInfectionAfter.Cmt == ptInfectionModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptInfection != null)
            {
                tenant.PtInfection.Remove(ptInfection);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SaveInfectionsItems

    #region SaveOtherDrugItems
    [Test]
    public void TC_018_SaveImportantNote_TestSaveOtherDrugItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };
        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);
        var ptOtherDrugModel = new PtOtherDrugModel(hpId, ptId, 0, random.Next(999, 99999), "itemCdUT", "drugNamePtOtherDrug", 20220202, 20220202, "cmtPtOtherDrug", 0);
        PtOtherDrug? ptOtherDrug = null;
        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { ptOtherDrugModel },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.OtherDrugGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            ptOtherDrug = tenant.PtOtherDrug.FirstOrDefault(item => item.HpId == ptOtherDrugModel.HpId
                                                                    && item.PtId == ptOtherDrugModel.PtId
                                                                    && item.SortNo == ptOtherDrugModel.SortNo
                                                                    && item.ItemCd == ptOtherDrugModel.ItemCd
                                                                    && item.DrugName == ptOtherDrugModel.DrugName
                                                                    && item.StartDate == ptOtherDrugModel.StartDate
                                                                    && item.EndDate == ptOtherDrugModel.EndDate
                                                                    && item.Cmt == ptOtherDrugModel.Cmt
                                                                    && item.IsDeleted == 0);

            result = result
                     && ptOtherDrug != null
                     && ptOtherDrug.HpId == ptOtherDrugModel.HpId
                     && ptOtherDrug.PtId == ptOtherDrugModel.PtId
                     && ptOtherDrug.SortNo == ptOtherDrugModel.SortNo
                     && ptOtherDrug.ItemCd == ptOtherDrugModel.ItemCd
                     && ptOtherDrug.DrugName == ptOtherDrugModel.DrugName
                     && ptOtherDrug.StartDate == ptOtherDrugModel.StartDate
                     && ptOtherDrug.EndDate == ptOtherDrugModel.EndDate
                     && ptOtherDrug.Cmt == ptOtherDrugModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptOtherDrug != null)
            {
                tenant.PtOtherDrug.Remove(ptOtherDrug);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_019_SaveImportantNote_TestUpdateOtherDrugItemmSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        var ptOtherDrugModel = new PtOtherDrugModel(hpId, ptId, random.Next(999, 99999), random.Next(999, 99999), "itemCdUT", "drugNamePtOtherDrug", 20220202, 20220202, "cmtPtOtherDrug", 0);
        PtOtherDrug? ptOtherDrug = new PtOtherDrug()
        {
            HpId = ptOtherDrugModel.HpId,
            PtId = ptOtherDrugModel.PtId,
            SeqNo = ptOtherDrugModel.SeqNo
        };
        tenant.PtOtherDrug.Add(ptOtherDrug);

        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { ptOtherDrugModel },
                                                    new() { },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.OtherDrugGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            var ptOtherDrugAfter = tenant.PtOtherDrug.FirstOrDefault(item => item.HpId == ptOtherDrugModel.HpId
                                                                             && item.PtId == ptOtherDrugModel.PtId
                                                                             && item.SortNo == ptOtherDrugModel.SortNo
                                                                             && item.SeqNo == ptOtherDrugModel.SeqNo
                                                                             && item.ItemCd == ptOtherDrugModel.ItemCd
                                                                             && item.DrugName == ptOtherDrugModel.DrugName
                                                                             && item.StartDate == ptOtherDrugModel.StartDate
                                                                             && item.EndDate == ptOtherDrugModel.EndDate
                                                                             && item.Cmt == ptOtherDrugModel.Cmt
                                                                             && item.IsDeleted == ptOtherDrugModel.IsDeleted);

            result = result
                     && ptOtherDrugAfter != null
                     && ptOtherDrugAfter.HpId == ptOtherDrugModel.HpId
                     && ptOtherDrugAfter.PtId == ptOtherDrugModel.PtId
                     && ptOtherDrugAfter.SortNo == ptOtherDrugModel.SortNo
                     && ptOtherDrugAfter.SeqNo == ptOtherDrugModel.SeqNo
                     && ptOtherDrugAfter.ItemCd == ptOtherDrugModel.ItemCd
                     && ptOtherDrugAfter.DrugName == ptOtherDrugModel.DrugName
                     && ptOtherDrugAfter.StartDate == ptOtherDrugModel.StartDate
                     && ptOtherDrugAfter.EndDate == ptOtherDrugModel.EndDate
                     && ptOtherDrugAfter.Cmt == ptOtherDrugModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptOtherDrug != null)
            {
                tenant.PtOtherDrug.Remove(ptOtherDrug);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SaveOtherDrugItems

    #region SaveOtcDrugItems
    [Test]
    public void TC_020_SaveImportantNote_TestSaveOtcDrugItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };
        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);
        var ptOtcDrugModel = new PtOtcDrugModel(hpId, ptId, 0, random.Next(999, 99999), random.Next(999, 99999), "tradeNamePtOtcDrug", 20220202, 20220202, "cmtPtOtcDrug", 0);
        PtOtcDrug? ptOtcDrug = null;
        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { ptOtcDrugModel },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.OtcDrugGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            ptOtcDrug = tenant.PtOtcDrug.FirstOrDefault(item => item.HpId == ptOtcDrugModel.HpId
                                                                && item.PtId == ptOtcDrugModel.PtId
                                                                && item.SortNo == ptOtcDrugModel.SortNo
                                                                && item.SerialNum == ptOtcDrugModel.SerialNum
                                                                && item.TradeName == ptOtcDrugModel.TradeName
                                                                && item.StartDate == ptOtcDrugModel.StartDate
                                                                && item.EndDate == ptOtcDrugModel.EndDate
                                                                && item.Cmt == ptOtcDrugModel.Cmt
                                                                && item.IsDeleted == 0);

            result = result
                     && ptOtcDrug != null
                     && ptOtcDrug.HpId == ptOtcDrugModel.HpId
                     && ptOtcDrug.PtId == ptOtcDrugModel.PtId
                     && ptOtcDrug.SortNo == ptOtcDrugModel.SortNo
                     && ptOtcDrug.SerialNum == ptOtcDrugModel.SerialNum
                     && ptOtcDrug.TradeName == ptOtcDrugModel.TradeName
                     && ptOtcDrug.StartDate == ptOtcDrugModel.StartDate
                     && ptOtcDrug.EndDate == ptOtcDrugModel.EndDate
                     && ptOtcDrug.Cmt == ptOtcDrugModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptOtcDrug != null)
            {
                tenant.PtOtcDrug.Remove(ptOtcDrug);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_021_SaveImportantNote_TestUpdateOtcDrugItemmSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        var ptOtcDrugModel = new PtOtcDrugModel(hpId, ptId, random.Next(999, 99999), random.Next(999, 99999), random.Next(999, 99999), "tradeNamePtOtcDrug", 20220202, 20220202, "cmtPtOtcDrug", 0);
        PtOtcDrug? ptOtcDrug = new PtOtcDrug()
        {
            HpId = ptOtcDrugModel.HpId,
            PtId = ptOtcDrugModel.PtId,
            SeqNo = ptOtcDrugModel.SeqNo
        };
        tenant.PtOtcDrug.Add(ptOtcDrug);

        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { ptOtcDrugModel },
                                                    new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.OtcDrugGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            var ptOtcDrugAfter = tenant.PtOtcDrug.FirstOrDefault(item => item.HpId == ptOtcDrugModel.HpId
                                                                         && item.PtId == ptOtcDrugModel.PtId
                                                                         && item.SeqNo == ptOtcDrugModel.SeqNo
                                                                         && item.SortNo == ptOtcDrugModel.SortNo
                                                                         && item.SerialNum == ptOtcDrugModel.SerialNum
                                                                         && item.TradeName == ptOtcDrugModel.TradeName
                                                                         && item.StartDate == ptOtcDrugModel.StartDate
                                                                         && item.EndDate == ptOtcDrugModel.EndDate
                                                                         && item.Cmt == ptOtcDrugModel.Cmt
                                                                         && item.IsDeleted == ptOtcDrugModel.IsDeleted);

            result = result
                     && ptOtcDrugAfter != null
                     && ptOtcDrugAfter.HpId == ptOtcDrugModel.HpId
                     && ptOtcDrugAfter.PtId == ptOtcDrugModel.PtId
                     && ptOtcDrugAfter.SeqNo == ptOtcDrugModel.SeqNo
                     && ptOtcDrugAfter.SortNo == ptOtcDrugModel.SortNo
                     && ptOtcDrugAfter.SerialNum == ptOtcDrugModel.SerialNum
                     && ptOtcDrugAfter.TradeName == ptOtcDrugModel.TradeName
                     && ptOtcDrugAfter.StartDate == ptOtcDrugModel.StartDate
                     && ptOtcDrugAfter.EndDate == ptOtcDrugModel.EndDate
                     && ptOtcDrugAfter.Cmt == ptOtcDrugModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptOtcDrug != null)
            {
                tenant.PtOtcDrug.Remove(ptOtcDrug);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SaveOtcDrugItems

    #region SaveSuppleItems
    [Test]
    public void TC_022_SaveImportantNote_TestSaveSuppleItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };
        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);
        var ptSuppleModel = new PtSuppleModel(hpId, ptId, 0, random.Next(999, 99999), "indexCdPtSupple", "indexWordPtSupple", 20220202, 20220202, "cmtPtSupple", 0);
        PtSupple? ptSupple = null;
        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { ptSuppleModel });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.SuppleGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            ptSupple = tenant.PtSupples.FirstOrDefault(item => item.HpId == ptSuppleModel.HpId
                                                               && item.PtId == ptSuppleModel.PtId
                                                               && item.SortNo == ptSuppleModel.SortNo
                                                               && item.IndexCd == ptSuppleModel.IndexCd
                                                               && item.IndexWord == ptSuppleModel.IndexWord
                                                               && item.StartDate == ptSuppleModel.StartDate
                                                               && item.EndDate == ptSuppleModel.EndDate
                                                               && item.Cmt == ptSuppleModel.Cmt
                                                               && item.IsDeleted == 0);

            result = result
                     && ptSupple != null
                     && ptSupple.HpId == ptSuppleModel.HpId
                     && ptSupple.PtId == ptSuppleModel.PtId
                     && ptSupple.SortNo == ptSuppleModel.SortNo
                     && ptSupple.IndexCd == ptSuppleModel.IndexCd
                     && ptSupple.IndexWord == ptSuppleModel.IndexWord
                     && ptSupple.StartDate == ptSuppleModel.StartDate
                     && ptSupple.EndDate == ptSuppleModel.EndDate
                     && ptSupple.Cmt == ptSuppleModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptSupple != null)
            {
                tenant.PtSupples.Remove(ptSupple);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_023_SaveImportantNote_TestUpdateSuppleItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        var ptSuppleModel = new PtSuppleModel(hpId, ptId, random.Next(999, 99999), random.Next(999, 99999), "indexCdPtSupple", "indexWordPtSupple", 20220202, 20220202, "cmtPtSupple", 0);
        PtSupple? ptSupple = new PtSupple()
        {
            HpId = ptSuppleModel.HpId,
            PtId = ptSuppleModel.PtId,
            SeqNo = ptSuppleModel.SeqNo
        };
        tenant.PtSupples.Add(ptSupple);

        ImportantNoteModel importantNoteModel = new ImportantNoteModel(
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { },
                                                    new() { ptSuppleModel });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.SuppleGetList + "_" + ptId + "_" + hpId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, importantNoteModel, null, userId);

            // Assert
            var ptSuppleAfter = tenant.PtSupples.FirstOrDefault(item => item.HpId == ptSuppleModel.HpId
                                                                         && item.PtId == ptSuppleModel.PtId
                                                                         && item.SortNo == ptSuppleModel.SortNo
                                                                         && item.SeqNo == ptSuppleModel.SeqNo
                                                                         && item.IndexCd == ptSuppleModel.IndexCd
                                                                         && item.IndexWord == ptSuppleModel.IndexWord
                                                                         && item.StartDate == ptSuppleModel.StartDate
                                                                         && item.EndDate == ptSuppleModel.EndDate
                                                                         && item.Cmt == ptSuppleModel.Cmt
                                                                         && item.IsDeleted == ptSuppleModel.IsDeleted);

            result = result
                     && ptSuppleAfter != null
                     && ptSuppleAfter.HpId == ptSuppleModel.HpId
                     && ptSuppleAfter.PtId == ptSuppleModel.PtId
                     && ptSuppleAfter.SortNo == ptSuppleModel.SortNo
                     && ptSuppleAfter.SeqNo == ptSuppleModel.SeqNo
                     && ptSuppleAfter.IndexCd == ptSuppleModel.IndexCd
                     && ptSuppleAfter.IndexWord == ptSuppleModel.IndexWord
                     && ptSuppleAfter.StartDate == ptSuppleModel.StartDate
                     && ptSuppleAfter.EndDate == ptSuppleModel.EndDate
                     && ptSuppleAfter.Cmt == ptSuppleModel.Cmt;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptSupple != null)
            {
                tenant.PtSupples.Remove(ptSupple);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SaveSuppleItems
    #endregion SaveImportantNote

    #region SavePatientInfo
    #region SavePregnancyItems
    [Test]
    public void TC_024_SaveImportantNote_TestSavePregnancyItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };
        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        PtPregnancyModel ptPregnancyModel = new PtPregnancyModel(0, hpId, ptId, random.Next(999, 99999), 20220202, 20220203, 20220204, 20220205, 20220206, 20220207, 0, DateTime.MinValue, userId, string.Empty, 20220210);
        PtPregnancy? ptPregnancy = null;
        PatientInfoModel patientInfoModel = new PatientInfoModel(
                                                new() { ptPregnancyModel },
                                                new() { },
                                                new() { },
                                                new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.PtPregnancyGetList + "_" + hpId + "_" + ptId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, null, patientInfoModel, userId);

            // Assert
            ptPregnancy = tenant.PtPregnancies.FirstOrDefault(item => item.HpId == ptPregnancyModel.HpId
                                                                      && item.PtId == ptPregnancyModel.PtId
                                                                      && item.SeqNo == ptPregnancyModel.SeqNo
                                                                      && item.StartDate == ptPregnancyModel.StartDate
                                                                      && item.EndDate == ptPregnancyModel.EndDate
                                                                      && item.PeriodDate == ptPregnancyModel.PeriodDate
                                                                      && item.PeriodDueDate == ptPregnancyModel.PeriodDueDate
                                                                      && item.OvulationDate == ptPregnancyModel.OvulationDate
                                                                      && item.OvulationDueDate == ptPregnancyModel.OvulationDueDate
                                                                      && item.IsDeleted == 0);

            result = result
                     && ptPregnancy != null
                     && ptPregnancy.HpId == ptPregnancyModel.HpId
                     && ptPregnancy.PtId == ptPregnancyModel.PtId
                     && ptPregnancy.SeqNo == ptPregnancyModel.SeqNo
                     && ptPregnancy.StartDate == ptPregnancyModel.StartDate
                     && ptPregnancy.EndDate == ptPregnancyModel.EndDate
                     && ptPregnancy.PeriodDate == ptPregnancyModel.PeriodDate
                     && ptPregnancy.PeriodDueDate == ptPregnancyModel.PeriodDueDate
                     && ptPregnancy.OvulationDate == ptPregnancyModel.OvulationDate
                     && ptPregnancy.OvulationDueDate == ptPregnancyModel.OvulationDueDate;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptPregnancy != null)
            {
                tenant.PtPregnancies.Remove(ptPregnancy);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_025_SaveImportantNote_TestUpdatePregnancyItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        PtPregnancyModel ptPregnancyModel = new PtPregnancyModel(random.Next(999, 99999), hpId, ptId, random.Next(999, 99999), 20220202, 20220203, 20220204, 20220205, 20220206, 20220207, 0, DateTime.MinValue, userId, string.Empty, 20220210);
        PtPregnancy? ptPregnancy = new PtPregnancy()
        {
            Id = ptPregnancyModel.Id,
            HpId = ptPregnancyModel.HpId,
            PtId = ptPregnancyModel.PtId,
            SeqNo = ptPregnancyModel.SeqNo
        };
        tenant.PtPregnancies.Add(ptPregnancy);

        PatientInfoModel patientInfoModel = new PatientInfoModel(
                                                new() { ptPregnancyModel },
                                                new() { },
                                                new() { },
                                                new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.PtPregnancyGetList + "_" + hpId + "_" + ptId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, null, patientInfoModel, userId);

            // Assert
            var ptPregnancyAfter = tenant.PtPregnancies.FirstOrDefault(item => item.HpId == ptPregnancyModel.HpId
                                                                               && item.PtId == ptPregnancyModel.PtId
                                                                               && item.Id == ptPregnancyModel.Id
                                                                               && item.SeqNo == ptPregnancyModel.SeqNo
                                                                               && item.StartDate == ptPregnancyModel.StartDate
                                                                               && item.EndDate == ptPregnancyModel.EndDate
                                                                               && item.PeriodDate == ptPregnancyModel.PeriodDate
                                                                               && item.PeriodDueDate == ptPregnancyModel.PeriodDueDate
                                                                               && item.OvulationDate == ptPregnancyModel.OvulationDate
                                                                               && item.OvulationDueDate == ptPregnancyModel.OvulationDueDate
                                                                               && item.IsDeleted == ptPregnancyModel.IsDeleted);

            result = result
                     && ptPregnancyAfter != null
                     && ptPregnancyAfter.HpId == ptPregnancyModel.HpId
                     && ptPregnancyAfter.PtId == ptPregnancyModel.PtId
                     && ptPregnancyAfter.Id == ptPregnancyModel.Id
                     && ptPregnancyAfter.SeqNo == ptPregnancyModel.SeqNo
                     && ptPregnancyAfter.StartDate == ptPregnancyModel.StartDate
                     && ptPregnancyAfter.EndDate == ptPregnancyModel.EndDate
                     && ptPregnancyAfter.PeriodDate == ptPregnancyModel.PeriodDate
                     && ptPregnancyAfter.PeriodDueDate == ptPregnancyModel.PeriodDueDate
                     && ptPregnancyAfter.OvulationDate == ptPregnancyModel.OvulationDate
                     && ptPregnancyAfter.OvulationDueDate == ptPregnancyModel.OvulationDueDate;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptPregnancy != null)
            {
                tenant.PtPregnancies.Remove(ptPregnancy);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SavePregnancyItems

    #region SavePtCmtInfItems
    [Test]
    public void TC_026_SaveImportantNote_TestSavePtCmtInfItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };
        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        PtCmtInfModel ptCmtInfModel = new PtCmtInfModel(hpId, ptId, random.Next(999, 99999), "textPtCmtInf", 0, 0);
        PtCmtInf? ptCmtInf = null;
        PatientInfoModel patientInfoModel = new PatientInfoModel(
                                                new() { },
                                                ptCmtInfModel,
                                                new() { },
                                                new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.PtCmtInfGetList + "_" + hpId + "_" + ptId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, null, patientInfoModel, userId);

            // Assert
            ptCmtInf = tenant.PtCmtInfs.FirstOrDefault(item => item.HpId == ptCmtInfModel.HpId
                                                               && item.PtId == ptCmtInfModel.PtId
                                                               && item.SeqNo == ptCmtInfModel.SeqNo
                                                               && item.Text == ptCmtInfModel.Text
                                                               && item.IsDeleted == 0);

            result = result
                     && ptCmtInf != null
                     && ptCmtInf.HpId == ptCmtInfModel.HpId
                     && ptCmtInf.PtId == ptCmtInfModel.PtId
                     && ptCmtInf.SeqNo == ptCmtInfModel.SeqNo
                     && ptCmtInf.Text == ptCmtInfModel.Text;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptCmtInf != null)
            {
                tenant.PtCmtInfs.Remove(ptCmtInf);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_027_SaveImportantNote_TestUpdatePtCmtInfItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        PtCmtInfModel ptCmtInfModel = new PtCmtInfModel(hpId, ptId, random.Next(999, 99999), "textPtCmtInf", 0, random.Next(999, 99999));
        PtCmtInf? ptCmtInf = new()
        {
            Id = ptCmtInfModel.Id,
            HpId = ptCmtInfModel.HpId,
            PtId = ptCmtInfModel.PtId,
            SeqNo = ptCmtInfModel.SeqNo
        };
        tenant.PtCmtInfs.Add(ptCmtInf);

        PatientInfoModel patientInfoModel = new PatientInfoModel(
                                                new() { },
                                                ptCmtInfModel,
                                                new() { },
                                                new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.PtCmtInfGetList + "_" + hpId + "_" + ptId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, null, patientInfoModel, userId);

            // Assert
            var ptCmtInfAfter = tenant.PtCmtInfs.FirstOrDefault(item => item.HpId == ptCmtInfModel.HpId
                                                               && item.PtId == ptCmtInfModel.PtId
                                                               && item.Id == ptCmtInfModel.Id
                                                               && item.SeqNo == ptCmtInfModel.SeqNo
                                                               && item.Text == ptCmtInfModel.Text
                                                               && item.IsDeleted == ptCmtInfModel.IsDeleted);

            result = result
                     && ptCmtInfAfter != null
                     && ptCmtInfAfter.HpId == ptCmtInfModel.HpId
                     && ptCmtInfAfter.PtId == ptCmtInfModel.PtId
                     && ptCmtInfAfter.Id == ptCmtInfModel.Id
                     && ptCmtInfAfter.SeqNo == ptCmtInfModel.SeqNo
                     && ptCmtInfAfter.Text == ptCmtInfModel.Text;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (ptCmtInf != null)
            {
                tenant.PtCmtInfs.Remove(ptCmtInf);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SavePtCmtInfItems

    #region SaveSeikatureInfItems
    [Test]
    public void TC_028_SaveImportantNote_TestSaveSeikatureInfItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };
        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        SeikaturekiInfModel seikaturekiInfModel = new SeikaturekiInfModel(0, hpId, ptId, random.Next(999, 99999), "textSeikaturekiInf");
        SeikaturekiInf? seikaturekiInf = null;
        PatientInfoModel patientInfoModel = new PatientInfoModel(
                                                new() { },
                                                new() { },
                                                seikaturekiInfModel,
                                                new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.SeikaturekiInfGetList + "_" + hpId + "_" + ptId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, null, patientInfoModel, userId);

            // Assert
            seikaturekiInf = tenant.SeikaturekiInfs.FirstOrDefault(item => item.HpId == seikaturekiInfModel.HpId
                                                                           && item.PtId == seikaturekiInfModel.PtId
                                                                           && item.SeqNo == seikaturekiInfModel.SeqNo
                                                                           && item.Text == seikaturekiInfModel.Text);

            result = result
                     && seikaturekiInf != null
                     && seikaturekiInf.HpId == seikaturekiInfModel.HpId
                     && seikaturekiInf.PtId == seikaturekiInfModel.PtId
                     && seikaturekiInf.SeqNo == seikaturekiInfModel.SeqNo
                     && seikaturekiInf.Text == seikaturekiInfModel.Text;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (seikaturekiInf != null)
            {
                tenant.SeikaturekiInfs.Remove(seikaturekiInf);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_029_SaveImportantNote_TestUpdatePtSeikatureInfItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        SeikaturekiInfModel seikaturekiInfModel = new SeikaturekiInfModel(random.Next(999, 99999), hpId, ptId, random.Next(999, 99999), "textSeikaturekiInf");
        SeikaturekiInf? seikaturekiInf = new()
        {
            Id = seikaturekiInfModel.Id,
            HpId = seikaturekiInfModel.HpId,
            PtId = seikaturekiInfModel.PtId,
            SeqNo = seikaturekiInfModel.SeqNo
        };
        tenant.SeikaturekiInfs.Add(seikaturekiInf);

        PatientInfoModel patientInfoModel = new PatientInfoModel(
                                                new() { },
                                                new() { },
                                                seikaturekiInfModel,
                                                new() { });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        var finalKey = "" + CacheKeyConstant.SeikaturekiInfGetList + "_" + hpId + "_" + ptId;
        _cache.StringAppend(finalKey, string.Empty);
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, null, patientInfoModel, userId);

            // Assert
            var ptSeikaturekiAfter = tenant.SeikaturekiInfs.FirstOrDefault(item => item.HpId == seikaturekiInfModel.HpId
                                                                                   && item.PtId == seikaturekiInfModel.PtId
                                                                                   && item.SeqNo == seikaturekiInfModel.SeqNo
                                                                                   && item.Id == seikaturekiInfModel.Id
                                                                                   && item.Text == seikaturekiInfModel.Text);

            result = result
                     && ptSeikaturekiAfter != null
                     && ptSeikaturekiAfter.HpId == seikaturekiInfModel.HpId
                     && ptSeikaturekiAfter.PtId == seikaturekiInfModel.PtId
                     && ptSeikaturekiAfter.Id == seikaturekiInfModel.Id
                     && ptSeikaturekiAfter.SeqNo == seikaturekiInfModel.SeqNo
                     && ptSeikaturekiAfter.Text == seikaturekiInfModel.Text;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();
            _cache.KeyDelete(finalKey);

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (seikaturekiInf != null)
            {
                tenant.SeikaturekiInfs.Remove(seikaturekiInf);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SaveSeikatureInfItems

    #region SavePhysicalInfItems
    [Test]
    public void TC_030_SaveImportantNote_TestSavePhysicalInfItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };
        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        KensaInfDetailModel kensaInfDetailModel = new KensaInfDetailModel(hpId, ptId, random.Next(999, 999999), 0, 20220202, random.Next(999, 999999), "KensaCdUT", "ValUT", "1", "2", 0, "Cd1", "Cd2", DateTime.MinValue, string.Empty, string.Empty, 0, string.Empty);
        PhysicalInfoModel physicalInfoModel = new PhysicalInfoModel(new() { kensaInfDetailModel });
        KensaInf? kensaInf = null;
        KensaInfDetail? kensaInfDetail = null;
        PatientInfoModel patientInfoModel = new PatientInfoModel(
                                                new() { },
                                                new() { },
                                                new() { },
                                                new() { physicalInfoModel });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, null, patientInfoModel, userId);

            // Assert
            kensaInf = tenant.KensaInfs.FirstOrDefault(item => item.HpId == hpId
                                                               && item.PtId == ptId
                                                               && item.RaiinNo == kensaInfDetailModel.RaiinNo
                                                               && item.IraiDate == kensaInfDetailModel.IraiDate
                                                               && item.Status == 2
                                                               && item.InoutKbn == 0
                                                               && item.IsDeleted == 0);

            kensaInfDetail = tenant.KensaInfDetails.FirstOrDefault(item => item.HpId == kensaInfDetailModel.HpId
                                                                           && item.PtId == kensaInfDetailModel.PtId
                                                                           && item.IraiDate == kensaInfDetailModel.IraiDate
                                                                           && item.RaiinNo == kensaInfDetailModel.RaiinNo
                                                                           && item.IraiCd == kensaInfDetailModel.IraiCd
                                                                           && item.KensaItemCd == kensaInfDetailModel.KensaItemCd
                                                                           && item.ResultVal == kensaInfDetailModel.ResultVal
                                                                           && item.ResultType == kensaInfDetailModel.ResultType
                                                                           && item.AbnormalKbn == kensaInfDetailModel.AbnormalKbn
                                                                           && item.CmtCd1 == kensaInfDetailModel.CmtCd1
                                                                           && item.CmtCd2 == kensaInfDetailModel.CmtCd2
                                                                           && item.IsDeleted == 0);

            result = result
                     && kensaInf != null
                     && kensaInfDetail != null
                     && kensaInf.HpId == kensaInfDetailModel.HpId
                     && kensaInf.PtId == kensaInfDetailModel.PtId
                     && kensaInf.RaiinNo == kensaInfDetailModel.RaiinNo
                     && kensaInf.IraiDate == kensaInfDetailModel.IraiDate
                     && kensaInf.Status == 2
                     && kensaInf.InoutKbn == 0
                     && kensaInfDetail.HpId == kensaInfDetailModel.HpId
                     && kensaInfDetail.PtId == kensaInfDetailModel.PtId
                     && kensaInfDetail.IraiDate == kensaInfDetailModel.IraiDate
                     && kensaInfDetail.RaiinNo == kensaInfDetailModel.RaiinNo
                     && kensaInfDetail.IraiCd == kensaInfDetailModel.IraiCd
                     && kensaInfDetail.KensaItemCd == kensaInfDetailModel.KensaItemCd
                     && kensaInfDetail.ResultVal == kensaInfDetailModel.ResultVal
                     && kensaInfDetail.ResultType == kensaInfDetailModel.ResultType
                     && kensaInfDetail.AbnormalKbn == kensaInfDetailModel.AbnormalKbn
                     && kensaInfDetail.CmtCd1 == kensaInfDetailModel.CmtCd1
                     && kensaInfDetail.CmtCd2 == kensaInfDetailModel.CmtCd2;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (kensaInf != null)
            {
                tenant.KensaInfs.Remove(kensaInf);
            }
            if (kensaInfDetail != null)
            {
                tenant.KensaInfDetails.Remove(kensaInfDetail);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_031_SaveImportantNote_TestUpdatePtPhysicalInfItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        KensaInfDetailModel kensaInfDetailModel = new KensaInfDetailModel(hpId, ptId, random.Next(999, 999999), random.Next(999, 9999999), 20220202, random.Next(999, 999999), "KensaCdUT", "ValUT", "1", "2", 0, "Cd1", "Cd2", DateTime.MinValue, string.Empty, string.Empty, 0, string.Empty);
        PhysicalInfoModel physicalInfoModel = new PhysicalInfoModel(new() { kensaInfDetailModel });
        KensaInf? kensaInf = null;
        KensaInfDetail? kensaInfDetail = new()
        {
            HpId = kensaInfDetailModel.HpId,
            PtId = kensaInfDetailModel.PtId,
            SeqNo = kensaInfDetailModel.SeqNo,
            IraiCd = kensaInfDetailModel.IraiCd
        };
        tenant.KensaInfDetails.Add(kensaInfDetail);

        PatientInfoModel patientInfoModel = new PatientInfoModel(
                                                new() { },
                                                new() { },
                                                new() { },
                                                new() { physicalInfoModel });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, null, patientInfoModel, userId);

            // Assert
            kensaInf = tenant.KensaInfs.FirstOrDefault(item => item.HpId == hpId
                                                               && item.PtId == ptId
                                                               && item.RaiinNo == kensaInfDetailModel.RaiinNo
                                                               && item.IraiDate == kensaInfDetailModel.IraiDate
                                                               && item.Status == 2
                                                               && item.InoutKbn == 0
                                                               && item.IsDeleted == 0);

            var kensaInfDetailAfter = tenant.KensaInfDetails.FirstOrDefault(item => item.HpId == kensaInfDetailModel.HpId
                                                                            && item.PtId == kensaInfDetailModel.PtId
                                                                            && item.IraiDate == kensaInfDetailModel.IraiDate
                                                                            && item.RaiinNo == kensaInfDetailModel.RaiinNo
                                                                            && item.IraiCd == kensaInfDetailModel.IraiCd
                                                                            && item.SeqNo == kensaInfDetailModel.SeqNo
                                                                            && item.KensaItemCd == kensaInfDetailModel.KensaItemCd
                                                                            && item.ResultVal == kensaInfDetailModel.ResultVal
                                                                            && item.ResultType == kensaInfDetailModel.ResultType
                                                                            && item.AbnormalKbn == kensaInfDetailModel.AbnormalKbn
                                                                            && item.CmtCd1 == kensaInfDetailModel.CmtCd1
                                                                            && item.CmtCd2 == kensaInfDetailModel.CmtCd2
                                                                            && item.IsDeleted == kensaInfDetailModel.IsDeleted);

            result = result
                     && kensaInf != null
                     && kensaInfDetailAfter != null
                     && kensaInf.HpId == kensaInfDetailModel.HpId
                     && kensaInf.PtId == kensaInfDetailModel.PtId
                     && kensaInf.RaiinNo == kensaInfDetailModel.RaiinNo
                     && kensaInf.IraiDate == kensaInfDetailModel.IraiDate
                     && kensaInf.Status == 2
                     && kensaInf.InoutKbn == 0
                     && kensaInfDetailAfter.HpId == kensaInfDetailModel.HpId
                     && kensaInfDetailAfter.PtId == kensaInfDetailModel.PtId
                     && kensaInfDetailAfter.IraiDate == kensaInfDetailModel.IraiDate
                     && kensaInfDetailAfter.RaiinNo == kensaInfDetailModel.RaiinNo
                     && kensaInfDetailAfter.IraiCd == kensaInfDetailModel.IraiCd
                     && kensaInfDetailAfter.SeqNo == kensaInfDetailModel.SeqNo
                     && kensaInfDetailAfter.KensaItemCd == kensaInfDetailModel.KensaItemCd
                     && kensaInfDetailAfter.ResultVal == kensaInfDetailModel.ResultVal
                     && kensaInfDetailAfter.ResultType == kensaInfDetailModel.ResultType
                     && kensaInfDetailAfter.AbnormalKbn == kensaInfDetailModel.AbnormalKbn
                     && kensaInfDetailAfter.CmtCd1 == kensaInfDetailModel.CmtCd1
                     && kensaInfDetailAfter.CmtCd2 == kensaInfDetailModel.CmtCd2;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (kensaInf != null)
            {
                tenant.KensaInfs.Remove(kensaInf);
            }
            if (kensaInfDetail != null)
            {
                tenant.KensaInfDetails.Remove(kensaInfDetail);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_032_SaveImportantNote_TestDeletedPtPhysicalInfItemSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.NextInt64(99999, 999999999);
        int sinDate = 20220202;

        var hpInf = new HpInf()
        {
            HpId = hpId,
        };
        var ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = ptId
        };

        tenant.HpInfs.Add(hpInf);
        tenant.PtInfs.Add(ptInf);

        KensaInfDetailModel kensaInfDetailModel = new KensaInfDetailModel(hpId, ptId, random.Next(999, 999999), random.Next(999, 9999999), 20220202, random.Next(999, 999999), "KensaCdUT", "ValUT", "1", "2", 1, "Cd1", "Cd2", DateTime.MinValue, string.Empty, string.Empty, 0, string.Empty);
        PhysicalInfoModel physicalInfoModel = new PhysicalInfoModel(new() { kensaInfDetailModel });
        KensaInf? kensaInf = null;
        KensaInfDetail? kensaInfDetailAfter = null;
        KensaInfDetail? kensaInfDetail = new()
        {
            HpId = kensaInfDetailModel.HpId,
            PtId = kensaInfDetailModel.PtId,
            SeqNo = kensaInfDetailModel.SeqNo,
            IraiCd = kensaInfDetailModel.IraiCd
        };
        tenant.KensaInfDetails.Add(kensaInfDetail);

        PatientInfoModel patientInfoModel = new PatientInfoModel(
                                                new() { },
                                                new() { },
                                                new() { },
                                                new() { physicalInfoModel });

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SpecialNoteRepository specialNoteRepository = new SpecialNoteRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = specialNoteRepository.SaveSpecialNote(hpId, ptId, sinDate, null, null, patientInfoModel, userId);

            // Assert
            kensaInf = tenant.KensaInfs.FirstOrDefault(item => item.HpId == hpId
                                                               && item.PtId == ptId
                                                               && item.RaiinNo == kensaInfDetailModel.RaiinNo
                                                               && item.IraiDate == kensaInfDetailModel.IraiDate
                                                               && item.Status == 2
                                                               && item.InoutKbn == 0
                                                               && item.IsDeleted == 0);

            kensaInfDetailAfter = tenant.KensaInfDetails.FirstOrDefault(item => item.HpId == kensaInfDetailModel.HpId
                                                                                    && item.PtId == kensaInfDetailModel.PtId
                                                                                    && item.IraiCd == kensaInfDetailModel.IraiCd
                                                                                    && item.SeqNo == kensaInfDetailModel.SeqNo);

            result = result
                     && kensaInf != null
                     && kensaInfDetailAfter == null
                     && kensaInf.HpId == kensaInfDetailModel.HpId
                     && kensaInf.PtId == kensaInfDetailModel.PtId
                     && kensaInf.RaiinNo == kensaInfDetailModel.RaiinNo
                     && kensaInf.IraiDate == kensaInfDetailModel.IraiDate
                     && kensaInf.Status == 2
                     && kensaInf.InoutKbn == 0;

            Assert.True(result);
        }
        finally
        {
            specialNoteRepository.ReleaseResource();

            #region Remove Data Fetch
            tenant.HpInfs.Remove(hpInf);
            tenant.PtInfs.Remove(ptInf);
            if (kensaInf != null)
            {
                tenant.KensaInfs.Remove(kensaInf);
            }
            if (kensaInfDetailAfter != null)
            {
                tenant.KensaInfDetails.Remove(kensaInfDetailAfter);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    #endregion SavePhysicalInfItems
    #endregion SavePatientInfo
}
