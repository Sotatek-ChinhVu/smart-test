using Domain.Models.AuditLog;
using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.Ka;
using Domain.Models.KarteInfs;
using Domain.Models.Medical;
using Domain.Models.MstItem;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.SystemConf;
using Domain.Models.SystemGenerationConf;
using Domain.Models.TodayOdr;
using Domain.Models.User;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Interactor.CalculateService;
using Interactor.Family.ValidateFamilyList;
using Interactor.MedicalExamination.KensaIraiCommon;
using Interactor.MedicalExamination;
using Microsoft.Extensions.Options;
using Moq;
using UseCase.MedicalExamination.UpsertTodayOrd;
using Domain.Models.OrdInf;
using Tuple = System.Tuple;
using Infrastructure.Repositories;
using Entity.Tenant;
using Helper.Redis;
using StackExchange.Redis;
using Helper.Constants;
using Microsoft.Extensions.Configuration;

namespace CloudUnitTest.Interactor.SaveMedical;

public class ConvertInputDataToOrderInfTest : BaseUT
{
    private readonly string baseAccessUrl = "BaseAccessUrl";
    private readonly IDatabase _cache;

    public ConvertInputDataToOrderInfTest()
    {
        string connection = string.Concat("10.2.15.78", ":", "6379");
        if (RedisConnectorHelper.RedisHost != connection)
        {
            RedisConnectorHelper.RedisHost = connection;
        }
        _cache = RedisConnectorHelper.Connection.GetDatabase();
    }

    #region ConvertInputDataToOrderInfs
    [Test]
    public void TC_001_ConvertInputDataToOrderInf_TestSaveSuccessIsTrue()
    {
        // Arrange
        var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockIReceptionRepository = new Mock<IReceptionRepository>();
        var mockIKaRepository = new Mock<IKaRepository>();
        var mockIMstItemRepository = new Mock<IMstItemRepository>();
        var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
        var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
        var mockIUserRepository = new Mock<IUserRepository>();
        var mockIHpInfRepository = new Mock<IHpInfRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
        var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
        var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
        var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
        var mockICalculateService = new Mock<ICalculateService>();
        var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
        var mockITenantProvider = new Mock<ITenantProvider>();
        var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
        var mockISystemConfRepository = new Mock<ISystemConfRepository>();
        var mockIAuditLogRepository = new Mock<IAuditLogRepository>();
        AmazonS3Options appSettings = new AmazonS3Options() { BaseAccessUrl = baseAccessUrl };
        IOptions<AmazonS3Options> options = Options.Create(appSettings);

        Random random = new();
        int hpId = random.Next(999, 99999);
        long raiinNo = random.Next(99999, 999999999);
        long rpNo = random.Next(99999, 99999999);
        long rpEdaNo = random.Next(99999, 99999999);
        long ptId = random.Next(99999, 999999999);
        int sinDate = 20220202;
        int hokenPid = random.Next(999, 99999);
        int odrKouiKbn = random.Next(999, 99999);
        string rpName = "rpName";
        int inoutKbn = random.Next(999, 99999);
        int sikyuKbn = random.Next(999, 99999);
        int syohoSbt = random.Next(999, 99999);
        int santeiKbn = random.Next(999, 99999);
        int tosekiKbn = random.Next(999, 99999);
        int daysCnt = random.Next(999, 99999);
        int sortNo = random.Next(999, 99999);
        int id = random.Next(999, 99999);
        int sinKouiKbn = random.Next(999, 99999);
        int rowNo = random.Next(999, 99999);
        string itemCd = "itemCd";
        string itemName = "itemName";
        double suryo = random.Next(999, 99999);
        string unitName = "unitName";
        int unitSbt = random.Next(999, 99999);
        double termVal = random.Next(999, 99999);
        int kohatuKbn = random.Next(999, 99999);
        int syohoKbn = random.Next(999, 99999);
        int syohoLimitKbn = random.Next(999, 99999);
        int drugKbn = random.Next(999, 99999);
        int yohoKbn = random.Next(999, 99999);
        string kokuji1 = "kokuji1";
        string kokuji2 = "kokuji2";
        int isNodspRece = random.Next(999, 99999);
        string ipnCd = "ipnCd";
        string ipnName = "ipnName";
        int jissiKbn = random.Next(999, 99999);
        DateTime jissiDate = DateTime.MinValue;
        int jissiId = random.Next(999, 99999);
        string jissiMachine = "jissiMachine";
        string reqCd = "reqCd";
        string bunkatu = "bunkatu";
        string cmtName = "cmtName";
        string cmtOpt = "cmtOpt";
        string fontColor = "fontColor";
        int commentNewline = random.Next(999, 99999);
        string ipnNameCd = "ipnNameCd";
        int startDate = 20220202;
        int endDate = 20220203;
        double yakka = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int refillSetting = random.Next(999, 99999);
        string masterSbt = "masterSbt";
        int cmtCol1 = random.Next(999, 99999);
        int ten = random.Next(999, 99999);
        OdrInfDetailItemInputData odrInfDetailItemInputData = new OdrInfDetailItemInputData(hpId, raiinNo, rpNo, rpEdaNo, rowNo, ptId, sinDate, sinKouiKbn, itemCd, itemName, suryo, unitName, unitSbt, termVal, kohatuKbn, syohoKbn, syohoLimitKbn, drugKbn, yohoKbn, kokuji1, kokuji2, isNodspRece, ipnCd, ipnName, jissiKbn, jissiDate, jissiId, jissiMachine, reqCd, bunkatu, cmtName, cmtOpt, fontColor, commentNewline);
        List<OdrInfDetailItemInputData> odrDetails = new() { odrInfDetailItemInputData };
        int isDeleted = 0;
        OdrInfItemInputData odrInfItemInputData = new OdrInfItemInputData(hpId, raiinNo, rpNo, rpEdaNo, ptId, sinDate, hokenPid, odrKouiKbn, rpName, inoutKbn, sikyuKbn, syohoSbt, santeiKbn, tosekiKbn, daysCnt, sortNo, id, odrDetails, isDeleted);

        var saveMedicalInteractor = new SaveMedicalInteractor(options, mockIAmazonS3Service.Object, mockITenantProvider.Object, mockIOrdInfRepository.Object, mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object, mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object, mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object, mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object, mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

        // Mock data
        List<TenItemModel> tenMstList = new() { new TenItemModel(hpId, itemCd, ipnNameCd, masterSbt, cmtCol1, ten) };
        mockIMstItemRepository.Setup(finder => finder.GetCheckTenItemModels(hpId, sinDate, new() { itemCd }))
        .Returns((int hpId, int sinDate, List<string> itemCds) => tenMstList);

        var ipnMinYakaMsts = new List<IpnMinYakkaMstModel> { new(random.Next(999, 99999), hpId, ipnNameCd, startDate, endDate, yakka, seqNo, 0, false) };
        mockIOrdInfRepository.Setup(finder => finder.GetCheckIpnMinYakkaMsts(hpId, sinDate, new() { ipnCd }))
        .Returns((int hpId, int sinDate, List<string> ipnNameCds) => ipnMinYakaMsts);

        mockIOrdInfRepository.Setup(finder => finder.CheckIsGetYakkaPrices(hpId, tenMstList, sinDate))
        .Returns((int hpId, List<TenItemModel> tenMsts, int sinDate) => new List<Tuple<string, string, bool>>() { Tuple.Create(ipnNameCd, itemCd, false && false) });

        mockISystemGenerationConfRepository.Setup(finder => finder.GetSettingValue(hpId, 2002, 0, sinDate, 999, "", false))
        .Returns((int hpId, int groupCd, int grpEdaNo, int presentDate, int defaultValue, string defaultParam, bool fromLastestDb) => (refillSetting, defaultParam));

        var ipnNameMsts = new List<Tuple<string, string>>() { Tuple.Create(ipnNameCd, ipnName) };
        mockIOrdInfRepository.Setup(finder => finder.GetIpnMst(hpId, 20220202, 20220202, new() { ipnCd }))
        .Returns((int hpId, int sinDateMin, int sinDateMax, List<string> ipnCds) => ipnNameMsts);

        // Act
        var result = saveMedicalInteractor.ConvertInputDataToOrderInfs(hpId, sinDate, new() { odrInfItemInputData });
        var ordInf = result.FirstOrDefault(item => item.HpId == hpId
                            && item.RaiinNo == raiinNo
                            && item.RpNo == rpNo
                            && item.RpEdaNo == rpEdaNo
                            && item.PtId == ptId
                            && item.SinDate == sinDate
                            && item.HokenPid == hokenPid
                            && item.OdrKouiKbn == odrKouiKbn
                            && item.RpName == rpName
                            && item.InoutKbn == inoutKbn
                            && item.SikyuKbn == sikyuKbn
                            && item.SyohoSbt == syohoSbt
                            && item.SanteiKbn == santeiKbn
                            && item.TosekiKbn == tosekiKbn
                            && item.DaysCnt == daysCnt
                            && item.SortNo == sortNo
                            && item.IsDeleted == isDeleted
                            && item.Id == id);
        if (ordInf == null)
        {
            Assert.That(false);
        }

        var inputItem = tenMstList?.FirstOrDefault(t => t.ItemCd == itemCd);
        var ipnMinYakaMst = (inputItem == null || (inputItem.HpId == 0 && string.IsNullOrEmpty(inputItem.ItemCd))) ? null : ipnMinYakaMsts.FirstOrDefault(i => i.IpnNameCd == ipnCd);
        var ordInfDetail = ordInf!.OrdInfDetails.FirstOrDefault(item => item.HpId == hpId
                                                 && item.RaiinNo == raiinNo
                                                 && item.RpNo == rpNo
                                                 && item.RpEdaNo == rpEdaNo
                                                 && item.RowNo == rowNo
                                                 && item.PtId == ptId
                                                 && item.SinDate == sinDate
                                                 && item.SinKouiKbn == sinKouiKbn
                                                 && item.ItemCd == itemCd
                                                 && item.ItemName == itemName
                                                 && item.Suryo == suryo
                                                 && item.UnitName == unitName
                                                 && item.UnitSbt == unitSbt
                                                 && item.TermVal == termVal
                                                 && item.KohatuKbn == kohatuKbn
                                                 && item.SyohoKbn == syohoKbn
                                                 && item.SyohoLimitKbn == syohoLimitKbn
                                                 && item.DrugKbn == drugKbn
                                                 && item.YohoKbn == yohoKbn
                                                 && item.Kokuji1 == kokuji1
                                                 && item.Kokuji2 == kokuji2
                                                 && item.IsNodspRece == isNodspRece
                                                 && item.IpnCd == ipnCd
                                                 && item.IpnName == (ipnNameMsts.FirstOrDefault(i => i.Item1 == item.IpnCd)?.Item2 ?? string.Empty)
                                                 && item.JissiKbn == jissiKbn
                                                 && item.JissiDate == jissiDate
                                                 && item.JissiId == jissiId
                                                 && item.JissiMachine == jissiMachine
                                                 && item.ReqCd == reqCd
                                                 && item.Bunkatu == bunkatu
                                                 && item.CmtName == cmtName
                                                 && item.CmtOpt == cmtOpt
                                                 && item.FontColor == fontColor
                                                 && item.CommentNewline == commentNewline
                                                 && item.InOutKbn == ordInf.InoutKbn
                                                 && item.Yakka == (ipnMinYakaMst?.Yakka ?? 0)
                                                 && item.IsGetPriceInYakka == false
                                                 && item.RefillSetting == refillSetting
                                                 && item.MasterSbt == inputItem?.MasterSbt
                                                 && item.CmtCol1 == inputItem?.CmtCol1
                                                 && item.Ten == inputItem?.Ten);


        // Assert
        Assert.That(ordInfDetail != null);
    }
    #endregion ConvertInputDataToOrderInfs

    #region GetCheckTenItemModels
    [Test]
    public void TC_002_GetCheckTenItemModels_TestSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        string itemCd = "itemCd";
        int rousaiKbn = random.Next(999, 99999);
        string kanaName1 = "KanaName1";
        string name = "Name";
        int kohatuKbn = random.Next(999, 99999);
        int madokuKbn = random.Next(999, 99999);
        int kouseisinKbn = random.Next(999, 99999);
        string odrUnitName = "OdrUnitName";
        int endDate = 99999999;
        int drugKbn = random.Next(999, 99999);
        string masterSbt = "M";
        int buiKbn = random.Next(999, 99999);
        int isAdopted = random.Next(999, 99999);
        int ten = random.Next(999, 99999);
        int tenId = random.Next(999, 99999);
        int cmtCol1 = random.Next(999, 99999);
        string ipnNameCd = "IpnNameCd";
        int sinKouiKbn = random.Next(999, 99999);
        string yjCd = "YjCd";
        string cnvUnitName = "CnvUnitName";
        int startDate = 11111111;
        int yohoKbn = random.Next(999, 99999);
        int cmtColKeta1 = random.Next(999, 99999);
        int cmtColKeta2 = random.Next(999, 99999);
        int cmtColKeta3 = random.Next(999, 99999);
        int cmtColKeta4 = random.Next(999, 99999);
        int cmtCol2 = random.Next(999, 99999);
        int cmtCol3 = random.Next(999, 99999);
        int cmtCol4 = random.Next(999, 99999);
        string minAge = "Mi";
        string maxAge = "Ma";
        string santeiItemCd = "SanCd";
        int odrTermVal = random.Next(999, 99999);
        int cnvTermVal = random.Next(999, 99999);
        int defaultVal = random.Next(999, 99999);
        string kokuji1 = "1";
        string kokuji2 = "2";

        TenMst tenMst = new TenMst()
        {
            HpId = hpId,
            ItemCd = itemCd,
            RousaiKbn = rousaiKbn,
            KanaName1 = kanaName1,
            Name = name,
            KohatuKbn = kohatuKbn,
            MadokuKbn = madokuKbn,
            KouseisinKbn = kouseisinKbn,
            OdrUnitName = odrUnitName,
            EndDate = endDate,
            DrugKbn = drugKbn,
            MasterSbt = masterSbt,
            BuiKbn = buiKbn,
            IsAdopted = isAdopted,
            Ten = ten,
            TenId = tenId,
            CmtCol1 = cmtCol1,
            IpnNameCd = ipnNameCd,
            SinKouiKbn = sinKouiKbn,
            YjCd = yjCd,
            CnvUnitName = cnvUnitName,
            StartDate = startDate,
            YohoKbn = yohoKbn,
            CmtColKeta1 = cmtColKeta1,
            CmtColKeta2 = cmtColKeta2,
            CmtColKeta3 = cmtColKeta3,
            CmtColKeta4 = cmtColKeta4,
            CmtCol2 = cmtCol2,
            CmtCol3 = cmtCol3,
            CmtCol4 = cmtCol4,
            MinAge = minAge,
            MaxAge = maxAge,
            SanteiItemCd = santeiItemCd,
            OdrTermVal = odrTermVal,
            CnvTermVal = cnvTermVal,
            DefaultVal = defaultVal,
            Kokuji1 = kokuji1,
            Kokuji2 = kokuji2,
        };

        tenant.Add(tenMst);
        List<string> itemCdList = new() { itemCd };
        var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
        MstItemRepository mstItemRepository = new MstItemRepository(TenantProvider, mockAmazonS3Options.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = mstItemRepository.GetCheckTenItemModels(hpId, sinDate, itemCdList);

            var success = result.Any(item => item.HpId == hpId
                                             && item.ItemCd == itemCd
                                             && item.RousaiKbn == rousaiKbn
                                             && item.KanaName1 == kanaName1
                                             && item.Name == name
                                             && item.KohatuKbn == kohatuKbn
                                             && item.MadokuKbn == madokuKbn
                                             && item.KouseisinKbn == kouseisinKbn
                                             && item.OdrUnitName == odrUnitName
                                             && item.EndDate == endDate
                                             && item.DrugKbn == drugKbn
                                             && item.MasterSbt == masterSbt
                                             && item.BuiKbn == buiKbn
                                             && item.IsAdopted == isAdopted
                                             && item.Ten == ten
                                             && item.TenId == tenId
                                             && item.CmtCol1 == cmtCol1
                                             && item.IpnNameCd == ipnNameCd
                                             && item.SinKouiKbn == sinKouiKbn
                                             && item.YjCd == yjCd
                                             && item.CnvUnitName == cnvUnitName
                                             && item.StartDate == startDate
                                             && item.YohoKbn == yohoKbn
                                             && item.CmtColKeta1 == cmtColKeta1
                                             && item.CmtColKeta2 == cmtColKeta2
                                             && item.CmtColKeta3 == cmtColKeta3
                                             && item.CmtColKeta4 == cmtColKeta4
                                             && item.CmtCol2 == cmtCol2
                                             && item.CmtCol3 == cmtCol3
                                             && item.CmtCol4 == cmtCol4
                                             && item.MinAge == minAge
                                             && item.MaxAge == maxAge
                                             && item.SanteiItemCd == santeiItemCd
                                             && item.OdrTermVal == odrTermVal
                                             && item.CnvTermVal == cnvTermVal
                                             && item.DefaultValue == defaultVal
                                             && item.Kokuji1 == kokuji1
                                             && item.Kokuji2 == kokuji2
                                             );

            // Assert
            Assert.True(success);
        }
        finally
        {
            mstItemRepository.ReleaseResource();
            tenant.TenMsts.Remove(tenMst);
            tenant.SaveChanges();
        }
    }
    #endregion GetCheckTenItemModels

    #region GetCheckIpnMinYakkaMsts
    [Test]
    public void TC_003_GetCheckIpnMinYakkaMsts_TestSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        int id = random.Next(999, 99999);
        string ipnNameCd = "IpnNameCd";
        int startDate = 11111111;
        int endDate = 99999999;
        int yakka = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        int isDeleted = 0;

        IpnMinYakkaMst ipnMinYakkaMst = new IpnMinYakkaMst()
        {
            Id = id,
            IpnNameCd = ipnNameCd,
            StartDate = startDate,
            EndDate = endDate,
            Yakka = yakka,
            SeqNo = seqNo,
            IsDeleted = isDeleted,
        };

        tenant.Add(ipnMinYakkaMst);
        List<string> ipnNameCds = new() { ipnNameCd };
        var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
        OrdInfRepository ordInfRepository = new OrdInfRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = ordInfRepository.GetCheckIpnMinYakkaMsts(hpId, sinDate, ipnNameCds);

            var success = result.Any(item => item.Id == id
                                             && item.HpId == hpId
                                             && item.IpnNameCd == ipnNameCd
                                             && item.StartDate == startDate
                                             && item.EndDate == endDate
                                             && item.Yakka == yakka
                                             && item.SeqNo == seqNo
                                             && item.IsDeleted == isDeleted
                                             && !item.ModelModified);
            // Assert
            Assert.True(success);
        }
        finally
        {
            ordInfRepository.ReleaseResource();
            tenant.IpnMinYakkaMsts.Remove(ipnMinYakkaMst);
            tenant.SaveChanges();
        }
    }
    #endregion GetCheckIpnMinYakkaMsts

    #region GetSettingValue
    [Test]
    public void TC_004_GetSettingValue_TestSuccess_01()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        int id = random.Next(999, 99999);
        int grpCd = random.Next(999, 99999);
        int grpEdaNo = random.Next(999, 99999);
        int startDate = 11111111;
        int endDate = 99999999;
        int val = random.Next(999, 99999);
        string param = "Param";
        int defaultVal = random.Next(999, 99999);
        string defaultParam = "DefaultParam";
        var finalKey = "" + CacheKeyConstant.SystemGenerationConf + hpId;

        var systemGenerationConf = new SystemGenerationConf()
        {
            Id = id,
            HpId = hpId,
            GrpCd = grpCd,
            GrpEdaNo = grpEdaNo,
            StartDate = startDate,
            EndDate = endDate,
            Val = val,
            Param = param
        };

        tenant.Add(systemGenerationConf);
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemGenerationConfRepository systemGenerationConfRepository = new SystemGenerationConfRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = systemGenerationConfRepository.GetSettingValue(hpId, grpCd, grpEdaNo, sinDate, defaultVal, defaultParam, false);

            var success = result.Item1 == val && result.Item2 == param;
           
            // Assert
            Assert.True(success);
        }
        finally
        {
            if (_cache.KeyExists(finalKey))
            {
                _cache.KeyDelete(finalKey);
            }
            systemGenerationConfRepository.ReleaseResource();
            tenant.SystemGenerationConfs.Remove(systemGenerationConf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_005_GetSettingValue_TestSuccess_02()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        int id = random.Next(999, 99999);
        int grpCd = random.Next(999, 99999);
        int grpEdaNo = random.Next(999, 99999);
        int startDate = 11111111;
        int endDate = 99999999;
        int val = random.Next(999, 99999);
        string param = "Param";
        int defaultVal = random.Next(999, 99999);
        string defaultParam = "DefaultParam";
        var finalKey = "" + CacheKeyConstant.SystemGenerationConf + hpId;

        var systemGenerationConf = new SystemGenerationConf()
        {
            Id = id,
            HpId = hpId,
            GrpCd = grpCd,
            GrpEdaNo = grpEdaNo,
            StartDate = startDate,
            EndDate = endDate,
            Val = val,
            Param = param
        };

        tenant.Add(systemGenerationConf);
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemGenerationConfRepository systemGenerationConfRepository = new SystemGenerationConfRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = systemGenerationConfRepository.GetSettingValue(hpId, grpCd, grpEdaNo, sinDate, defaultVal, defaultParam, true);

            // Assert
            var success = result.Item1 == val && result.Item2 == param;
            Assert.True(success);
        }
        finally
        {
            if (_cache.KeyExists(finalKey))
            {
                _cache.KeyDelete(finalKey);
            }
            systemGenerationConfRepository.ReleaseResource();
            tenant.SystemGenerationConfs.Remove(systemGenerationConf);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_006_GetSettingValue_TestSuccess_03()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        int id = random.Next(999, 99999);
        int grpCd = random.Next(999, 99999);
        int grpEdaNo = random.Next(999, 99999);
        int startDate = 11111111;
        int endDate = 99999999;
        int val = random.Next(999, 99999);
        string param = "Param";
        int defaultVal = random.Next(999, 99999);
        string defaultParam = "DefaultParam";
        var finalKey = "" + CacheKeyConstant.SystemGenerationConf + hpId;

        var systemGenerationConf = new SystemGenerationConf()
        {
            Id = id,
            HpId = hpId,
            GrpCd = grpCd,
            GrpEdaNo = grpEdaNo,
            StartDate = startDate,
            EndDate = endDate,
            Val = val,
            Param = param
        };

        tenant.Add(systemGenerationConf);
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        SystemGenerationConfRepository systemGenerationConfRepository = new SystemGenerationConfRepository(TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = systemGenerationConfRepository.GetSettingValue(hpId, random.Next(999, 99999), random.Next(999, 99999), sinDate, defaultVal, defaultParam, true);

            // Assert
            var success = result.Item1 == defaultVal && result.Item2 == defaultParam;
            Assert.True(success);
        }
        finally
        {
            if (_cache.KeyExists(finalKey))
            {
                _cache.KeyDelete(finalKey);
            }
            systemGenerationConfRepository.ReleaseResource();
            tenant.SystemGenerationConfs.Remove(systemGenerationConf);
            tenant.SaveChanges();
        }
    }
    #endregion GetSettingValue

    #region CheckIsGetYakkaPrices
    [Test]
    public void TC_007_CheckIsGetYakkaPrices_TestSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        int id = random.Next(999, 99999);
        string ipnNameCd = "IpnNameCd";
        string itemCd = "ItemCd";
        int startDate = 11111111;
        int endDate = 99999999;
        int yakka = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);

        List<TenItemModel> tenMstList = new() { new TenItemModel(hpId, itemCd, ipnNameCd, string.Empty, 0, 0) };
        var ipnKasanExclude = new IpnKasanExclude()
        {
            IpnNameCd = ipnNameCd,
            StartDate = startDate,
            EndDate = endDate,
        };
        var ipnKasanExcludeItem = new IpnKasanExcludeItem()
        {
            ItemCd = itemCd,
            StartDate = startDate,
            EndDate = endDate,
        };

        tenant.Add(ipnKasanExclude);
        tenant.Add(ipnKasanExcludeItem);

        List<string> ipnNameCds = new() { ipnNameCd };
        var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
        OrdInfRepository ordInfRepository = new OrdInfRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = ordInfRepository.CheckIsGetYakkaPrices(hpId, tenMstList, sinDate);

            // Assert
            var success = result != null
                          && result.Any(item => item.Item1 == ipnNameCd
                                                && item.Item2 == itemCd
                                                && !item.Item3);
            Assert.True(success);
        }
        finally
        {
            ordInfRepository.ReleaseResource();
            tenant.ipnKasanExcludes.Remove(ipnKasanExclude);
            tenant.ipnKasanExcludeItems.Remove(ipnKasanExcludeItem);
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_008_CheckIsGetYakkaPrices_TestSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        int id = random.Next(999, 99999);
        string ipnNameCd = "IpnNameCd";
        int yakka = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);

        List<string> ipnNameCds = new() { ipnNameCd };
        var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
        OrdInfRepository ordInfRepository = new OrdInfRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = ordInfRepository.CheckIsGetYakkaPrices(hpId, new(), sinDate);

            // Assert
            var success = !result.Any();
            Assert.True(success);
        }
        finally
        {
            ordInfRepository.ReleaseResource();
            tenant.SaveChanges();
        }
    }

    #endregion CheckIsGetYakkaPrices

    #region GetIpnMst
    [Test]
    public void TC_009_GetIpnMst_TestSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int sinDate = 20220202;
        int id = random.Next(999, 99999);
        string ipnNameCd = "IpnNameCd";
        int startDate = 11111111;
        int endDate = 99999999;
        int yakka = random.Next(999, 99999);
        int seqNo = random.Next(999, 99999);
        string ipnName = "IpnName";

        var ipnNameMst = new IpnNameMst()
        {
            IpnNameCd = ipnNameCd,
            StartDate = startDate,
            EndDate = endDate,
            IpnName = ipnName,
        };

        tenant.Add(ipnNameMst);

        List<string> ipnNameCds = new() { ipnNameCd };
        var mockAmazonS3Options = new Mock<IOptions<AmazonS3Options>>();
        OrdInfRepository ordInfRepository = new OrdInfRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var result = ordInfRepository.GetIpnMst(hpId, sinDate, sinDate, ipnNameCds);

            // Assert
            var success = result != null
                          && result.Any(item => item.Item1 == ipnNameCd
                                                && item.Item2 == ipnName);
            Assert.True(success);
        }
        finally
        {
            ordInfRepository.ReleaseResource();
            tenant.IpnNameMsts.Remove(ipnNameMst);
            tenant.SaveChanges();
        }
    }
    #endregion GetIpnMst
}
