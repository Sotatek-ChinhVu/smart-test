using CloudUnitTest.SampleData;
using Domain.Models.Santei;
using Entity.Tenant;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using PostgreDataContext;

namespace CloudUnitTest.Repository.Santei;

public class SanteiInfRepositoryTest : BaseUT
{
    #region Get List SanteiInf
    [Test]
    public void GetListSanteiInf_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();

        // SanteiInf
        var santeiInfs = ReadDataSanteiInf.ReadSanteiInf();
        tenant.SanteiInfs.AddRange(santeiInfs);

        // SanteiInfDetail
        var santeiInfDetails = ReadDataSanteiInf.ReadSanteiInfDetail();
        tenant.SanteiInfDetails.AddRange(santeiInfDetails);

        // OrderInf
        var orderInfs = ReadDataSanteiInf.ReadOrderInf();
        tenant.OdrInfs.AddRange(orderInfs);

        // OrderInfDetail
        var orderInfDetails = ReadDataSanteiInf.ReadOrderInfDetail();
        tenant.OdrInfDetails.AddRange(orderInfDetails);

        // TenMst
        var tenMsts = ReadDataSanteiInf.ReadTenMst();
        tenant.TenMsts.AddRange(tenMsts);
        tenant.SaveChanges();
        #endregion

        // Arrange
        SanteiInfRepository santeiInfRepository = new SanteiInfRepository(TenantProvider);

        // Act
        long ptId = 123456789;
        var resultQuery = santeiInfRepository.GetListSanteiInf(1, ptId, 20221212);

        // Assert
        try
        {
            Assert.True(CompareListSanteiInf(ptId, resultQuery, santeiInfs, santeiInfDetails, orderInfs, orderInfDetails, tenMsts));
        }
        finally
        {
            #region Remove Data Fetch
            santeiInfRepository.ReleaseResource();
            tenant.SanteiInfs.RemoveRange(santeiInfs);
            tenant.SanteiInfDetails.RemoveRange(santeiInfDetails);
            tenant.OdrInfs.RemoveRange(orderInfs);
            tenant.OdrInfDetails.RemoveRange(orderInfDetails);
            tenant.TenMsts.RemoveRange(tenMsts);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void GetListSanteiInfDetail_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();
        // SanteiInfDetail
        var santeiInfDetails = ReadDataSanteiInf.ReadSanteiInfDetail();
        tenant.SanteiInfDetails.AddRange(santeiInfDetails);
        tenant.SaveChanges();
        #endregion

        // Arrange
        SanteiInfRepository santeiInfRepository = new SanteiInfRepository(TenantProvider);

        // Act
        long ptId = 123456789;
        var resultQuery = santeiInfRepository.GetListSanteiInfDetails(1, ptId);

        // Assert
        try
        {
            bool result = false;
            var santeiInfDetailModel = resultQuery.FirstOrDefault();
            if (santeiInfDetailModel == null)
            {
                result = false;
            }
            else
            {
                result = CompareListSanteiInfDetail(ptId, santeiInfDetailModel, santeiInfDetails);
            }
            Assert.True(result);
        }
        finally
        {
            #region Remove Data Fetch
            santeiInfRepository.ReleaseResource();
            tenant.SanteiInfDetails.RemoveRange(santeiInfDetails);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void GetOnlyListSanteiInf_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();
        // SanteiInfDetail
        var santeiInfs = ReadDataSanteiInf.ReadSanteiInf();
        tenant.SanteiInfs.AddRange(santeiInfs);
        tenant.SaveChanges();
        #endregion

        // Arrange
        SanteiInfRepository santeiInfRepository = new SanteiInfRepository(TenantProvider);

        // Act
        long ptId = 123456789;
        var resultQuery = santeiInfRepository.GetOnlyListSanteiInf(1, ptId);

        // Assert
        try
        {
            Assert.True(CompareOnlySanteiInf(ptId, resultQuery, santeiInfs));
        }
        finally
        {
            #region Remove Data Fetch
            santeiInfRepository.ReleaseResource();
            tenant.SanteiInfs.RemoveRange(santeiInfs);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void CheckExistItemCd_TestSuccess()
    {
        #region Fetch data
        var tenant = TenantProvider.GetNoTrackingDataContext();
        // SanteiInfDetail
        var tenMsts = ReadDataSanteiInf.ReadTenMst();
        tenant.TenMsts.AddRange(tenMsts);
        tenant.SaveChanges();
        #endregion

        // Arrange
        SanteiInfRepository santeiInfRepository = new SanteiInfRepository(TenantProvider);

        // Act
        bool result = false;
        List<string> listItemCds = tenMsts.Select(item => item.ItemCd ?? string.Empty).ToList();
        result = santeiInfRepository.CheckExistItemCd(1, listItemCds);

        // Assert
        try
        {
            Assert.True(result);
        }
        finally
        {
            #region Remove Data Fetch
            santeiInfRepository.ReleaseResource();
            tenant.TenMsts.RemoveRange(tenMsts);
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion

    #region SaveSantei
    [Test]
    public void SaveSantei_TestCreateNewSuccess()
    {
        Random random = new();
        long ptId = long.MaxValue;
        string itemCd = "ItemCdTest";
        int alertDays = int.MaxValue;
        int alertTerm = random.Next(1, 6);
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var listSanteiModels = new List<SanteiInfModel>() { new SanteiInfModel(
                                                                                    0,
                                                                                    ptId,
                                                                                    itemCd,
                                                                                    alertDays,
                                                                                    alertTerm,
                                                                                    new(),
                                                                                    false
                                                                                )
                                                            };

        // Arrange
        SanteiInfRepository santeiInfRepository = new SanteiInfRepository(TenantProvider);

        // Act
        var resultComman = santeiInfRepository.SaveSantei(1, 1, listSanteiModels);
        var santeiInf = tenant.SanteiInfs.OrderBy(item => item.Id)
                                         .LastOrDefault(item => item.PtId == ptId
                                                             && item.AlertDays == alertDays
                                                             && item.AlertTerm == alertTerm
                                                             && item.ItemCd == itemCd);
        var listSanteiInfs = new List<SanteiInf>() { santeiInf ?? new SanteiInf() };
        if (listSanteiInfs == null)
        {
            resultComman = false;
        }
        else
        {
            listSanteiModels = new List<SanteiInfModel>() { new SanteiInfModel(
                                                                                    santeiInf != null ? santeiInf.Id : 0,
                                                                                    ptId,
                                                                                    itemCd,
                                                                                    alertDays,
                                                                                    alertTerm,
                                                                                    new(),
                                                                                    false
                                                                                )
                                                            };
            resultComman = CompareOnlySanteiInf(ptId, listSanteiModels, listSanteiInfs);
        }

        // Assert
        try
        {
            Assert.True(resultComman);
        }
        finally
        {
            #region Remove Data Fetch
            santeiInfRepository.ReleaseResource();
            if (listSanteiInfs != null)
            {
                tenant.SanteiInfs.RemoveRange(listSanteiInfs);
                tenant.SaveChanges();
            }
            #endregion
        }
    }

    [Test]
    public void SaveSantei_TestUpdateSuccess()
    {
        Random random = new();
        long ptId = long.MaxValue;
        string itemCd = "ItemCdTest";
        int alertDays = int.MaxValue;
        int alertTerm = random.Next(1, 6);
        int newAlertDays = random.Next(int.MaxValue);
        int newAlertTerm = random.Next(1, 6);
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var santeiInfUnitTest = new SanteiInf()
        {
            HpId = 1,
            PtId = ptId,
            ItemCd = itemCd,
            AlertDays = alertDays,
            AlertTerm = alertTerm,
            CreateDate = DateTime.UtcNow,
            CreateId = 1,
            UpdateDate = DateTime.UtcNow,
            UpdateId = 1
        };
        tenant.SanteiInfs.Add(santeiInfUnitTest);
        tenant.SaveChanges();
        // Arrange
        SanteiInfRepository santeiInfRepository = new SanteiInfRepository(TenantProvider);

        // Act
        var listSanteiUpdateModels = new List<SanteiInfModel>() { new SanteiInfModel(
                                                                                    santeiInfUnitTest.Id,
                                                                                    ptId,
                                                                                    itemCd,
                                                                                    newAlertDays,
                                                                                    newAlertTerm,
                                                                                    new(),
                                                                                    false
                                                                                )
                                                            };
        var resultComman = santeiInfRepository.SaveSantei(1, 1, listSanteiUpdateModels);
        var santeiInfCheck = tenant.SanteiInfs.OrderBy(item => item.Id)
                                              .LastOrDefault(item => item.Id == santeiInfUnitTest.Id);
        var listSanteiInfChecks = new List<SanteiInf>() { santeiInfCheck ?? new SanteiInf() };
        if (listSanteiInfChecks == null)
        {
            resultComman = false;
        }
        else
        {
            resultComman = CompareOnlySanteiInf(ptId, listSanteiUpdateModels, listSanteiInfChecks);
        }

        // Assert
        try
        {
            Assert.True(resultComman);
        }
        finally
        {
            santeiInfRepository.ReleaseResource();
            #region Remove Data Fetch
            tenant.SanteiInfs.Remove(santeiInfUnitTest);
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void SaveSantei_TestDeleteSuccess()
    {
        Random random = new();
        long ptId = long.MaxValue;
        string itemCd = "ItemCdTest";
        int alertDays = int.MaxValue;
        int alertTerm = random.Next(1, 6);
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var santeiInfUnitTest = new SanteiInf()
        {
            HpId = 1,
            PtId = ptId,
            ItemCd = itemCd,
            AlertDays = alertDays,
            AlertTerm = alertTerm,
            CreateDate = DateTime.UtcNow,
            CreateId = 1,
            UpdateDate = DateTime.UtcNow,
            UpdateId = 1
        };
        tenant.SanteiInfs.Add(santeiInfUnitTest);
        tenant.SaveChanges();
        // Arrange
        SanteiInfRepository santeiInfRepository = new SanteiInfRepository(TenantProvider);

        // Act
        var listSanteiUpdateModels = new List<SanteiInfModel>() { new SanteiInfModel(
                                                                                    santeiInfUnitTest.Id,
                                                                                    ptId,
                                                                                    itemCd,
                                                                                    alertDays,
                                                                                    alertTerm,
                                                                                    new(),
                                                                                    true
                                                                                )
                                                            };
        var resultComman = santeiInfRepository.SaveSantei(1, 1, listSanteiUpdateModels);
        var santeiInfCheck = tenant.SanteiInfs.OrderBy(item => item.Id)
                                              .LastOrDefault(item => item.Id == santeiInfUnitTest.Id);
        // Assert
        try
        {
            Assert.True(resultComman && santeiInfCheck == null);
        }
        finally
        {
            santeiInfRepository.ReleaseResource();
            #region Remove Data Fetch
            if (santeiInfCheck != null)
            {
                tenant.SanteiInfs.Remove(santeiInfUnitTest);
                tenant.SaveChanges();
            }
            #endregion
        }
    }

    [Test]
    public void SaveListSanteiInfDetail_TestCreateNewSuccess()
    {
        Random random = new();
        long ptId = long.MaxValue;
        string itemCd = "ItemCdTest";
        int endDate = 20221212;
        int kisanSbt = random.Next(1, 6);
        int kisanDate = 20221214;
        string byomei = "byomeiForUnitTest";
        string hosokuComment = "hosokuCommentUnitTest";
        string comment = "commentUnitTest";
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var listSanteiDetailModels = new List<SanteiInfDetailModel>() { new SanteiInfDetailModel(
                                                                                    0,
                                                                                    ptId,
                                                                                    itemCd,
                                                                                    endDate,
                                                                                    kisanSbt,
                                                                                    kisanDate,
                                                                                    byomei,
                                                                                    hosokuComment,
                                                                                    comment
                                                                                )
                                                                        };

        // Arrange
        SanteiInfRepository santeiInfRepository = new SanteiInfRepository(TenantProvider);

        // Act
        var resultComman = santeiInfRepository.SaveListSanteiInfDetail(1, 1, listSanteiDetailModels);

        var santeiInfDetail = tenant.SanteiInfDetails.OrderBy(item => item.Id)
                                                     .LastOrDefault(item => item.PtId == ptId
                                                                                         && item.ItemCd == itemCd
                                                                                         && item.EndDate == endDate
                                                                                         && item.KisanSbt == kisanSbt
                                                                                         && item.KisanDate == kisanDate
                                                                                         && item.Byomei == byomei
                                                                                         && item.HosokuComment == hosokuComment
                                                                                         && item.Comment == comment);
        var listSanteiInfDetails = new List<SanteiInfDetail>() { santeiInfDetail ?? new SanteiInfDetail() };
        if (listSanteiInfDetails == null)
        {
            resultComman = false;
        }
        else
        {
            var santeiDetailModel = new SanteiInfDetailModel(
                                                                santeiInfDetail != null ? santeiInfDetail.Id : 0,
                                                                ptId,
                                                                itemCd,
                                                                endDate,
                                                                kisanSbt,
                                                                kisanDate,
                                                                byomei,
                                                                hosokuComment,
                                                                comment
                                                            );
            resultComman = CompareListSanteiInfDetail(ptId, santeiDetailModel, listSanteiInfDetails);
        }

        // Assert
        try
        {
            Assert.True(resultComman);
        }
        finally
        {
            #region Remove Data Fetch
            santeiInfRepository.ReleaseResource();
            if (santeiInfDetail != null)
            {
                tenant.SanteiInfDetails.Remove(santeiInfDetail);
                tenant.SaveChanges();
            }
            #endregion
        }
    }

    [Test]
    public void SaveListSanteiInfDetail_TestUpdateSuccess()
    {
        Random random = new();
        long ptId = long.MaxValue;
        string itemCd = "ItemCdTest";
        int endDate = 20221212;
        int kisanSbt = random.Next(1, 6);
        int kisanDate = 20221214;
        string byomei = "byomeiForUnitTest";
        string hosokuComment = "hosokuCommentUnitTest";
        string comment = "commentUnitTest";
        int endDateNew = 20221112;
        int kisanSbtNew = random.Next(1, 6);
        int kisanDateNew = 20011214;
        string byomeiNew = "byomeiNewForUnitTest";
        string hosokuCommentNew = "hosokuCommentNewUnitTest";
        string commentNew = "commentNewUnitTest";
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var santeiInfDetailUnitTest = new SanteiInfDetail()
        {
            PtId = ptId,
            ItemCd = itemCd,
            EndDate = endDate,
            KisanSbt = kisanSbt,
            KisanDate = kisanDate,
            Byomei = byomei,
            HosokuComment = hosokuComment,
            Comment = comment
        };
        tenant.SanteiInfDetails.Add(santeiInfDetailUnitTest);
        tenant.SaveChanges();

        // Arrange
        SanteiInfRepository santeiInfRepository = new SanteiInfRepository(TenantProvider);

        // Act
        var santeiInfDetailModel = new SanteiInfDetailModel(
                                                                santeiInfDetailUnitTest.Id,
                                                                ptId,
                                                                itemCd,
                                                                endDateNew,
                                                                kisanSbtNew,
                                                                kisanDateNew,
                                                                byomeiNew,
                                                                hosokuCommentNew,
                                                                commentNew
                                                            );
        var resultComman = santeiInfRepository.SaveListSanteiInfDetail(1, 1, new List<SanteiInfDetailModel>() { santeiInfDetailModel });

        var santeiInfDetail = tenant.SanteiInfDetails.OrderBy(item => item.Id)
                                                     .LastOrDefault(item => item.Id == santeiInfDetailUnitTest.Id);
        var listSanteiInfDetails = new List<SanteiInfDetail>() { santeiInfDetail ?? new SanteiInfDetail() };
        if (listSanteiInfDetails == null)
        {
            resultComman = false;
        }
        else
        {
            resultComman = CompareListSanteiInfDetail(ptId, santeiInfDetailModel, listSanteiInfDetails);
        }

        // Assert
        try
        {
            Assert.True(resultComman);
        }
        finally
        {
            #region Remove Data Fetch
            santeiInfRepository.ReleaseResource();
            if (santeiInfDetailUnitTest != null)
            {
                tenant.SanteiInfDetails.Remove(santeiInfDetailUnitTest);
                tenant.SaveChanges();
            }
            #endregion
        }
    }

    [Test]
    public void SaveListSanteiInfDetail_TestDeleteSuccess()
    {
        Random random = new();
        long ptId = long.MaxValue;
        string itemCd = "ItemCdTest";
        int endDate = 20221212;
        int kisanSbt = random.Next(1, 6);
        int kisanDate = 20221214;
        string byomei = "byomeiForUnitTest";
        string hosokuComment = "hosokuCommentUnitTest";
        string comment = "commentUnitTest";
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var santeiInfDetailUnitTest = new SanteiInfDetail()
        {
            PtId = ptId,
            ItemCd = itemCd,
            EndDate = endDate,
            KisanSbt = kisanSbt,
            KisanDate = kisanDate,
            Byomei = byomei,
            HosokuComment = hosokuComment,
            Comment = comment
        };
        tenant.SanteiInfDetails.Add(santeiInfDetailUnitTest);
        tenant.SaveChanges();

        // Arrange
        SanteiInfRepository santeiInfRepository = new SanteiInfRepository(TenantProvider);

        // Act
        var santeiInfDetailModel = new SanteiInfDetailModel(
                                                                santeiInfDetailUnitTest.Id,
                                                                ptId,
                                                                itemCd,
                                                                endDate,
                                                                kisanSbt,
                                                                kisanDate,
                                                                byomei,
                                                                hosokuComment,
                                                                comment,
                                                                true
                                                            );
        var resultComman = santeiInfRepository.SaveListSanteiInfDetail(1, 1, new List<SanteiInfDetailModel>() { santeiInfDetailModel });

        var santeiInfDetail = tenant.SanteiInfDetails.OrderBy(item => item.Id)
                                                     .LastOrDefault(item => item.Id == santeiInfDetailUnitTest.Id);
        if (santeiInfDetail == null)
        {
            resultComman = false;
        }
        else
        {
            resultComman = santeiInfDetail.IsDeleted == 1;
        }

        // Assert
        try
        {
            Assert.True(resultComman);
        }
        finally
        {
            #region Remove Data Fetch
            santeiInfRepository.ReleaseResource();
            tenant.SanteiInfDetails.Remove(santeiInfDetailUnitTest);
            tenant.SaveChanges();
            #endregion
        }
    }


    #endregion

    #region private function
    private bool CompareOnlySanteiInf(long ptId, List<SanteiInfModel> santeiModels, List<SanteiInf> santeiInfs)
    {
        var santeiInf = santeiInfs.FirstOrDefault(item => item.PtId == ptId);
        if (santeiInf == null)
        {
            return false;
        }
        var santeiModel = santeiModels.FirstOrDefault(item => item.PtId == ptId && item.ItemCd == santeiInf.ItemCd);
        if (santeiModel == null)
        {
            return false;
        }
        if (santeiInf.Id != santeiModel.Id)
        {
            return false;
        }
        else if (santeiInf.AlertDays != santeiModel.AlertDays)
        {
            return false;
        }
        else if (santeiInf.AlertTerm != santeiModel.AlertTerm)
        {
            return false;
        }
        return true;
    }

    private bool CompareListSanteiInf(long ptId, List<SanteiInfModel> listSanteiModels, List<SanteiInf> santeiInfs, List<SanteiInfDetail> santeiInfDetails, List<OdrInf> odrInfs, List<OdrInfDetail> odrInfDetails, List<TenMst> tenMsts)
    {
        long id = 0;
        string itemCd = string.Empty;
        int seqNo = 0;
        int alertDays = 0;
        int alertTerm = 0;
        string itemName = string.Empty;
        int lastOdrDate = 0;
        int santeiItemCount = 0;
        double santeiItemSum = 0;
        int currentMonthSanteiItemCount = 0;
        double currentMonthSanteiItemSum = 0;
        bool isDeleted = false;

        var santeiInf = santeiInfs.FirstOrDefault();
        if (santeiInf != null)
        {
            id = santeiInf.Id;
            itemCd = santeiInf.ItemCd ?? string.Empty;
            seqNo = santeiInf.SeqNo;
            alertDays = santeiInf.AlertDays;
            alertTerm = santeiInf.AlertTerm;
        }

        var tenMst = tenMsts.FirstOrDefault();
        if (tenMst != null)
        {
            itemName = tenMst.Name ?? string.Empty;
        }

        var orderInf = odrInfs.FirstOrDefault();
        var orderInfDetail = odrInfDetails.FirstOrDefault();
        if (orderInf != null && orderInfDetail != null && orderInf.SinDate == orderInfDetail.SinDate)
        {
            lastOdrDate = orderInf.SinDate;
            santeiItemCount = 1;
            santeiItemSum = orderInfDetail.Suryo;
            currentMonthSanteiItemCount = 1;
            currentMonthSanteiItemSum = orderInfDetail.Suryo;
        }

        var santeiModel = listSanteiModels.FirstOrDefault(item => item.Id == id);
        if (santeiModel == null)
        {
            return false;
        }
        else if (santeiModel.Id != id)
        {
            return false;
        }
        else if (santeiModel.PtId != ptId)
        {
            return false;
        }
        else if (santeiModel.ItemCd != itemCd)
        {
            return false;
        }
        else if (santeiModel.SeqNo != seqNo)
        {
            return false;
        }
        else if (santeiModel.AlertDays != alertDays)
        {
            return false;
        }
        else if (santeiModel.AlertTerm != alertTerm)
        {
            return false;
        }
        else if (santeiModel.ItemName != itemName)
        {
            return false;
        }
        else if (santeiModel.LastOdrDate != lastOdrDate)
        {
            return false;
        }
        else if (santeiModel.SanteiItemCount != santeiItemCount)
        {
            return false;
        }
        else if (santeiModel.SanteiItemSum != santeiItemSum)
        {
            return false;
        }
        else if (santeiModel.CurrentMonthSanteiItemCount != currentMonthSanteiItemCount)
        {
            return false;
        }
        else if (santeiModel.CurrentMonthSanteiItemSum != currentMonthSanteiItemSum)
        {
            return false;
        }
        else if (santeiModel.IsDeleted != isDeleted)
        {
            return false;
        }
        var santeiInfDetail = santeiModel.ListSanteiInfDetails.FirstOrDefault();
        if (santeiInfDetail == null)
        {
            return false;
        }
        return CompareListSanteiInfDetail(ptId, santeiInfDetail, santeiInfDetails);
    }

    private bool CompareListSanteiInfDetail(long ptId, SanteiInfDetailModel santeiDetailModel, List<SanteiInfDetail> santeiInfDetails)
    {
        var santeiInfDetail = santeiInfDetails.FirstOrDefault(item => item.Id == santeiDetailModel.Id);
        if (santeiInfDetail == null)
        {
            return false;
        }
        else if (santeiDetailModel.PtId != ptId)
        {
            return false;
        }
        else if (santeiDetailModel.ItemCd != santeiInfDetail.ItemCd)
        {
            return false;
        }
        else if (santeiDetailModel.EndDate != santeiInfDetail.EndDate)
        {
            return false;
        }
        else if (santeiDetailModel.KisanSbt != santeiInfDetail.KisanSbt)
        {
            return false;
        }
        else if (santeiDetailModel.KisanDate != santeiInfDetail.KisanDate)
        {
            return false;
        }
        else if (santeiDetailModel.Byomei != santeiInfDetail.Byomei)
        {
            return false;
        }
        else if (santeiDetailModel.HosokuComment != santeiInfDetail.HosokuComment)
        {
            return false;
        }
        else if (santeiDetailModel.Comment != santeiInfDetail.Comment)
        {
            return false;
        }
        else if (santeiDetailModel.IsDeleted)
        {
            return false;
        }
        return true;
    }
    #endregion
}
