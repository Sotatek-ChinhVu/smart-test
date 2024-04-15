using Entity.Tenant;
using Infrastructure.Repositories;
using PostgreDataContext;

namespace CloudUnitTest.Repository.ReceSeikyuRepo;

public class GetListReceSeikyModelTest : BaseUT
{

    [Test]
    public void TC_001_GetListReceSeikyModel_Test_IsGetDataPendingTrue()
    {
        // Arrange
        SetupTestEnvironment(out ReceSeikyuRepository receiptRepository, out TenantNoTrackingDataContext tenant, out PtInf ptInf, out ReceSeikyu receSeikyu, out PtHokenInf ptHokenInf, out RecedenHenJiyuu recedenHenJiyuu, out int hpId, out int sinDate, out int sinYm, out long ptNum, out int seikyuYm, out long ptId);

        receSeikyu.PreHokenId = recedenHenJiyuu.HokenId;
        tenant.PtInfs.Add(ptInf);
        tenant.ReceSeikyus.Add(receSeikyu);
        tenant.PtHokenInfs.Add(ptHokenInf);
        tenant.RecedenHenJiyuus.Add(recedenHenJiyuu);
        bool isIncludingUnConfirmed = false;
        long ptNumSearch = ptNum;
        bool noFilter = true;
        bool isFilterMonthlyDelay = false;
        bool isFilterReturn = false;
        bool isFilterOnlineReturn = false;
        bool isGetDataPending = true;
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetListReceSeikyModel(hpId, sinDate, sinYm, isIncludingUnConfirmed, ptNumSearch, noFilter, isFilterMonthlyDelay, isFilterReturn, isFilterOnlineReturn, isGetDataPending);

            // Assert
            var success = result.Any(item => item.SinDay == sinDate
                                             && item.HpId == hpId
                                             && item.PtId == (ptInf?.PtId ?? 0)
                                             && item.PtName == (ptInf?.Name ?? string.Empty)
                                             && item.SinYm == sinYm
                                             && item.ReceListSinYm == (receSeikyu?.SinYm ?? 0)
                                             && item.HokenId == (ptHokenInf?.HokenId ?? 0)
                                             && item.HokensyaNo == (ptHokenInf?.HokensyaNo ?? string.Empty)
                                             && item.SeqNo == (receSeikyu?.SeqNo ?? 0)
                                             && item.SeikyuYm == (receSeikyu?.SeikyuYm ?? 0)
                                             && item.SeikyuKbn == (receSeikyu?.SeikyuKbn ?? 0)
                                             && item.PreHokenId == (receSeikyu?.PreHokenId ?? 0)
                                             && item.Cmt == (receSeikyu?.Cmt ?? string.Empty)
                                             && item.PtNum == (ptInf?.PtNum ?? 0)
                                             && item.HokenKbn == (ptHokenInf?.HokenKbn ?? 0)
                                             && item.Houbetu == (ptHokenInf?.Houbetu ?? string.Empty)
                                             && item.HokenStartDate == (ptHokenInf?.StartDate ?? 0)
                                             && item.HokenEndDate == (ptHokenInf?.EndDate ?? 0)
                                             && item.IsModified == false
                                             && item.OriginSeikyuYm == (receSeikyu?.SeikyuYm ?? 0)
                                             && item.OriginSinYm == (receSeikyu?.SinYm ?? 0)
                                             && item.IsAddNew == false
                                             && item.IsChecked == (receSeikyu?.SeikyuYm != 999999)
                                             && item.ListRecedenHenJiyuuModel.Any(detail => detail.HpId == (recedenHenJiyuu?.HpId ?? 0)
                                                                                            && detail.PtId == (recedenHenJiyuu?.PtId ?? 0)
                                                                                            && detail.HokenId == (recedenHenJiyuu?.HokenId ?? 0)
                                                                                            && detail.SinYm == (recedenHenJiyuu?.SinYm ?? 0)
                                                                                            && detail.SeqNo == (recedenHenJiyuu?.SeqNo ?? 0)
                                                                                            && detail.HenreiJiyuuCd == (recedenHenJiyuu?.HenreiJiyuuCd ?? string.Empty)
                                                                                            && detail.HenreiJiyuu == (recedenHenJiyuu?.HenreiJiyuu ?? string.Empty)
                                                                                            && detail.Hosoku == (recedenHenJiyuu?.Hosoku ?? string.Empty)
                                                                                            && detail.HokenKbn == (ptHokenInf?.HokenKbn ?? 0)
                                                                                            && detail.Houbetu == (ptHokenInf?.Houbetu ?? string.Empty)
                                                                                            && detail.HokenStartDate == (ptHokenInf?.StartDate ?? 0)
                                                                                            && detail.HokenEndDate == (ptHokenInf?.EndDate ?? 0)
                                                                                            && detail.HokensyaNo == (ptHokenInf?.HokensyaNo ?? string.Empty)
                                                                                            )
                                             );

            Assert.IsTrue(success);
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.PtInfs.Remove(ptInf);
            tenant.ReceSeikyus.Remove(receSeikyu);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RecedenHenJiyuus.Remove(recedenHenJiyuu);
        }
    }

    [Test]
    public void TC_002_GetListReceSeikyModel_Test_IsGetDataPendingFalse()
    {
        // Arrange
        SetupTestEnvironment(out ReceSeikyuRepository receiptRepository, out TenantNoTrackingDataContext tenant, out PtInf ptInf, out ReceSeikyu receSeikyu, out PtHokenInf ptHokenInf, out RecedenHenJiyuu recedenHenJiyuu, out int hpId, out int sinDate, out int sinYm, out long ptNum, out int seikyuYm, out long ptId);

        tenant.PtInfs.Add(ptInf);
        tenant.ReceSeikyus.Add(receSeikyu);
        tenant.PtHokenInfs.Add(ptHokenInf);
        tenant.RecedenHenJiyuus.Add(recedenHenJiyuu);
        bool isIncludingUnConfirmed = false;
        long ptNumSearch = ptNum;
        bool noFilter = true;
        bool isFilterMonthlyDelay = false;
        bool isFilterReturn = false;
        bool isFilterOnlineReturn = false;
        bool isGetDataPending = false;
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetListReceSeikyModel(hpId, sinDate, sinYm, isIncludingUnConfirmed, ptNumSearch, noFilter, isFilterMonthlyDelay, isFilterReturn, isFilterOnlineReturn, isGetDataPending);

            // Assert
            var success = result.Any(item => item.SinDay == sinDate
                                             && item.HpId == hpId
                                             && item.PtId == (ptInf?.PtId ?? 0)
                                             && item.PtName == (ptInf?.Name ?? string.Empty)
                                             && item.SinYm == sinYm
                                             && item.ReceListSinYm == (receSeikyu?.SinYm ?? 0)
                                             && item.HokenId == (ptHokenInf?.HokenId ?? 0)
                                             && item.HokensyaNo == (ptHokenInf?.HokensyaNo ?? string.Empty)
                                             && item.SeqNo == (receSeikyu?.SeqNo ?? 0)
                                             && item.SeikyuYm == (receSeikyu?.SeikyuYm ?? 0)
                                             && item.SeikyuKbn == (receSeikyu?.SeikyuKbn ?? 0)
                                             && item.PreHokenId == (receSeikyu?.PreHokenId ?? 0)
                                             && item.Cmt == (receSeikyu?.Cmt ?? string.Empty)
                                             && item.PtNum == (ptInf?.PtNum ?? 0)
                                             && item.HokenKbn == (ptHokenInf?.HokenKbn ?? 0)
                                             && item.Houbetu == (ptHokenInf?.Houbetu ?? string.Empty)
                                             && item.HokenStartDate == (ptHokenInf?.StartDate ?? 0)
                                             && item.HokenEndDate == (ptHokenInf?.EndDate ?? 0)
                                             && item.IsModified == false
                                             && item.OriginSeikyuYm == (receSeikyu?.SeikyuYm ?? 0)
                                             && item.OriginSinYm == (receSeikyu?.SinYm ?? 0)
                                             && item.IsAddNew == false
                                             && item.IsChecked == (receSeikyu?.SeikyuYm != 999999)
                                             && item.ListRecedenHenJiyuuModel.Any(detail => detail.HpId == (recedenHenJiyuu?.HpId ?? 0)
                                                                                            && detail.PtId == (recedenHenJiyuu?.PtId ?? 0)
                                                                                            && detail.HokenId == (recedenHenJiyuu?.HokenId ?? 0)
                                                                                            && detail.SinYm == (recedenHenJiyuu?.SinYm ?? 0)
                                                                                            && detail.SeqNo == (recedenHenJiyuu?.SeqNo ?? 0)
                                                                                            && detail.HenreiJiyuuCd == (recedenHenJiyuu?.HenreiJiyuuCd ?? string.Empty)
                                                                                            && detail.HenreiJiyuu == (recedenHenJiyuu?.HenreiJiyuu ?? string.Empty)
                                                                                            && detail.Hosoku == (recedenHenJiyuu?.Hosoku ?? string.Empty)
                                                                                            && detail.HokenKbn == (ptHokenInf?.HokenKbn ?? 0)
                                                                                            && detail.Houbetu == (ptHokenInf?.Houbetu ?? string.Empty)
                                                                                            && detail.HokenStartDate == (ptHokenInf?.SikakuDate ?? 0)
                                                                                            && detail.HokenEndDate == (ptHokenInf?.EndDate ?? 0)
                                                                                            && detail.HokensyaNo == (ptHokenInf?.HokensyaNo ?? string.Empty)
                                                                                            )

                                             );

            Assert.IsTrue(success);
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.PtInfs.Remove(ptInf);
            tenant.ReceSeikyus.Remove(receSeikyu);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RecedenHenJiyuus.Remove(recedenHenJiyuu);
        }
    }

    [Test]
    public void TC_003_GetListReceSeikyModel_TestToRunCalculationSuccess()
    {
        // Arrange
        SetupTestEnvironment(out ReceSeikyuRepository receiptRepository, out TenantNoTrackingDataContext tenant, out PtInf ptInf, out ReceSeikyu receSeikyu, out PtHokenInf ptHokenInf, out RecedenHenJiyuu recedenHenJiyuu, out int hpId, out int sinDate, out int sinYm, out long ptNum, out int seikyuYm, out long ptId);

        tenant.PtInfs.Add(ptInf);
        tenant.ReceSeikyus.Add(receSeikyu);
        tenant.PtHokenInfs.Add(ptHokenInf);
        tenant.RecedenHenJiyuus.Add(recedenHenJiyuu);
        List<long> ptIdList = new() { ptInf.PtId };
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetListReceSeikyModel(hpId, seikyuYm, ptIdList);

            // Assert
            var success = result.Any(item => item.PtId == (ptInf?.PtId ?? 0)
                                             && item.SinYm == sinYm
                                             && item.HokenId == (ptHokenInf?.HokenId ?? 0)
                                             && item.SeikyuKbn == (receSeikyu?.SeikyuKbn ?? 0)
                                             && item.PtNum == (ptInf?.PtNum ?? 0));

            Assert.IsTrue(success);
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.PtInfs.Remove(ptInf);
            tenant.ReceSeikyus.Remove(receSeikyu);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RecedenHenJiyuus.Remove(recedenHenJiyuu);
        }
    }

    [Test]
    public void TC_004_GetListReceSeikyModel_TestToRunCalculationFalse()
    {
        // Arrange
        SetupTestEnvironment(out ReceSeikyuRepository receiptRepository, out TenantNoTrackingDataContext tenant, out PtInf ptInf, out ReceSeikyu receSeikyu, out PtHokenInf ptHokenInf, out RecedenHenJiyuu recedenHenJiyuu, out int hpId, out int sinDate, out int sinYm, out long ptNum, out int seikyuYm, out long ptId);

        tenant.PtInfs.Add(ptInf);
        tenant.PtHokenInfs.Add(ptHokenInf);
        tenant.RecedenHenJiyuus.Add(recedenHenJiyuu);
        List<long> ptIdList = new() { ptInf.PtId };
        try
        {
            tenant.SaveChanges();

            // Act
            var result = receiptRepository.GetListReceSeikyModel(hpId, seikyuYm, ptIdList);

            // Assert
            var success = !result.Any();

            Assert.IsTrue(success);
        }
        finally
        {
            receiptRepository.ReleaseResource();
            tenant.PtInfs.Remove(ptInf);
            tenant.PtHokenInfs.Remove(ptHokenInf);
            tenant.RecedenHenJiyuus.Remove(recedenHenJiyuu);
        }
    }

    private void SetupTestEnvironment(out ReceSeikyuRepository receSeikyuRepository, out TenantNoTrackingDataContext tenant, out PtInf ptInf, out ReceSeikyu receSeikyu, out PtHokenInf ptHokenInf, out RecedenHenJiyuu recedenHenJiyuu, out int hpId, out int sinDate, out int sinYm, out long ptNum, out int seikyuYm, out long ptId)
    {
        receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);
        tenant = TenantProvider.GetNoTrackingDataContext();

        Random random = new();
        hpId = random.Next(999, 999999);
        ptId = random.Next(9999, 999999999);
        ptNum = random.Next(9999, 999999999);
        int hokenId = random.Next(9999, 999999999);
        int preHokenId = random.Next(9999, 999999999);
        int seikyuKbn = random.Next(9999, 999999999);
        int hokenKbn = random.Next(9999, 999999999);
        string hokensyaNo = random.Next(9999, 99999999).ToString();
        string houbetu = random.Next(9, 999).ToString();
        string henreiJiyuuCd = random.Next(9999, 999999999).ToString();
        string henreiJiyuu = random.Next(9999, 999999999).ToString();
        string hosoku = random.Next(9999, 999999999).ToString();
        sinYm = 202202;
        seikyuYm = 202202;
        int startDate = 20220201;
        int sikakuDate = 20220207;
        int endDate = 20220228;
        string ptInfoName = "PtInfoName";
        string receSeikyuCmt = "ReceSeikyuCmt";
        sinDate = 20220210;

        ptInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            PtNum = ptNum,
            Name = ptInfoName,
            IsDelete = 0
        };

        receSeikyu = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SinYm = sinYm,
            SeikyuYm = seikyuYm,
            PreHokenId = preHokenId,
            SeikyuKbn = seikyuKbn,
            Cmt = receSeikyuCmt,
            IsDeleted = 0
        };

        ptHokenInf = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            HokensyaNo = hokensyaNo,
            HokenKbn = hokenKbn,
            Houbetu = houbetu,
            StartDate = startDate,
            EndDate = endDate,
            SikakuDate = sikakuDate,
            IsDeleted = 0
        };

        recedenHenJiyuu = new()
        {
            HpId = hpId,
            PtId = ptId,
            HokenId = hokenId,
            SinYm = sinYm,
            HenreiJiyuuCd = henreiJiyuuCd,
            HenreiJiyuu = henreiJiyuu,
            Hosoku = hosoku,
            IsDeleted = 0
        };
    }
}
