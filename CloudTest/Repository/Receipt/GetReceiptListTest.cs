using Domain.CalculationInf;
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




    #endregion ActionGetReceiptList



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
