using Domain.Models.Diseases;
using Domain.Models.Family;
using Domain.Models.FlowSheet;
using Domain.Models.KarteInfs;
using Domain.Models.MonshinInf;
using Domain.Models.NextOrder;
using Domain.Models.OrdInfs;
using Domain.Models.SpecialNote;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.TodayOdr;
using Infrastructure.Repositories;
using Moq;

namespace CloudUnitTest.Repository.SaveMedical;

public class SaveMedicalRepositoryTest : BaseUT
{
    #region Upsert

    [Test]
    public void TC_001_SaveMedicalRepository_TestSuccess()
    {
        //Setup Data Test

        var mockIFamilyRepository = new Mock<IFamilyRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockINextOrderRepository = new Mock<INextOrderRepository>();
        var mockISpecialNoteRepository = new Mock<ISpecialNoteRepository>();
        var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
        var mockIFlowSheetRepository = new Mock<IFlowSheetRepository>();
        var mockIMonshinInforRepository = new Mock<IMonshinInforRepository>();

        // Arrange
        var saveMedicalRepository = new SaveMedicalRepository(TenantProvider, mockIFamilyRepository.Object, mockITodayOdrRepository.Object, mockINextOrderRepository.Object, mockISpecialNoteRepository.Object, mockIPtDiseaseRepository.Object, mockIFlowSheetRepository.Object, mockIMonshinInforRepository.Object);

        // Mock data
        int hpId = 999;
        long ptId = 999;
        long raiinNo = 999;
        int sinDate = It.IsAny<int>();
        int syosaiKbn = It.IsAny<int>();
        int jikanKbn = It.IsAny<int>();
        int hokenPid = It.IsAny<int>();
        int santeiKbn = It.IsAny<int>();
        int tantoId = It.IsAny<int>();
        int kaId = It.IsAny<int>();
        string uketukeTime = It.IsAny<string>();
        string sinStartTime = It.IsAny<string>();
        string sinEndTime = It.IsAny<string>();
        byte status = It.IsAny<byte>();
        List<OrdInfModel> odrInfs = new();
        KarteInfModel karteInfModel = new(hpId, raiinNo);
        int userId = It.IsAny<int>();
        List<FamilyModel> familyList = new() { new FamilyModel() };
        List<NextOrderModel> rsvkrtOrderInfModels = new() { new NextOrderModel() };
        SummaryInfModel summaryInfModel = new();
        ImportantNoteModel importantNoteModel = new();
        PatientInfoModel patientInfoModel = new();
        List<PtDiseaseModel> ptDiseaseModels = new();
        List<FlowSheetModel> flowSheetData = new();
        int seqNo = 999;
        MonshinInforModel monshin = new(hpId, ptId, raiinNo, sinDate, string.Empty, string.Empty, 0, 0, seqNo);
        bool rsvkrtNo = true;
        List<long> byomeiIdList = new List<long>() { It.IsAny<long>(), It.IsAny<long>() };

        mockITodayOdrRepository.Setup(finder => finder.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, odrInfs, karteInfModel, userId, status))
        .Returns((int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId, byte modeSaveData) => true);

        mockINextOrderRepository.Setup(finder => finder.Upsert(userId, hpId, ptId, rsvkrtOrderInfModels))
        .Returns((int userId, int hpId, long ptId, List<NextOrderModel> nextOrderModels) => rsvkrtNo);

        mockISpecialNoteRepository.Setup(finder => finder.SaveSpecialNote(hpId, ptId, sinDate, summaryInfModel, importantNoteModel, patientInfoModel, userId))
        .Returns((int hpId, long ptId, int sinDate, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, int userId) => true);

        mockIPtDiseaseRepository.Setup(finder => finder.Upsert(ptDiseaseModels, hpId, userId))
        .Returns((List<PtDiseaseModel> inputDatas, int hpId, int userId) => byomeiIdList);

        mockIFlowSheetRepository.Setup(finder => finder.UpsertTag(flowSheetData, hpId, userId));
        mockIFlowSheetRepository.Setup(finder => finder.UpsertCmt(flowSheetData, hpId, userId));

        mockIFamilyRepository.Setup(finder => finder.SaveFamilyList(hpId, userId, familyList))
        .Returns((int hpId, int userId, List<FamilyModel> familyList) => true);

        mockIMonshinInforRepository.Setup(finder => finder.SaveMonshinSheet(monshin))
        .Returns((MonshinInforModel monshin) => true);

        // Act
        var result = saveMedicalRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrInfs, karteInfModel, userId, familyList, rsvkrtOrderInfModels, summaryInfModel, importantNoteModel, patientInfoModel, ptDiseaseModels, flowSheetData, monshin);

        // Assert
        Assert.That(result);
    }

    [Test]
    public void TC_002_SaveMedicalRepository_TestUpsertTodayOrderFalse()
    {
        //Setup Data Test

        var mockIFamilyRepository = new Mock<IFamilyRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockINextOrderRepository = new Mock<INextOrderRepository>();
        var mockISpecialNoteRepository = new Mock<ISpecialNoteRepository>();
        var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
        var mockIFlowSheetRepository = new Mock<IFlowSheetRepository>();
        var mockIMonshinInforRepository = new Mock<IMonshinInforRepository>();

        // Arrange
        var saveMedicalRepository = new SaveMedicalRepository(TenantProvider, mockIFamilyRepository.Object, mockITodayOdrRepository.Object, mockINextOrderRepository.Object, mockISpecialNoteRepository.Object, mockIPtDiseaseRepository.Object, mockIFlowSheetRepository.Object, mockIMonshinInforRepository.Object);

        // Mock data
        int hpId = 999;
        long ptId = 999;
        long raiinNo = 999;
        int sinDate = It.IsAny<int>();
        int syosaiKbn = It.IsAny<int>();
        int jikanKbn = It.IsAny<int>();
        int hokenPid = It.IsAny<int>();
        int santeiKbn = It.IsAny<int>();
        int tantoId = It.IsAny<int>();
        int kaId = It.IsAny<int>();
        string uketukeTime = It.IsAny<string>();
        string sinStartTime = It.IsAny<string>();
        string sinEndTime = It.IsAny<string>();
        byte status = It.IsAny<byte>();
        List<OrdInfModel> odrInfs = new();
        KarteInfModel karteInfModel = new(hpId, raiinNo);
        int userId = It.IsAny<int>();
        List<FamilyModel> familyList = new() { new FamilyModel() };
        List<NextOrderModel> rsvkrtOrderInfModels = new() { new NextOrderModel() };
        SummaryInfModel summaryInfModel = new();
        ImportantNoteModel importantNoteModel = new();
        PatientInfoModel patientInfoModel = new();
        List<PtDiseaseModel> ptDiseaseModels = new();
        List<FlowSheetModel> flowSheetData = new();
        int seqNo = 999;
        MonshinInforModel monshin = new(hpId, ptId, raiinNo, sinDate, string.Empty, string.Empty, 0, 0, seqNo);
        bool rsvkrtNo = true;
        List<long> byomeiIdList = new List<long>() { It.IsAny<long>(), It.IsAny<long>() };

        mockITodayOdrRepository.Setup(finder => finder.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, odrInfs, karteInfModel, userId, status))
        .Returns((int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId, byte modeSaveData) => false);

        mockINextOrderRepository.Setup(finder => finder.Upsert(userId, hpId, ptId, rsvkrtOrderInfModels))
        .Returns((int userId, int hpId, long ptId, List<NextOrderModel> nextOrderModels) => rsvkrtNo);

        mockISpecialNoteRepository.Setup(finder => finder.SaveSpecialNote(hpId, ptId, sinDate, summaryInfModel, importantNoteModel, patientInfoModel, userId))
        .Returns((int hpId, long ptId, int sinDate, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, int userId) => true);

        mockIPtDiseaseRepository.Setup(finder => finder.Upsert(ptDiseaseModels, hpId, userId))
        .Returns((List<PtDiseaseModel> inputDatas, int hpId, int userId) => byomeiIdList);

        mockIFlowSheetRepository.Setup(finder => finder.UpsertTag(flowSheetData, hpId, userId));
        mockIFlowSheetRepository.Setup(finder => finder.UpsertCmt(flowSheetData, hpId, userId));

        mockIFamilyRepository.Setup(finder => finder.SaveFamilyList(hpId, userId, familyList))
        .Returns((int hpId, int userId, List<FamilyModel> familyList) => true);

        mockIMonshinInforRepository.Setup(finder => finder.SaveMonshinSheet(monshin))
        .Returns((MonshinInforModel monshin) => true);

        // Act
        var result = saveMedicalRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrInfs, karteInfModel, userId, familyList, rsvkrtOrderInfModels, summaryInfModel, importantNoteModel, patientInfoModel, ptDiseaseModels, flowSheetData, monshin);

        // Assert
        Assert.That(result == false);
    }

    [Test]
    public void TC_003_SaveMedicalRepository_TestUpsertNextOrderFalse()
    {
        //Setup Data Test

        var mockIFamilyRepository = new Mock<IFamilyRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockINextOrderRepository = new Mock<INextOrderRepository>();
        var mockISpecialNoteRepository = new Mock<ISpecialNoteRepository>();
        var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
        var mockIFlowSheetRepository = new Mock<IFlowSheetRepository>();
        var mockIMonshinInforRepository = new Mock<IMonshinInforRepository>();

        // Arrange
        var saveMedicalRepository = new SaveMedicalRepository(TenantProvider, mockIFamilyRepository.Object, mockITodayOdrRepository.Object, mockINextOrderRepository.Object, mockISpecialNoteRepository.Object, mockIPtDiseaseRepository.Object, mockIFlowSheetRepository.Object, mockIMonshinInforRepository.Object);

        // Mock data
        int hpId = 999;
        long ptId = 999;
        long raiinNo = 999;
        int sinDate = It.IsAny<int>();
        int syosaiKbn = It.IsAny<int>();
        int jikanKbn = It.IsAny<int>();
        int hokenPid = It.IsAny<int>();
        int santeiKbn = It.IsAny<int>();
        int tantoId = It.IsAny<int>();
        int kaId = It.IsAny<int>();
        string uketukeTime = It.IsAny<string>();
        string sinStartTime = It.IsAny<string>();
        string sinEndTime = It.IsAny<string>();
        byte status = It.IsAny<byte>();
        List<OrdInfModel> odrInfs = new();
        KarteInfModel karteInfModel = new(hpId, raiinNo);
        int userId = It.IsAny<int>();
        List<FamilyModel> familyList = new() { new FamilyModel() };
        List<NextOrderModel> rsvkrtOrderInfModels = new() { new NextOrderModel() };
        SummaryInfModel summaryInfModel = new();
        ImportantNoteModel importantNoteModel = new();
        PatientInfoModel patientInfoModel = new();
        List<PtDiseaseModel> ptDiseaseModels = new();
        List<FlowSheetModel> flowSheetData = new();
        int seqNo = 999;
        MonshinInforModel monshin = new(hpId, ptId, raiinNo, sinDate, string.Empty, string.Empty, 0, 0, seqNo);
        List<long> byomeiIdList = new List<long>() { It.IsAny<long>(), It.IsAny<long>() };

        mockITodayOdrRepository.Setup(finder => finder.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, odrInfs, karteInfModel, userId, status))
        .Returns((int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId, byte modeSaveData) => true);

        mockINextOrderRepository.Setup(finder => finder.Upsert(userId, hpId, ptId, rsvkrtOrderInfModels))
        .Returns((int userId, int hpId, long ptId, List<NextOrderModel> nextOrderModels) => false);

        mockISpecialNoteRepository.Setup(finder => finder.SaveSpecialNote(hpId, ptId, sinDate, summaryInfModel, importantNoteModel, patientInfoModel, userId))
        .Returns((int hpId, long ptId, int sinDate, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, int userId) => true);

        mockIPtDiseaseRepository.Setup(finder => finder.Upsert(ptDiseaseModels, hpId, userId))
        .Returns((List<PtDiseaseModel> inputDatas, int hpId, int userId) => byomeiIdList);

        mockIFlowSheetRepository.Setup(finder => finder.UpsertTag(flowSheetData, hpId, userId));
        mockIFlowSheetRepository.Setup(finder => finder.UpsertCmt(flowSheetData, hpId, userId));

        mockIFamilyRepository.Setup(finder => finder.SaveFamilyList(hpId, userId, familyList))
        .Returns((int hpId, int userId, List<FamilyModel> familyList) => true);

        mockIMonshinInforRepository.Setup(finder => finder.SaveMonshinSheet(monshin))
        .Returns((MonshinInforModel monshin) => true);

        // Act
        var result = saveMedicalRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrInfs, karteInfModel, userId, familyList, rsvkrtOrderInfModels, summaryInfModel, importantNoteModel, patientInfoModel, ptDiseaseModels, flowSheetData, monshin);

        // Assert
        Assert.That(result == false);
    }

    [Test]
    public void TC_004_SaveMedicalRepository_TestUpsertSpecialNoteFalse()
    {
        //Setup Data Test

        var mockIFamilyRepository = new Mock<IFamilyRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockINextOrderRepository = new Mock<INextOrderRepository>();
        var mockISpecialNoteRepository = new Mock<ISpecialNoteRepository>();
        var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
        var mockIFlowSheetRepository = new Mock<IFlowSheetRepository>();
        var mockIMonshinInforRepository = new Mock<IMonshinInforRepository>();

        // Arrange
        var saveMedicalRepository = new SaveMedicalRepository(TenantProvider, mockIFamilyRepository.Object, mockITodayOdrRepository.Object, mockINextOrderRepository.Object, mockISpecialNoteRepository.Object, mockIPtDiseaseRepository.Object, mockIFlowSheetRepository.Object, mockIMonshinInforRepository.Object);

        // Mock data
        int hpId = 999;
        long ptId = 999;
        long raiinNo = 999;
        int sinDate = It.IsAny<int>();
        int syosaiKbn = It.IsAny<int>();
        int jikanKbn = It.IsAny<int>();
        int hokenPid = It.IsAny<int>();
        int santeiKbn = It.IsAny<int>();
        int tantoId = It.IsAny<int>();
        int kaId = It.IsAny<int>();
        string uketukeTime = It.IsAny<string>();
        string sinStartTime = It.IsAny<string>();
        string sinEndTime = It.IsAny<string>();
        byte status = It.IsAny<byte>();
        List<OrdInfModel> odrInfs = new();
        KarteInfModel karteInfModel = new(hpId, raiinNo);
        int userId = It.IsAny<int>();
        List<FamilyModel> familyList = new() { new FamilyModel() };
        List<NextOrderModel> rsvkrtOrderInfModels = new() { new NextOrderModel() };
        SummaryInfModel summaryInfModel = new();
        ImportantNoteModel importantNoteModel = new();
        PatientInfoModel patientInfoModel = new();
        List<PtDiseaseModel> ptDiseaseModels = new();
        List<FlowSheetModel> flowSheetData = new();
        int seqNo = 999;
        MonshinInforModel monshin = new(hpId, ptId, raiinNo, sinDate, string.Empty, string.Empty, 0, 0, seqNo);
        bool rsvkrtNo = true;
        List<long> byomeiIdList = new List<long>() { It.IsAny<long>(), It.IsAny<long>() };

        mockITodayOdrRepository.Setup(finder => finder.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, odrInfs, karteInfModel, userId, status))
        .Returns((int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId, byte modeSaveData) => true);

        mockINextOrderRepository.Setup(finder => finder.Upsert(userId, hpId, ptId, rsvkrtOrderInfModels))
        .Returns((int userId, int hpId, long ptId, List<NextOrderModel> nextOrderModels) => rsvkrtNo);

        mockISpecialNoteRepository.Setup(finder => finder.SaveSpecialNote(hpId, ptId, sinDate, summaryInfModel, importantNoteModel, patientInfoModel, userId))
        .Returns((int hpId, long ptId, int sinDate, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, int userId) => false);

        mockIPtDiseaseRepository.Setup(finder => finder.Upsert(ptDiseaseModels, hpId, userId))
        .Returns((List<PtDiseaseModel> inputDatas, int hpId, int userId) => byomeiIdList);

        mockIFlowSheetRepository.Setup(finder => finder.UpsertTag(flowSheetData, hpId, userId));
        mockIFlowSheetRepository.Setup(finder => finder.UpsertCmt(flowSheetData, hpId, userId));

        mockIFamilyRepository.Setup(finder => finder.SaveFamilyList(hpId, userId, familyList))
        .Returns((int hpId, int userId, List<FamilyModel> familyList) => true);

        mockIMonshinInforRepository.Setup(finder => finder.SaveMonshinSheet(monshin))
        .Returns((MonshinInforModel monshin) => true);

        // Act
        var result = saveMedicalRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrInfs, karteInfModel, userId, familyList, rsvkrtOrderInfModels, summaryInfModel, importantNoteModel, patientInfoModel, ptDiseaseModels, flowSheetData, monshin);

        // Assert
        Assert.That(result == false);
    }

    [Test]
    public void TC_005_SaveMedicalRepository_TestUpsertFamilyListFalse()
    {
        //Setup Data Test

        var mockIFamilyRepository = new Mock<IFamilyRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockINextOrderRepository = new Mock<INextOrderRepository>();
        var mockISpecialNoteRepository = new Mock<ISpecialNoteRepository>();
        var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
        var mockIFlowSheetRepository = new Mock<IFlowSheetRepository>();
        var mockIMonshinInforRepository = new Mock<IMonshinInforRepository>();

        // Arrange
        var saveMedicalRepository = new SaveMedicalRepository(TenantProvider, mockIFamilyRepository.Object, mockITodayOdrRepository.Object, mockINextOrderRepository.Object, mockISpecialNoteRepository.Object, mockIPtDiseaseRepository.Object, mockIFlowSheetRepository.Object, mockIMonshinInforRepository.Object);

        // Mock data
        int hpId = 999;
        long ptId = 999;
        long raiinNo = 999;
        int sinDate = It.IsAny<int>();
        int syosaiKbn = It.IsAny<int>();
        int jikanKbn = It.IsAny<int>();
        int hokenPid = It.IsAny<int>();
        int santeiKbn = It.IsAny<int>();
        int tantoId = It.IsAny<int>();
        int kaId = It.IsAny<int>();
        string uketukeTime = It.IsAny<string>();
        string sinStartTime = It.IsAny<string>();
        string sinEndTime = It.IsAny<string>();
        byte status = It.IsAny<byte>();
        List<OrdInfModel> odrInfs = new();
        KarteInfModel karteInfModel = new(hpId, raiinNo);
        int userId = It.IsAny<int>();
        List<FamilyModel> familyList = new() { new FamilyModel() };
        List<NextOrderModel> rsvkrtOrderInfModels = new() { new NextOrderModel() };
        SummaryInfModel summaryInfModel = new();
        ImportantNoteModel importantNoteModel = new();
        PatientInfoModel patientInfoModel = new();
        List<PtDiseaseModel> ptDiseaseModels = new();
        List<FlowSheetModel> flowSheetData = new();
        int seqNo = 999;
        MonshinInforModel monshin = new(hpId, ptId, raiinNo, sinDate, string.Empty, string.Empty, 0, 0, seqNo);
        bool rsvkrtNo = true;
        List<long> byomeiIdList = new List<long>() { It.IsAny<long>(), It.IsAny<long>() };

        mockITodayOdrRepository.Setup(finder => finder.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, odrInfs, karteInfModel, userId, status))
        .Returns((int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId, byte modeSaveData) => true);

        mockINextOrderRepository.Setup(finder => finder.Upsert(userId, hpId, ptId, rsvkrtOrderInfModels))
        .Returns((int userId, int hpId, long ptId, List<NextOrderModel> nextOrderModels) => rsvkrtNo);

        mockISpecialNoteRepository.Setup(finder => finder.SaveSpecialNote(hpId, ptId, sinDate, summaryInfModel, importantNoteModel, patientInfoModel, userId))
        .Returns((int hpId, long ptId, int sinDate, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, int userId) => true);

        mockIPtDiseaseRepository.Setup(finder => finder.Upsert(ptDiseaseModels, hpId, userId))
        .Returns((List<PtDiseaseModel> inputDatas, int hpId, int userId) => byomeiIdList);

        mockIFlowSheetRepository.Setup(finder => finder.UpsertTag(flowSheetData, hpId, userId));
        mockIFlowSheetRepository.Setup(finder => finder.UpsertCmt(flowSheetData, hpId, userId));

        mockIFamilyRepository.Setup(finder => finder.SaveFamilyList(hpId, userId, familyList))
        .Returns((int hpId, int userId, List<FamilyModel> familyList) => false);

        mockIMonshinInforRepository.Setup(finder => finder.SaveMonshinSheet(monshin))
        .Returns((MonshinInforModel monshin) => true);

        // Act
        var result = saveMedicalRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrInfs, karteInfModel, userId, familyList, rsvkrtOrderInfModels, summaryInfModel, importantNoteModel, patientInfoModel, ptDiseaseModels, flowSheetData, monshin);

        // Assert
        Assert.That(result == false);
    }

    [Test]
    public void TC_006_SaveMedicalRepository_TestUpsertMonshinSheetSuccess()
    {
        //Setup Data Test

        var mockIFamilyRepository = new Mock<IFamilyRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockINextOrderRepository = new Mock<INextOrderRepository>();
        var mockISpecialNoteRepository = new Mock<ISpecialNoteRepository>();
        var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
        var mockIFlowSheetRepository = new Mock<IFlowSheetRepository>();
        var mockIMonshinInforRepository = new Mock<IMonshinInforRepository>();

        // Arrange
        var saveMedicalRepository = new SaveMedicalRepository(TenantProvider, mockIFamilyRepository.Object, mockITodayOdrRepository.Object, mockINextOrderRepository.Object, mockISpecialNoteRepository.Object, mockIPtDiseaseRepository.Object, mockIFlowSheetRepository.Object, mockIMonshinInforRepository.Object);

        // Mock data
        int hpId = 999;
        long ptId = 999;
        long raiinNo = 999;
        int sinDate = It.IsAny<int>();
        int syosaiKbn = It.IsAny<int>();
        int jikanKbn = It.IsAny<int>();
        int hokenPid = It.IsAny<int>();
        int santeiKbn = It.IsAny<int>();
        int tantoId = It.IsAny<int>();
        int kaId = It.IsAny<int>();
        string uketukeTime = It.IsAny<string>();
        string sinStartTime = It.IsAny<string>();
        string sinEndTime = It.IsAny<string>();
        byte status = It.IsAny<byte>();
        List<OrdInfModel> odrInfs = new();
        KarteInfModel karteInfModel = new(hpId, raiinNo);
        int userId = It.IsAny<int>();
        List<FamilyModel> familyList = new() { new FamilyModel() };
        List<NextOrderModel> rsvkrtOrderInfModels = new() { new NextOrderModel() };
        SummaryInfModel summaryInfModel = new();
        ImportantNoteModel importantNoteModel = new();
        PatientInfoModel patientInfoModel = new();
        List<PtDiseaseModel> ptDiseaseModels = new();
        List<FlowSheetModel> flowSheetData = new();
        int seqNo = 999;
        MonshinInforModel monshin = new(hpId, ptId, raiinNo, sinDate, string.Empty, string.Empty, 0, 0, seqNo);
        bool rsvkrtNo = true;
        List<long> byomeiIdList = new List<long>() { It.IsAny<long>(), It.IsAny<long>() };

        mockITodayOdrRepository.Setup(finder => finder.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, odrInfs, karteInfModel, userId, status))
        .Returns((int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId, byte modeSaveData) => true);

        mockINextOrderRepository.Setup(finder => finder.Upsert(userId, hpId, ptId, rsvkrtOrderInfModels))
        .Returns((int userId, int hpId, long ptId, List<NextOrderModel> nextOrderModels) => rsvkrtNo);

        mockISpecialNoteRepository.Setup(finder => finder.SaveSpecialNote(hpId, ptId, sinDate, summaryInfModel, importantNoteModel, patientInfoModel, userId))
        .Returns((int hpId, long ptId, int sinDate, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, int userId) => true);

        mockIPtDiseaseRepository.Setup(finder => finder.Upsert(ptDiseaseModels, hpId, userId))
        .Returns((List<PtDiseaseModel> inputDatas, int hpId, int userId) => byomeiIdList);

        mockIFlowSheetRepository.Setup(finder => finder.UpsertTag(flowSheetData, hpId, userId));
        mockIFlowSheetRepository.Setup(finder => finder.UpsertCmt(flowSheetData, hpId, userId));

        mockIFamilyRepository.Setup(finder => finder.SaveFamilyList(hpId, userId, familyList))
        .Returns((int hpId, int userId, List<FamilyModel> familyList) => true);

        mockIMonshinInforRepository.Setup(finder => finder.SaveMonshinSheet(monshin))
        .Returns((MonshinInforModel monshin) => true);

        // Act
        var result = saveMedicalRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrInfs, karteInfModel, userId, familyList, rsvkrtOrderInfModels, summaryInfModel, importantNoteModel, patientInfoModel, ptDiseaseModels, flowSheetData, monshin);

        // Assert
        Assert.That(result);
    }

    [Test]
    public void TC_007_SaveMedicalRepository_TestUpsertMonshinSheetFalse()
    {
        //Setup Data Test

        var mockIFamilyRepository = new Mock<IFamilyRepository>();
        var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockINextOrderRepository = new Mock<INextOrderRepository>();
        var mockISpecialNoteRepository = new Mock<ISpecialNoteRepository>();
        var mockIPtDiseaseRepository = new Mock<IPtDiseaseRepository>();
        var mockIFlowSheetRepository = new Mock<IFlowSheetRepository>();
        var mockIMonshinInforRepository = new Mock<IMonshinInforRepository>();

        // Arrange
        var saveMedicalRepository = new SaveMedicalRepository(TenantProvider, mockIFamilyRepository.Object, mockITodayOdrRepository.Object, mockINextOrderRepository.Object, mockISpecialNoteRepository.Object, mockIPtDiseaseRepository.Object, mockIFlowSheetRepository.Object, mockIMonshinInforRepository.Object);

        // Mock data
        int hpId = 999;
        long ptId = 999;
        long raiinNo = 999;
        int sinDate = It.IsAny<int>();
        int syosaiKbn = It.IsAny<int>();
        int jikanKbn = It.IsAny<int>();
        int hokenPid = It.IsAny<int>();
        int santeiKbn = It.IsAny<int>();
        int tantoId = It.IsAny<int>();
        int kaId = It.IsAny<int>();
        string uketukeTime = It.IsAny<string>();
        string sinStartTime = It.IsAny<string>();
        string sinEndTime = It.IsAny<string>();
        byte status = It.IsAny<byte>();
        List<OrdInfModel> odrInfs = new();
        KarteInfModel karteInfModel = new(hpId, raiinNo);
        int userId = It.IsAny<int>();
        List<FamilyModel> familyList = new() { new FamilyModel() };
        List<NextOrderModel> rsvkrtOrderInfModels = new() { new NextOrderModel() };
        SummaryInfModel summaryInfModel = new();
        ImportantNoteModel importantNoteModel = new();
        PatientInfoModel patientInfoModel = new();
        List<PtDiseaseModel> ptDiseaseModels = new();
        List<FlowSheetModel> flowSheetData = new();
        int seqNo = 999;
        MonshinInforModel monshin = new(hpId, ptId, raiinNo, sinDate, string.Empty, string.Empty, 0, 0, seqNo);
        bool rsvkrtNo = true;
        List<long> byomeiIdList = new List<long>() { It.IsAny<long>(), It.IsAny<long>() };

        mockITodayOdrRepository.Setup(finder => finder.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, odrInfs, karteInfModel, userId, status))
        .Returns((int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId, byte modeSaveData) => true);

        mockINextOrderRepository.Setup(finder => finder.Upsert(userId, hpId, ptId, rsvkrtOrderInfModels))
        .Returns((int userId, int hpId, long ptId, List<NextOrderModel> nextOrderModels) => rsvkrtNo);

        mockISpecialNoteRepository.Setup(finder => finder.SaveSpecialNote(hpId, ptId, sinDate, summaryInfModel, importantNoteModel, patientInfoModel, userId))
        .Returns((int hpId, long ptId, int sinDate, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, int userId) => true);

        mockIPtDiseaseRepository.Setup(finder => finder.Upsert(ptDiseaseModels, hpId, userId))
        .Returns((List<PtDiseaseModel> inputDatas, int hpId, int userId) => byomeiIdList);

        mockIFlowSheetRepository.Setup(finder => finder.UpsertTag(flowSheetData, hpId, userId));
        mockIFlowSheetRepository.Setup(finder => finder.UpsertCmt(flowSheetData, hpId, userId));

        mockIFamilyRepository.Setup(finder => finder.SaveFamilyList(hpId, userId, familyList))
        .Returns((int hpId, int userId, List<FamilyModel> familyList) => true);

        mockIMonshinInforRepository.Setup(finder => finder.SaveMonshinSheet(monshin))
        .Returns((MonshinInforModel monshin) => false);

        // Act
        var result = saveMedicalRepository.Upsert(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, status, odrInfs, karteInfModel, userId, familyList, rsvkrtOrderInfModels, summaryInfModel, importantNoteModel, patientInfoModel, ptDiseaseModels, flowSheetData, monshin);

        // Assert
        Assert.That(result == false);
    }

    #endregion Upsert
}
