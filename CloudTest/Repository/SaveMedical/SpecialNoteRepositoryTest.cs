using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Models.SpecialNote.SummaryInf;
using Entity.Tenant;
using Infrastructure.Repositories.SpecialNote;
using Microsoft.Extensions.Configuration;
using Moq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text;
using ZstdSharp.Unsafe;
using Domain.Models.SpecialNote.ImportantNote;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

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
                                                                     && item.Cmt == ptAlrgyFoodModel.Cmt);

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
                                                                              && item.Cmt == ptAlrgyFoodModel.Cmt);

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
    #endregion SaveImportantNote

}
