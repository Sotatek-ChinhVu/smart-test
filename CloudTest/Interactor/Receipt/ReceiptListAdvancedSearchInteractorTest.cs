using Domain.Models.Receipt;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using Helper.Common;
using Helper.Enum;
using Interactor.Receipt;
using Moq;
using UseCase.Receipt.ReceiptListAdvancedSearch;

namespace CloudUnitTest.Interactor.Receipt;

public class ReceiptListAdvancedSearchInteractorTest : BaseUT
{
    [Test]
    public void TC_001_ConvertToInputAdvancedSearch_TestSuccess()
    {
        // Arrange
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
        int ptSearchOption = (int)PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = random.Next(999, 999999);
        long tensuTo = random.Next(999, 999999);
        int lastRaiinDateFrom = random.Next(999, 999999);
        int lastRaiinDateTo = random.Next(999, 999999);
        int birthDayFrom = random.Next(999, 999999);
        int birthDayTo = random.Next(999, 999999);
        string itemCd = "itemCdUT";
        string inputName = "inputNameUT";
        string rangeSeach = "=";
        int amount = random.Next(999, 999999);
        int orderStatus = random.Next(999, 999999);
        bool isComment = false;
        List<ItemSearchInputItem> itemList = new() { new ItemSearchInputItem(itemCd, inputName, rangeSeach, amount, orderStatus, isComment) };
        int itemQuery = 1;
        bool isOnlySuspectedDisease = false;
        int byomeiQuery = 1;
        string byomeiCd = "byomeiCd";
        List<SearchByoMstInputItem> byomeiCdList = new() { new SearchByoMstInputItem(byomeiCd, inputName, isComment) };
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

        var mockIReceiptRepository = new Mock<IReceiptRepository>();
        var interactor = new ReceiptListAdvancedSearchInteractor(mockIReceiptRepository.Object);

        // Act
        var inputData = new ReceiptListAdvancedSearchInputData(hpId, seikyuYm, tokki, isAdvanceSearch, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptId, ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper);
        var output = interactor.ConvertToInputAdvancedSearch(inputData);

        // Assert
        var success = output.IsAdvanceSearch == isAdvanceSearch
                      && output.Tokki == tokki
                      && output.HokenSbts == hokenSbts
                      && output.IsAll == isAll
                      && output.IsNoSetting == isNoSetting
                      && output.IsSystemSave == isSystemSave
                      && output.IsSave1 == isSave1
                      && output.IsSave2 == isSave2
                      && output.IsSave3 == isSave3
                      && output.IsTempSave == isTempSave
                      && output.IsDone == isDone
                      && output.ReceSbtCenter == receSbtCenter
                      && output.ReceSbtRight == receSbtRight
                      && output.HokenHoubetu == hokenHoubetu
                      && output.Kohi1Houbetu == kohi1Houbetu
                      && output.Kohi2Houbetu == kohi2Houbetu
                      && output.Kohi3Houbetu == kohi3Houbetu
                      && output.Kohi4Houbetu == kohi4Houbetu
                      && output.IsIncludeSingle == isIncludeSingle
                      && output.HokensyaNoFrom == hokensyaNoFrom
                      && output.HokensyaNoTo == hokensyaNoTo
                      && output.HokensyaNoFromLong == hokensyaNoFromLong
                      && output.HokensyaNoToLong == hokensyaNoToLong
                      && output.PtId == ptId
                      && output.PtIdFrom == ptIdFrom
                      && output.PtIdTo == ptIdTo
                      && (int)output.PtSearchOption == ptSearchOption
                      && output.TensuFrom == tensuFrom
                      && output.TensuTo == tensuTo
                      && output.LastRaiinDateFrom == lastRaiinDateFrom
                      && output.LastRaiinDateTo == lastRaiinDateTo
                      && output.BirthDayFrom == birthDayFrom
                      && output.BirthDayTo == birthDayTo
                      && output.ItemList.Any(output => itemList.Any(item => item.ItemCd == output.ItemCd
                                                                            && item.IsComment == output.IsComment
                                                                            && item.InputName == output.InputName
                                                                            && item.RangeSeach == output.RangeSeach
                                                                            && item.Amount == output.Amount
                                                                            && item.OrderStatus == output.OrderStatus))
                      && (int)output.ItemQuery == itemQuery
                      && output.IsOnlySuspectedDisease == isOnlySuspectedDisease
                      && (int)output.ByomeiQuery == byomeiQuery
                      && output.ByomeiCdList.Any(output => byomeiCdList.Any(item => item.IsComment == output.IsComment
                                                                                    && item.ByomeiCd == byomeiCd
                                                                                    && item.InputName == inputName))
                      && output.IsFutanIncludeSingle == isFutanIncludeSingle
                      && output.FutansyaNoFromLong == futansyaNoFromLong
                      && output.FutansyaNoToLong == futansyaNoToLong
                      && output.KaId == kaId
                      && output.DoctorId == doctorId
                      && output.Name == name
                      && output.IsTestPatientSearch == isTestPatientSearch
                      && output.IsNotDisplayPrinted == isNotDisplayPrinted
                      && output.GroupSearchModels == groupSearchModels
                      && output.SeikyuKbnAll == seikyuKbnAll
                      && output.SeikyuKbnDenshi == seikyuKbnDenshi
                      && output.SeikyuKbnPaper == seikyuKbnPaper;
        Assert.True(success);
    }

    [Test]
    public void TC_002_Handle_TestSuccess()
    {
        // Arrange
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
        long ptIdFrom = random.Next(999, 999999);
        long ptIdTo = random.Next(999, 999999);
        int ptSearchOption = (int)PtIdSearchOptionEnum.RangeSearch;
        long tensuFrom = random.Next(999, 999999);
        long tensuTo = random.Next(999, 999999);
        int lastRaiinDateFrom = random.Next(999, 999999);
        int lastRaiinDateTo = random.Next(999, 999999);
        int birthDayFrom = random.Next(999, 999999);
        int birthDayTo = random.Next(999, 999999);
        string itemCd = "itemCdUT";
        string inputName = "inputNameUT";
        string rangeSeach = "=";
        int amount = random.Next(999, 999999);
        int orderStatus = random.Next(999, 999999);
        bool isComment = false;
        List<ItemSearchInputItem> itemList = new() { new ItemSearchInputItem(itemCd, inputName, rangeSeach, amount, orderStatus, isComment) };
        int itemQuery = 1;
        bool isOnlySuspectedDisease = false;
        int byomeiQuery = 1;
        string byomeiCd = "byomeiCd";
        List<SearchByoMstInputItem> byomeiCdList = new() { new SearchByoMstInputItem(byomeiCd, inputName, isComment) };
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

        int seikyuKbn = random.Next(999, 999999);
        int sinYm = random.Next(111111, 999999);
        int isReceInfDetailExist = random.Next(999, 999999);
        int isPaperRece = random.Next(999, 999999);
        int hokenId = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        int output = random.Next(999, 999999);
        int fusenKbn = random.Next(999, 999999);
        int statusKbn = random.Next(999, 999999);
        int isPending = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        string kanaName = "KanaName";
        int sex = 1;
        int lastSinDateByHokenId = random.Next(999, 999999);
        int birthDay = random.Next(10000000, 99999999);
        string receSbt = "receSbt";
        string hokensyaNo = "hokenSyano";
        int tensu = random.Next(999, 999999);
        int hokenSbtCd = random.Next(999, 999999);
        int kohi1Nissu = random.Next(999, 999999);
        int isSyoukiInfExist = random.Next(999, 999999);
        int isReceCmtExist = random.Next(999, 999999);
        int isSyobyoKeikaExist = random.Next(999, 999999);
        string receSeikyuCmt = "receSeikyuCmt";
        int lastVisitDate = random.Next(999, 999999);
        string kaName = "kaName";
        string sName = "sName";
        int isPtKyuseiExist = random.Next(999, 999999);
        string futansyaNoKohi1 = "futansyaNoKohi1";
        string futansyaNoKohi2 = "futansyaNoKohi2";
        string futansyaNoKohi3 = "futansyaNoKohi3";
        string futansyaNoKohi4 = "futansyaNoKohi4";
        bool isPtTest = false;
        int kohi1ReceKisai = random.Next(999, 999999);
        int kohi2ReceKisai = random.Next(999, 999999);
        int kohi3ReceKisai = random.Next(999, 999999);
        int kohi4ReceKisai = random.Next(999, 999999);
        int hokenNissu = random.Next(999, 999999);
        string receCheckCmt = "receCheckCmt";

        // Act
        List<ReceiptListModel> outputList = new()
        {
            new ReceiptListModel(seikyuKbn, sinYm, isReceInfDetailExist, isPaperRece, hokenId, hokenKbn, output, fusenKbn, statusKbn, isPending, ptId, ptNum, kanaName, name, sex, lastSinDateByHokenId, birthDay, receSbt, hokensyaNo, tensu, hokenSbtCd, kohi1Nissu, isSyoukiInfExist, isReceCmtExist, isSyobyoKeikaExist, receSeikyuCmt, lastVisitDate, kaName, sName, isPtKyuseiExist, futansyaNoKohi1, futansyaNoKohi2, futansyaNoKohi3, futansyaNoKohi4, isPtTest, kohi1ReceKisai, kohi2ReceKisai, kohi3ReceKisai, kohi4ReceKisai, tokki, hokenNissu, receCheckCmt)
        };
        var mockIReceiptRepository = new Mock<IReceiptRepository>();
        mockIReceiptRepository.Setup(repo => repo.GetReceiptList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ReceiptListAdvancedSearchInput>()))
                              .Returns(outputList);
        var interactor = new ReceiptListAdvancedSearchInteractor(mockIReceiptRepository.Object);
        var inputData = new ReceiptListAdvancedSearchInputData(hpId, seikyuYm, tokki, isAdvanceSearch, hokenSbts, isAll, isNoSetting, isSystemSave, isSave1, isSave2, isSave3, isTempSave, isDone, receSbtCenter, receSbtRight, hokenHoubetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, isIncludeSingle, hokensyaNoFrom, hokensyaNoTo, hokensyaNoFromLong, hokensyaNoToLong, ptId.ToString(), ptIdFrom, ptIdTo, ptSearchOption, tensuFrom, tensuTo, lastRaiinDateFrom, lastRaiinDateTo, birthDayFrom, birthDayTo, itemList, itemQuery, isOnlySuspectedDisease, byomeiQuery, byomeiCdList, isFutanIncludeSingle, futansyaNoFromLong, futansyaNoToLong, kaId, doctorId, name, isTestPatientSearch, isNotDisplayPrinted, groupSearchModels, seikyuKbnAll, seikyuKbnDenshi, seikyuKbnPaper);

        var result = interactor.Handle(inputData);

        // Assert
        var success = result.ReceiptList.Any(item => item.SeikyuKbn == seikyuKbn
                                                     && item.SinYm == sinYm
                                                     && item.IsReceInfDetailExist == isReceInfDetailExist
                                                     && item.IsPaperRece == isPaperRece
                                                     && item.HokenKbn == hokenKbn
                                                     && item.HokenId == hokenId
                                                     && item.Output == output
                                                     && item.FusenKbn == fusenKbn
                                                     && item.StatusKbn == statusKbn
                                                     && item.IsPending == isPending
                                                     && item.PtNum == ptNum
                                                     && item.PtId == ptId
                                                     && item.KanaName == kanaName
                                                     && item.Name == name
                                                     && item.Sex == sex
                                                     && item.Age == CIUtil.SDateToAge(birthDay, lastSinDateByHokenId)
                                                     && item.LastSinDateByHokenId == lastSinDateByHokenId
                                                     && item.BirthDay == birthDay
                                                     && item.ReceSbt == receSbt
                                                     && item.HokensyaNo == hokensyaNo
                                                     && item.Tensu == tensu
                                                     && item.HokenSbtCd == hokenSbtCd
                                                     && item.Kohi1Nissu == kohi1Nissu
                                                     && item.IsSyoukiInfExist == isSyoukiInfExist
                                                     && item.IsReceCmtExist == isReceCmtExist
                                                     && item.IsSyobyoKeikaExist == isSyobyoKeikaExist
                                                     && item.ReceSeikyuCmt == receSeikyuCmt
                                                     && item.LastVisitDate == lastVisitDate
                                                     && item.KaName == kaName
                                                     && item.SName == sName
                                                     && item.IsPtKyuseiExist == isPtKyuseiExist
                                                     && item.FutansyaNoKohi1 == futansyaNoKohi1
                                                     && item.FutansyaNoKohi2 == futansyaNoKohi2
                                                     && item.FutansyaNoKohi3 == futansyaNoKohi3
                                                     && item.FutansyaNoKohi4 == futansyaNoKohi4
                                                     && item.IsPtTest == isPtTest
                                                     && item.HokenNissu == hokenNissu
                                                     && item.ReceCheckCmt == receCheckCmt);
        Assert.True(success);
    }

}
