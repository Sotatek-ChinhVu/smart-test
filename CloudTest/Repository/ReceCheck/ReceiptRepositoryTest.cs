using DocumentFormat.OpenXml.Wordprocessing;
using Domain.CalculationInf;
using Domain.Models.MstItem;
using Domain.Models.Receipt;
using Entity.Tenant;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using PostgreDataContext;
using System;

namespace CloudUnitTest.Repository.ReceCheck;

public class ReceiptRepositoryTest : BaseUT
{
    #region GetReceCmtList
    [Test]
    public void TC_001_GetReceCmtList_TestSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int cmtKbn = random.Next(9999, 999999999);
        int cmtSbt = random.Next(9999, 999999999);
        int cmtCol1 = random.Next(9999, 999999999);
        int cmtCol2 = random.Next(9999, 999999999);
        int cmtCol3 = random.Next(9999, 999999999);
        int cmtCol4 = random.Next(9999, 999999999);
        int cmtColKeta1 = random.Next(9999, 999999999);
        int cmtColKeta2 = random.Next(9999, 999999999);
        int cmtColKeta3 = random.Next(9999, 999999999);
        int cmtColKeta4 = random.Next(9999, 999999999);
        int sinYm = 202202;
        int startDate = 20220201;
        int endDate = 20220228;
        string cmt = "Cmt";
        string cmtData = "CmtData";
        string itemCd = "ItemCdUT";
        int sinDate = 20220210;
        ReceCmt receCmt = new()
        {
            HpId = hpId,
            SinYm = sinYm,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            CmtKbn = cmtKbn,
            CmtSbt = cmtSbt,
            Cmt = cmt,
            CmtData = cmtData,
            ItemCd = itemCd,
            IsDeleted = 0
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            StartDate = startDate,
            EndDate = endDate,
            ItemCd = itemCd,
            CmtCol1 = cmtCol1,
            CmtCol2 = cmtCol2,
            CmtCol3 = cmtCol3,
            CmtCol4 = cmtCol4,
            CmtColKeta1 = cmtColKeta1,
            CmtColKeta2 = cmtColKeta2,
            CmtColKeta3 = cmtColKeta3,
            CmtColKeta4 = cmtColKeta4,
        };

        tenant.ReceCmts.Add(receCmt);
        tenant.TenMsts.Add(tenMst);

        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceCmtList(hpId, sinYm, ptId, hokenId, sinDate, false);

            var success = result.Any(item => item.PtId == receCmt.PtId
                                             && item.SeqNo == receCmt.SeqNo
                                             && item.SinYm == receCmt.SinYm
                                             && item.HokenId == receCmt.HokenId
                                             && item.CmtKbn == receCmt.CmtKbn
                                             && item.CmtSbt == receCmt.CmtSbt
                                             && item.Cmt == receCmt.Cmt
                                             && item.CmtData == receCmt.CmtData
                                             && item.ItemCd == receCmt.ItemCd
                                             && item.CmtCol1 == tenMst.CmtCol1
                                             && item.CmtCol2 == tenMst.CmtCol2
                                             && item.CmtCol3 == tenMst.CmtCol3
                                             && item.CmtCol4 == tenMst.CmtCol4
                                             && item.CmtColKeta1 == tenMst.CmtColKeta1
                                             && item.CmtColKeta2 == tenMst.CmtColKeta2
                                             && item.CmtColKeta3 == tenMst.CmtColKeta3
                                             && item.CmtColKeta4 == tenMst.CmtColKeta4);
            // Assert
            Assert.IsTrue(success);
        }
        finally
        {
            tenant.ReceCmts.Remove(receCmt);
            tenant.TenMsts.Remove(tenMst);
            tenant.SaveChanges();
        }
    }
    #endregion GetReceCmtList

    #region SaveReceCmtList
    [Test]
    public void TC_002_SaveReceCmtList_TestSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long id = 0;
        long ptId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int cmtKbn = random.Next(9999, 999999999);
        int cmtSbt = random.Next(9999, 999999999);
        int cmtCol1 = random.Next(9999, 999999999);
        int cmtCol2 = random.Next(9999, 999999999);
        int cmtCol3 = random.Next(9999, 999999999);
        int cmtCol4 = random.Next(9999, 999999999);
        int cmtColKeta1 = random.Next(9999, 999999999);
        int cmtColKeta2 = random.Next(9999, 999999999);
        int cmtColKeta3 = random.Next(9999, 999999999);
        int cmtColKeta4 = random.Next(9999, 999999999);
        int sinYm = 202202;
        string cmt = "Cmt";
        string cmtData = "CmtData";
        string itemCd = "ItemCdUT";
        ReceCmtModel receCmtModel = new ReceCmtModel(id, ptId, seqNo, sinYm, hokenId, cmtKbn, cmtSbt, cmt, cmtData, itemCd, cmtCol1, cmtCol2, cmtCol3, cmtCol4, cmtColKeta1, cmtColKeta2, cmtColKeta3, cmtColKeta4, false);
        ReceCmt? receCmt = null;

        try
        {
            // Act
            var result = receiptRepository.SaveReceCmtList(hpId, userId, new List<ReceCmtModel>() { receCmtModel });

            receCmt = tenant.ReceCmts.FirstOrDefault(item => item.PtId == ptId
                                                             && item.HpId == hpId
                                                             && item.SinYm == sinYm
                                                             && item.HokenId == hokenId
                                                             && item.CmtKbn == cmtKbn
                                                             && item.CmtSbt == cmtSbt
                                                             && item.Cmt == cmt
                                                             && item.CmtData == cmtData
                                                             && item.ItemCd == itemCd);
            // Assert
            Assert.IsTrue(result && receCmt != null);
        }
        finally
        {
            if (receCmt != null)
            {
                tenant.ReceCmts.Remove(receCmt);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_003_SaveReceCmtList_TestReceCmtListEmpty()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long id = random.Next(9999, 999999999);
        long ptId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int cmtKbn = random.Next(9999, 999999999);
        int cmtSbt = random.Next(9999, 999999999);
        int sinYm = 202202;
        string cmt = "Cmt";
        string cmtData = "CmtData";
        string itemCd = "ItemCdUT";
        ReceCmt? receCmt = null;

        try
        {
            // Act
            var result = receiptRepository.SaveReceCmtList(hpId, userId, new());

            receCmt = tenant.ReceCmts.FirstOrDefault(item => item.PtId == ptId
                                                             && item.HpId == hpId
                                                             && item.SinYm == sinYm
                                                             && item.HokenId == hokenId
                                                             && item.CmtKbn == cmtKbn
                                                             && item.CmtSbt == cmtSbt
                                                             && item.Cmt == cmt
                                                             && item.CmtData == cmtData
                                                             && item.ItemCd == itemCd);
            // Assert
            Assert.IsTrue(result && receCmt == null);
        }
        finally
        {
            if (receCmt != null)
            {
                tenant.ReceCmts.Remove(receCmt);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_004_SaveReceCmtListAction_TestAddNew()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long id = 0;
        long ptId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int cmtKbn = random.Next(9999, 999999999);
        int cmtSbt = random.Next(9999, 999999999);
        int cmtCol1 = random.Next(9999, 999999999);
        int cmtCol2 = random.Next(9999, 999999999);
        int cmtCol3 = random.Next(9999, 999999999);
        int cmtCol4 = random.Next(9999, 999999999);
        int cmtColKeta1 = random.Next(9999, 999999999);
        int cmtColKeta2 = random.Next(9999, 999999999);
        int cmtColKeta3 = random.Next(9999, 999999999);
        int cmtColKeta4 = random.Next(9999, 999999999);
        int sinYm = 202202;
        string cmt = "Cmt";
        string cmtData = "CmtData";
        string itemCd = "ItemCdUT";
        ReceCmtModel receCmtModel = new ReceCmtModel(id, ptId, seqNo, sinYm, hokenId, cmtKbn, cmtSbt, cmt, cmtData, itemCd, cmtCol1, cmtCol2, cmtCol3, cmtCol4, cmtColKeta1, cmtColKeta2, cmtColKeta3, cmtColKeta4, false);
        ReceCmt? receCmt = null;

        try
        {
            // Act
            var result = receiptRepository.SaveReceCmtList(hpId, userId, new List<ReceCmtModel>() { receCmtModel });

            receCmt = tenant.ReceCmts.FirstOrDefault(item => item.PtId == ptId
                                                             && item.HpId == hpId
                                                             && item.SinYm == sinYm
                                                             && item.HokenId == hokenId
                                                             && item.CmtKbn == cmtKbn
                                                             && item.CmtSbt == cmtSbt
                                                             && item.Cmt == cmt
                                                             && item.CmtData == cmtData
                                                             && item.ItemCd == itemCd);
            // Assert
            Assert.IsTrue(result && receCmt != null);
        }
        finally
        {
            if (receCmt != null)
            {
                tenant.ReceCmts.Remove(receCmt);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_005_SaveReceCmtListAction_TestContinue()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long id = random.Next(9999, 999999999);
        long ptId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int cmtKbn = random.Next(9999, 999999999);
        int cmtSbt = random.Next(9999, 999999999);
        int cmtCol1 = random.Next(9999, 999999999);
        int cmtCol2 = random.Next(9999, 999999999);
        int cmtCol3 = random.Next(9999, 999999999);
        int cmtCol4 = random.Next(9999, 999999999);
        int cmtColKeta1 = random.Next(9999, 999999999);
        int cmtColKeta2 = random.Next(9999, 999999999);
        int cmtColKeta3 = random.Next(9999, 999999999);
        int cmtColKeta4 = random.Next(9999, 999999999);
        int sinYm = 202202;
        string cmt = "Cmt";
        string cmtData = "CmtData";
        string itemCd = "ItemCdUT";
        ReceCmtModel receCmtModel = new ReceCmtModel(id, ptId, seqNo, sinYm, hokenId, cmtKbn, cmtSbt, cmt, cmtData, itemCd, cmtCol1, cmtCol2, cmtCol3, cmtCol4, cmtColKeta1, cmtColKeta2, cmtColKeta3, cmtColKeta4, false);
        ReceCmt? receCmt = null;

        try
        {
            // Act
            var result = receiptRepository.SaveReceCmtList(hpId, userId, new List<ReceCmtModel>() { receCmtModel });

            receCmt = tenant.ReceCmts.FirstOrDefault(item => item.PtId == ptId
                                                             && item.HpId == hpId
                                                             && item.SinYm == sinYm
                                                             && item.HokenId == hokenId
                                                             && item.CmtKbn == cmtKbn
                                                             && item.CmtSbt == cmtSbt
                                                             && item.Cmt == cmt
                                                             && item.CmtData == cmtData
                                                             && item.ItemCd == itemCd);
            // Assert
            Assert.IsTrue(result && receCmt == null);
        }
        finally
        {
            if (receCmt != null)
            {
                tenant.ReceCmts.Remove(receCmt);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_006_SaveReceCmtListAction_TestUpdate()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long id = random.Next(9999, 999999999);
        long ptId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int cmtKbn = random.Next(9999, 999999999);
        int cmtSbt = random.Next(9999, 999999999);
        int cmtCol1 = random.Next(9999, 999999999);
        int cmtCol2 = random.Next(9999, 999999999);
        int cmtCol3 = random.Next(9999, 999999999);
        int cmtCol4 = random.Next(9999, 999999999);
        int cmtColKeta1 = random.Next(9999, 999999999);
        int cmtColKeta2 = random.Next(9999, 999999999);
        int cmtColKeta3 = random.Next(9999, 999999999);
        int cmtColKeta4 = random.Next(9999, 999999999);
        int sinYm = 202202;
        string cmt = "Cmt";
        string cmtData = "CmtData";
        string itemCd = "ItemCdUT";
        ReceCmtModel receCmtModel = new ReceCmtModel(id, ptId, seqNo, sinYm, hokenId, cmtKbn, cmtSbt, cmt, cmtData, itemCd, cmtCol1, cmtCol2, cmtCol3, cmtCol4, cmtColKeta1, cmtColKeta2, cmtColKeta3, cmtColKeta4, false);
        ReceCmt? receCmt = new()
        {
            Id = id,
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            IsDeleted = 0,
            SeqNo = seqNo,
        };

        tenant.ReceCmts.Add(receCmt);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveReceCmtList(hpId, userId, new List<ReceCmtModel>() { receCmtModel });

            var receCmtAfter = tenant.ReceCmts.FirstOrDefault(item => item.PtId == ptId
                                                               && item.HpId == hpId
                                                               && item.SinYm == sinYm
                                                               && item.HokenId == hokenId
                                                               && item.Cmt == cmt
                                                               && item.CmtData == cmtData
                                                               && item.ItemCd == itemCd);
            // Assert
            Assert.IsTrue(result && receCmtAfter != null);
        }
        finally
        {
            if (receCmt != null)
            {
                tenant.ReceCmts.Remove(receCmt);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_007_SaveReceCmtListAction_TestDeleted()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long id = random.Next(9999, 999999999);
        long ptId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int cmtKbn = random.Next(9999, 999999999);
        int cmtSbt = random.Next(9999, 999999999);
        int cmtCol1 = random.Next(9999, 999999999);
        int cmtCol2 = random.Next(9999, 999999999);
        int cmtCol3 = random.Next(9999, 999999999);
        int cmtCol4 = random.Next(9999, 999999999);
        int cmtColKeta1 = random.Next(9999, 999999999);
        int cmtColKeta2 = random.Next(9999, 999999999);
        int cmtColKeta3 = random.Next(9999, 999999999);
        int cmtColKeta4 = random.Next(9999, 999999999);
        int sinYm = 202202;
        string cmt = "Cmt";
        string cmtData = "CmtData";
        string itemCd = "ItemCdUT";
        ReceCmtModel receCmtModel = new ReceCmtModel(id, ptId, seqNo, sinYm, hokenId, cmtKbn, cmtSbt, cmt, cmtData, itemCd, cmtCol1, cmtCol2, cmtCol3, cmtCol4, cmtColKeta1, cmtColKeta2, cmtColKeta3, cmtColKeta4, true);
        ReceCmt? receCmt = new()
        {
            Id = id,
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            IsDeleted = 0,
            SeqNo = seqNo,
        };

        tenant.ReceCmts.Add(receCmt);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveReceCmtList(hpId, userId, new List<ReceCmtModel>() { receCmtModel });

            var receCmtAfter = tenant.ReceCmts.FirstOrDefault(item => item.PtId == ptId
                                                               && item.HpId == hpId
                                                               && item.SinYm == sinYm
                                                               && item.HokenId == hokenId
                                                               && item.IsDeleted == 1);
            // Assert
            Assert.IsTrue(result && receCmtAfter != null);
        }
        finally
        {
            if (receCmt != null)
            {
                tenant.ReceCmts.Remove(receCmt);
                tenant.SaveChanges();
            }
        }
    }
    #endregion SaveReceCmtList

    private void SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant)
    {
        var mockICalculationInfRepository = new Mock<ICalculationInfRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockIConfiguration = new Mock<IConfiguration>();
        mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

        receiptRepository = new ReceiptRepository(TenantProvider, mockIMstItemRepository.Object, mockICalculationInfRepository.Object, mockIConfiguration.Object);
        tenant = TenantProvider.GetNoTrackingDataContext();
    }
}
