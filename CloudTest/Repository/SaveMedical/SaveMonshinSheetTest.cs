using Domain.Models.MonshinInf;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.SaveMedical;

public class SaveMonshinSheetTest : BaseUT
{
    [Test]
    public void TC_001_SaveMonshinSheet_TestSaveSuccess()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        MonshinInforModel monshinInforModel = new MonshinInforModel(hpId, random.Next(999, 99999), random.Next(999, 99999), 20220202, string.Empty, string.Empty, random.Next(999, 99999), 0, random.Next(999, 99999));
        MonshinInfo? monshinInfo = new MonshinInfo()
        {
            HpId = hpId,
            PtId = monshinInforModel.PtId,
            RaiinNo = monshinInforModel.RaiinNo,
            SeqNo = monshinInforModel.SeqNo,
            GetKbn = 0
        };

        tenant.MonshinInfo.Add(monshinInfo);
        MonshinInforRepository monshinInforRepository = new MonshinInforRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = monshinInforRepository.SaveMonshinSheet(monshinInforModel);

            // Assert
            var monshinInfoAfter = tenant.MonshinInfo.FirstOrDefault(item => item.HpId == hpId
                                                               && item.PtId == monshinInforModel.PtId
                                                               && item.RaiinNo == monshinInforModel.RaiinNo
                                                               && item.SeqNo == monshinInforModel.SeqNo
                                                               && item.GetKbn == 1
                                                               && item.IsDeleted == 0);

            result = result && monshinInfoAfter != null;

            Assert.True(result);
        }
        finally
        {
            monshinInforRepository.ReleaseResource();

            if (monshinInfo != null)
            {
                tenant.MonshinInfo.Remove(monshinInfo);
            }
            tenant.SaveChanges();
        }
    }

    [Test]
    public void TC_002_SaveMonshinSheet_TestSaveFalse()
    {
        // Arrange
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);
        MonshinInforModel monshinInforModel = new MonshinInforModel(hpId, random.Next(999, 99999), random.Next(999, 99999), 20220202, string.Empty, string.Empty, random.Next(999, 99999), 0, random.Next(999, 99999));
        MonshinInfo? monshinInfo = new MonshinInfo()
        {
            HpId = hpId,
            PtId = random.Next(999, 99999),
            RaiinNo = monshinInforModel.RaiinNo,
            SeqNo = monshinInforModel.SeqNo,
            GetKbn = 0
        };

        tenant.MonshinInfo.Add(monshinInfo);
        MonshinInforRepository monshinInforRepository = new MonshinInforRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = monshinInforRepository.SaveMonshinSheet(monshinInforModel);

            // Assert
            var monshinInfoAfter = tenant.MonshinInfo.FirstOrDefault(item => item.HpId == hpId
                                                               && item.PtId == monshinInforModel.PtId
                                                               && item.RaiinNo == monshinInforModel.RaiinNo
                                                               && item.SeqNo == monshinInforModel.SeqNo
                                                               && item.GetKbn == 1
                                                               && item.IsDeleted == 0);

            result = result && monshinInfoAfter == null;

            Assert.True(result);
        }
        finally
        {
            monshinInforRepository.ReleaseResource();

            if (monshinInfo != null)
            {
                tenant.MonshinInfo.Remove(monshinInfo);
            }
            tenant.SaveChanges();
        }
    }
}
