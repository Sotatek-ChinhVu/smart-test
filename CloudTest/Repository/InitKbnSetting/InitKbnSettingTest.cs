using CloudUnitTest.SampleData;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.RaiinKubunMst;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.InitKbnSetting;

public class InitKbnSettingTest : BaseUT
{
    [Test]
    public void GetRaiinKbns_TestSuccess()
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

        tenant.SaveChanges();
        #endregion

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);

        // Act
        long ptId = 123456789;
        long raiinNo = 999999999;
        int sindate = 22221212;
        var resultQuery = raiinKubunMstRepository.GetRaiinKbns(1, ptId, raiinNo, sindate);

        // Assert
        try
        {
            Assert.True(CompareListRaiinKubunMst(ptId, raiinNo, sindate, resultQuery, raiinKbnMstList, raiinKbnDetailList, raiinKbnInflList));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RaiinKbnMsts.RemoveRange(raiinKbnMstList);
            tenant.RaiinKbnDetails.RemoveRange(raiinKbnDetailList);
            tenant.RaiinKbnInfs.RemoveRange(raiinKbnInflList);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void GetRaiinKouiKbns_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        // RaiinKbnKoui
        var raiinKbnKouiList = ReadDataInitKbnSetting.ReadRaiinKbnKoui();
        tenant.RaiinKbnKouis.AddRange(raiinKbnKouiList);

        // KouiKbnMst
        var kouiKbnMstlList = ReadDataInitKbnSetting.ReadKouiKbnMst();
        tenant.KouiKbnMsts.AddRange(kouiKbnMstlList);

        tenant.SaveChanges();
        #endregion

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);

        // Act
        var resultQuery = raiinKubunMstRepository.GetRaiinKouiKbns(1);

        // Assert
        try
        {
            Assert.True(CompareListRaiinKouiKbn(resultQuery, raiinKbnKouiList, kouiKbnMstlList));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RaiinKbnKouis.RemoveRange(raiinKbnKouiList);
            tenant.KouiKbnMsts.RemoveRange(kouiKbnMstlList);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void GetRaiinKbnItems_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        // RaiinKbItem
        var raiinKbnItemList = ReadDataInitKbnSetting.ReadRaiinKbnItem();
        tenant.RaiinKbItems.AddRange(raiinKbnItemList);

        tenant.SaveChanges();
        #endregion

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);

        // Act
        var resultQuery = raiinKubunMstRepository.GetRaiinKbnItems(1);

        // Assert
        try
        {
            Assert.True(CompareListRaiinKbnItem(resultQuery, raiinKbnItemList));
        }
        finally
        {
            #region Remove Data Fetch
            raiinKubunMstRepository.ReleaseResource();
            tenant.RaiinKbItems.RemoveRange(raiinKbnItemList);
            tenant.SaveChanges();
            #endregion
        }
    }

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
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        NextOrderRepository nextOrderRepository = new NextOrderRepository(TenantProvider);

        // Act
        int hpId = 1;
        long ptId = 123456789;
        long raiinNo = 999999999;
        int sinDate = 22221212;

        var raiinKbnModels = raiinKubunMstRepository.GetRaiinKbns(hpId, ptId, raiinNo, sinDate);
        var raiinKouiKbns = raiinKubunMstRepository.GetRaiinKouiKbns(hpId);
        var raiinKbnItemCds = raiinKubunMstRepository.GetRaiinKbnItems(hpId);

        var resultQuery = nextOrderRepository.InitDefaultByNextOrder(hpId, ptId, sinDate, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds);

        // Assert
        try
        {
            Assert.True(CompareInitDefault(resultQuery, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds));
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
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);
        TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider);

        // Act
        int hpId = 1;
        long ptId = 123456789;
        long raiinNo = 999999999;
        int sinDate = 22221212;

        var raiinKbnModels = raiinKubunMstRepository.GetRaiinKbns(hpId, ptId, raiinNo, sinDate);
        var raiinKouiKbns = raiinKubunMstRepository.GetRaiinKouiKbns(hpId);
        var raiinKbnItemCds = raiinKubunMstRepository.GetRaiinKbnItems(hpId);

        List<OrdInfModel> orderInfItems = new() {
                    new OrdInfModel(
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
                        0,
                        999999999,
                        new(){
                            new OrdInfDetailModel(
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
                                string.Empty,
                                0,
                                DateTime.MinValue,
                                0,
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
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
                                "",
                                new List<YohoSetMstModel>(),
                                0,
                                0,
                                "",
                                "",
                                "",
                                "")
                        },
                        DateTime.MinValue,
                        0,
                        "",
                        DateTime.MinValue,
                        0,
                        "",
                        string.Empty,
                        string.Empty
                    )};

        var resultQuery = todayOdrRepository.InitDefaultByTodayOrder(raiinKbnModels, raiinKouiKbns, raiinKbnItemCds, orderInfItems);

        // Assert
        try
        {
            Assert.True(CompareInitDefault(resultQuery, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds));
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

        // RaiinKbnYayoku
        var raiinKbnYayokuList = ReadDataInitKbnSetting.ReadRaiinKbnYayoku();
        tenant.RaiinKbnYayokus.AddRange(raiinKbnYayokuList);

        tenant.SaveChanges();
        #endregion

        // Arrange
        RaiinKubunMstRepository raiinKubunMstRepository = new RaiinKubunMstRepository(TenantProvider);

        // Act
        int hpId = 1;
        long ptId = 123456789;
        long raiinNo = 999999999;
        int sinDate = 22221212;

        var raiinKbnModels = raiinKubunMstRepository.GetRaiinKbns(hpId, ptId, raiinNo, sinDate);
        var raiinKouiKbns = raiinKubunMstRepository.GetRaiinKouiKbns(hpId);
        var raiinKbnItemCds = raiinKubunMstRepository.GetRaiinKbnItems(hpId);

        int frameID = 12345;
        var resultQuery = raiinKubunMstRepository.InitDefaultByRsv(hpId, frameID, raiinKbnModels);

        // Assert
        try
        {
            Assert.True(CompareInitDefault(resultQuery, raiinKbnModels, raiinKouiKbns, raiinKbnItemCds));
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
            tenant.RaiinKbnYayokus.RemoveRange(raiinKbnYayokuList);
            tenant.SaveChanges();
            #endregion
        }
    }

    #region private function

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

    private bool CompareListRaiinKbnItem(List<RaiinKbnItemModel> resultQuery, List<RaiinKbItem> raiinKbnItemList)
    {
        int id = raiinKbnItemList.FirstOrDefault()?.GrpCd ?? 0;
        var result = resultQuery.FirstOrDefault(item => item.GrpCd == id);
        if (result == null)
        {
            return false;
        }
        var raiinKbnItem = raiinKbnItemList.FirstOrDefault();
        if (raiinKbnItem == null)
        {
            return false;
        }
        else if (result.HpId != raiinKbnItem.HpId)
        {
            return false;
        }
        else if (result.GrpCd != raiinKbnItem.GrpCd)
        {
            return false;
        }
        else if (result.KbnCd != raiinKbnItem.KbnCd)
        {
            return false;
        }
        else if (result.SeqNo != raiinKbnItem.SeqNo)
        {
            return false;
        }
        else if (result.ItemCd != raiinKbnItem.ItemCd)
        {
            return false;
        }
        else if (result.IsExclude != raiinKbnItem.IsExclude)
        {
            return false;
        }
        else if (result.SortNo != raiinKbnItem.SortNo)
        {
            return false;
        }
        return true;
    }

    private bool CompareListRaiinKubunMst(long ptId, long raiinNo, int sinDate, List<RaiinKbnModel> resultQuery, List<RaiinKbnMst> raiinKbnMstList, List<RaiinKbnDetail> raiinKbnDetailList, List<RaiinKbnInf> raiinKbnInflList)
    {
        int id = raiinKbnMstList.FirstOrDefault()?.GrpCd ?? 0;
        var result = resultQuery.FirstOrDefault(item => item.GrpCd == id);
        if (result == null)
        {
            return false;
        }
        var raiinKbnMst = raiinKbnMstList.FirstOrDefault();
        var raiinKbnDetail = raiinKbnDetailList.FirstOrDefault();
        var raiinKbnInf = raiinKbnInflList.FirstOrDefault();
        if (raiinKbnMst == null || raiinKbnDetail == null || raiinKbnInf == null)
        {
            return false;
        }
        if (result.HpId != 1)
        {
            return false;
        }
        else if (result.GrpCd != raiinKbnMst.GrpCd)
        {
            return false;
        }
        else if (result.SortNo != raiinKbnMst.SortNo)
        {
            return false;
        }
        else if (result.GrpName != raiinKbnMst.GrpName)
        {
            return false;
        }
        else if (result.IsDeleted != raiinKbnMst.IsDeleted)
        {
            return false;
        }

        var raiinKbnInfModel = result.RaiinKbnInfModel;
        if (raiinKbnInfModel == null)
        {
            return false;
        }
        else if (raiinKbnInfModel.HpId != raiinKbnInf.HpId)
        {
            return false;
        }
        else if (raiinKbnInfModel.PtId != ptId)
        {
            return false;
        }
        else if (raiinKbnInfModel.SinDate != sinDate)
        {
            return false;
        }
        else if (raiinKbnInfModel.RaiinNo != raiinNo)
        {
            return false;
        }
        else if (raiinKbnInfModel.GrpId != raiinKbnMst.GrpCd)
        {
            return false;
        }
        else if (raiinKbnInfModel.SeqNo != raiinKbnInf.SeqNo)
        {
            return false;
        }
        else if (raiinKbnInfModel.KbnCd != raiinKbnInf.KbnCd)
        {
            return false;
        }
        else if (raiinKbnInfModel.IsDelete != raiinKbnInf.IsDelete)
        {
            return false;
        }

        var raiinKbnDetailModel = result.RaiinKbnDetailModels.FirstOrDefault();
        if (raiinKbnDetailModel == null)
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

    private bool CompareListRaiinKouiKbn(List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> resultQuery, List<RaiinKbnKoui> raiinKbnKouiList, List<KouiKbnMst> kouiKbnMstlList)
    {
        int grpId = raiinKbnKouiList.FirstOrDefault()?.GrpId ?? 0;
        int kbnCd = raiinKbnKouiList.FirstOrDefault()?.KbnCd ?? 0;
        int kouiKbn1 = kouiKbnMstlList.FirstOrDefault()?.KouiKbn1 ?? 0;
        int kouiKbn2 = kouiKbnMstlList.FirstOrDefault()?.KouiKbn2 ?? 0;
        var result = resultQuery.FirstOrDefault(item => item.grpId == grpId
                                                        && item.kbnCd == kbnCd
                                                        && item.kouiKbn1 == kouiKbn1
                                                        && item.kouiKbn2 == kouiKbn2);
        if (result.grpId != grpId)
        {
            return false;
        }

        return true;
    }
    #endregion
}
