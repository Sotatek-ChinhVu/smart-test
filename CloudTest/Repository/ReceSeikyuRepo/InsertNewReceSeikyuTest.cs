using Domain.Models.ReceSeikyu;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.ReceSeikyuRepo;

public class InsertNewReceSeikyuTest : BaseUT
{
    [Test]
    public void TC_001_InsertNewReceSeikyu_ByListTest()
    {
        // Arrange
        ReceSeikyuRepository receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int sinDate = 20220202;
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        string ptName = "ptName";
        int sinYm = 202202;
        int receListSinYm = 202202;
        int hokenId = random.Next(999, 999999);
        string hokensyaNo = random.Next(999, 999999).ToString();
        int seqNo = random.Next(999, 999999);
        int seikyuYm = 202202;
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        string cmt = "Cmt";
        long ptNum = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        string houbetu = "Houbetu";
        int hokenStartDate = 20220201;
        int hokenEndDate = 20220228;
        bool isModified = false;
        int originSeikyuYm = 20220205;
        int originSinYm = 20220202;
        bool isAddNew = false;
        int isDeleted = 0;
        bool isChecked = false;
        bool isCompletedSeikyu = originSeikyuYm != 999999;
        if (!isChecked)
        {
            seikyuYm = 999999;
        }
        else if (isCompletedSeikyu)
        {
            seikyuYm = originSeikyuYm;
        }
        else if (seikyuYm == 0 || seikyuYm == 999999)
        {
            seikyuYm = receListSinYm;
        }

        var receSeikyuModel = new ReceSeikyuModel(
                                  sinDate,
                                  hpId,
                                  ptId,
                                  ptName,
                                  sinYm,
                                  receListSinYm,
                                  hokenId,
                                  hokensyaNo,
                                  seqNo,
                                  seikyuYm,
                                  seikyuKbn,
                                  preHokenId,
                                  cmt,
                                  ptNum,
                                  hokenKbn,
                                  houbetu,
                                  hokenStartDate,
                                  hokenEndDate,
                                  isModified,
                                  originSeikyuYm,
                                  originSinYm,
                                  isAddNew,
                                  isDeleted,
                                  isChecked,
                                  new());

        ReceSeikyu? receSeikyu = null;
        try
        {
            // Act
            var success = receSeikyuRepository.InsertNewReceSeikyu(new List<ReceSeikyuModel>() { receSeikyuModel }, userId, hpId);
            receSeikyu = tenant.ReceSeikyus.FirstOrDefault(item => item.HpId == hpId
                                                                   && item.PtId == ptId
                                                                   && item.SinYm == sinYm
                                                                   && item.SeikyuKbn == seikyuKbn
                                                                   && item.HokenId == hokenId
                                                                   && item.SeikyuYm == seikyuYm
                                                                   && item.Cmt == cmt);

            // Assert
            Assert.IsTrue(success && receSeikyu != null);
        }
        finally
        {
            if (receSeikyu != null)
            {
                tenant.ReceSeikyus.Remove(receSeikyu);
                tenant.Remove(receSeikyu);
            }
        }
    }

    [Test]
    public void TC_002_InsertNewReceSeikyu_ByListWithHpIdEqual0()
    {
        // Arrange
        ReceSeikyuRepository receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int sinDate = 20220202;
        int hpId = 0;
        string cmt = "返戻ファイルより登録";
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        string ptName = "ptName";
        int sinYm = 202202;
        int receListSinYm = 202202;
        int hokenId = random.Next(999, 999999);
        string hokensyaNo = random.Next(999, 999999).ToString();
        int seqNo = random.Next(999, 999999);
        int seikyuYm = 202202;
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        long ptNum = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        string houbetu = "Houbetu";
        int hokenStartDate = 20220201;
        int hokenEndDate = 20220228;
        bool isModified = false;
        int originSeikyuYm = 20220205;
        int originSinYm = 20220202;
        bool isAddNew = false;
        int isDeleted = 0;
        bool isChecked = false;
        bool isCompletedSeikyu = originSeikyuYm != 999999;
        if (!isChecked)
        {
            seikyuYm = 999999;
        }
        else if (isCompletedSeikyu)
        {
            seikyuYm = originSeikyuYm;
        }
        else if (seikyuYm == 0 || seikyuYm == 999999)
        {
            seikyuYm = receListSinYm;
        }

        var receSeikyuModel = new ReceSeikyuModel(
                                  sinDate,
                                  hpId,
                                  ptId,
                                  ptName,
                                  sinYm,
                                  receListSinYm,
                                  hokenId,
                                  hokensyaNo,
                                  seqNo,
                                  seikyuYm,
                                  seikyuKbn,
                                  preHokenId,
                                  cmt,
                                  ptNum,
                                  hokenKbn,
                                  houbetu,
                                  hokenStartDate,
                                  hokenEndDate,
                                  isModified,
                                  originSeikyuYm,
                                  originSinYm,
                                  isAddNew,
                                  isDeleted,
                                  isChecked,
                                  new());

        ReceSeikyu? receSeikyu = null;
        try
        {
            // Act
            var success = receSeikyuRepository.InsertNewReceSeikyu(new List<ReceSeikyuModel>() { receSeikyuModel }, userId, hpId);
            receSeikyu = tenant.ReceSeikyus.FirstOrDefault(item => item.HpId == hpId
                                                                   && item.PtId == ptId
                                                                   && item.SinYm == sinYm
                                                                   && item.SeikyuYm == seikyuYm
                                                                   && item.HokenId == hokenId
                                                                   && item.Cmt == cmt
                                                                   && item.SeikyuKbn == 3);

            // Assert
            Assert.IsTrue(success && receSeikyu != null);
        }
        finally
        {
            if (receSeikyu != null)
            {
                tenant.ReceSeikyus.Remove(receSeikyu);
                tenant.Remove(receSeikyu);
            }
        }
    }

    [Test]
    public void TC_003_InsertNewReceSeikyu_BySingleTest()
    {
        // Arrange
        ReceSeikyuRepository receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);
        var tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        int sinDate = 20220202;
        int hpId = random.Next(999, 999999);
        int userId = random.Next(999, 999999);
        long ptId = random.Next(999, 999999);
        string ptName = "ptName";
        int sinYm = 202202;
        int receListSinYm = 202202;
        int hokenId = random.Next(999, 999999);
        string hokensyaNo = random.Next(999, 999999).ToString();
        int seqNo = random.Next(999, 999999);
        int seikyuYm = 202202;
        int seikyuKbn = random.Next(999, 999999);
        int preHokenId = random.Next(999, 999999);
        string cmt = "Cmt";
        long ptNum = random.Next(999, 999999);
        int hokenKbn = random.Next(999, 999999);
        string houbetu = "Houbetu";
        int hokenStartDate = 20220201;
        int hokenEndDate = 20220228;
        bool isModified = false;
        int originSeikyuYm = 20220205;
        int originSinYm = 20220202;
        bool isAddNew = false;
        int isDeleted = 0;
        bool isChecked = false;
        bool isCompletedSeikyu = originSeikyuYm != 999999;
        if (!isChecked)
        {
            seikyuYm = 999999;
        }
        else if (isCompletedSeikyu)
        {
            seikyuYm = originSeikyuYm;
        }
        else if (seikyuYm == 0 || seikyuYm == 999999)
        {
            seikyuYm = receListSinYm;
        }

        var receSeikyuModel = new ReceSeikyuModel(
                                  sinDate,
                                  hpId,
                                  ptId,
                                  ptName,
                                  sinYm,
                                  receListSinYm,
                                  hokenId,
                                  hokensyaNo,
                                  seqNo,
                                  seikyuYm,
                                  seikyuKbn,
                                  preHokenId,
                                  cmt,
                                  ptNum,
                                  hokenKbn,
                                  houbetu,
                                  hokenStartDate,
                                  hokenEndDate,
                                  isModified,
                                  originSeikyuYm,
                                  originSinYm,
                                  isAddNew,
                                  isDeleted,
                                  isChecked,
                                  new());

        ReceSeikyu? receSeikyu = null;
        try
        {
            // Act
            int seqNoResult = receSeikyuRepository.InsertNewReceSeikyu( receSeikyuModel , userId, hpId);
            receSeikyu = tenant.ReceSeikyus.FirstOrDefault(item => item.HpId == hpId
                                                                   && item.PtId == ptId
                                                                   && item.SinYm == sinYm
                                                                   && item.SeikyuKbn == seikyuKbn
                                                                   && item.HokenId == hokenId
                                                                   && item.SeikyuYm == seikyuYm
                                                                   && item.SeqNo== seqNoResult);

            // Assert
            Assert.IsTrue(receSeikyu != null);
        }
        finally
        {
            if (receSeikyu != null)
            {
                tenant.ReceSeikyus.Remove(receSeikyu);
                tenant.Remove(receSeikyu);
            }
        }
    }
}
