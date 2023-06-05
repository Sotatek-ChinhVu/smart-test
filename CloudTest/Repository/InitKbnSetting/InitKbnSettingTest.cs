using CloudUnitTest.SampleData;
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

    private bool CompareListRaiinKubunMst(long ptId, long raiinNo, int sinDate, List<RaiinKbnModel> resultQuery, List<RaiinKbnMst> raiinKbnMstList, List<RaiinKbnDetail> raiinKbnDetailList, List<RaiinKbnInf> raiinKbnInflList)
    {
        var result = resultQuery.FirstOrDefault();
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

        return true;
    }
}
