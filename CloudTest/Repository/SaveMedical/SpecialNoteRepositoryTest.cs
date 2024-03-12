using Domain.Models.SpecialNote.SummaryInf;
using Entity.Tenant;
using Infrastructure.Repositories.SpecialNote;
using Microsoft.Extensions.Configuration;
using Moq;
using Domain.Models.SpecialNote.ImportantNote;
using DocumentFormat.OpenXml.Wordprocessing;
using ZstdSharp.Unsafe;

namespace CloudUnitTest.Repository.SaveMedical;

public class SpecialNoteRepositoryTest : BaseUT
{
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

    #region SaveSummaryInf
    [Test]
    public void TC_004_SaveSummaryInf_TestCreateSuccess()
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
    public void TC_005_SaveSummaryInf_TestUpdateSuccess()
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
                                              random.Next(999, 99999),
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
    public void TC_006_SaveImportantNote_TestSaveAlrgyFoodItemSuccess()
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
    public void TC_007_SaveImportantNote_TestUpdateAlrgyFoodItemSuccess()
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
    public void TC_008_SaveImportantNote_TestSaveElseItemSuccess()
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
    public void TC_009_SaveImportantNote_TestUpdateElseItemSuccess()
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
    public void TC_010_SaveImportantNote_TestSaveDrugItemSuccess()
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
    public void TC_011_SaveImportantNote_TestUpdateDrugItemsSuccess()
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
    public void TC_012_SaveImportantNote_TestSaveKioRekiItemSuccess()
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
    public void TC_013_SaveImportantNote_TestUpdateKioRekiItemSuccess()
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
    public void TC_014_SaveImportantNote_TestSaveInfectionsItemSuccess()
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
    public void TC_015_SaveImportantNote_TestUpdateInfectionsItemSuccess()
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
    public void TC_016_SaveImportantNote_TestSaveOtherDrugItemSuccess()
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
    public void TC_017_SaveImportantNote_TestUpdateOtherDrugItemmSuccess()
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
    public void TC_018_SaveImportantNote_TestSaveOtcDrugItemSuccess()
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
    public void TC_019_SaveImportantNote_TestUpdateOtcDrugItemmSuccess()
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

    #endregion SaveImportantNote

}
