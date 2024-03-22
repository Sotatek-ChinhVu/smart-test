using Domain.Models.FlowSheet;
using Entity.Tenant;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.Repository.SaveMedical;

public class SaveFlowSheetRepositoryTest : BaseUT
{
    #region UpsertTag
    [Test]
    public void TC_001_SaveFlowSheetRepository_UpsertTag_TestSaveSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        FlowSheetModel flowSheetModel = new FlowSheetModel(20220202, random.Next(999, 99999), string.Empty, random.Next(99999, 99999999), 0, string.Empty, 0, false, false, new(), random.Next(999, 99999), false);
        RaiinListTag? raiinListTag = null;

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        FlowSheetRepository flowSheetRepository = new FlowSheetRepository(TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            flowSheetRepository.UpsertTag(new() { flowSheetModel }, hpId, userId);

            // Assert
            raiinListTag = tenant.RaiinListTags.FirstOrDefault(item => item.HpId == hpId
                                                                       && item.PtId == flowSheetModel.PtId
                                                                       && item.SinDate == flowSheetModel.SinDate
                                                                       && item.RaiinNo == flowSheetModel.RaiinNo
                                                                       && item.TagNo == flowSheetModel.TagNo);

            var result = raiinListTag != null
                         && raiinListTag.HpId == hpId
                         && raiinListTag.PtId == flowSheetModel.PtId
                         && raiinListTag.SinDate == flowSheetModel.SinDate
                         && raiinListTag.RaiinNo == flowSheetModel.RaiinNo
                         && raiinListTag.TagNo == flowSheetModel.TagNo;

            Assert.True(result);
        }
        finally
        {
            flowSheetRepository.ReleaseResource();

            #region Remove Data Fetch
            if (raiinListTag != null)
            {
                tenant.RaiinListTags.Remove(raiinListTag);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_002_SaveFlowSheetRepository_UpsertTag_TestUpdateSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        FlowSheetModel flowSheetModel = new FlowSheetModel(20220202, random.Next(999, 99999), string.Empty, random.Next(99999, 99999999), 0, string.Empty, 0, false, false, new(), random.Next(999, 99999), false);
        RaiinListTag? raiinListTag = new RaiinListTag()
        {
            HpId = hpId,
            RaiinNo = flowSheetModel.RaiinNo,
            PtId = flowSheetModel.PtId
        };
        tenant.RaiinListTags.Add(raiinListTag);

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        FlowSheetRepository flowSheetRepository = new FlowSheetRepository(TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            flowSheetRepository.UpsertTag(new() { flowSheetModel }, hpId, userId);

            // Assert
            var raiinListTagAfter = tenant.RaiinListTags.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.PtId == flowSheetModel.PtId
                                                                        && item.RaiinNo == flowSheetModel.RaiinNo
                                                                        && item.TagNo == flowSheetModel.TagNo);

            var result = raiinListTagAfter != null
                         && raiinListTagAfter.HpId == hpId
                         && raiinListTagAfter.PtId == flowSheetModel.PtId
                         && raiinListTagAfter.RaiinNo == flowSheetModel.RaiinNo
                         && raiinListTagAfter.TagNo == flowSheetModel.TagNo;

            Assert.True(result);
        }
        finally
        {
            flowSheetRepository.ReleaseResource();

            #region Remove Data Fetch
            if (raiinListTag != null)
            {
                tenant.RaiinListTags.Remove(raiinListTag);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion UpsertTag

    #region UpsertCmt
    [Test]
    public void TC_003_SaveFlowSheetRepository_UpsertCmt_TestSaveSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        FlowSheetModel flowSheetModel = new FlowSheetModel(20220202, 0, string.Empty, random.Next(99999, 9999999), 0, "commentRaiinListCmt", 0, false, false, new(), random.Next(99999, 9999999), false);
        RaiinListCmt? raiinListCmt = null;

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        FlowSheetRepository flowSheetRepository = new FlowSheetRepository(TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            flowSheetRepository.UpsertCmt(new() { flowSheetModel }, hpId, userId);

            // Assert
            raiinListCmt = tenant.RaiinListCmts.FirstOrDefault(item => item.HpId == hpId
                                                                       && item.PtId == flowSheetModel.PtId
                                                                       && item.SinDate == flowSheetModel.SinDate
                                                                       && item.RaiinNo == flowSheetModel.RaiinNo
                                                                       && item.CmtKbn == 9
                                                                       && item.Text == flowSheetModel.Comment);

            var result = raiinListCmt != null
                         && raiinListCmt.HpId == hpId
                         && raiinListCmt.PtId == flowSheetModel.PtId
                         && raiinListCmt.SinDate == flowSheetModel.SinDate
                         && raiinListCmt.RaiinNo == flowSheetModel.RaiinNo
                         && raiinListCmt.Text == flowSheetModel.Comment;

            Assert.True(result);
        }
        finally
        {
            flowSheetRepository.ReleaseResource();

            #region Remove Data Fetch
            if (raiinListCmt != null)
            {
                tenant.RaiinListCmts.Remove(raiinListCmt);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_004_SaveFlowSheetRepository_UpsertCmt_TestUpdateSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        FlowSheetModel flowSheetModel = new FlowSheetModel(20220202, 0, string.Empty, random.Next(99999, 9999999), 0, "commentRaiinListCmt", 0, false, false, new(), random.Next(99999, 9999999), false);
        RaiinListCmt? raiinListCmt = new RaiinListCmt()
        {
            HpId = hpId,
            RaiinNo = flowSheetModel.RaiinNo,
            PtId = flowSheetModel.PtId
        };
        tenant.RaiinListCmts.Add(raiinListCmt);

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        FlowSheetRepository flowSheetRepository = new FlowSheetRepository(TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, TenantProvider, mockConfiguration.Object);
        try
        {
            tenant.SaveChanges();

            // Act
            flowSheetRepository.UpsertCmt(new() { flowSheetModel }, hpId, userId);

            // Assert
           var raiinListCmtAfter = tenant.RaiinListCmts.FirstOrDefault(item => item.HpId == hpId
                                                                               && item.PtId == flowSheetModel.PtId
                                                                               && item.RaiinNo == flowSheetModel.RaiinNo
                                                                               && item.Text == flowSheetModel.Comment);

            var result = raiinListCmtAfter != null
                         && raiinListCmtAfter.HpId == hpId
                         && raiinListCmtAfter.PtId == flowSheetModel.PtId
                         && raiinListCmtAfter.RaiinNo == flowSheetModel.RaiinNo
                         && raiinListCmtAfter.Text == flowSheetModel.Comment;

            Assert.True(result);
        }
        finally
        {
            flowSheetRepository.ReleaseResource();

            #region Remove Data Fetch
            if (raiinListCmt != null)
            {
                tenant.RaiinListCmts.Remove(raiinListCmt);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion UpsertCmt
}
