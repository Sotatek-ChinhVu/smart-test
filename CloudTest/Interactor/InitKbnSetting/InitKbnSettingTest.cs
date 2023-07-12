using CloudUnitTest.SampleData;
using Domain.Models.NextOrder;
using Domain.Models.OrdInfs;
using Domain.Models.RaiinKubunMst;
using Domain.Models.TodayOdr;
using Helper.Enum;
using Infrastructure.Repositories;
using Interactor.MedicalExamination;
using Moq;
using UseCase.MedicalExamination.InitKbnSetting;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace CloudUnitTest.Interactor.InitKbnSetting;

public class InitKbnSettingTest : BaseUT
{
    #region Test Validate
    [Test]
    public void InvalidHpId_TestSuccess()
    {
        // Arrange
        var mockTodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockRaiinKubunMstRepository = new Mock<IRaiinKubunMstRepository>();
        var mockNextOrderRepository = new Mock<INextOrderRepository>();

        var interactor = new InitKbnSettingInteractor(mockTodayOdrRepository.Object, mockRaiinKubunMstRepository.Object, mockNextOrderRepository.Object);

        // Act
        int hpId = 0;
        long ptId = 123456789;
        long raiinNo = 999999999;
        int sinDate = 22221212;


        InitKbnSettingInputData inputData = new InitKbnSettingInputData(
                                                                            hpId,
                                                                            WindowType.ReceptionInInsertMode,
                                                                            0,
                                                                            true,
                                                                            ptId,
                                                                            raiinNo,
                                                                            sinDate,
                                                                            new()
                                                                        );
        var resultQuery = interactor.Handle(inputData);

        // Assert
        Assert.True(resultQuery.Status == InitKbnSettingStatus.InvalidHpId);
    }

    [Test]
    public void InvalidFrameId_TestSuccess()
    {
        // Arrange
        var mockTodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockRaiinKubunMstRepository = new Mock<IRaiinKubunMstRepository>();
        var mockNextOrderRepository = new Mock<INextOrderRepository>();

        var interactor = new InitKbnSettingInteractor(mockTodayOdrRepository.Object, mockRaiinKubunMstRepository.Object, mockNextOrderRepository.Object);

        // Act
        int hpId = 1;
        long ptId = 123456789;
        long raiinNo = 999999999;
        int sinDate = 22221212;
        int frameId = -1;

        InitKbnSettingInputData inputData = new InitKbnSettingInputData(
                                                                            hpId,
                                                                            WindowType.ReceptionInInsertMode,
                                                                            frameId,
                                                                            true,
                                                                            ptId,
                                                                            raiinNo,
                                                                            sinDate,
                                                                            new()
                                                                        );
        var resultQuery = interactor.Handle(inputData);

        // Assert
        Assert.True(resultQuery.Status == InitKbnSettingStatus.InvalidFrameId);
    }

    [Test]
    public void InvalidPtId_TestSuccess()
    {
        // Arrange
        var mockTodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockRaiinKubunMstRepository = new Mock<IRaiinKubunMstRepository>();
        var mockNextOrderRepository = new Mock<INextOrderRepository>();

        var interactor = new InitKbnSettingInteractor(mockTodayOdrRepository.Object, mockRaiinKubunMstRepository.Object, mockNextOrderRepository.Object);

        // Act
        int hpId = 1;
        long ptId = 0;
        long raiinNo = 999999999;
        int sinDate = 22221212;
        int frameId = 0;

        InitKbnSettingInputData inputData = new InitKbnSettingInputData(
                                                                            hpId,
                                                                            WindowType.ReceptionInInsertMode,
                                                                            frameId,
                                                                            true,
                                                                            ptId,
                                                                            raiinNo,
                                                                            sinDate,
                                                                            new()
                                                                        );
        var resultQuery = interactor.Handle(inputData);

        // Assert
        Assert.True(resultQuery.Status == InitKbnSettingStatus.InvalidPtId);
    }

    [Test]
    public void InvalidRaiinNo_TestSuccess()
    {
        // Arrange
        var mockTodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockRaiinKubunMstRepository = new Mock<IRaiinKubunMstRepository>();
        var mockNextOrderRepository = new Mock<INextOrderRepository>();

        var interactor = new InitKbnSettingInteractor(mockTodayOdrRepository.Object, mockRaiinKubunMstRepository.Object, mockNextOrderRepository.Object);

        // Act
        int hpId = 1;
        long ptId = 123456789;
        long raiinNo = 0;
        int sinDate = 22221212;
        int frameId = 0;

        InitKbnSettingInputData inputData = new InitKbnSettingInputData(
                                                                            hpId,
                                                                            WindowType.ReceptionInInsertMode,
                                                                            frameId,
                                                                            true,
                                                                            ptId,
                                                                            raiinNo,
                                                                            sinDate,
                                                                            new()
                                                                        );
        var resultQuery = interactor.Handle(inputData);

        // Assert
        Assert.True(resultQuery.Status == InitKbnSettingStatus.InvalidRaiinNo);
    }

    [Test]
    public void InvalidSinDate_TestSuccess()
    {
        // Arrange
        var mockTodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockRaiinKubunMstRepository = new Mock<IRaiinKubunMstRepository>();
        var mockNextOrderRepository = new Mock<INextOrderRepository>();

        var interactor = new InitKbnSettingInteractor(mockTodayOdrRepository.Object, mockRaiinKubunMstRepository.Object, mockNextOrderRepository.Object);

        // Act
        int hpId = 1;
        long ptId = 123456789;
        long raiinNo = 999999999;
        int sinDate = 0;
        int frameId = 0;

        InitKbnSettingInputData inputData = new InitKbnSettingInputData(
                                                                            hpId,
                                                                            WindowType.ReceptionInInsertMode,
                                                                            frameId,
                                                                            true,
                                                                            ptId,
                                                                            raiinNo,
                                                                            sinDate,
                                                                            new()
                                                                        );
        var resultQuery = interactor.Handle(inputData);

        // Assert
        Assert.True(resultQuery.Status == InitKbnSettingStatus.InvalidSinDate);
    }

    #endregion

    #region Test handle
    [Test]
    public void InitDefaultByNextOrder_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        // RaiinKbnMst
        var raiinKbnMstList = ReadDataInitKbnSetting.ReadRaiinKbnMst();
        tenant.RaiinKbnMsts.AddRange(raiinKbnMstList);

        // RaiinKbnDetail
        var raiinKbnDetailList = ReadDataInitKbnSetting.ReadRaiinKbnDetail();
        tenant.RaiinKbnDetails.AddRange(raiinKbnDetailList);

        // RaiinKbnInf
        var raiinKbnInflList = ReadDataInitKbnSetting.ReadRaiinKbnInf();
        tenant.RaiinKbnInfs.AddRange(raiinKbnInflList);

        // RaiinKbnKoui
        var raiinKbnKouiList = ReadDataInitKbnSetting.ReadRaiinKbnKoui();
        tenant.RaiinKbnKouis.AddRange(raiinKbnKouiList);

        // KouiKbnMst
        var kouiKbnMstlList = ReadDataInitKbnSetting.ReadKouiKbnMst();
        tenant.KouiKbnMsts.AddRange(kouiKbnMstlList);

        // RaiinKbItem
        var raiinKbnItemList = ReadDataInitKbnSetting.ReadRaiinKbnItem();
        tenant.RaiinKbItems.AddRange(raiinKbnItemList);

        tenant.SaveChanges();
        #endregion

        // Arrange
        var mockTodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockRaiinKubunMstRepository = new Mock<IRaiinKubunMstRepository>();
        var mockNextOrderRepository = new Mock<INextOrderRepository>();

        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        var interactor = new InitKbnSettingInteractor(mockTodayOdrRepository.Object, mockRaiinKubunMstRepository.Object, mockNextOrderRepository.Object);

        // Act
        int hpId = 1;
        long ptId = 123456789;
        long raiinNo = 999999999;
        int sinDate = 22221212;

        var raiinKbnModels = raiinKubunMstRepository.GetRaiinKbns(hpId, ptId, raiinNo, sinDate);
        var raiinKouiKbns = raiinKubunMstRepository.GetRaiinKouiKbns(hpId);
        var raiinKbnItemCds = raiinKubunMstRepository.GetRaiinKbnItems(hpId);

        mockRaiinKubunMstRepository.Setup(repo => repo.GetRaiinKbns(hpId, ptId, raiinNo, sinDate))
                                   .Returns(raiinKbnModels);

        mockRaiinKubunMstRepository.Setup(repo => repo.GetRaiinKouiKbns(hpId))
                                   .Returns(raiinKouiKbns);

        mockRaiinKubunMstRepository.Setup(repo => repo.GetRaiinKbnItems(hpId))
                                   .Returns(raiinKbnItemCds);

        InitKbnSettingInputData inputData = new InitKbnSettingInputData(
                                                                            hpId,
                                                                            WindowType.ReceptionInInsertMode,
                                                                            0,
                                                                            true,
                                                                            ptId,
                                                                            raiinNo,
                                                                            sinDate,
                                                                            new()
                                                                        );
        var resultQuery = interactor.Handle(inputData);

        // Assert
        try
        {
            Assert.True(CompareInitDefault(resultQuery.RaiinKbnModels, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RaiinKbItems.RemoveRange(raiinKbnItemList);
            tenant.RaiinKbnKouis.RemoveRange(raiinKbnKouiList);
            tenant.KouiKbnMsts.RemoveRange(kouiKbnMstlList);
            tenant.RaiinKbnMsts.RemoveRange(raiinKbnMstList);
            tenant.RaiinKbnDetails.RemoveRange(raiinKbnDetailList);
            tenant.RaiinKbnInfs.RemoveRange(raiinKbnInflList);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void InitDefaultByTodayOrder_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        // RaiinKbnMst
        var raiinKbnMstList = ReadDataInitKbnSetting.ReadRaiinKbnMst();
        tenant.RaiinKbnMsts.AddRange(raiinKbnMstList);

        // RaiinKbnDetail
        var raiinKbnDetailList = ReadDataInitKbnSetting.ReadRaiinKbnDetail();
        tenant.RaiinKbnDetails.AddRange(raiinKbnDetailList);

        // RaiinKbnInf
        var raiinKbnInflList = ReadDataInitKbnSetting.ReadRaiinKbnInf();
        tenant.RaiinKbnInfs.AddRange(raiinKbnInflList);

        // RaiinKbnKoui
        var raiinKbnKouiList = ReadDataInitKbnSetting.ReadRaiinKbnKoui();
        tenant.RaiinKbnKouis.AddRange(raiinKbnKouiList);

        // KouiKbnMst
        var kouiKbnMstlList = ReadDataInitKbnSetting.ReadKouiKbnMst();
        tenant.KouiKbnMsts.AddRange(kouiKbnMstlList);

        // RaiinKbItem
        var raiinKbnItemList = ReadDataInitKbnSetting.ReadRaiinKbnItem();
        tenant.RaiinKbItems.AddRange(raiinKbnItemList);

        tenant.SaveChanges();
        #endregion

        // Arrange
        var mockTodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockRaiinKubunMstRepository = new Mock<IRaiinKubunMstRepository>();
        var mockNextOrderRepository = new Mock<INextOrderRepository>();

        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        var interactor = new InitKbnSettingInteractor(mockTodayOdrRepository.Object, mockRaiinKubunMstRepository.Object, mockNextOrderRepository.Object);

        // Act
        int hpId = 1;
        long ptId = 123456789;
        long raiinNo = 999999999;
        int sinDate = 22221212;

        var raiinKbnModels = raiinKubunMstRepository.GetRaiinKbns(hpId, ptId, raiinNo, sinDate);
        var raiinKouiKbns = raiinKubunMstRepository.GetRaiinKouiKbns(hpId);
        var raiinKbnItemCds = raiinKubunMstRepository.GetRaiinKbnItems(hpId);

        mockRaiinKubunMstRepository.Setup(repo => repo.GetRaiinKbns(hpId, ptId, raiinNo, sinDate))
                                   .Returns(raiinKbnModels);

        mockRaiinKubunMstRepository.Setup(repo => repo.GetRaiinKouiKbns(hpId))
                                   .Returns(raiinKouiKbns);

        mockRaiinKubunMstRepository.Setup(repo => repo.GetRaiinKbnItems(hpId))
                                   .Returns(raiinKbnItemCds);

        List<OdrInfItemInputData> orderInfItems = new() {
                    new OdrInfItemInputData(
                        1,
                        raiinNo,
                        99,
                        99,
                        ptId,
                        sinDate,
                        1,
                        999,
                        "",
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        999999999,
                        new(){
                            new OdrInfDetailItemInputData(
                                1,
                                raiinNo,
                                99,
                                99,
                                99,
                                ptId,
                                sinDate,
                                999,
                                "613120001",
                                "",
                                0,
                                "",
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                "",
                                0,
                                "",
                                "",
                                0,
                                DateTime.MinValue,
                                0,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                0
                             )
                        },
                        0
                    )};

        InitKbnSettingInputData inputData = new InitKbnSettingInputData(
                                                                            hpId,
                                                                            WindowType.MedicalExamination,
                                                                            0,
                                                                            true,
                                                                            ptId,
                                                                            raiinNo,
                                                                            sinDate,
                                                                            orderInfItems
                                                                        );
        var resultQuery = interactor.Handle(inputData);

        // Assert
        try
        {
            Assert.True(CompareInitDefault(resultQuery.RaiinKbnModels, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RaiinKbItems.RemoveRange(raiinKbnItemList);
            tenant.RaiinKbnKouis.RemoveRange(raiinKbnKouiList);
            tenant.KouiKbnMsts.RemoveRange(kouiKbnMstlList);
            tenant.RaiinKbnMsts.RemoveRange(raiinKbnMstList);
            tenant.RaiinKbnDetails.RemoveRange(raiinKbnDetailList);
            tenant.RaiinKbnInfs.RemoveRange(raiinKbnInflList);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void InitDefaultByRsv_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        // RaiinKbnMst
        var raiinKbnMstList = ReadDataInitKbnSetting.ReadRaiinKbnMst();
        tenant.RaiinKbnMsts.AddRange(raiinKbnMstList);

        // RaiinKbnDetail
        var raiinKbnDetailList = ReadDataInitKbnSetting.ReadRaiinKbnDetail();
        tenant.RaiinKbnDetails.AddRange(raiinKbnDetailList);

        // RaiinKbnInf
        var raiinKbnInflList = ReadDataInitKbnSetting.ReadRaiinKbnInf();
        tenant.RaiinKbnInfs.AddRange(raiinKbnInflList);

        // RaiinKbnKoui
        var raiinKbnKouiList = ReadDataInitKbnSetting.ReadRaiinKbnKoui();
        tenant.RaiinKbnKouis.AddRange(raiinKbnKouiList);

        // KouiKbnMst
        var kouiKbnMstlList = ReadDataInitKbnSetting.ReadKouiKbnMst();
        tenant.KouiKbnMsts.AddRange(kouiKbnMstlList);

        // RaiinKbItem
        var raiinKbnItemList = ReadDataInitKbnSetting.ReadRaiinKbnItem();
        tenant.RaiinKbItems.AddRange(raiinKbnItemList);

        tenant.SaveChanges();
        #endregion

        // Arrange
        var mockTodayOdrRepository = new Mock<ITodayOdrRepository>();
        var mockRaiinKubunMstRepository = new Mock<IRaiinKubunMstRepository>();
        var mockNextOrderRepository = new Mock<INextOrderRepository>();

        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        var interactor = new InitKbnSettingInteractor(mockTodayOdrRepository.Object, mockRaiinKubunMstRepository.Object, mockNextOrderRepository.Object);

        // Act
        int hpId = 1;
        long ptId = 123456789;
        long raiinNo = 999999999;
        int sinDate = 22221212;
        int frameID = 12345;

        var raiinKbnModels = raiinKubunMstRepository.GetRaiinKbns(hpId, ptId, raiinNo, sinDate);
        var raiinKouiKbns = raiinKubunMstRepository.GetRaiinKouiKbns(hpId);
        var raiinKbnItemCds = raiinKubunMstRepository.GetRaiinKbnItems(hpId);

        mockRaiinKubunMstRepository.Setup(repo => repo.GetRaiinKbns(hpId, ptId, raiinNo, sinDate))
                                   .Returns(raiinKbnModels);

        mockRaiinKubunMstRepository.Setup(repo => repo.GetRaiinKouiKbns(hpId))
                                   .Returns(raiinKouiKbns);

        mockRaiinKubunMstRepository.Setup(repo => repo.GetRaiinKbnItems(hpId))
                                   .Returns(raiinKbnItemCds);

        InitKbnSettingInputData inputData = new InitKbnSettingInputData(
                                                                            hpId,
                                                                            WindowType.Booking,
                                                                            frameID,
                                                                            true,
                                                                            ptId,
                                                                            raiinNo,
                                                                            sinDate,
                                                                            new()
                                                                        );
        var resultQuery = interactor.Handle(inputData);

        // Assert
        try
        {
            Assert.True(CompareInitDefault(resultQuery.RaiinKbnModels, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RaiinKbItems.RemoveRange(raiinKbnItemList);
            tenant.RaiinKbnKouis.RemoveRange(raiinKbnKouiList);
            tenant.KouiKbnMsts.RemoveRange(kouiKbnMstlList);
            tenant.RaiinKbnMsts.RemoveRange(raiinKbnMstList);
            tenant.RaiinKbnDetails.RemoveRange(raiinKbnDetailList);
            tenant.RaiinKbnInfs.RemoveRange(raiinKbnInflList);
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion

    private bool CompareInitDefault(List<RaiinKbnModel> resultQuery, List<RaiinKbnModel> raiinKbnModels, List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns, List<RaiinKbnItemModel> raiinKbnItemCds)
    {
        var grpCd = raiinKbnModels.FirstOrDefault()?.GrpCd ?? 0;
        var raiinKbnModel = raiinKbnModels.FirstOrDefault(item => item.GrpCd == grpCd);
        if (raiinKbnModel == null)
        {
            return false;
        }
        var resultTest = resultQuery.FirstOrDefault(item => item.GrpCd == grpCd);
        if (resultTest == null)
        {
            return false;
        }
        if (resultTest.HpId != 1)
        {
            return false;
        }
        else if (resultTest.GrpCd != raiinKbnModel.GrpCd)
        {
            return false;
        }
        else if (resultTest.SortNo != raiinKbnModel.SortNo)
        {
            return false;
        }
        else if (resultTest.GrpName != raiinKbnModel.GrpName)
        {
            return false;
        }
        else if (resultTest.IsDeleted != raiinKbnModel.IsDeleted)
        {
            return false;
        }

        var raiinKbnInfModel = resultTest.RaiinKbnInfModel;
        if (raiinKbnInfModel == null)
        {
            return false;
        }
        else if (raiinKbnInfModel.HpId != raiinKbnModel.RaiinKbnInfModel.HpId)
        {
            return false;
        }
        else if (raiinKbnInfModel.PtId != raiinKbnModel.RaiinKbnInfModel.PtId)
        {
            return false;
        }
        else if (raiinKbnInfModel.SinDate != raiinKbnModel.RaiinKbnInfModel.SinDate)
        {
            return false;
        }
        else if (raiinKbnInfModel.RaiinNo != raiinKbnModel.RaiinKbnInfModel.RaiinNo)
        {
            return false;
        }
        else if (raiinKbnInfModel.GrpId != raiinKbnModel.GrpCd)
        {
            return false;
        }
        else if (raiinKbnInfModel.SeqNo != raiinKbnModel.RaiinKbnInfModel.SeqNo)
        {
            return false;
        }
        else if (raiinKbnInfModel.KbnCd != raiinKbnModel.RaiinKbnInfModel.KbnCd)
        {
            return false;
        }
        else if (raiinKbnInfModel.IsDelete != raiinKbnModel.RaiinKbnInfModel.IsDelete)
        {
            return false;
        }

        var raiinKbnDetailModel = resultTest.RaiinKbnDetailModels.FirstOrDefault();
        if (raiinKbnDetailModel == null)
        {
            return false;
        }
        var raiinKbnDetail = raiinKbnModel.RaiinKbnDetailModels.FirstOrDefault();
        if (raiinKbnDetail == null)
        {
            return false;
        }
        else if (raiinKbnDetailModel.HpId != raiinKbnDetail.HpId)
        {
            return false;
        }
        else if (raiinKbnDetailModel.GrpCd != raiinKbnDetail.GrpCd)
        {
            return false;
        }
        else if (raiinKbnDetailModel.KbnCd != raiinKbnDetail.KbnCd)
        {
            return false;
        }
        else if (raiinKbnDetailModel.SortNo != raiinKbnDetail.SortNo)
        {
            return false;
        }
        else if (raiinKbnDetailModel.KbnName != raiinKbnDetail.KbnName)
        {
            return false;
        }
        else if (raiinKbnDetailModel.ColorCd != raiinKbnDetail.ColorCd)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsConfirmed != raiinKbnDetail.IsConfirmed)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsAuto != raiinKbnDetail.IsAuto)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsAutoDelete != raiinKbnDetail.IsAutoDelete)
        {
            return false;
        }
        else if (raiinKbnDetailModel.IsDeleted != raiinKbnDetail.IsDeleted)
        {
            return false;
        }
        return true;
    }
}
