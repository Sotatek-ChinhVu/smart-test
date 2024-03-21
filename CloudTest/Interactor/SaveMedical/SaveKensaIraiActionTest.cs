using Domain.Models.OrdInfs;
using Moq;
using Domain.Models.GroupInf;
using Domain.Models.KensaIrai;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Reporting.Kensalrai.DB;
using Reporting.Kensalrai.Service;
using Interactor.MedicalExamination.KensaIraiCommon;
using KensaCenterMstModel = Domain.Models.KensaIrai.KensaCenterMstModel;
using Helper.Constants;
using Domain.Models.OrdInfDetails;
using KensaInfDetailModel = Domain.Models.KensaIrai.KensaInfDetailModel;
using UseCase.MedicalExamination.SaveKensaIrai;
using Helper.Common;

namespace CloudUnitTest.Interactor.SaveMedical;

public class SaveKensaIraiActionTest : BaseUT
{
    [Test]
    public void TC_001_SaveKensaIraiAction_TestSuccess()
    {
        //Setup Data Test
        var mockKensaIraiRepository = new Mock<IKensaIraiRepository>();
        var mockSystemConfRepository = new Mock<ISystemConfRepository>();
        var mockPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockReceptionRepository = new Mock<IReceptionRepository>();
        var mockOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockKensaIraiCoReportService = new Mock<IKensaIraiCoReportService>();
        var mockGroupInfRepository = new Mock<IGroupInfRepository>();
        var mockCoKensaIraiFinder = new Mock<ICoKensaIraiFinder>();

        // Arrange
        var kensaIraiCommon = new KensaIraiCommon(TenantProvider, mockKensaIraiRepository.Object, mockSystemConfRepository.Object, mockPatientInforRepository.Object, mockReceptionRepository.Object, mockOrdInfRepository.Object, mockKensaIraiCoReportService.Object, mockGroupInfRepository.Object, mockCoKensaIraiFinder.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long ptNum = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        int sinDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
        string odrKensaIraiCenterCd = "centerCd";
        string centerCd = "centerCd";
        string centerName = "centerName";
        int primaryKbn = random.Next(999, 99999);

        var kensaCenterMstModel = new KensaCenterMstModel(centerCd, centerName, primaryKbn);
        var patientInforModel = new PatientInforModel(
                hpId,
                ptId,
                random.Next(999, 9999999),
                random.Next(999, 9999999),
                ptNum,
                "kanaName",
                "name",
                random.Next(999, 9999999),
                random.Next(999, 9999999),
                random.Next(999, 9999999),
                0,
                random.Next(999, 9999999),
                "HomePost",
                "HomeAddress1",
                "HomeAddress2",
                "Tel1",
                "Tel2",
                "Mail",
                "Setanusi",
                "Zokugara",
                "Job",
                "RenrakuName",
                "RenrakuPost",
                "RenrakuAddress1",
                "RenrakuAddress2",
                "RenrakuTel",
                "RenrakuMemo",
                "OfficeName",
                "OfficePost",
                "OfficeAddress1",
                "OfficeAddress2",
                "OfficeTel",
                "OfficeMemo",
                0,
                0,
                0,
                random.Next(999, 9999999),
                "memo",
                random.Next(999, 9999999),
                random.Next(999, 9999999),
                random.Next(999, 9999999),
                "comment",
                sinDate,
                false);

        var receptionRowModel = new ReceptionRowModel(
                raiinNo,
                ptId,
                raiinNo,
                random.Next(999, 9999999),
                false,
                0,
                0,
                patientInforModel.PtNum,
                patientInforModel.KanaName ?? string.Empty,
                patientInforModel.Name ?? string.Empty,
                patientInforModel.Sex,
                patientInforModel.Birthday,
                string.Empty,
                0,
                string.Empty,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                0,
                0,
                string.Empty,
                string.Empty,
                0,
                string.Empty,
                new(),
                new(),
                0,
                // Fields needed to create Hoken name
                CommonConstants.InvalidId,
                0,
                0,
                CommonConstants.InvalidId,
                CommonConstants.InvalidId,
                CommonConstants.InvalidId,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty
            );

        OrdInfModel ordInfModel = new OrdInfModel(
                        hpId,
                        raiinNo,
                        random.Next(999, 9999999),
                        random.Next(999, 9999999),
                        ptId,
                        sinDate,
                        receptionRowModel.HokenPid,
                        0,
                        string.Empty,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        new(),
                        DateTime.MinValue,
                        0,
                        string.Empty,
                        DateTime.MinValue,
                        0,
                        string.Empty,
                        string.Empty,
                        string.Empty
                   );


        var kensaMstModel = new KensaMstModel(
                              "KensaItemCd",
                              0,
                              centerCd,
                              "KensaName",
                              "KensaKana",
                              "Unit",
                              0,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              0,
                              string.Empty,
                              0,
                              0,
                              string.Empty,
                              string.Empty);

        var ordInfDetailModel = new OrdInfDetailModel(
                        hpId,
                        raiinNo,
                        0,
                        0,
                        0,
                        ptId,
                        sinDate,
                        0,
                        string.Empty,
                        string.Empty,
                        0,
                        string.Empty,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        string.Empty,
                        string.Empty,
                        0,
                        string.Empty,
                        string.Empty,
                        0,
                        DateTime.MinValue,
                        0,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        0,
                        string.Empty,
                        0,
                        0,
                        false,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        string.Empty,
                        new(),
                        0,
                        0,
                        string.Empty,
                        "odrUnitName",
                        "centerItemCd1",
                        "centerItemCd2",
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        false,
                        kensaMstModel
            );

        var kensaInfModel = new KensaInfModel(
                        ptId,
                        0,
                        raiinNo,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        centerCd,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        false,
                        0);

        var kensaInfDetailModel = new KensaInfDetailModel(
                                      ptId,
                                      0,
                                      raiinNo,
                                      0,
                                      0,
                                      string.Empty,
                                      string.Empty,
                                      string.Empty,
                                      string.Empty,
                                      0,
                                      string.Empty,
                                      string.Empty,
                                      new());

        var groupInfModel = new GroupInfModel(
                                hpId,
                                ptId,
                                0,
                                string.Empty,
                                string.Empty);


        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 0))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 0, 1, string.Empty, string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 1))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 1, 0, "0-0", string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 2))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 2, 0, odrKensaIraiCenterCd, string.Empty));
        mockKensaIraiRepository.Setup(finder => finder.GetKensaCenterMst(hpId, odrKensaIraiCenterCd))
            .Returns((int hpId, string centerCd) => kensaCenterMstModel);
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 7))
           .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 7, 2, string.Empty, string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 8))
           .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 8, 1, "1=1=ｻﾞｲﾀｸ`", string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 9))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 9, 1, string.Empty, string.Empty));

        mockPatientInforRepository.Setup(finder => finder.GetById(hpId, ptId, sinDate, 0, false, null))
            .Returns((int hpId, long ptId, int sinDate, long raiinNo, bool isShowKyuSeiName, List<int>? listStatus) => patientInforModel);
        mockReceptionRepository.Setup(finder => finder.GetList(hpId, sinDate, raiinNo, ptId, false, false, 2, false))
            .Returns((int hpId, int sinDate, long raiinNo, long ptId, bool isGetAccountDue, bool isGetFamily, int isDeleted, bool searchSameVisit) => new() { receptionRowModel });
        mockOrdInfRepository.Setup(finder => finder.GetIngaiKensaOdrInf(hpId, ptId, sinDate, raiinNo))
            .Returns((int hpId, long ptId, int sinDate, long raiinNo) => new() { ordInfModel });
        mockOrdInfRepository.Setup(finder => finder.GetIngaiKensaOdrInfDetail(hpId, ptId, sinDate, raiinNo, kensaCenterMstModel.CenterCd, kensaCenterMstModel.PrimaryKbn))
            .Returns((int hpId, long ptId, int sinDate, long raiinNo, string centerCd, int primaryKbn) => new() { ordInfDetailModel });
        mockKensaIraiRepository.Setup(finder => finder.GetKensaInf(hpId, ptId, raiinNo, kensaCenterMstModel.CenterCd))
            .Returns((int hpId, long ptId, long raiinNo, string centerCd) => new() { kensaInfModel });
        mockKensaIraiRepository.Setup(finder => finder.GetKensaInfDetail(hpId, ptId, raiinNo, kensaCenterMstModel.CenterCd))
            .Returns((int hpId, long ptId, long raiinNo, string centerCd) => new() { kensaInfDetailModel });
        mockGroupInfRepository.Setup(finder => finder.GetAllByPtIdList(hpId, new() { ptId }))
            .Returns((int hpId, List<long> ptIdList) => new() { groupInfModel });

        // Act
        var result = kensaIraiCommon.SaveKensaIraiAction(hpId, userId, ptId, sinDate, raiinNo);

        bool success = result.KensaIraiReportItemList.Count == 0 && result.Message == string.Empty && result.Status == SaveKensaIraiStatus.Successed;
        // Assert
        Assert.That(success);
    }
    
    [Test]
    public void TC_002_SaveKensaIraiAction_TestSuccess()
    {
        //Setup Data Test
        var mockKensaIraiRepository = new Mock<IKensaIraiRepository>();
        var mockSystemConfRepository = new Mock<ISystemConfRepository>();
        var mockPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockReceptionRepository = new Mock<IReceptionRepository>();
        var mockOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockKensaIraiCoReportService = new Mock<IKensaIraiCoReportService>();
        var mockGroupInfRepository = new Mock<IGroupInfRepository>();
        var mockCoKensaIraiFinder = new Mock<ICoKensaIraiFinder>();

        // Arrange
        var kensaIraiCommon = new KensaIraiCommon(TenantProvider, mockKensaIraiRepository.Object, mockSystemConfRepository.Object, mockPatientInforRepository.Object, mockReceptionRepository.Object, mockOrdInfRepository.Object, mockKensaIraiCoReportService.Object, mockGroupInfRepository.Object, mockCoKensaIraiFinder.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long ptNum = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        int sinDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
        int primaryKbn = random.Next(999, 99999);

        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 0))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 0, 0, string.Empty, string.Empty));

        // Act
        var result = kensaIraiCommon.SaveKensaIraiAction(hpId, userId, ptId, sinDate, raiinNo);

        bool success = result.KensaIraiReportItemList.Count == 0 && result.Message == "ライセンスがありません" && result.Status == SaveKensaIraiStatus.Successed;
        // Assert
        Assert.That(success);
    }

    [Test]
    public void TC_003_SaveKensaIraiAction_TestFalse()
    {
        //Setup Data Test
        var mockKensaIraiRepository = new Mock<IKensaIraiRepository>();
        var mockSystemConfRepository = new Mock<ISystemConfRepository>();
        var mockPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockReceptionRepository = new Mock<IReceptionRepository>();
        var mockOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockKensaIraiCoReportService = new Mock<IKensaIraiCoReportService>();
        var mockGroupInfRepository = new Mock<IGroupInfRepository>();
        var mockCoKensaIraiFinder = new Mock<ICoKensaIraiFinder>();

        // Arrange
        var kensaIraiCommon = new KensaIraiCommon(TenantProvider, mockKensaIraiRepository.Object, mockSystemConfRepository.Object, mockPatientInforRepository.Object, mockReceptionRepository.Object, mockOrdInfRepository.Object, mockKensaIraiCoReportService.Object, mockGroupInfRepository.Object, mockCoKensaIraiFinder.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long ptNum = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        int sinDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
        string odrKensaIraiCenterCd = "centerCd";
        string centerCd = "centerCd";
        string centerName = "centerName";
        int primaryKbn = random.Next(999, 99999);

        var kensaCenterMstModel = new KensaCenterMstModel(centerCd, centerName, primaryKbn);
        PatientInforModel? patientInforModel = null;

        var receptionRowModel = new ReceptionRowModel(
                raiinNo,
                ptId,
                raiinNo,
                random.Next(999, 9999999),
                false,
                0,
                0,
                patientInforModel?.PtNum ?? 0,
                patientInforModel?.KanaName ?? string.Empty,
                patientInforModel?.Name ?? string.Empty,
                patientInforModel?.Sex ?? 0,
                patientInforModel?.Birthday ?? 0,
                string.Empty,
                0,
                string.Empty,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                0,
                0,
                string.Empty,
                string.Empty,
                0,
                string.Empty,
                new(),
                new(),
                0,
                // Fields needed to create Hoken name
                CommonConstants.InvalidId,
                0,
                0,
                CommonConstants.InvalidId,
                CommonConstants.InvalidId,
                CommonConstants.InvalidId,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty
            );

        OrdInfModel ordInfModel = new OrdInfModel(
                        hpId,
                        raiinNo,
                        random.Next(999, 9999999),
                        random.Next(999, 9999999),
                        ptId,
                        sinDate,
                        receptionRowModel.HokenPid,
                        0,
                        string.Empty,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        new(),
                        DateTime.MinValue,
                        0,
                        string.Empty,
                        DateTime.MinValue,
                        0,
                        string.Empty,
                        string.Empty,
                        string.Empty
                   );


        var kensaMstModel = new KensaMstModel(
                              "KensaItemCd",
                              0,
                              centerCd,
                              "KensaName",
                              "KensaKana",
                              "Unit",
                              0,
                              0,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              0,
                              string.Empty,
                              0,
                              0,
                              string.Empty,
                              string.Empty);

        var ordInfDetailModel = new OrdInfDetailModel(
                        hpId,
                        raiinNo,
                        0,
                        0,
                        0,
                        ptId,
                        sinDate,
                        0,
                        string.Empty,
                        string.Empty,
                        0,
                        string.Empty,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        string.Empty,
                        string.Empty,
                        0,
                        string.Empty,
                        string.Empty,
                        0,
                        DateTime.MinValue,
                        0,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        0,
                        string.Empty,
                        0,
                        0,
                        false,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        string.Empty,
                        new(),
                        0,
                        0,
                        string.Empty,
                        "odrUnitName",
                        "centerItemCd1",
                        "centerItemCd2",
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        false,
                        kensaMstModel
            );

        var kensaInfModel = new KensaInfModel(
                        ptId,
                        0,
                        raiinNo,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        centerCd,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        false,
                        0);

        var kensaInfDetailModel = new KensaInfDetailModel(
                                      ptId,
                                      0,
                                      raiinNo,
                                      0,
                                      0,
                                      string.Empty,
                                      string.Empty,
                                      string.Empty,
                                      string.Empty,
                                      0,
                                      string.Empty,
                                      string.Empty,
                                      new());

        var groupInfModel = new GroupInfModel(
                                hpId,
                                ptId,
                                0,
                                string.Empty,
                                string.Empty);


        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 0))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 0, 1, string.Empty, string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 1))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 1, 0, "0-0", string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 2))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 2, 0, odrKensaIraiCenterCd, string.Empty));
        mockKensaIraiRepository.Setup(finder => finder.GetKensaCenterMst(hpId, odrKensaIraiCenterCd))
            .Returns((int hpId, string centerCd) => kensaCenterMstModel);
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 7))
           .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 7, 2, string.Empty, string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 8))
           .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 8, 1, "1=1=ｻﾞｲﾀｸ`", string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 9))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 9, 1, string.Empty, string.Empty));

        mockPatientInforRepository.Setup(finder => finder.GetById(hpId, ptId, sinDate, 0, false, null))
            .Returns((int hpId, long ptId, int sinDate, long raiinNo, bool isShowKyuSeiName, List<int>? listStatus) => patientInforModel);
        mockReceptionRepository.Setup(finder => finder.GetList(hpId, sinDate, raiinNo, ptId, false, false, 2, false))
            .Returns((int hpId, int sinDate, long raiinNo, long ptId, bool isGetAccountDue, bool isGetFamily, int isDeleted, bool searchSameVisit) => new() { receptionRowModel });
        mockOrdInfRepository.Setup(finder => finder.GetIngaiKensaOdrInf(hpId, ptId, sinDate, raiinNo))
            .Returns((int hpId, long ptId, int sinDate, long raiinNo) => new() { ordInfModel });
        mockOrdInfRepository.Setup(finder => finder.GetIngaiKensaOdrInfDetail(hpId, ptId, sinDate, raiinNo, kensaCenterMstModel.CenterCd, kensaCenterMstModel.PrimaryKbn))
            .Returns((int hpId, long ptId, int sinDate, long raiinNo, string centerCd, int primaryKbn) => new() { ordInfDetailModel });
        mockKensaIraiRepository.Setup(finder => finder.GetKensaInf(hpId, ptId, raiinNo, kensaCenterMstModel.CenterCd))
            .Returns((int hpId, long ptId, long raiinNo, string centerCd) => new() { kensaInfModel });
        mockKensaIraiRepository.Setup(finder => finder.GetKensaInfDetail(hpId, ptId, raiinNo, kensaCenterMstModel.CenterCd))
            .Returns((int hpId, long ptId, long raiinNo, string centerCd) => new() { kensaInfDetailModel });
        mockGroupInfRepository.Setup(finder => finder.GetAllByPtIdList(hpId, new() { ptId }))
            .Returns((int hpId, List<long> ptIdList) => new() { groupInfModel });

        // Act
        var result = kensaIraiCommon.SaveKensaIraiAction(hpId, userId, ptId, sinDate, raiinNo);

        bool success = result.KensaIraiReportItemList.Count == 0 && result.Message == $"患者情報がみつかりません。 ptid:{ptId}" && result.Status == SaveKensaIraiStatus.Failed;
        // Assert
        Assert.That(success);
    }

    [Test]
    public void TC_004_SaveKensaIraiAction_TestFalse()
    {
        //Setup Data Test
        var mockKensaIraiRepository = new Mock<IKensaIraiRepository>();
        var mockSystemConfRepository = new Mock<ISystemConfRepository>();
        var mockPatientInforRepository = new Mock<IPatientInforRepository>();
        var mockReceptionRepository = new Mock<IReceptionRepository>();
        var mockOrdInfRepository = new Mock<IOrdInfRepository>();
        var mockKensaIraiCoReportService = new Mock<IKensaIraiCoReportService>();
        var mockGroupInfRepository = new Mock<IGroupInfRepository>();
        var mockCoKensaIraiFinder = new Mock<ICoKensaIraiFinder>();

        // Arrange
        var kensaIraiCommon = new KensaIraiCommon(TenantProvider, mockKensaIraiRepository.Object, mockSystemConfRepository.Object, mockPatientInforRepository.Object, mockReceptionRepository.Object, mockOrdInfRepository.Object, mockKensaIraiCoReportService.Object, mockGroupInfRepository.Object, mockCoKensaIraiFinder.Object);

        // Mock data
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        long ptId = random.Next(999, 99999999);
        long ptNum = random.Next(999, 99999999);
        long raiinNo = random.Next(999, 9999999);
        int sinDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
        string odrKensaIraiCenterCd = "centerCd";
        string centerCd = "centerCd";
        string centerName = "centerName";
        int primaryKbn = random.Next(999, 99999);

        var kensaCenterMstModel = new KensaCenterMstModel(centerCd, centerName, primaryKbn);
        var patientInforModel = new PatientInforModel(
                hpId,
                ptId,
                random.Next(999, 9999999),
                random.Next(999, 9999999),
                ptNum,
                "kanaName",
                "name",
                random.Next(999, 9999999),
                random.Next(999, 9999999),
                random.Next(999, 9999999),
                0,
                random.Next(999, 9999999),
                "HomePost",
                "HomeAddress1",
                "HomeAddress2",
                "Tel1",
                "Tel2",
                "Mail",
                "Setanusi",
                "Zokugara",
                "Job",
                "RenrakuName",
                "RenrakuPost",
                "RenrakuAddress1",
                "RenrakuAddress2",
                "RenrakuTel",
                "RenrakuMemo",
                "OfficeName",
                "OfficePost",
                "OfficeAddress1",
                "OfficeAddress2",
                "OfficeTel",
                "OfficeMemo",
                0,
                0,
                0,
                random.Next(999, 9999999),
                "memo",
                random.Next(999, 9999999),
                random.Next(999, 9999999),
                random.Next(999, 9999999),
                "comment",
                sinDate,
                false);

        var receptionRowModel = new ReceptionRowModel(
                raiinNo,
                ptId,
                raiinNo,
                random.Next(999, 9999999),
                false,
                0,
                0,
                patientInforModel.PtNum,
                patientInforModel.KanaName ?? string.Empty,
                patientInforModel.Name ?? string.Empty,
                patientInforModel.Sex,
                patientInforModel.Birthday,
                string.Empty,
                0,
                string.Empty,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                0,
                0,
                string.Empty,
                string.Empty,
                0,
                string.Empty,
                new(),
                new(),
                0,
                // Fields needed to create Hoken name
                CommonConstants.InvalidId,
                0,
                0,
                CommonConstants.InvalidId,
                CommonConstants.InvalidId,
                CommonConstants.InvalidId,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty,
                CommonConstants.InvalidId,
                string.Empty
            );

        var kensaInfModel = new KensaInfModel(
                        ptId,
                        0,
                        raiinNo,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        centerCd,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        false,
                        0);

        var kensaInfDetailModel = new KensaInfDetailModel(
                                      ptId,
                                      0,
                                      raiinNo,
                                      0,
                                      0,
                                      string.Empty,
                                      string.Empty,
                                      string.Empty,
                                      string.Empty,
                                      0,
                                      string.Empty,
                                      string.Empty,
                                      new());

        var groupInfModel = new GroupInfModel(
                                hpId,
                                ptId,
                                0,
                                string.Empty,
                                string.Empty);


        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 0))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 0, 1, string.Empty, string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 1))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 1, 0, "0-0", string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 2))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 2, 0, odrKensaIraiCenterCd, string.Empty));
        mockKensaIraiRepository.Setup(finder => finder.GetKensaCenterMst(hpId, odrKensaIraiCenterCd))
            .Returns((int hpId, string centerCd) => kensaCenterMstModel);
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 7))
           .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 7, 2, string.Empty, string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 8))
           .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 8, 1, "1=1=ｻﾞｲﾀｸ`", string.Empty));
        mockSystemConfRepository.Setup(finder => finder.GetByGrpCd(hpId, 100019, 9))
            .Returns((int hpId, int grpCd, int grpEdaNo) => new SystemConfModel(100019, 9, 1, string.Empty, string.Empty));

        mockPatientInforRepository.Setup(finder => finder.GetById(hpId, ptId, sinDate, 0, false, null))
            .Returns((int hpId, long ptId, int sinDate, long raiinNo, bool isShowKyuSeiName, List<int>? listStatus) => patientInforModel);
        mockReceptionRepository.Setup(finder => finder.GetList(hpId, sinDate, raiinNo, ptId, false, false, 2, false))
            .Returns((int hpId, int sinDate, long raiinNo, long ptId, bool isGetAccountDue, bool isGetFamily, int isDeleted, bool searchSameVisit) => new() { receptionRowModel });
        mockOrdInfRepository.Setup(finder => finder.GetIngaiKensaOdrInf(hpId, ptId, sinDate, raiinNo))
            .Returns((int hpId, long ptId, int sinDate, long raiinNo) => new());
        mockOrdInfRepository.Setup(finder => finder.GetIngaiKensaOdrInfDetail(hpId, ptId, sinDate, raiinNo, kensaCenterMstModel.CenterCd, kensaCenterMstModel.PrimaryKbn))
            .Returns((int hpId, long ptId, int sinDate, long raiinNo, string centerCd, int primaryKbn) => new() { });
        mockKensaIraiRepository.Setup(finder => finder.GetKensaInf(hpId, ptId, raiinNo, kensaCenterMstModel.CenterCd))
            .Returns((int hpId, long ptId, long raiinNo, string centerCd) => new() { kensaInfModel });
        mockKensaIraiRepository.Setup(finder => finder.GetKensaInfDetail(hpId, ptId, raiinNo, kensaCenterMstModel.CenterCd))
            .Returns((int hpId, long ptId, long raiinNo, string centerCd) => new() { kensaInfDetailModel });
        mockGroupInfRepository.Setup(finder => finder.GetAllByPtIdList(hpId, new() { ptId }))
            .Returns((int hpId, List<long> ptIdList) => new() { groupInfModel });

        // Act
        var result = kensaIraiCommon.SaveKensaIraiAction(hpId, userId, ptId, sinDate, raiinNo);

        bool success = result.KensaIraiReportItemList.Count == 0 && result.Message == $"院外検査オーダーがみつかりません。 ptid:{ptId} raiinNo:{raiinNo}" && result.Status == SaveKensaIraiStatus.Failed;
        // Assert
        Assert.That(success);
    }

}
