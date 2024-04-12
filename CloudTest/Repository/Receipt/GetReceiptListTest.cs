using DocumentFormat.OpenXml.Wordprocessing;
using Domain.CalculationInf;
using Domain.Constant;
using Domain.Models.MstItem;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using Entity.Tenant;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using PostgreDataContext;
using ZstdSharp.Unsafe;

namespace CloudUnitTest.Repository.Receipt;

public class GetReceiptListTest : BaseUT
{
    #region GetReceiptList
    [Test]
    public void TC_001_GetReceiptList_Test_SeikyuYmEqual0()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 0;
        bool isAdvanceSearch = false;
        string tokki = "tokkiUT";
        List<int> hokenSbts = new() { random.Next(999, 999999) };
        bool isAll = false;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        int receSbtCenter = random.Next(999, 999999);
        int receSbtRight = random.Next(999, 999999);
        string hokenHoubetu = "hokenHoubetuUT";
        int kohi1Houbetu = random.Next(999, 999999);
        int kohi2Houbetu = random.Next(999, 999999);
        int kohi3Houbetu = random.Next(999, 999999);
        int kohi4Houbetu = random.Next(999, 999999);
        bool isIncludeSingle = false;
        string hokensyaNoFrom = "20220201";
        string hokensyaNoTo = "20220230";
        long hokensyaNoFromLong = random.Next(999, 999999);
        long hokensyaNoToLong = random.Next(999, 999999);
        string ptId = random.Next(999, 999999).ToString();
        long ptIdFrom = random.Next(999, 999999);
        long ptIdTo = random.Next(999, 999999);
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = random.Next(999, 999999);
        long tensuTo = random.Next(999, 999999);
        int lastRaiinDateFrom = random.Next(999, 999999);
        int lastRaiinDateTo = random.Next(999, 999999);
        int birthDayFrom = random.Next(999, 999999);
        int birthDayTo = random.Next(999, 999999);
        string itemCd = "itemCdUT";
        string inputName = "inputNameUT";
        string rangeSeach = "and";
        int amount = random.Next(999, 999999);
        int orderStatus = random.Next(999, 999999);
        bool isComment = false;
        List<ItemSearchModel> itemList = new() { new ItemSearchModel(itemCd, inputName, rangeSeach, amount, orderStatus, isComment) };
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        string byomeiCd = "byomeiCd";
        List<SearchByoMstModel> byomeiCdList = new() { new SearchByoMstModel(byomeiCd, inputName, isComment) };
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = random.Next(999, 999999);
        long futansyaNoToLong = random.Next(999, 999999);
        int kaId = random.Next(999, 999999);
        int doctorId = random.Next(999, 999999);
        string name = "nameUT";
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;

        try
        {
            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptId, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(!result.Any());
        }
        finally
        {
            receiptRepository.ReleaseResource();
        }
    }

    [Test]
    public void TC_002_GetReceiptList_Test_SeikyuYmNotEqual0()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        bool isAdvanceSearch = false;
        string tokki = "tokkiUT";
        List<int> hokenSbts = new() { random.Next(999, 999999) };
        bool isAll = false;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        int receSbtCenter = random.Next(999, 999999);
        int receSbtRight = random.Next(999, 999999);
        string hokenHoubetu = "hokenHoubetuUT";
        int kohi1Houbetu = random.Next(999, 999999);
        int kohi2Houbetu = random.Next(999, 999999);
        int kohi3Houbetu = random.Next(999, 999999);
        int kohi4Houbetu = random.Next(999, 999999);
        bool isIncludeSingle = false;
        string hokensyaNoFrom = "20220201";
        string hokensyaNoTo = "20220230";
        long hokensyaNoFromLong = random.Next(999, 999999);
        long hokensyaNoToLong = random.Next(999, 999999);
        string ptId = random.Next(999, 999999).ToString();
        long ptIdFrom = random.Next(999, 999999);
        long ptIdTo = random.Next(999, 999999);
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = random.Next(999, 999999);
        long tensuTo = random.Next(999, 999999);
        int lastRaiinDateFrom = random.Next(999, 999999);
        int lastRaiinDateTo = random.Next(999, 999999);
        int birthDayFrom = random.Next(999, 999999);
        int birthDayTo = random.Next(999, 999999);
        string itemCd = "itemCdUT";
        string inputName = "inputNameUT";
        string rangeSeach = "and";
        int amount = random.Next(999, 999999);
        int orderStatus = random.Next(999, 999999);
        bool isComment = false;
        List<ItemSearchModel> itemList = new() { new ItemSearchModel(itemCd, inputName, rangeSeach, amount, orderStatus, isComment) };
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        string byomeiCd = "byomeiCd";
        List<SearchByoMstModel> byomeiCdList = new() { new SearchByoMstModel(byomeiCd, inputName, isComment) };
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = random.Next(999, 999999);
        long futansyaNoToLong = random.Next(999, 999999);
        int kaId = random.Next(999, 999999);
        int doctorId = random.Next(999, 999999);
        string name = "nameUT";
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;

        try
        {
            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptId, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(!result.Any());
        }
        finally
        {
            receiptRepository.ReleaseResource();
        }
    }
    #endregion GetReceiptList

    #region ActionGetReceiptList
    [Test]
    public void TC_003_ActionGetReceiptList_Test_IsTestPatientSearchFalse()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = false;
        string tokki = string.Empty;
        List<int> hokenSbts = new();
        bool isAll = false;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        int receSbtCenter = 0;
        int receSbtRight = 0;
        string hokenHoubetu = string.Empty;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        bool isIncludeSingle = false;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = 0;
        long hokensyaNoToLong = 0;
        long ptIdFrom = 0;
        long ptIdTo = 0;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = 0;
        long tensuTo = 0;
        int lastRaiinDateFrom = 0;
        int lastRaiinDateTo = 0;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = 0;
        long futansyaNoToLong = 0;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert

            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && !item.IsPtTest));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_004_ActionGetReceiptList_Test_IsAdvanceSearch()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        int receSbtCenter = -1;
        int receSbtRight = -1;
        string hokenHoubetu = string.Empty;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        bool isIncludeSingle = false;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_005_ActionGetReceiptList_Test_HokenSbts_IsAny()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new() { hokenKbn };
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        int receSbtCenter = -1;
        int receSbtRight = -1;
        string hokenHoubetu = string.Empty;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        bool isIncludeSingle = false;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_006_ActionGetReceiptList_Test_ReceSbtCenter()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new() { hokenKbn };
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        int receSbtCenter = It.IsAny<int>();
        int receSbtRight = -1;
        string hokenHoubetu = string.Empty;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        bool isIncludeSingle = false;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_007_ActionGetReceiptList_Test_ReceSbtRight()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new() { hokenKbn };
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        int receSbtCenter = -1;
        int receSbtRight = It.IsAny<int>();
        string hokenHoubetu = string.Empty;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        bool isIncludeSingle = false;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_008_ActionGetReceiptList_Test_IsNotAny_ReceSbtRight()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        string hokenHoubetu = string.Empty;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        bool isIncludeSingle = false;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        int.TryParse(receSbt.Substring(2, 1), out int receSbtCenter);
        int receSbtRight = -1;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_009_ActionGetReceiptList_Test_IsNotAny_ReceSbtRight()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        string hokenHoubetu = string.Empty;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        bool isIncludeSingle = false;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        int.TryParse(receSbt.Substring(3, 1), out int receSbtRight);
        int receSbtCenter = -1;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_010_ActionGetReceiptList_Test_HokenHoubetu()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        bool isIncludeSingle = false;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        int receSbtRight = -1;
        int receSbtCenter = -1;
        string houbetu = random.Next(100, 999).ToString();
        string hokenHoubetu = houbetu;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_011_ActionGetReceiptList_Test_Kohi1Houbetu()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = true;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        int receSbtRight = -1;
        int receSbtCenter = -1;
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int.TryParse(kohi1HoubetuInput, out int kohi1Houbetu);
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_012_ActionGetReceiptList_Test_Kohi2Houbetu()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = true;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        int receSbtRight = -1;
        int receSbtCenter = -1;
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi1Houbetu = 0;
        int.TryParse(kohi2HoubetuInput, out int kohi2Houbetu);
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_013_ActionGetReceiptList_Test_Kohi3Houbetu()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = true;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        int receSbtRight = -1;
        int receSbtCenter = -1;
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int.TryParse(kohi3HoubetuInput, out int kohi3Houbetu);
        int kohi4Houbetu = 0;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_014_ActionGetReceiptList_Test_Kohi4Houbetu()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = true;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        int receSbtRight = -1;
        int receSbtCenter = -1;
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int.TryParse(kohi4HoubetuInput, out int kohi4Houbetu);

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_015_ActionGetReceiptList_Test_IsIncludeSingle_IsFalse()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        int receSbtRight = -1;
        int receSbtCenter = -1;
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int.TryParse(kohi4HoubetuInput, out int kohi4Houbetu);
        int kohi4ReceKisai = 1;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_016_ActionGetReceiptList_Test_HokensyaNoFromLong()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        int receSbtRight = -1;
        int receSbtCenter = -1;
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        long hokensyaNoFromLong = hokensyaNo.AsLong();
        long hokensyaNoToLong = -1;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_017_ActionGetReceiptList_Test_HokensyaNoToLong()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        int receSbtRight = -1;
        int receSbtCenter = -1;
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = hokensyaNo.AsLong();

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_018_ActionGetReceiptList_Test_TensuFrom()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int tensuFrom = tensu;
        long tensuTo = -1;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_019_ActionGetReceiptList_Test_TensuTo()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int tensuFrom = -1;
        long tensuTo = tensu;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_020_ActionGetReceiptList_Test_LastRaiinDateFrom_LastRaiinDateTo()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int lastRaiinDateFrom = sinDate;
        int lastRaiinDateTo = sinDate;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_021_ActionGetReceiptList_Test_PtIdFrom()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        long ptIdFrom = ptNum;
        long ptIdTo = -1;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_022_ActionGetReceiptList_Test_PtIdTo()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        string ptIdString = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.RangeSearch;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        long ptIdFrom = -1;
        long ptIdTo = ptNum;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_023_ActionGetReceiptList_Test_IndividualSearch()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        int kaId = 0;
        int doctorId = 0;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        string ptIdString = $"{ptNum},{random.Next(100, 999999999)},{random.Next(100, 999999999)}";

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_024_ActionGetReceiptList_Test_KaId_TantoId_SYSTEM_CONFIG_6002()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        string ptIdString = $"{ptNum},{random.Next(100, 999999999)},{random.Next(100, 999999999)}";
        int kaId = random.Next(100, 99999);
        int tantoId = random.Next(100, 99999);
        int doctorId = tantoId;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = 6002,
            GrpEdaNo = 0,
            Val = 1
        };

        SystemConf systemConf2 = new()
        {
            HpId = hpId,
            GrpCd = 6002,
            GrpEdaNo = 1,
            Val = 1
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SystemConfs.Add(systemConf);
            tenant.SystemConfs.Add(systemConf2);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SystemConfs.Remove(systemConf);
            tenant.SystemConfs.Remove(systemConf2);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_025_ActionGetReceiptList_Test_KaId_SYSTEM_CONFIG_6002_0_Val_0()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        string ptIdString = $"{ptNum},{random.Next(100, 999999999)},{random.Next(100, 999999999)}";
        int kaId = random.Next(100, 99999);
        int tantoId = -1;
        int doctorId = tantoId;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            KaId = kaId
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = 6002,
            GrpEdaNo = 0,
            Val = 0
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SystemConfs.Add(systemConf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SystemConfs.Remove(systemConf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_026_ActionGetReceiptList_Test_KaId_SYSTEM_CONFIG_6002_1_Val_1()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        string ptIdString = $"{ptNum},{random.Next(100, 999999999)},{random.Next(100, 999999999)}";
        int kaId = random.Next(100, 99999);
        int tantoId = -1;
        int doctorId = tantoId;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = 6002,
            GrpEdaNo = 0,
            Val = 1
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SystemConfs.Add(systemConf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SystemConfs.Remove(systemConf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_027_ActionGetReceiptList_Test_TantoId_SYSTEM_CONFIG_6002_1_Val_0()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        long ptIdTo = -1;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int kaId = 0;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        string ptIdString = $"{ptNum},{random.Next(100, 999999999)},{random.Next(100, 999999999)}";
        int tantoId = random.Next(100, 99999);
        int doctorId = tantoId;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = 6002,
            GrpEdaNo = 1,
            Val = 0
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SystemConfs.Add(systemConf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SystemConfs.Remove(systemConf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_028_ActionGetReceiptList_Test_TantoId_SYSTEM_CONFIG_6002_1_Val_1()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int kaId = 0;
        long ptIdTo = -1;
        string name = string.Empty;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        string ptIdString = $"{ptNum},{random.Next(100, 999999999)},{random.Next(100, 999999999)}";
        int tantoId = random.Next(100, 99999);
        int doctorId = tantoId;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = 6002,
            GrpEdaNo = 1,
            Val = 1
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SystemConfs.Add(systemConf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SystemConfs.Remove(systemConf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_029_ActionGetReceiptList_Test_Name()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string ptIdString = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        int kaId = 0;
        long ptIdTo = -1;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        string name = "PtNameUT";

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = name
        };

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = 6002,
            GrpEdaNo = 1,
            Val = 1
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SystemConfs.Add(systemConf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
                                             && item.Name == name
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SystemConfs.Remove(systemConf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_030_ActionGetReceiptList_Test_BirthDayFrom()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        string ptIdString = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int birthDay = 20000202;
        int birthDayFrom = birthDay;
        int birthDayTo = 0;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Birthday = birthDay
        };

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = 6002,
            GrpEdaNo = 1,
            Val = 1
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SystemConfs.Add(systemConf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
                                             && item.BirthDay == birthDay
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SystemConfs.Remove(systemConf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_031_ActionGetReceiptList_Test_BirthDayTo()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int birthDay = 20000202;
        int birthDayFrom = 0;
        int birthDayTo = birthDay;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Birthday = birthDay
        };

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = 6002,
            GrpEdaNo = 1,
            Val = 1
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SystemConfs.Add(systemConf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
                                             && item.BirthDay == birthDay
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SystemConfs.Remove(systemConf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_032_ActionGetReceiptList_Test_IsNotDisplayPrinted()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = true;
        Dictionary<int, string> groupSearchModels = new();
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int statusKbn = random.Next(0, 9);
        int fusenKbn = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = 6002,
            GrpEdaNo = 1,
            Val = 1
        };

        ReceStatus receStatus = new()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            StatusKbn = statusKbn,
            FusenKbn = fusenKbn,
            IsPaperRece = isPaperRece,
            Output = output
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SystemConfs.Add(systemConf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.ReceStatuses.Add(receStatus);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
                                             && item.StatusKbn == statusKbn
                                             && item.FusenKbn == fusenKbn
                                             && item.IsPaperRece == isPaperRece
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SystemConfs.Remove(systemConf);
            tenant.ReceStatuses.Remove(receStatus);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_033_ActionGetReceiptList_Test_GroupSearchModels()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        List<ItemSearchModel> itemList = new();
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int groupId = random.Next(100, 999999999);
        string groupCode = random.Next(100, 9999).ToString();
        Dictionary<int, string> groupSearchModels = new()
        {
            { random.Next(100, 999999999), string.Empty },
            { groupId, groupCode}
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = 6002,
            GrpEdaNo = 1,
            Val = 1
        };

        PtGrpInf ptGrpInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            GroupId = groupId,
            GroupCode = groupCode
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.SystemConfs.Add(systemConf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtGrpInfs.Add(ptGrpInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SystemConfs.Remove(systemConf);
            tenant.PtGrpInfs.Remove(ptGrpInf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_034_ActionGetReceiptList_Test_ItemList_OriginItemOrderList_OriginItemOrderList_Any()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);

        string itemCd = "ItemCdUT";
        string santeiItemCd = string.Empty;
        string inputName = "InputNameUT";
        string itemName = "ItemNameUT";
        string rangeSeach = "=";
        int amount = 1;
        int orderStatus = 1;
        bool isComment = false;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment)
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_035_ActionGetReceiptList_Test_ItemList_OriginItemOrderList_SanteiItemCd_Continue()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);

        string itemCd = "ItemCdUT";
        string santeiItemCd = "SanteiCdUT";
        string inputName = "InputNameUT";
        string itemName = "ItemNameUT";
        string rangeSeach = "=";
        int amount = 1;
        int orderStatus = 1;
        bool isComment = false;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment),
            new(santeiItemCd, inputName, rangeSeach, amount, orderStatus, isComment),
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(!result.Any());
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_036_ActionGetReceiptList_Test_ItemList_OriginItemOrderList_OriginItemOrderList_IsSanteiItem()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);

        string itemCd = "ItemCdUT";
        string santeiItemCd = "SanteiUT";
        string inputName = "InputNameUT";
        string itemName = "ItemNameUT";
        string rangeSeach = "=";
        int amount = 1;
        int orderStatus = 0;
        bool isComment = false;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment),
            new(itemCd, inputName, rangeSeach, amount, 1, isComment)
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = santeiItemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(!result.Any());
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_037_ActionGetReceiptList_Test_ItemList_OriginItemOrderList_OriginItemOrderList_IsFreeComment()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);

        string itemCd = "COCdUT";
        string santeiItemCd = "SanteiUT";
        string itemName = "ItemNameUT";
        string inputName = itemName;
        string rangeSeach = "=";
        int amount = 1;
        int orderStatus = 1;
        bool isComment = true;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment)
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(!result.Any());
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_038_ActionGetReceiptList_Test_ItemList_OriginItemOrderList_OriginItemSanteiList_Any()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);
        int count = random.Next(100, 999999999);
        int inoutKbn = random.Next(2, 9);

        string itemCd = "COCdUT";
        string santeiItemCd = "santeiCd";
        string inputName = "InputNameUT";
        string itemName = inputName;
        string rangeSeach = "=";
        int amount = count;
        int orderStatus = 0;
        bool isComment = false;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment)
        };

        SinKouiDetail sinKouiDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            ItemCd = santeiItemCd,
            SeqNo = seqNo,
            RpNo = rpNo,
            IsDeleted = 0,
            ItemName = itemName
        };

        SinKoui sinKoui = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            RpNo = rpNo,
            SeqNo = seqNo,
            HokenId = hokenId,
            InoutKbn = inoutKbn
        };

        SinKouiCount sinKouiCount = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeqNo = seqNo,
            RpNo = rpNo,
            Count = count
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0,
            StartDate = sinDate,
            EndDate = sinDate
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SinKouiDetails.Add(sinKouiDetail);
            tenant.SinKouis.Add(sinKoui);
            tenant.SinKouiCounts.Add(sinKouiCount);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SinKouiDetails.Remove(sinKouiDetail);
            tenant.SinKouis.Remove(sinKoui);
            tenant.SinKouiCounts.Remove(sinKouiCount);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_039_ActionGetReceiptList_Test_ItemList_OriginItemOrderList_OriginItemSanteiList_IsFreeComment()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);
        int count = random.Next(100, 999999999);
        int inoutKbn = random.Next(2, 9);

        string itemCd = ItemCdConst.CommentFree;
        string santeiItemCd = "santeiCd";
        string inputName = "InputNameUT";
        string itemName = inputName;
        string rangeSeach = "";
        int amount = count;
        int orderStatus = 0;
        bool isComment = true;
        List<ItemSearchModel> itemList = new()
        {
            new("", inputName, rangeSeach, amount, orderStatus, isComment)
        };

        SinKouiDetail sinKouiDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            ItemCd = itemCd,
            SeqNo = seqNo,
            RpNo = rpNo,
            IsDeleted = 0,
            ItemName = itemName
        };

        SinKoui sinKoui = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            RpNo = rpNo,
            SeqNo = seqNo,
            HokenId = hokenId,
            InoutKbn = inoutKbn
        };

        SinKouiCount sinKouiCount = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeqNo = seqNo,
            RpNo = rpNo,
            Count = count
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0,
            StartDate = sinDate,
            EndDate = sinDate
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SinKouiDetails.Add(sinKouiDetail);
            tenant.SinKouis.Add(sinKoui);
            tenant.SinKouiCounts.Add(sinKouiCount);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SinKouiDetails.Remove(sinKouiDetail);
            tenant.SinKouis.Remove(sinKoui);
            tenant.SinKouiCounts.Remove(sinKouiCount);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_040_ActionGetReceiptList_Test_ItemList_OriginItemOrderList_OriginItemSanteiList_ItemSanteiIsNull()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);
        int count = random.Next(100, 999999999);
        int inoutKbn = random.Next(2, 9);

        string itemCd = ItemCdConst.CommentFree;
        string santeiItemCd = "santeiCd";
        string inputName = "InputNameUT";
        string itemName = inputName;
        string rangeSeach = "";
        int amount = count;
        int orderStatus = 0;
        bool isComment = true;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment)
        };

        SinKouiDetail sinKouiDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            ItemCd = itemCd,
            SeqNo = seqNo,
            RpNo = rpNo,
            IsDeleted = 0,
            ItemName = itemName
        };

        SinKoui sinKoui = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            RpNo = rpNo,
            SeqNo = seqNo,
            HokenId = hokenId,
            InoutKbn = inoutKbn
        };

        SinKouiCount sinKouiCount = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeqNo = seqNo,
            RpNo = rpNo,
            Count = count
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            IsDeleted = 0,
            StartDate = sinDate,
            EndDate = sinDate
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SinKouiDetails.Add(sinKouiDetail);
            tenant.SinKouis.Add(sinKoui);
            tenant.SinKouiCounts.Add(sinKouiCount);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(!result.Any());
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SinKouiDetails.Remove(sinKouiDetail);
            tenant.SinKouis.Remove(sinKoui);
            tenant.SinKouiCounts.Remove(sinKouiCount);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_041_ActionGetReceiptList_Test_ItemList_OriginItemOrderList_OriginItemSanteiList_ItemQueryORAndItemSumListEmpty()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = 0;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);
        int count = random.Next(100, 999999999);
        int inoutKbn = random.Next(2, 9);

        string itemCd = ItemCdConst.CommentFree;
        string santeiItemCd = "santeiCd";
        string inputName = "InputNameUT";
        string itemName = inputName;
        string rangeSeach = "";
        int amount = count;
        int orderStatus = 0;
        bool isComment = true;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment)
        };

        SinKouiDetail sinKouiDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            ItemCd = itemCd,
            SeqNo = seqNo,
            RpNo = rpNo,
            IsDeleted = 0,
            ItemName = itemName
        };

        SinKoui sinKoui = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            RpNo = rpNo,
            SeqNo = seqNo,
            HokenId = hokenId,
            InoutKbn = inoutKbn
        };

        SinKouiCount sinKouiCount = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeqNo = seqNo,
            RpNo = rpNo,
            Count = count
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0,
            StartDate = sinDate,
            EndDate = sinDate
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SinKouiDetails.Add(sinKouiDetail);
            tenant.SinKouis.Add(sinKoui);
            tenant.SinKouiCounts.Add(sinKouiCount);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(!result.Any());
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SinKouiDetails.Remove(sinKouiDetail);
            tenant.SinKouis.Remove(sinKoui);
            tenant.SinKouiCounts.Remove(sinKouiCount);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_042_ActionGetReceiptList_Test_ItemList_RangeSeachGreater()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);
        int count = random.Next(100, 999999999);
        int inoutKbn = random.Next(2, 9);

        string itemCd = "COCdUT";
        string santeiItemCd = "santeiCd";
        string inputName = "InputNameUT";
        string itemName = inputName;
        string rangeSeach = ">";
        int amount = count - 1;
        int orderStatus = 0;
        bool isComment = false;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment)
        };

        SinKouiDetail sinKouiDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            ItemCd = santeiItemCd,
            SeqNo = seqNo,
            RpNo = rpNo,
            IsDeleted = 0,
            ItemName = itemName
        };

        SinKoui sinKoui = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            RpNo = rpNo,
            SeqNo = seqNo,
            HokenId = hokenId,
            InoutKbn = inoutKbn
        };

        SinKouiCount sinKouiCount = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeqNo = seqNo,
            RpNo = rpNo,
            Count = count
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0,
            StartDate = sinDate,
            EndDate = sinDate
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SinKouiDetails.Add(sinKouiDetail);
            tenant.SinKouis.Add(sinKoui);
            tenant.SinKouiCounts.Add(sinKouiCount);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SinKouiDetails.Remove(sinKouiDetail);
            tenant.SinKouis.Remove(sinKoui);
            tenant.SinKouiCounts.Remove(sinKouiCount);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_043_ActionGetReceiptList_Test_ItemList_RangeSeachLess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);
        int count = random.Next(100, 999999999);
        int inoutKbn = random.Next(2, 9);

        string itemCd = "COCdUT";
        string santeiItemCd = "santeiCd";
        string inputName = "InputNameUT";
        string itemName = inputName;
        string rangeSeach = "<";
        int amount = count + 1;
        int orderStatus = 0;
        bool isComment = false;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment)
        };

        SinKouiDetail sinKouiDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            ItemCd = santeiItemCd,
            SeqNo = seqNo,
            RpNo = rpNo,
            IsDeleted = 0,
            ItemName = itemName
        };

        SinKoui sinKoui = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            RpNo = rpNo,
            SeqNo = seqNo,
            HokenId = hokenId,
            InoutKbn = inoutKbn
        };

        SinKouiCount sinKouiCount = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeqNo = seqNo,
            RpNo = rpNo,
            Count = count
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0,
            StartDate = sinDate,
            EndDate = sinDate
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SinKouiDetails.Add(sinKouiDetail);
            tenant.SinKouis.Add(sinKoui);
            tenant.SinKouiCounts.Add(sinKouiCount);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SinKouiDetails.Remove(sinKouiDetail);
            tenant.SinKouis.Remove(sinKoui);
            tenant.SinKouiCounts.Remove(sinKouiCount);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_044_ActionGetReceiptList_Test_ItemList_RangeSeachLessOrEqual()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);
        int count = random.Next(100, 999999999);
        int inoutKbn = random.Next(2, 9);

        string itemCd = "COCdUT";
        string santeiItemCd = "santeiCd";
        string inputName = "InputNameUT";
        string itemName = inputName;
        string rangeSeach = "<=";
        int amount = count;
        int orderStatus = 0;
        bool isComment = false;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment)
        };

        SinKouiDetail sinKouiDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            ItemCd = santeiItemCd,
            SeqNo = seqNo,
            RpNo = rpNo,
            IsDeleted = 0,
            ItemName = itemName
        };

        SinKoui sinKoui = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            RpNo = rpNo,
            SeqNo = seqNo,
            HokenId = hokenId,
            InoutKbn = inoutKbn
        };

        SinKouiCount sinKouiCount = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeqNo = seqNo,
            RpNo = rpNo,
            Count = count
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0,
            StartDate = sinDate,
            EndDate = sinDate
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SinKouiDetails.Add(sinKouiDetail);
            tenant.SinKouis.Add(sinKoui);
            tenant.SinKouiCounts.Add(sinKouiCount);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SinKouiDetails.Remove(sinKouiDetail);
            tenant.SinKouis.Remove(sinKoui);
            tenant.SinKouiCounts.Remove(sinKouiCount);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_045_ActionGetReceiptList_Test_ItemList_RangeSeachGreaterOrEqual()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);
        int count = random.Next(100, 999999999);
        int inoutKbn = random.Next(2, 9);

        string itemCd = "COCdUT";
        string santeiItemCd = "santeiCd";
        string inputName = "InputNameUT";
        string itemName = inputName;
        string rangeSeach = ">=";
        int amount = count;
        int orderStatus = 0;
        bool isComment = false;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment)
        };

        SinKouiDetail sinKouiDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            ItemCd = santeiItemCd,
            SeqNo = seqNo,
            RpNo = rpNo,
            IsDeleted = 0,
            ItemName = itemName
        };

        SinKoui sinKoui = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            RpNo = rpNo,
            SeqNo = seqNo,
            HokenId = hokenId,
            InoutKbn = inoutKbn
        };

        SinKouiCount sinKouiCount = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeqNo = seqNo,
            RpNo = rpNo,
            Count = count
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0,
            StartDate = sinDate,
            EndDate = sinDate
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SinKouiDetails.Add(sinKouiDetail);
            tenant.SinKouis.Add(sinKoui);
            tenant.SinKouiCounts.Add(sinKouiCount);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SinKouiDetails.Remove(sinKouiDetail);
            tenant.SinKouis.Remove(sinKoui);
            tenant.SinKouiCounts.Remove(sinKouiCount);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_046_ActionGetReceiptList_Test_ItemList_ItemQueryIsOr()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = 0;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);
        int count = random.Next(100, 999999999);
        int inoutKbn = random.Next(2, 9);

        string itemCd = "COCdUT";
        string santeiItemCd = "santeiCd";
        string inputName = "InputNameUT";
        string itemName = inputName;
        string rangeSeach = ">=";
        int amount = count;
        int orderStatus = 0;
        bool isComment = false;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment)
        };

        SinKouiDetail sinKouiDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            ItemCd = santeiItemCd,
            SeqNo = seqNo,
            RpNo = rpNo,
            IsDeleted = 0,
            ItemName = itemName
        };

        SinKoui sinKoui = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            RpNo = rpNo,
            SeqNo = seqNo,
            HokenId = hokenId,
            InoutKbn = inoutKbn
        };

        SinKouiCount sinKouiCount = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeqNo = seqNo,
            RpNo = rpNo,
            Count = count
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0,
            StartDate = sinDate,
            EndDate = sinDate
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SinKouiDetails.Add(sinKouiDetail);
            tenant.SinKouis.Add(sinKoui);
            tenant.SinKouiCounts.Add(sinKouiCount);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SinKouiDetails.Remove(sinKouiDetail);
            tenant.SinKouis.Remove(sinKoui);
            tenant.SinKouiCounts.Remove(sinKouiCount);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_047_ActionGetReceiptList_Test_ItemList_ItemQueryIsAnd()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        List<SearchByoMstModel> byomeiCdList = new();
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int suryo = random.Next(100, 999999999);
        int count = random.Next(100, 999999999);
        int inoutKbn = random.Next(2, 9);

        string itemCd = "COCdUT";
        string santeiItemCd = "santeiCd";
        string inputName = "InputNameUT";
        string itemName = inputName;
        string rangeSeach = ">=";
        int amount = count;
        int orderStatus = 0;
        bool isComment = false;
        List<ItemSearchModel> itemList = new()
        {
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment),
            new(itemCd, inputName, rangeSeach, amount, orderStatus, isComment)
        };

        SinKouiDetail sinKouiDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            ItemCd = santeiItemCd,
            SeqNo = seqNo,
            RpNo = rpNo,
            IsDeleted = 0,
            ItemName = itemName
        };

        SinKoui sinKoui = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            RpNo = rpNo,
            SeqNo = seqNo,
            HokenId = hokenId,
            InoutKbn = inoutKbn
        };

        SinKouiCount sinKouiCount = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            SeqNo = seqNo,
            RpNo = rpNo,
            Count = count
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0,
            StartDate = sinDate,
            EndDate = sinDate
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.SinKouiDetails.Add(sinKouiDetail);
            tenant.SinKouis.Add(sinKoui);
            tenant.SinKouiCounts.Add(sinKouiCount);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                             && item.PtId == ptId
                                             && item.SinYm == sinYm
                                             && item.HokenId == hokenId
                                             && item.HokenKbn == hokenKbn
                                             && item.ReceSbt == receSbt
                                             && item.HokensyaNo == hokensyaNo
            ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.SinKouiDetails.Remove(sinKouiDetail);
            tenant.SinKouis.Remove(sinKoui);
            tenant.SinKouiCounts.Remove(sinKouiCount);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_048_ActionGetReceiptList_Test_ByomeiCdList_Any()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        string itemCd = "ItemCdUT";
        string santeiItemCd = string.Empty;
        string itemName = "ItemNameUT";
        string byomeiCd = "ByoCd";
        string inputName = "InputName";
        bool isComment = false;
        List<SearchByoMstModel> byomeiCdList = new()
        {
            new(byomeiCd, inputName, isComment)
        };

        PtByomei ptByomei = new()
        {
            HpId = hpId,
            PtId = ptId,
            ByomeiCd = byomeiCd,
            Byomei = inputName,
            StartDate = sinDate,
            TenkiDate = sinDate,
            HokenPid = hokenId,
            TenkiKbn = TenkiKbnConst.Continued
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.PtByomeis.Add(ptByomei);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                    ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.PtByomeis.Remove(ptByomei);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_049_ActionGetReceiptList_Test_ByomeiCdList_QuerySearchEnumAND()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = (QuerySearchEnum)1;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        string itemCd = "ItemCdUT";
        string santeiItemCd = string.Empty;
        string itemName = "ItemNameUT";
        string byomeiCd = "ByoCd";
        string inputName = "InputName";
        bool isComment = false;
        List<SearchByoMstModel> byomeiCdList = new()
        {
            new(byomeiCd, inputName, isComment),
            new(byomeiCd, inputName, isComment)
        };

        PtByomei ptByomei = new()
        {
            HpId = hpId,
            PtId = ptId,
            ByomeiCd = byomeiCd,
            Byomei = inputName,
            StartDate = sinDate,
            TenkiDate = sinDate,
            HokenPid = hokenId,
            TenkiKbn = TenkiKbnConst.Continued
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.PtByomeis.Add(ptByomei);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                    ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.PtByomeis.Remove(ptByomei);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_050_ActionGetReceiptList_Test_ByomeiCdList_QuerySearchEnumOR()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = false;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        string itemCd = "ItemCdUT";
        string santeiItemCd = string.Empty;
        string itemName = "ItemNameUT";
        string byomeiCd = "ByoCd";
        string inputName = "InputName";
        bool isComment = false;
        List<SearchByoMstModel> byomeiCdList = new()
        {
            new(byomeiCd, inputName, isComment),
            new(byomeiCd, inputName, isComment)
        };

        PtByomei ptByomei = new()
        {
            HpId = hpId,
            PtId = ptId,
            ByomeiCd = byomeiCd,
            Byomei = inputName,
            StartDate = sinDate,
            TenkiDate = sinDate,
            HokenPid = hokenId,
            TenkiKbn = TenkiKbnConst.Continued
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.PtByomeis.Add(ptByomei);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                    ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.PtByomeis.Remove(ptByomei);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_051_ActionGetReceiptList_Test_ByomeiCdList_IsOnlySuspectedDisease()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        string itemCd = "ItemCdUT";
        string santeiItemCd = string.Empty;
        string itemName = "ItemNameUT";
        string byomeiCd = "ByoCd";
        string inputName = "InputName";
        bool isComment = false;
        List<SearchByoMstModel> byomeiCdList = new()
        {
            new(byomeiCd, inputName, isComment)
        };

        PtByomei ptByomei = new()
        {
            HpId = hpId,
            PtId = ptId,
            ByomeiCd = byomeiCd,
            Byomei = inputName,
            StartDate = sinDate,
            TenkiDate = sinDate,
            HokenPid = hokenId,
            TenkiKbn = TenkiKbnConst.Continued,
            SyusyokuCd1 = ByomeiConstant.SuspectedCode
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.PtByomeis.Add(ptByomei);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                    ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.PtByomeis.Remove(ptByomei);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_052_ActionGetReceiptList_Test_FullParam()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int statusKbn = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string itemCd = "ItemCdUT";
        string santeiItemCd = string.Empty;
        string itemName = "ItemNameUT";
        string byomeiCd = "ByoCd";
        string inputName = "InputName";
        string message1 = "Message1";
        string message2 = "Message2";

        PtByomei ptByomei = new()
        {
            HpId = hpId,
            PtId = ptId,
            ByomeiCd = byomeiCd,
            Byomei = inputName,
            StartDate = sinDate,
            TenkiDate = sinDate,
            HokenPid = hokenId,
            TenkiKbn = TenkiKbnConst.Continued,
            SyusyokuCd1 = ByomeiConstant.SuspectedCode
        };

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
        };

        ReceStatus receStatus = new()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            StatusKbn = statusKbn,
            FusenKbn = fusenKbn,
            IsPaperRece = isPaperRece,
            Output = output
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        ReceInfEdit receInfEdit = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            IsDeleted = 0,
            SinYm = sinYm,
            HokenId = hokenId
        };

        ReceCheckCmt receCheckCmt = new()
        {
            HpId = hpId,
            PtId = ptId,
            IsChecked = 0,
            SinYm = sinYm,
            IsDeleted = 0,
            HokenId = hokenId
        };

        ReceCheckErr receCheckErr = new()
        {
            HpId = hpId,
            PtId = ptId,
            IsChecked = 0,
            SinYm = sinYm,
            HokenId = hokenId,
            Message1 = message1,
            Message2 = message2
        };

        ReceCmt receCmt = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SinYm = sinYm,
            IsDeleted = 0,
        };

        ReceSeikyu receSeikyu = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            SeikyuYm = seikyuYm,
            IsDeleted = 0,
            Cmt = "ReceSeikyuCmt"
        };

        SyoukiInf syoukiInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            IsDeleted = 0,
        };

        SyobyoKeika syobyoKeika = new()
        {
            HpId = hpId,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            Keika = "Keika",
            IsDeleted = 0,
        };

        KaikeiInf kaikeiInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SinDate = sinDate
        };

        PtKyusei ptKyusei = new()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            EndDate = sinDate,
            KanaName = "ptKyuseiKanaName",
            Name = "ptKyuseiName",
        };

        KaMst kaMst = new()
        {
            HpId = hpId,
            IsDeleted = 0,
            KaName = "KaName"
        };

        UserMst userMst = new()
        {
            HpId = hpId,
            UserId = tantoId,
            Name = "UserMstName",
            JobCd = 1,
            StartDate = 20220201,
            EndDate = 20220228
        };

        PtKohi ptKohi1 = new()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            HokenId = kohi1Id,
            FutansyaNo = random.Next(100, 99999999).ToString()
        };

        PtKohi ptKohi2 = new()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            HokenId = kohi2Id,
            FutansyaNo = random.Next(100, 99999999).ToString()
        };

        PtKohi ptKohi3 = new()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            HokenId = kohi3Id,
            FutansyaNo = random.Next(100, 99999999).ToString()
        };

        PtKohi ptKohi4 = new()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            HokenId = kohi4Id,
            FutansyaNo = random.Next(100, 99999999).ToString()
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.PtByomeis.Add(ptByomei);
            tenant.ReceInfEdits.Add(receInfEdit);
            tenant.ReceStatuses.Add(receStatus);
            tenant.ReceCheckCmts.Add(receCheckCmt);
            tenant.ReceCheckErrs.Add(receCheckErr);
            tenant.ReceCmts.Add(receCmt);
            tenant.ReceSeikyus.Add(receSeikyu);
            tenant.SyoukiInfs.Add(syoukiInf);
            tenant.SyobyoKeikas.Add(syobyoKeika);
            tenant.KaikeiInfs.Add(kaikeiInf);
            tenant.PtKyuseis.Add(ptKyusei);
            tenant.KaMsts.Add(kaMst);
            tenant.UserMsts.Add(userMst);
            tenant.PtKohis.Add(ptKohi1);
            tenant.PtKohis.Add(ptKohi2);
            tenant.PtKohis.Add(ptKohi3);
            tenant.PtKohis.Add(ptKohi4);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     && item.IsReceInfDetailExist == 1
                                                     && item.ReceCheckCmt == (receCheckCmt != null ? receCheckCmt.Cmt : (receCheckErr != null ? (receCheckErr?.Message1 ?? string.Empty) + (receCheckErr?.Message2 ?? string.Empty) : string.Empty))
                                                     && item.IsPending == (receCheckCmt != null ? receCheckCmt.IsPending : -1)
                                                     && item.IsPaperRece == (receStatus != null ? receStatus.IsPaperRece : 0)
                                                     && item.Output == (receStatus != null ? receStatus.Output : 0)
                                                     && item.FusenKbn == (receStatus != null ? receStatus.FusenKbn : 0)
                                                     && item.StatusKbn == (receStatus != null ? receStatus.StatusKbn : 0)
                                                     && item.IsReceCmtExist == (receCmt != null ? 1 : 0)
                                                     && item.ReceSeikyuCmt == (receSeikyu != null ? receSeikyu.Cmt ?? string.Empty : string.Empty)
                                                     && item.IsSyoukiInfExist == (syoukiInf != null ? 1 : 0)
                                                     && item.LastSinDateByHokenId == (kaikeiInf?.SinDate ?? 0)
                                                     && item.Name == (ptKyusei != null ? ptKyusei.Name : ptInf.Name)
                                                     && item.KanaName == (ptKyusei != null ? ptKyusei.KanaName : ptInf.KanaName)
                                                     && item.IsPtKyuseiExist == (ptKyusei != null ? 1 : 0)
                                                     && item.KaName == (kaMst != null ? kaMst.KaName : string.Empty)
                                                     && item.SName == (userMst?.Name ?? string.Empty)
                                                     && item.IsSyobyoKeikaExist == (syobyoKeika != null ? 1 : 0)
                                                     && item.FutansyaNoKohi1 == (ptKohi1 != null ? ptKohi1.FutansyaNo : string.Empty)
                                                     && item.FutansyaNoKohi2 == (ptKohi2 != null ? ptKohi2.FutansyaNo : string.Empty)
                                                     && item.FutansyaNoKohi3 == (ptKohi3 != null ? ptKohi3.FutansyaNo : string.Empty)
                                                     && item.FutansyaNoKohi4 == (ptKohi4 != null ? ptKohi4.FutansyaNo : string.Empty)
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.PtByomeis.Remove(ptByomei);
            tenant.ReceInfEdits.Remove(receInfEdit);
            tenant.ReceStatuses.Remove(receStatus);
            tenant.ReceCheckCmts.Remove(receCheckCmt);
            tenant.ReceCheckErrs.Remove(receCheckErr);
            tenant.ReceCmts.Remove(receCmt);
            tenant.ReceSeikyus.Remove(receSeikyu);
            tenant.SyoukiInfs.Remove(syoukiInf);
            tenant.SyobyoKeikas.Remove(syobyoKeika);
            tenant.KaikeiInfs.Remove(kaikeiInf);
            tenant.PtKyuseis.Remove(ptKyusei);
            tenant.KaMsts.Remove(kaMst);
            tenant.UserMsts.Remove(userMst);
            tenant.PtKohis.Remove(ptKohi1);
            tenant.PtKohis.Remove(ptKohi2);
            tenant.PtKohis.Remove(ptKohi3);
            tenant.PtKohis.Remove(ptKohi4);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_053_ActionGetReceiptList_Test_SeikyuKbnDenshi()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = random.Next(99, 999);
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int statusKbn = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string itemCd = "ItemCdUT";
        string santeiItemCd = string.Empty;
        string itemName = "ItemNameUT";
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = true;
        bool seikyuKbnPaper = false;

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        ReceInfEdit receInfEdit = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            IsDeleted = 0,
            SinYm = sinYm,
            HokenId = hokenId
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.ReceInfEdits.Add(receInfEdit);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.ReceInfEdits.Remove(receInfEdit);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_054_ActionGetReceiptList_Test_SeikyuKbnPaper()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int statusKbn = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string itemCd = "ItemCdUT";
        string santeiItemCd = string.Empty;
        string itemName = "ItemNameUT";
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = true;

        TenMst tenMst = new()
        {
            HpId = hpId,
            ItemCd = itemCd,
            SanteiItemCd = santeiItemCd,
            IsDeleted = 0
        };

        PtHokenPattern ptHokenPattern = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokenPid = hokenPId,
            IsDeleted = 0
        };

        OdrInf odrInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            HokenPid = hokenPId,
            IsDeleted = 0,
            RpEdaNo = rpEdaNo,
            RpNo = rpNo
        };

        OdrInfDetail odrInfDetail = new()
        {
            HpId = hpId,
            PtId = ptId,
            RaiinNo = raiinNo,
            RpNo = rpNo,
            RpEdaNo = rpEdaNo,
            RowNo = rowNo,
            SinDate = sinDate,
            ItemCd = itemCd,
            ItemName = itemName
        };

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        ReceInfEdit receInfEdit = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            IsDeleted = 0,
            SinYm = sinYm,
            HokenId = hokenId
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.PtHokenPatterns.Add(ptHokenPattern);
            tenant.TenMsts.Add(tenMst);
            tenant.OdrInfs.Add(odrInf);
            tenant.OdrInfDetails.Add(odrInfDetail);
            tenant.ReceInfEdits.Add(receInfEdit);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.PtHokenPatterns.Remove(ptHokenPattern);
            tenant.TenMsts.Remove(tenMst);
            tenant.OdrInfs.Remove(odrInf);
            tenant.OdrInfDetails.Remove(odrInfDetail);
            tenant.ReceInfEdits.Remove(receInfEdit);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_055_ActionGetReceiptList_Test_IsSystemSave()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string santeiItemCd = string.Empty;
        int statusKbn = (int)ReceCheckStatusEnum.SystemPending;

        bool isAll = false;
        bool isNoSetting = false;
        bool isSystemSave = true;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
        };

        ReceStatus receStatus = new()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            StatusKbn = statusKbn,
            FusenKbn = fusenKbn,
            IsPaperRece = isPaperRece,
            Output = output
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.ReceStatuses.Add(receStatus);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     && item.IsPaperRece == (receStatus != null ? receStatus.IsPaperRece : 0)
                                                     && item.Output == (receStatus != null ? receStatus.Output : 0)
                                                     && item.FusenKbn == (receStatus != null ? receStatus.FusenKbn : 0)
                                                     && item.StatusKbn == (receStatus != null ? receStatus.StatusKbn : 0)
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.ReceStatuses.Remove(receStatus);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_056_ActionGetReceiptList_Test_IsSave1()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string santeiItemCd = string.Empty;
        int statusKbn = (int)ReceCheckStatusEnum.Keep1;

        bool isAll = false;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = true;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
        };

        ReceStatus receStatus = new()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            StatusKbn = statusKbn,
            FusenKbn = fusenKbn,
            IsPaperRece = isPaperRece,
            Output = output
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.ReceStatuses.Add(receStatus);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     && item.IsPaperRece == (receStatus != null ? receStatus.IsPaperRece : 0)
                                                     && item.Output == (receStatus != null ? receStatus.Output : 0)
                                                     && item.FusenKbn == (receStatus != null ? receStatus.FusenKbn : 0)
                                                     && item.StatusKbn == (receStatus != null ? receStatus.StatusKbn : 0)
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.ReceStatuses.Remove(receStatus);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_057_ActionGetReceiptList_Test_IsSave2()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string santeiItemCd = string.Empty;
        int statusKbn = (int)ReceCheckStatusEnum.Keep2;

        bool isAll = false;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = true;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
        };

        ReceStatus receStatus = new()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            StatusKbn = statusKbn,
            FusenKbn = fusenKbn,
            IsPaperRece = isPaperRece,
            Output = output
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.ReceStatuses.Add(receStatus);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     && item.IsPaperRece == (receStatus != null ? receStatus.IsPaperRece : 0)
                                                     && item.Output == (receStatus != null ? receStatus.Output : 0)
                                                     && item.FusenKbn == (receStatus != null ? receStatus.FusenKbn : 0)
                                                     && item.StatusKbn == (receStatus != null ? receStatus.StatusKbn : 0)
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.ReceStatuses.Remove(receStatus);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_058_ActionGetReceiptList_Test_IsSave3()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string santeiItemCd = string.Empty;
        int statusKbn = (int)ReceCheckStatusEnum.Keep3;

        bool isAll = false;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = true;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
        };

        ReceStatus receStatus = new()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            StatusKbn = statusKbn,
            FusenKbn = fusenKbn,
            IsPaperRece = isPaperRece,
            Output = output
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.ReceStatuses.Add(receStatus);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     && item.IsPaperRece == (receStatus != null ? receStatus.IsPaperRece : 0)
                                                     && item.Output == (receStatus != null ? receStatus.Output : 0)
                                                     && item.FusenKbn == (receStatus != null ? receStatus.FusenKbn : 0)
                                                     && item.StatusKbn == (receStatus != null ? receStatus.StatusKbn : 0)
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.ReceStatuses.Remove(receStatus);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_059_ActionGetReceiptList_Test_IsTempSave()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string santeiItemCd = string.Empty;
        int statusKbn = (int)ReceCheckStatusEnum.TempComfirmed;

        bool isAll = false;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = true;
        bool isDone = false;
        bool isIncludeSingle = false;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
        };

        ReceStatus receStatus = new()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            StatusKbn = statusKbn,
            FusenKbn = fusenKbn,
            IsPaperRece = isPaperRece,
            Output = output
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.ReceStatuses.Add(receStatus);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     && item.IsPaperRece == (receStatus != null ? receStatus.IsPaperRece : 0)
                                                     && item.Output == (receStatus != null ? receStatus.Output : 0)
                                                     && item.FusenKbn == (receStatus != null ? receStatus.FusenKbn : 0)
                                                     && item.StatusKbn == (receStatus != null ? receStatus.StatusKbn : 0)
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.ReceStatuses.Remove(receStatus);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_060_ActionGetReceiptList_Test_IsDone()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string santeiItemCd = string.Empty;
        int statusKbn = (int)ReceCheckStatusEnum.Confirmed;

        bool isAll = false;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = true;
        bool isIncludeSingle = false;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
        };

        ReceStatus receStatus = new()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            StatusKbn = statusKbn,
            FusenKbn = fusenKbn,
            IsPaperRece = isPaperRece,
            Output = output
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.ReceStatuses.Add(receStatus);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     && item.IsPaperRece == (receStatus != null ? receStatus.IsPaperRece : 0)
                                                     && item.Output == (receStatus != null ? receStatus.Output : 0)
                                                     && item.FusenKbn == (receStatus != null ? receStatus.FusenKbn : 0)
                                                     && item.StatusKbn == (receStatus != null ? receStatus.StatusKbn : 0)
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.ReceStatuses.Remove(receStatus);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_061_ActionGetReceiptList_Test_IsNoSetting()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        long futansyaNoFromLong = -1;
        long futansyaNoToLong = -1;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string santeiItemCd = string.Empty;
        int statusKbn = (int)ReceCheckStatusEnum.UnConfirmed;

        bool isAll = false;
        bool isNoSetting = true;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                    ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.SaveChanges();
        }
    }
    #endregion ActionGetReceiptList

    #region SettingValue
    [Test]
    public void TC_062_GetSettingValue_TestSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int groupCd = random.Next(999, 999999);
        int grpEdaNo = random.Next(999, 999999);
        int val = random.Next(999, 999999);

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = groupCd,
            GrpEdaNo = grpEdaNo,
            Val = val
        };
        try
        {
            tenant.Add(systemConf);
            tenant.SaveChanges();

            // Act
            double result = receiptRepository.GetSettingValue(hpId, groupCd, grpEdaNo);

            // Assert
            Assert.IsTrue(result == val);
        }
        finally
        {
            tenant.SystemConfs.Remove(systemConf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_063_GetSettingParam_TestSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int groupCd = random.Next(999, 999999);
        int grpEdaNo = random.Next(999, 999999);
        string param = random.Next(999, 999999).ToString();

        SystemConf systemConf = new()
        {
            HpId = hpId,
            GrpCd = groupCd,
            GrpEdaNo = grpEdaNo,
            Param = param
        };
        try
        {
            tenant.Add(systemConf);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetSettingParam(hpId, groupCd, grpEdaNo);

            // Assert
            Assert.IsTrue(result == param);
        }
        finally
        {
            tenant.SystemConfs.Remove(systemConf);
            tenant.SaveChanges();
        }
    }
    #endregion SettingValue

    #region FilterAfterConvertModel
    [Test]
    public void TC_064_FilterAfterConvertModel_Test_FilterAfterConvertModel_FutansyaNoFromLongAndFutansyaNoToLong()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string santeiItemCd = string.Empty;
        int statusKbn = (int)ReceCheckStatusEnum.SystemPending;
        long futansyaNo = random.Next(100, 99999999);
        long futansyaNoFromLong = futansyaNo;
        long futansyaNoToLong = futansyaNo;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
            Kohi1ReceKisai = 1
        };

        ReceStatus receStatus = new()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            StatusKbn = statusKbn,
            FusenKbn = fusenKbn,
            IsPaperRece = isPaperRece,
            Output = output
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        PtKohi ptKohi1 = new()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            HokenId = kohi1Id,
            FutansyaNo = futansyaNo.ToString()
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.ReceStatuses.Add(receStatus);
            tenant.PtKohis.Add(ptKohi1);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     && item.IsPaperRece == (receStatus != null ? receStatus.IsPaperRece : 0)
                                                     && item.Output == (receStatus != null ? receStatus.Output : 0)
                                                     && item.FusenKbn == (receStatus != null ? receStatus.FusenKbn : 0)
                                                     && item.StatusKbn == (receStatus != null ? receStatus.StatusKbn : 0)
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.ReceStatuses.Remove(receStatus);
            tenant.PtKohis.Remove(ptKohi1);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_065_FilterAfterConvertModel_Test_FilterAfterConvertModel_FutansyaNoFromLong()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        long futansyaNoToLong = -1;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string santeiItemCd = string.Empty;
        int statusKbn = (int)ReceCheckStatusEnum.SystemPending;
        long futansyaNo = random.Next(100, 99999999);
        long futansyaNoFromLong = futansyaNo;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
            Kohi1ReceKisai = 1
        };

        ReceStatus receStatus = new()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            StatusKbn = statusKbn,
            FusenKbn = fusenKbn,
            IsPaperRece = isPaperRece,
            Output = output
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        PtKohi ptKohi1 = new()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            HokenId = kohi1Id,
            FutansyaNo = futansyaNo.ToString()
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.ReceStatuses.Add(receStatus);
            tenant.PtKohis.Add(ptKohi1);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     && item.IsPaperRece == (receStatus != null ? receStatus.IsPaperRece : 0)
                                                     && item.Output == (receStatus != null ? receStatus.Output : 0)
                                                     && item.FusenKbn == (receStatus != null ? receStatus.FusenKbn : 0)
                                                     && item.StatusKbn == (receStatus != null ? receStatus.StatusKbn : 0)
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.ReceStatuses.Remove(receStatus);
            tenant.PtKohis.Remove(ptKohi1);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_066_FilterAfterConvertModel_Test_FilterAfterConvertModel_FutansyaNoToLong()
    {
        // Arrange
        SetupTestEnvironment(out ReceiptRepository receiptRepository, out TenantNoTrackingDataContext tenant);

        Random random = new();
        int hpId = random.Next(999, 999999);
        int sinDate = 20220202;
        int seikyuYm = 202202;
        int sinYm = 202202;
        long ptId = random.Next(999, 999999999);
        long ptNum = random.Next(999, 999999999);
        int hokenId = random.Next(999, 999999);
        int seqNo = random.Next(999, 999999);
        bool isAdvanceSearch = true;
        string tokki = string.Empty;
        int hokenKbn = 14;
        List<int> hokenSbts = new();
        string hokenHoubetu = string.Empty;
        string hokensyaNoFrom = string.Empty;
        string hokensyaNoTo = string.Empty;
        PtIdSearchOptionEnum ptSearchOption = PtIdSearchOptionEnum.IndividualSearch;
        int kohi1Houbetu = 0;
        int kohi2Houbetu = 0;
        int kohi3Houbetu = 0;
        int kohi4Houbetu = 0;
        QuerySearchEnum itemQuery = (QuerySearchEnum)1;
        bool isOnlySuspectedDisease = true;
        QuerySearchEnum byomeiQuery = 0;
        bool isFutanIncludeSingle = false;
        int receSbtRight = -1;
        int receSbtCenter = -1;
        long hokensyaNoFromLong = -1;
        long hokensyaNoToLong = -1;
        int tensuFrom = -1;
        long tensuTo = -1;
        int lastRaiinDateFrom = -1;
        int lastRaiinDateTo = -1;
        long ptIdFrom = -1;
        int tantoId = 0;
        int doctorId = 0;
        string name = string.Empty;
        int kaId = 0;
        long ptIdTo = -1;
        int birthDayFrom = 0;
        int birthDayTo = 0;
        Dictionary<int, string> groupSearchModels = new();
        List<SearchByoMstModel> byomeiCdList = new();
        bool isTestPatientSearch = false;
        bool isNotDisplayPrinted = false;
        bool seikyuKbnAll = false;
        bool seikyuKbnDenshi = false;
        bool seikyuKbnPaper = false;
        string ptIdString = string.Empty;
        bool isAll = true;
        bool isNoSetting = false;
        bool isSystemSave = false;
        bool isSave1 = false;
        bool isSave2 = false;
        bool isSave3 = false;
        bool isTempSave = false;
        bool isDone = false;
        bool isIncludeSingle = false;
        long futansyaNoFromLong = -1;
        List<ItemSearchModel> itemList = new();
        int isTester = 0;
        string receSbt = random.Next(1000, 9999).ToString();
        string houbetu = random.Next(100, 999).ToString();
        string kohi1HoubetuInput = random.Next(100, 999).ToString();
        string kohi2HoubetuInput = random.Next(100, 999).ToString();
        string kohi3HoubetuInput = random.Next(100, 999).ToString();
        string kohi4HoubetuInput = random.Next(100, 999).ToString();
        int kohi4ReceKisai = 1;
        string hokensyaNo = random.Next(100, 99999999).ToString();
        int tensu = random.Next(100, 999);
        int raiinNo = random.Next(100, 999999999);
        int hokenPId = random.Next(100, 999999999);
        int rpNo = random.Next(100, 999999999);
        int rpEdaNo = random.Next(100, 999999999);
        int rowNo = random.Next(100, 999999999);
        int fusenKbn = random.Next(100, 999999999);
        int output = random.Next(100, 999999999);
        int isPaperRece = random.Next(100, 999999999);
        int kohi1Id = random.Next(100, 999999999);
        int kohi2Id = random.Next(100, 999999999);
        int kohi3Id = random.Next(100, 999999999);
        int kohi4Id = random.Next(100, 999999999);
        string santeiItemCd = string.Empty;
        int statusKbn = (int)ReceCheckStatusEnum.SystemPending;
        long futansyaNo = random.Next(100, 99999999);
        long futansyaNoToLong = futansyaNo;

        ReceInf receInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            SeikyuYm = seikyuYm,
            SinYm = sinYm,
            HokenId = hokenId,
            IsTester = isTester,
            HokenKbn = hokenKbn,
            ReceSbt = receSbt,
            Houbetu = houbetu,
            Kohi1Houbetu = kohi1HoubetuInput,
            Kohi2Houbetu = kohi2HoubetuInput,
            Kohi3Houbetu = kohi3HoubetuInput,
            Kohi4Houbetu = kohi4HoubetuInput,
            Kohi4ReceKisai = kohi4ReceKisai,
            HokensyaNo = hokensyaNo,
            Tensu = tensu,
            TantoId = tantoId,
            Kohi1Id = kohi1Id,
            Kohi2Id = kohi2Id,
            Kohi3Id = kohi3Id,
            Kohi4Id = kohi4Id,
            Kohi1ReceKisai = 1
        };

        ReceStatus receStatus = new()
        {
            HpId = hpId,
            SeikyuYm = seikyuYm,
            PtId = ptId,
            SinYm = sinYm,
            HokenId = hokenId,
            StatusKbn = statusKbn,
            FusenKbn = fusenKbn,
            IsPaperRece = isPaperRece,
            Output = output
        };

        RaiinInf raiinInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            Status = RaiinState.Calculate,
            RaiinNo = raiinNo,
            SinDate = sinDate,
            IsDeleted = 0,
            KaId = kaId,
            TantoId = tantoId
        };

        PtHokenInf ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SeqNo = seqNo,
            HokensyaNo = hokensyaNo,
        };

        PtInf ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            SeqNo = seqNo,
            Name = "ptInfName",
            KanaName = "ptInfKanaName"
        };

        PtKohi ptKohi1 = new()
        {
            HpId = hpId,
            PtId = ptId,
            IsDeleted = 0,
            HokenId = kohi1Id,
            FutansyaNo = futansyaNo.ToString()
        };

        try
        {
            tenant.ReceInfs.Add(receInf);
            tenant.PtInfs.Add(ptInf);
            tenant.PtHokenInfs.Add(ptHokenInf);
            tenant.RaiinInfs.Add(raiinInf);
            tenant.ReceStatuses.Add(receStatus);
            tenant.PtKohis.Add(ptKohi1);
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetReceiptList(hpId, seikyuYm, new ReceiptListAdvancedSearchInput(isAdvanceSearch, tokki, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptIdString, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper));

            // Assert
            Assert.IsTrue(result.Any(item => item.HpId == hpId
                                                     && item.PtId == ptId
                                                     && item.SinYm == sinYm
                                                     && item.HokenId == hokenId
                                                     && item.HokenKbn == hokenKbn
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     && item.IsPaperRece == (receStatus != null ? receStatus.IsPaperRece : 0)
                                                     && item.Output == (receStatus != null ? receStatus.Output : 0)
                                                     && item.FusenKbn == (receStatus != null ? receStatus.FusenKbn : 0)
                                                     && item.StatusKbn == (receStatus != null ? receStatus.StatusKbn : 0)
                                                     ));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RaiinInfs.Remove(raiinInf);
            tenant.ReceStatuses.Remove(receStatus);
            tenant.PtKohis.Remove(ptKohi1);
            tenant.SaveChanges();
        }
    }

    #endregion FilterAfterConvertModel

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
