using Domain.CalculationInf;
using Domain.Models.MstItem;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using Entity.Tenant;
using Helper.Enum;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using PostgreDataContext;

namespace CloudUnitTest.Repository.Receipt;

public class GetReceiptListTest : BaseUT
{
    #region GetReceiptList
    [Test]
    public void TC_001_GetReceiptList_Test_seikyuYmEqual0()
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
    public void TC_002_GetReceiptList_Test_seikyuYmNotEqual0()
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
            Assert.IsTrue(result.Any(item => item.PtId == ptId && !item.IsPtTest));
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.ReceInfs.Remove(receInf);
            tenant.PtInfs.Remove(ptInf);
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
