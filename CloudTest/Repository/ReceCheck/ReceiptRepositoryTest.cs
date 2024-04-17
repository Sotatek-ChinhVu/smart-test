using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.CalculationInf;
using Domain.Models.MstItem;
using Domain.Models.Receipt;
using Entity.Tenant;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using PostgreDataContext;
using System;
using System.Linq.Dynamic.Core.Tokenizer;
using ZstdSharp.Unsafe;

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

    #region GetSyoukiInfList
    [Test]
    public void TC_008_GetSyoukiInfList_TestSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int syoukiKbn = random.Next(9999, 999999999);
        int sinYm = 202202;
        string syouki = "Syouki";
        SyoukiInf? syoukiInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            IsDeleted = 0,
            SeqNo = seqNo,
            SyoukiKbn = syoukiKbn,
            Syouki = syouki,
        };

        tenant.SyoukiInfs.Add(syoukiInf);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetSyoukiInfList(hpId, sinYm, ptId, hokenId, false);

            var syoukiInfAfter = result.FirstOrDefault(item => item.PtId == ptId
                                                               && item.SinYm == sinYm
                                                               && item.HokenId == hokenId
                                                               && item.SyoukiKbn == syoukiKbn
                                                               && item.Syouki == syouki);
            // Assert
            Assert.IsTrue(syoukiInfAfter != null);
        }
        finally
        {
            if (syoukiInf != null)
            {
                tenant.SyoukiInfs.Remove(syoukiInf);
                tenant.SaveChanges();
            }
        }
    }
    #endregion GetSyoukiInfList

    #region GetSyoukiKbnMstList
    [Test]
    public void TC_009_GetSyoukiKbnMstList_TestReloadCache()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(9999, 999999999);
        int syoukiKbn = random.Next(9999, 999999999);
        int sinYm = 202202;
        int startYm = 202201;
        int endYm = 202203;
        string name = "Name";
        SyoukiKbnMst? syoukiKbnMst = new()
        {
            HpId = hpId,
            SyoukiKbn = syoukiKbn,
            StartYm = startYm,
            EndYm = endYm,
            Name = name
        };

        tenant.SyoukiKbnMsts.Add(syoukiKbnMst);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetSyoukiKbnMstList(hpId, sinYm);

            var syoukiInfMst = result.FirstOrDefault(item => item.SyoukiKbn == syoukiKbn
                                                             && item.StartYm == startYm
                                                             && item.EndYm == endYm
                                                             && item.Name == name);
            // Assert
            Assert.IsTrue(syoukiInfMst != null);
        }
        finally
        {
            if (syoukiKbnMst != null)
            {
                tenant.SyoukiKbnMsts.Remove(syoukiKbnMst);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_010_GetSyoukiKbnMstList_TestReadCache()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(9999, 999999999);
        int syoukiKbn = random.Next(9999, 999999999);
        int sinYm = 202202;
        int startYm = 202201;
        int endYm = 202203;
        string name = "Name";
        SyoukiKbnMst? syoukiKbnMst = new()
        {
            HpId = hpId,
            SyoukiKbn = syoukiKbn,
            StartYm = startYm,
            EndYm = endYm,
            Name = name
        };

        tenant.SyoukiKbnMsts.Add(syoukiKbnMst);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetSyoukiKbnMstList(hpId, sinYm);
            result = receiptRepository.GetSyoukiKbnMstList(hpId, sinYm);

            var syoukiInfMst = result.FirstOrDefault(item => item.SyoukiKbn == syoukiKbn
                                                             && item.StartYm == startYm
                                                             && item.EndYm == endYm
                                                             && item.Name == name);
            // Assert
            Assert.IsTrue(syoukiInfMst != null);
        }
        finally
        {
            if (syoukiKbnMst != null)
            {
                tenant.SyoukiKbnMsts.Remove(syoukiKbnMst);
                tenant.SaveChanges();
            }
        }
    }
    #endregion GetSyoukiKbnMstList

    #region SaveSyoukiInfList
    [Test]
    public void TC_011_SaveSyoukiInfList_TestCreateSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int seqNo = 0;
        int sortNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int syoukiKbn = random.Next(9999, 999999999);
        int sinYm = 202202;
        string syouki = "Syouki";
        bool isDeleted = false;

        SyoukiInfModel syoukiInfModel = new SyoukiInfModel(ptId, sinYm, hokenId, seqNo, sortNo, syoukiKbn, syouki, isDeleted);
        SyoukiInf? syoukiInf = null;
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveSyoukiInfList(hpId, userId, new() { syoukiInfModel });

            syoukiInf = tenant.SyoukiInfs.FirstOrDefault(item => item.PtId == ptId
                                                                 && item.HpId == hpId
                                                                 && item.SinYm == sinYm
                                                                 && item.HokenId == hokenId
                                                                 && item.SortNo == sortNo
                                                                 && item.SyoukiKbn == syoukiKbn
                                                                 && item.Syouki == syouki
                                                                 && item.IsDeleted == 0);
            // Assert
            Assert.IsTrue(syoukiInf != null);
        }
        finally
        {
            if (syoukiInf != null)
            {
                tenant.SyoukiInfs.Remove(syoukiInf);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_012_SaveSyoukiInfList_TestUpdateSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int syoukiKbn = random.Next(9999, 999999999);
        int sinYm = 202202;
        string syouki = "Syouki";
        bool isDeleted = false;

        SyoukiInf? syoukiInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            IsDeleted = 0,
            SeqNo = seqNo,
        };

        tenant.SyoukiInfs.Add(syoukiInf);
        SyoukiInfModel syoukiInfModel = new SyoukiInfModel(ptId, sinYm, hokenId, seqNo, sortNo, syoukiKbn, syouki, isDeleted);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveSyoukiInfList(hpId, userId, new() { syoukiInfModel });

            syoukiInf = tenant.SyoukiInfs.FirstOrDefault(item => item.PtId == ptId
                                                                 && item.HpId == hpId
                                                                 && item.SinYm == sinYm
                                                                 && item.HokenId == hokenId
                                                                 && item.SyoukiKbn == syoukiKbn
                                                                 && item.Syouki == syouki
                                                                 && item.SeqNo == seqNo
                                                                 && item.SortNo == sortNo
                                                                 && item.IsDeleted == 0);
            // Assert
            Assert.IsTrue(syoukiInf != null);
        }
        finally
        {
            if (syoukiInf != null)
            {
                tenant = TenantProvider.GetNoTrackingDataContext();
                tenant.SyoukiInfs.Remove(syoukiInf);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_013_SaveSyoukiInfList_TestContinueCondition()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int syoukiKbn = random.Next(9999, 999999999);
        int sinYm = 202202;
        string syouki = "Syouki";
        bool isDeleted = false;

        SyoukiInf? syoukiInf = null;
        SyoukiInfModel syoukiInfModel = new SyoukiInfModel(ptId, sinYm, hokenId, seqNo, sortNo, syoukiKbn, syouki, isDeleted);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveSyoukiInfList(hpId, userId, new() { syoukiInfModel });

            syoukiInf = tenant.SyoukiInfs.FirstOrDefault(item => item.PtId == ptId
                                                                 && item.HpId == hpId
                                                                 && item.SinYm == sinYm
                                                                 && item.HokenId == hokenId
                                                                 && item.SyoukiKbn == syoukiKbn
                                                                 && item.Syouki == syouki
                                                                 && item.SeqNo == seqNo
                                                                 && item.SortNo == sortNo
                                                                 && item.IsDeleted == 0);
            // Assert
            Assert.IsTrue(syoukiInf == null);
        }
        finally
        {
            if (syoukiInf != null)
            {
                tenant = TenantProvider.GetNoTrackingDataContext();
                tenant.SyoukiInfs.Remove(syoukiInf);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_014_SaveSyoukiInfList_TestDeleteSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int syoukiKbn = random.Next(9999, 999999999);
        int sinYm = 202202;
        string syouki = "Syouki";
        bool isDeleted = true;

        SyoukiInf? syoukiInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            IsDeleted = 0,
            SeqNo = seqNo,
        };

        tenant.SyoukiInfs.Add(syoukiInf);
        SyoukiInfModel syoukiInfModel = new SyoukiInfModel(ptId, sinYm, hokenId, seqNo, sortNo, syoukiKbn, syouki, isDeleted);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveSyoukiInfList(hpId, userId, new() { syoukiInfModel });

            syoukiInf = tenant.SyoukiInfs.FirstOrDefault(item => item.PtId == ptId
                                                                 && item.HpId == hpId
                                                                 && item.SinYm == sinYm
                                                                 && item.HokenId == hokenId
                                                                 && item.SeqNo == seqNo
                                                                 && item.IsDeleted == 1);
            // Assert
            Assert.IsTrue(syoukiInf != null);
        }
        finally
        {
            if (syoukiInf != null)
            {
                tenant = TenantProvider.GetNoTrackingDataContext();
                tenant.SyoukiInfs.Remove(syoukiInf);
                tenant.SaveChanges();
            }
        }
    }
    #endregion SaveSyoukiInfList

    #region GetSyobyoKeikaList
    [Test]
    public void TC_015_GetSyobyoKeikaList_TestSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = 0;
        int seqNo = random.Next(9999, 999999999);
        int hokenKbn = random.Next(11, 13);
        int sinDay = 20220201;
        int sinYm = 202202;
        string keika = "Keika";

        SyobyoKeika? syobyoKeika = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            IsDeleted = 0,
            SeqNo = seqNo,
            SinDay = sinDay,
            Keika = keika
        };

        tenant.SyobyoKeikas.Add(syobyoKeika);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetSyobyoKeikaList(hpId, sinYm, ptId, hokenId, hokenKbn);

            var syoukiInfAfter = result.FirstOrDefault(item => item.PtId == ptId
                                                               && item.SinYm == sinYm
                                                               && item.SinDay == sinDay
                                                               && item.HokenId == hokenId
                                                               && item.SeqNo == seqNo
                                                               && item.Keika == keika);

            // Assert
            Assert.IsTrue(syoukiInfAfter != null);
        }
        finally
        {
            if (syobyoKeika != null)
            {
                tenant.SyobyoKeikas.Remove(syobyoKeika);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_016_GetSyobyoKeikaList_TestContinueProgress()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenKbn = random.Next(14, 9999);
        int sinDay = 20220201;
        int sinYm = 202202;
        string keika = "Keika";

        // Act
        var result = receiptRepository.GetSyobyoKeikaList(hpId, sinYm, ptId, hokenId, hokenKbn);

        var syoukiInfAfter = result.FirstOrDefault(item => item.PtId == ptId
                                                           && item.SinYm == sinYm
                                                           && item.SinDay == sinDay
                                                           && item.HokenId == hokenId
                                                           && item.SeqNo == seqNo
                                                           && item.Keika == keika);

        // Assert
        Assert.IsTrue(syoukiInfAfter == null);
    }
    #endregion GetSyobyoKeikaList

    #region SaveSyobyoKeikaList
    [Test]
    public void TC_017_SaveSyobyoKeikaList_TestCreateSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = 0;
        int hokenKbn = random.Next(11, 13);
        int sinDay = 20220201;
        int sinYm = 202202;
        string keika = "Keika";
        bool isDeleted = false;

        SyobyoKeikaModel syobyoKeikaModel = new(ptId, sinYm, sinDay, hokenId, seqNo, keika, isDeleted);

        SyobyoKeika? syobyoKeika = null;

        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveSyobyoKeikaList(hpId, userId, new() { syobyoKeikaModel });

            syobyoKeika = tenant.SyobyoKeikas.FirstOrDefault(item => item.PtId == ptId
                                                                     && item.HpId == hpId
                                                                     && item.SinYm == sinYm
                                                                     && item.SinDay == sinDay
                                                                     && item.HokenId == hokenId
                                                                     && item.Keika == keika
                                                                     && item.IsDeleted == 0);

            // Assert
            Assert.IsTrue(result && syobyoKeika != null);
        }
        finally
        {
            if (syobyoKeika != null)
            {
                tenant.SyobyoKeikas.Remove(syobyoKeika);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_018_SaveSyobyoKeikaList_TestCreateAndContinueSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = 0;
        int hokenKbn = random.Next(11, 13);
        int sinDay = 20220201;
        int sinYm = 202202;
        string keika = "Keika";
        bool isDeleted = false;

        SyobyoKeikaModel syobyoKeikaModel = new(ptId, sinYm, sinDay, hokenId, seqNo, keika, isDeleted);

        SyobyoKeika syobyoKeika2 = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            IsDeleted = 0,
            SinDay = sinDay,
            SeqNo = seqNo,
        };

        tenant.SyobyoKeikas.Add(syobyoKeika2);

        SyobyoKeika? syobyoKeika = null;

        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveSyobyoKeikaList(hpId, userId, new() { syobyoKeikaModel });

            syobyoKeika = tenant.SyobyoKeikas.FirstOrDefault(item => item.PtId == ptId
                                                                     && item.HpId == hpId
                                                                     && item.SinYm == sinYm
                                                                     && item.SinDay == sinDay
                                                                     && item.HokenId == hokenId
                                                                     && item.Keika == keika
                                                                     && item.IsDeleted == 0);

            // Assert
            Assert.IsTrue(result && syobyoKeika != null);
        }
        finally
        {
            tenant.SyobyoKeikas.Remove(syobyoKeika2);
            if (syobyoKeika != null)
            {
                tenant.SyobyoKeikas.Remove(syobyoKeika);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_019_SaveSyobyoKeikaList_TestContinueCondition()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenKbn = random.Next(11, 13);
        int sinDay = 20220201;
        int sinYm = 202202;
        string keika = "Keika";
        bool isDeleted = false;

        SyobyoKeikaModel syobyoKeikaModel = new(ptId, sinYm, sinDay, hokenId, seqNo, keika, isDeleted);

        SyobyoKeika? syobyoKeika = null;

        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveSyobyoKeikaList(hpId, userId, new() { syobyoKeikaModel });

            syobyoKeika = tenant.SyobyoKeikas.FirstOrDefault(item => item.PtId == ptId
                                                                     && item.HpId == hpId
                                                                     && item.SinYm == sinYm
                                                                     && item.SinDay == sinDay
                                                                     && item.HokenId == hokenId
                                                                     && item.Keika == keika
                                                                     && item.IsDeleted == 0);

            // Assert
            Assert.IsTrue(!result && syobyoKeika == null);
        }
        finally
        {
            if (syobyoKeika != null)
            {
                tenant.SyobyoKeikas.Remove(syobyoKeika);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_020_SaveSyobyoKeikaList_TestSyobyoKeikaListNotAny()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenKbn = random.Next(11, 13);
        int sinDay = 20220201;
        int sinYm = 202202;
        string keika = "Keika";

        SyobyoKeika? syobyoKeika = null;

        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveSyobyoKeikaList(hpId, userId, new());

            syobyoKeika = tenant.SyobyoKeikas.FirstOrDefault(item => item.PtId == ptId
                                                                     && item.HpId == hpId
                                                                     && item.SinYm == sinYm
                                                                     && item.SinDay == sinDay
                                                                     && item.HokenId == hokenId
                                                                     && item.Keika == keika
                                                                     && item.IsDeleted == 0);

            // Assert
            Assert.IsTrue(!result && syobyoKeika == null);
        }
        finally
        {
            if (syobyoKeika != null)
            {
                tenant.SyobyoKeikas.Remove(syobyoKeika);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_021_SaveSyobyoKeikaList_TestUpdateSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenKbn = random.Next(11, 13);
        int sinDay = 20220201;
        int sinYm = 202202;
        string keika = "Keika";
        bool isDeleted = false;

        SyobyoKeikaModel syobyoKeikaModel = new(ptId, sinYm, sinDay, hokenId, seqNo, keika, isDeleted);

        SyobyoKeika? syobyoKeika = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            IsDeleted = 0,
            SinDay = sinDay,
            SeqNo = seqNo,
        };

        tenant.SyobyoKeikas.Add(syobyoKeika);

        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveSyobyoKeikaList(hpId, userId, new() { syobyoKeikaModel });

            var syobyoKeikaAfter = tenant.SyobyoKeikas.FirstOrDefault(item => item.PtId == ptId
                                                                              && item.HpId == hpId
                                                                              && item.SinYm == sinYm
                                                                              && item.SinDay == sinDay
                                                                              && item.HokenId == hokenId
                                                                              && item.Keika == keika
                                                                              && item.SeqNo == seqNo
                                                                              && item.IsDeleted == 0);

            // Assert
            Assert.IsTrue(result && syobyoKeikaAfter != null);
        }
        finally
        {
            if (syobyoKeika != null)
            {
                tenant.SyobyoKeikas.Remove(syobyoKeika);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_022_SaveSyobyoKeikaList_TestDeletedSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenKbn = random.Next(11, 13);
        int sinDay = 20220201;
        int sinYm = 202202;
        string keika = "Keika";
        bool isDeleted = true;

        SyobyoKeikaModel syobyoKeikaModel = new(ptId, sinYm, sinDay, hokenId, seqNo, keika, isDeleted);

        SyobyoKeika? syobyoKeika = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            IsDeleted = 0,
            SeqNo = seqNo,
        };

        tenant.SyobyoKeikas.Add(syobyoKeika);

        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveSyobyoKeikaList(hpId, userId, new() { syobyoKeikaModel });

            var syobyoKeikaAfter = tenant.SyobyoKeikas.FirstOrDefault(item => item.PtId == ptId
                                                                              && item.HpId == hpId
                                                                              && item.SinYm == sinYm
                                                                              && item.HokenId == hokenId
                                                                              && item.SeqNo == seqNo
                                                                              && item.IsDeleted == 1);

            // Assert
            Assert.IsTrue(result && syobyoKeikaAfter != null);
        }
        finally
        {
            if (syobyoKeika != null)
            {
                tenant.SyobyoKeikas.Remove(syobyoKeika);
                tenant.SaveChanges();
            }
        }
    }
    #endregion SaveSyobyoKeikaList

    #region GetReceReasonList
    [Test]
    public void TC_023_GetReceReasonList_TestSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int hokenKbn = random.Next(11, 13);
        int sinDate = 20220201;
        int sinYm = 202202;
        int seikyuYm = 202202;
        string henreiJiyuuCd = "HenreiCd";
        string henreiJiyuu = "HenreiJiyuu";
        string hosoku = "Hosoku";

        ReceInf? receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeikyuYm = seikyuYm,
            HokenId = hokenId,
        };

        ReceSeikyu? receSeikyu = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeikyuYm = seikyuYm,
            HokenId = hokenId,
            IsDeleted = 0
        };

        RecedenHenJiyuu recedenHenJiyuu = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            HenreiJiyuuCd = henreiJiyuuCd,
            HenreiJiyuu = henreiJiyuu,
            Hosoku = hosoku,
            IsDeleted = 0
        };

        tenant.ReceInfs.Add(receInf);
        tenant.ReceSeikyus.Add(receSeikyu);
        tenant.RecedenHenJiyuus.Add(recedenHenJiyuu);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceReasonList(hpId, seikyuYm, sinDate, ptId, hokenId);

            var receReasonAfter = result.FirstOrDefault(item => item.HokenId == hokenId
                                                                && item.SinYm == sinYm
                                                                && item.HenreiJiyuuCd == henreiJiyuuCd
                                                                && item.HenreiJiyuu == henreiJiyuu
                                                                && item.Hosoku == hosoku);

            // Assert
            Assert.IsTrue(receReasonAfter != null);
        }
        finally
        {
            if (receInf != null)
            {
                tenant.ReceInfs.Remove(receInf);
            }
            if (receSeikyu != null)
            {
                tenant.ReceSeikyus.Remove(receSeikyu);
            }
            if (recedenHenJiyuu != null)
            {
                tenant.RecedenHenJiyuus.Remove(recedenHenJiyuu);
            }
            tenant.SaveChanges();
        }
    }
    #endregion GetReceReasonList

    #region GetReceCheckCmtList
    [Test]
    public void TC_024_GetReceCheckCmtList_TestSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int isPending = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int isChecked = random.Next(9999, 999999999);
        int sinYm = 202202;
        string cmt = "Cmt";

        ReceCheckCmt? receCheckCmt = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            SeqNo = seqNo,
            IsPending = isPending,
            Cmt = cmt,
            IsChecked = isChecked,
            SortNo = sortNo
        };

        tenant.ReceCheckCmts.Add(receCheckCmt);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceCheckCmtList(hpId, sinYm, ptId, hokenId);

            var receCheckCmtAfter = result.FirstOrDefault(item => item.PtId == ptId
                                                                  && item.SeqNo == seqNo
                                                                  && item.SinYm == sinYm
                                                                  && item.HokenId == hokenId
                                                                  && item.IsPending == isPending
                                                                  && item.Cmt == cmt
                                                                  && item.IsChecked == isChecked
                                                                  && item.SortNo == sortNo);

            // Assert
            Assert.IsTrue(receCheckCmtAfter != null);
        }
        finally
        {
            if (receCheckCmt != null)
            {
                tenant.ReceCheckCmts.Remove(receCheckCmt);
                tenant.SaveChanges();
            }
        }
    }
    #endregion GetReceCheckCmtList

    #region GetReceCheckErrList
    [Test]
    public void TC_025_GetReceCheckErrList_TestSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int isPending = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int isChecked = random.Next(9999, 999999999);
        int sinYm = 202202;
        int sinDate = 20220201;
        string errCd = "ErrCd";
        string aCd = "ACd";
        string bCd = "BCd";
        string message1 = "Message1";
        string message2 = "Message2";

        ReceCheckErr? receCheckErr = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            ErrCd = errCd,
            SinDate = sinDate,
            ACd = aCd,
            BCd = bCd,
            Message1 = message1,
            Message2 = message2,
            IsChecked = isChecked,
        };

        tenant.ReceCheckErrs.Add(receCheckErr);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceCheckErrList(hpId, sinYm, ptId, hokenId);

            var receCheckErrAfter = result.FirstOrDefault(item => item.PtId == ptId
                                                                  && item.SinYm == sinYm
                                                                  && item.HokenId == hokenId
                                                                  && item.ErrCd == errCd
                                                                  && item.SinDate == sinDate
                                                                  && item.ACd == aCd
                                                                  && item.BCd == bCd
                                                                  && item.Message1 == message1
                                                                  && item.Message2 == message2
                                                                  && item.IsChecked == isChecked);

            // Assert
            Assert.IsTrue(receCheckErrAfter != null);
        }
        finally
        {
            if (receCheckErr != null)
            {
                tenant.ReceCheckErrs.Remove(receCheckErr);
                tenant.SaveChanges();
            }
        }
    }
    #endregion GetReceCheckErrList

    #region SaveReceCheckErrList
    [Test]
    public void TC_026_SaveReceCheckErrList_TestCreateSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int isPending = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int isChecked = random.Next(9999, 999999999);
        int sinYm = 202202;
        int sinDate = 20220201;
        string errCd = "ErrCd";
        string aCd = "ACd";
        string bCd = "BCd";
        string message1 = "Message1";
        string message2 = "Message2";

        ReceCheckErr? receCheckErr = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            ErrCd = errCd,
            SinDate = sinDate,
            ACd = aCd,
            BCd = bCd,
            Message1 = message1,
            Message2 = message2,
            IsChecked = 0,
        };

        ReceCheckErrModel receCheckErrModel = new ReceCheckErrModel(errCd, sinDate, aCd, bCd, isChecked);

        tenant.ReceCheckErrs.Add(receCheckErr);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveReceCheckErrList(hpId, userId, hokenId, sinYm, ptId, new() { receCheckErrModel });

            var receCheckErrAfter = tenant.ReceCheckErrs.FirstOrDefault(item => item.PtId == ptId
                                                                                && item.SinYm == sinYm
                                                                                && item.HokenId == hokenId
                                                                                && item.ErrCd == errCd
                                                                                && item.SinDate == sinDate
                                                                                && item.ACd == aCd
                                                                                && item.BCd == bCd
                                                                                && item.Message1 == message1
                                                                                && item.Message2 == message2
                                                                                && item.IsChecked == isChecked);

            // Assert
            Assert.IsTrue(result && receCheckErrAfter != null);
        }
        finally
        {
            if (receCheckErr != null)
            {
                tenant.ReceCheckErrs.Remove(receCheckErr);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_027_SaveReceCheckErrList_TestReceCheckErrorListNotAny()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int isPending = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int isChecked = random.Next(9999, 999999999);
        int sinYm = 202202;
        int sinDate = 20220201;
        string errCd = "ErrCd";
        string aCd = "ACd";
        string bCd = "BCd";
        string message1 = "Message1";
        string message2 = "Message2";

        ReceCheckErr? receCheckErr = null;

        try
        {
            // Act
            var result = receiptRepository.SaveReceCheckErrList(hpId, userId, hokenId, sinYm, ptId, new());

            var receCheckErrAfter = tenant.ReceCheckErrs.FirstOrDefault(item => item.PtId == ptId
                                                                                && item.SinYm == sinYm
                                                                                && item.HokenId == hokenId
                                                                                && item.ErrCd == errCd
                                                                                && item.SinDate == sinDate
                                                                                && item.ACd == aCd
                                                                                && item.BCd == bCd
                                                                                && item.Message1 == message1
                                                                                && item.Message2 == message2
                                                                                && item.IsChecked == isChecked);

            // Assert
            Assert.IsTrue(result && receCheckErrAfter == null);
        }
        finally
        {
            if (receCheckErr != null)
            {
                tenant.ReceCheckErrs.Remove(receCheckErr);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_028_SaveReceCheckErrList_TestContinueCondition()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int isPending = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int isChecked = random.Next(9999, 999999999);
        int sinYm = 202202;
        int sinDate = 20220201;
        string errCd = "ErrCd";
        string aCd = "ACd";
        string bCd = "BCd";
        string message1 = "Message1";
        string message2 = "Message2";

        ReceCheckErr? receCheckErr = null;

        ReceCheckErrModel receCheckErrModel = new ReceCheckErrModel(errCd, sinDate, aCd, bCd, isChecked);

        try
        {
            // Act
            var result = receiptRepository.SaveReceCheckErrList(hpId, userId, hokenId, sinYm, ptId, new() { receCheckErrModel });

            var receCheckErrAfter = tenant.ReceCheckErrs.FirstOrDefault(item => item.PtId == ptId
                                                                                && item.SinYm == sinYm
                                                                                && item.HokenId == hokenId
                                                                                && item.ErrCd == errCd
                                                                                && item.SinDate == sinDate
                                                                                && item.ACd == aCd
                                                                                && item.BCd == bCd
                                                                                && item.Message1 == message1
                                                                                && item.Message2 == message2
                                                                                && item.IsChecked == isChecked);

            // Assert
            Assert.IsTrue(!result && receCheckErrAfter == null);
        }
        finally
        {
            if (receCheckErr != null)
            {
                tenant.ReceCheckErrs.Remove(receCheckErr);
                tenant.SaveChanges();
            }
        }
    }
    #endregion SaveReceCheckErrList

    #region SaveReceCheckCmtList
    [Test]
    public void TC_029_SaveReceCheckCmtList_TestCreateSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = 0;
        int isPending = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int isChecked = random.Next(9999, 999999999);
        bool isDeleted = false;
        int sinYm = 202202;
        string cmt = "Cmt";

        ReceCheckCmt? receCheckCmt = null;
        ReceCheckCmtModel receCheckCmtModel = new ReceCheckCmtModel(seqNo, isPending, cmt, isChecked, sortNo, isDeleted);

        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveReceCheckCmtList(hpId, userId, hokenId, sinYm, ptId, new() { receCheckCmtModel });

            receCheckCmt = tenant.ReceCheckCmts.FirstOrDefault(item => item.PtId == ptId
                                                                       && item.SinYm == sinYm
                                                                       && item.HokenId == hokenId
                                                                       && item.IsPending == isPending
                                                                       && item.Cmt == cmt
                                                                       && item.SortNo == sortNo
                                                                       && item.IsDeleted == 0
                                                                       && item.IsChecked == isChecked);

            // Assert
            Assert.IsTrue(result.Any() && receCheckCmt != null);
        }
        finally
        {
            if (receCheckCmt != null)
            {
                tenant.ReceCheckCmts.Remove(receCheckCmt);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_030_SaveReceCheckCmtList_TestContinueCondition()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int isPending = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int isChecked = random.Next(9999, 999999999);
        bool isDeleted = false;
        int sinYm = 202202;
        string cmt = "Cmt";

        ReceCheckCmt? receCheckCmt = null;
        ReceCheckCmtModel receCheckCmtModel = new ReceCheckCmtModel(seqNo, isPending, cmt, isChecked, sortNo, isDeleted);

        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveReceCheckCmtList(hpId, userId, hokenId, sinYm, ptId, new() { receCheckCmtModel });

            receCheckCmt = tenant.ReceCheckCmts.FirstOrDefault(item => item.PtId == ptId
                                                                       && item.SinYm == sinYm
                                                                       && item.HokenId == hokenId
                                                                       && item.SeqNo == seqNo
                                                                       && item.IsPending == isPending
                                                                       && item.Cmt == cmt
                                                                       && item.SortNo == sortNo
                                                                       && item.IsDeleted == 0
                                                                       && item.IsChecked == isChecked);

            // Assert
            Assert.IsTrue(!result.Any() && receCheckCmt == null);
        }
        finally
        {
            if (receCheckCmt != null)
            {
                tenant.ReceCheckCmts.Remove(receCheckCmt);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_031_SaveReceCheckCmtList_TestReceCheckCmtListNotAny()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int isPending = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int isChecked = random.Next(9999, 999999999);
        int sinYm = 202202;
        string cmt = "Cmt";

        ReceCheckCmt? receCheckCmt = null;
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveReceCheckCmtList(hpId, userId, hokenId, sinYm, ptId, new());

            receCheckCmt = tenant.ReceCheckCmts.FirstOrDefault(item => item.PtId == ptId
                                                                       && item.SinYm == sinYm
                                                                       && item.HokenId == hokenId
                                                                       && item.SeqNo == seqNo
                                                                       && item.IsPending == isPending
                                                                       && item.Cmt == cmt
                                                                       && item.SortNo == sortNo
                                                                       && item.IsDeleted == 0
                                                                       && item.IsChecked == isChecked);

            // Assert
            Assert.IsTrue(!result.Any() && receCheckCmt == null);
        }
        finally
        {
            if (receCheckCmt != null)
            {
                tenant.ReceCheckCmts.Remove(receCheckCmt);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_032_SaveReceCheckCmtList_TestUpdateSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int isPending = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int isChecked = random.Next(9999, 999999999);
        bool isDeleted = false;
        int sinYm = 202202;
        string cmt = "Cmt";

        ReceCheckCmt? receCheckCmt = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            SeqNo = seqNo,
            IsChecked = isChecked,
        };

        ReceCheckCmtModel receCheckCmtModel = new ReceCheckCmtModel(seqNo, isPending, cmt, isChecked, sortNo, isDeleted);

        tenant.ReceCheckCmts.Add(receCheckCmt);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveReceCheckCmtList(hpId, userId, hokenId, sinYm, ptId, new() { receCheckCmtModel });

            receCheckCmt = tenant.ReceCheckCmts.FirstOrDefault(item => item.PtId == ptId
                                                                       && item.SinYm == sinYm
                                                                       && item.HokenId == hokenId
                                                                       && item.IsPending == isPending
                                                                       && item.Cmt == cmt
                                                                       && item.SortNo == sortNo
                                                                       && item.IsDeleted == 0
                                                                       && item.IsChecked == isChecked);

            // Assert
            Assert.IsTrue(result.Any() && receCheckCmt != null);
        }
        finally
        {
            if (receCheckCmt != null)
            {
                tenant = TenantProvider.GetNoTrackingDataContext();
                tenant.ReceCheckCmts.Remove(receCheckCmt);
                tenant.SaveChanges();
            }
        }
    }

    [Test]
    public void TC_033_SaveReceCheckCmtList_TestDeleteSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int seqNo = random.Next(9999, 999999999);
        int isPending = random.Next(9999, 999999999);
        int sortNo = random.Next(9999, 999999999);
        int isChecked = random.Next(9999, 999999999);
        bool isDeleted = true;
        int sinYm = 202202;
        string cmt = "Cmt";

        ReceCheckCmt? receCheckCmt = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            SeqNo = seqNo,
            IsChecked = isChecked,
        };

        ReceCheckCmtModel receCheckCmtModel = new ReceCheckCmtModel(seqNo, isPending, cmt, isChecked, sortNo, isDeleted);

        tenant.ReceCheckCmts.Add(receCheckCmt);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.SaveReceCheckCmtList(hpId, userId, hokenId, sinYm, ptId, new() { receCheckCmtModel });

            receCheckCmt = tenant.ReceCheckCmts.FirstOrDefault(item => item.PtId == ptId
                                                                       && item.SinYm == sinYm
                                                                       && item.HokenId == hokenId
                                                                       && item.IsDeleted == 1);

            // Assert
            Assert.IsTrue(result.Any() && receCheckCmt != null);
        }
        finally
        {
            if (receCheckCmt != null)
            {
                tenant = TenantProvider.GetNoTrackingDataContext();
                tenant.ReceCheckCmts.Remove(receCheckCmt);
                tenant.SaveChanges();
            }
        }
    }


    #endregion SaveReceCheckCmtList

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
