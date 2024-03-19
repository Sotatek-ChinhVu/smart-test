using Domain.Constant;
using Domain.Models.Diseases;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.SaveMedical;

public class UpsertDiseaseRepositoryTest : BaseUT
{
    #region UpsertDiseaseRepository
    [Test]
    public void TC_001_UpsertDiseaseRepository_TestCreateByomeiSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PrefixSuffixModel prefixSuffixModel = new PrefixSuffixModel("codeUT", "nameUT");
        PtDiseaseModel ptDiseaseModel = new PtDiseaseModel(
                                            hpId,
                                            random.Next(9999, 9999999),
                                            random.Next(9999, 9999999),
                                            "ByoCdUT",
                                            random.Next(999, 99999),
                                            new() { prefixSuffixModel },
                                            new() { prefixSuffixModel },
                                            "ByomeiPtDisease",
                                            20220202,
                                            random.Next(999, 99999),
                                            20220202,
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            0,
                                            0,
                                            0,// isDeleted
                                            0,// id
                                            0,
                                            0,
                                            "",
                                            "",
                                            "",
                                            "",
                                            random.Next(999, 99999),
                                            "HosokuCmtPtDisease");
        PtByomei? ptByomei = null;

        DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var idList = diseaseRepository.Upsert(new() { ptDiseaseModel }, hpId, userId);

            // Assert
            ptByomei = tenant.PtByomeis.FirstOrDefault(item => item.HpId == hpId
                                                               && idList.Contains(item.Id)
                                                               && item.PtId == ptDiseaseModel.PtId
                                                               && item.ByomeiCd == ptDiseaseModel.ByomeiCd
                                                               && item.SortNo == ptDiseaseModel.SortNo
                                                               && item.SyusyokuCd1 == (ptDiseaseModel.PrefixSuffixList.Count > 0 ? ptDiseaseModel.PrefixSuffixList[0].Code : string.Empty)
                                                               && item.SyusyokuCd2 == (ptDiseaseModel.PrefixSuffixList.Count > 1 ? ptDiseaseModel.PrefixSuffixList[1].Code : string.Empty)
                                                               && item.SyusyokuCd3 == (ptDiseaseModel.PrefixSuffixList.Count > 2 ? ptDiseaseModel.PrefixSuffixList[2].Code : string.Empty)
                                                               && item.SyusyokuCd4 == (ptDiseaseModel.PrefixSuffixList.Count > 3 ? ptDiseaseModel.PrefixSuffixList[3].Code : string.Empty)
                                                               && item.SyusyokuCd5 == (ptDiseaseModel.PrefixSuffixList.Count > 4 ? ptDiseaseModel.PrefixSuffixList[4].Code : string.Empty)
                                                               && item.SyusyokuCd6 == (ptDiseaseModel.PrefixSuffixList.Count > 5 ? ptDiseaseModel.PrefixSuffixList[5].Code : string.Empty)
                                                               && item.SyusyokuCd7 == (ptDiseaseModel.PrefixSuffixList.Count > 6 ? ptDiseaseModel.PrefixSuffixList[6].Code : string.Empty)
                                                               && item.SyusyokuCd8 == (ptDiseaseModel.PrefixSuffixList.Count > 7 ? ptDiseaseModel.PrefixSuffixList[7].Code : string.Empty)
                                                               && item.SyusyokuCd9 == (ptDiseaseModel.PrefixSuffixList.Count > 8 ? ptDiseaseModel.PrefixSuffixList[8].Code : string.Empty)
                                                               && item.SyusyokuCd10 == (ptDiseaseModel.PrefixSuffixList.Count > 9 ? ptDiseaseModel.PrefixSuffixList[9].Code : string.Empty)
                                                               && item.SyusyokuCd11 == (ptDiseaseModel.PrefixSuffixList.Count > 10 ? ptDiseaseModel.PrefixSuffixList[10].Code : string.Empty)
                                                               && item.SyusyokuCd12 == (ptDiseaseModel.PrefixSuffixList.Count > 11 ? ptDiseaseModel.PrefixSuffixList[11].Code : string.Empty)
                                                               && item.SyusyokuCd13 == (ptDiseaseModel.PrefixSuffixList.Count > 12 ? ptDiseaseModel.PrefixSuffixList[12].Code : string.Empty)
                                                               && item.SyusyokuCd14 == (ptDiseaseModel.PrefixSuffixList.Count > 13 ? ptDiseaseModel.PrefixSuffixList[13].Code : string.Empty)
                                                               && item.SyusyokuCd15 == (ptDiseaseModel.PrefixSuffixList.Count > 14 ? ptDiseaseModel.PrefixSuffixList[14].Code : string.Empty)
                                                               && item.SyusyokuCd16 == (ptDiseaseModel.PrefixSuffixList.Count > 15 ? ptDiseaseModel.PrefixSuffixList[15].Code : string.Empty)
                                                               && item.SyusyokuCd17 == (ptDiseaseModel.PrefixSuffixList.Count > 16 ? ptDiseaseModel.PrefixSuffixList[16].Code : string.Empty)
                                                               && item.SyusyokuCd18 == (ptDiseaseModel.PrefixSuffixList.Count > 17 ? ptDiseaseModel.PrefixSuffixList[17].Code : string.Empty)
                                                               && item.SyusyokuCd19 == (ptDiseaseModel.PrefixSuffixList.Count > 18 ? ptDiseaseModel.PrefixSuffixList[18].Code : string.Empty)
                                                               && item.SyusyokuCd20 == (ptDiseaseModel.PrefixSuffixList.Count > 19 ? ptDiseaseModel.PrefixSuffixList[19].Code : string.Empty)
                                                               && item.SyusyokuCd21 == (ptDiseaseModel.PrefixSuffixList.Count > 20 ? ptDiseaseModel.PrefixSuffixList[20].Code : string.Empty)
                                                               && item.Byomei == ptDiseaseModel.Byomei
                                                               && item.StartDate == ptDiseaseModel.StartDate
                                                               && item.TenkiKbn == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? TenkiKbnConst.Cured : ptDiseaseModel.TenkiKbn)
                                                               && item.TenkiDate == ptDiseaseModel.TenkiDate
                                                               && item.SyubyoKbn == ptDiseaseModel.SyubyoKbn
                                                               && item.SikkanKbn == ptDiseaseModel.SikkanKbn
                                                               && item.NanByoCd == ptDiseaseModel.NanbyoCd
                                                               && item.HosokuCmt == ptDiseaseModel.HosokuCmt
                                                               && item.HokenPid == ptDiseaseModel.HokenPid
                                                               && item.TogetuByomei == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? 1 : 0)
                                                               && item.IsNodspRece == ptDiseaseModel.IsNodspRece
                                                               && item.IsNodspKarte == ptDiseaseModel.IsNodspKarte
                                                               && item.IsDeleted == 0);

            var result = idList.Any() && ptByomei != null;

            Assert.True(result);
        }
        finally
        {
            diseaseRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptByomei != null)
            {
                tenant.PtByomeis.Remove(ptByomei);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_002_UpsertDiseaseRepository_TestCreateByomeiWithoutSortNoSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PrefixSuffixModel prefixSuffixModel = new PrefixSuffixModel("codeUT", "nameUT");
        PtDiseaseModel ptDiseaseModel = new PtDiseaseModel(
                                            hpId,
                                            random.Next(9999, 9999999),
                                            random.Next(9999, 9999999),
                                            "ByoCdUT",
                                            0,// sortNo
                                            new() { prefixSuffixModel },
                                            new() { prefixSuffixModel },
                                            "ByomeiPtDisease",
                                            20220202,
                                            random.Next(999, 99999),
                                            20220202,
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            0,
                                            0,
                                            0,// isDeleted
                                            0,// id
                                            0,
                                            0,
                                            "",
                                            "",
                                            "",
                                            "",
                                            random.Next(999, 99999),
                                            "HosokuCmtPtDisease");
        PtByomei? ptByomei = null;

        DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var idList = diseaseRepository.Upsert(new() { ptDiseaseModel }, hpId, userId);

            // Assert
            ptByomei = tenant.PtByomeis.FirstOrDefault(item => item.HpId == hpId
                                                               && idList.Contains(item.Id)
                                                               && item.PtId == ptDiseaseModel.PtId
                                                               && item.ByomeiCd == ptDiseaseModel.ByomeiCd
                                                               && item.SyusyokuCd1 == (ptDiseaseModel.PrefixSuffixList.Count > 0 ? ptDiseaseModel.PrefixSuffixList[0].Code : string.Empty)
                                                               && item.SyusyokuCd2 == (ptDiseaseModel.PrefixSuffixList.Count > 1 ? ptDiseaseModel.PrefixSuffixList[1].Code : string.Empty)
                                                               && item.SyusyokuCd3 == (ptDiseaseModel.PrefixSuffixList.Count > 2 ? ptDiseaseModel.PrefixSuffixList[2].Code : string.Empty)
                                                               && item.SyusyokuCd4 == (ptDiseaseModel.PrefixSuffixList.Count > 3 ? ptDiseaseModel.PrefixSuffixList[3].Code : string.Empty)
                                                               && item.SyusyokuCd5 == (ptDiseaseModel.PrefixSuffixList.Count > 4 ? ptDiseaseModel.PrefixSuffixList[4].Code : string.Empty)
                                                               && item.SyusyokuCd6 == (ptDiseaseModel.PrefixSuffixList.Count > 5 ? ptDiseaseModel.PrefixSuffixList[5].Code : string.Empty)
                                                               && item.SyusyokuCd7 == (ptDiseaseModel.PrefixSuffixList.Count > 6 ? ptDiseaseModel.PrefixSuffixList[6].Code : string.Empty)
                                                               && item.SyusyokuCd8 == (ptDiseaseModel.PrefixSuffixList.Count > 7 ? ptDiseaseModel.PrefixSuffixList[7].Code : string.Empty)
                                                               && item.SyusyokuCd9 == (ptDiseaseModel.PrefixSuffixList.Count > 8 ? ptDiseaseModel.PrefixSuffixList[8].Code : string.Empty)
                                                               && item.SyusyokuCd10 == (ptDiseaseModel.PrefixSuffixList.Count > 9 ? ptDiseaseModel.PrefixSuffixList[9].Code : string.Empty)
                                                               && item.SyusyokuCd11 == (ptDiseaseModel.PrefixSuffixList.Count > 10 ? ptDiseaseModel.PrefixSuffixList[10].Code : string.Empty)
                                                               && item.SyusyokuCd12 == (ptDiseaseModel.PrefixSuffixList.Count > 11 ? ptDiseaseModel.PrefixSuffixList[11].Code : string.Empty)
                                                               && item.SyusyokuCd13 == (ptDiseaseModel.PrefixSuffixList.Count > 12 ? ptDiseaseModel.PrefixSuffixList[12].Code : string.Empty)
                                                               && item.SyusyokuCd14 == (ptDiseaseModel.PrefixSuffixList.Count > 13 ? ptDiseaseModel.PrefixSuffixList[13].Code : string.Empty)
                                                               && item.SyusyokuCd15 == (ptDiseaseModel.PrefixSuffixList.Count > 14 ? ptDiseaseModel.PrefixSuffixList[14].Code : string.Empty)
                                                               && item.SyusyokuCd16 == (ptDiseaseModel.PrefixSuffixList.Count > 15 ? ptDiseaseModel.PrefixSuffixList[15].Code : string.Empty)
                                                               && item.SyusyokuCd17 == (ptDiseaseModel.PrefixSuffixList.Count > 16 ? ptDiseaseModel.PrefixSuffixList[16].Code : string.Empty)
                                                               && item.SyusyokuCd18 == (ptDiseaseModel.PrefixSuffixList.Count > 17 ? ptDiseaseModel.PrefixSuffixList[17].Code : string.Empty)
                                                               && item.SyusyokuCd19 == (ptDiseaseModel.PrefixSuffixList.Count > 18 ? ptDiseaseModel.PrefixSuffixList[18].Code : string.Empty)
                                                               && item.SyusyokuCd20 == (ptDiseaseModel.PrefixSuffixList.Count > 19 ? ptDiseaseModel.PrefixSuffixList[19].Code : string.Empty)
                                                               && item.SyusyokuCd21 == (ptDiseaseModel.PrefixSuffixList.Count > 20 ? ptDiseaseModel.PrefixSuffixList[20].Code : string.Empty)
                                                               && item.Byomei == ptDiseaseModel.Byomei
                                                               && item.StartDate == ptDiseaseModel.StartDate
                                                               && item.TenkiKbn == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? TenkiKbnConst.Cured : ptDiseaseModel.TenkiKbn)
                                                               && item.TenkiDate == ptDiseaseModel.TenkiDate
                                                               && item.SyubyoKbn == ptDiseaseModel.SyubyoKbn
                                                               && item.SikkanKbn == ptDiseaseModel.SikkanKbn
                                                               && item.NanByoCd == ptDiseaseModel.NanbyoCd
                                                               && item.HosokuCmt == ptDiseaseModel.HosokuCmt
                                                               && item.HokenPid == ptDiseaseModel.HokenPid
                                                               && item.TogetuByomei == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? 1 : 0)
                                                               && item.IsNodspRece == ptDiseaseModel.IsNodspRece
                                                               && item.IsNodspKarte == ptDiseaseModel.IsNodspKarte
                                                               && item.IsDeleted == 0);

            var result = idList.Any() && ptByomei != null;

            Assert.True(result);
        }
        finally
        {
            diseaseRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptByomei != null)
            {
                tenant.PtByomeis.Remove(ptByomei);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_003_UpsertDiseaseRepository_TestUpdateOnlySortNoSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PrefixSuffixModel prefixSuffixModel = new PrefixSuffixModel("codeUT", "nameUT");
        PtDiseaseModel ptDiseaseModel = new PtDiseaseModel(
                                            hpId,
                                            random.Next(9999, 9999999),
                                            random.Next(9999, 9999999),
                                            "ByoCdUT",
                                            random.Next(999, 99999),
                                            new() { prefixSuffixModel },
                                            new() { prefixSuffixModel },
                                            "ByomeiPtDisease",
                                            20220202,
                                            random.Next(999, 99999),
                                            20220202,
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            0,
                                            0,
                                            0,// isDeleted
                                            random.Next(999, 99999),// id
                                            0,
                                            0,
                                            "",
                                            "",
                                            "",
                                            "",
                                            random.Next(999, 99999),
                                            "HosokuCmtPtDisease");
        PtByomei? ptByomei = new PtByomei()
        {
            HpId = hpId,
            PtId = ptDiseaseModel.PtId,
            SeqNo = ptDiseaseModel.SeqNo,
            ByomeiCd = ptDiseaseModel.ByomeiCd,
            SortNo = ptDiseaseModel.SortNo,
            SyusyokuCd1 = ptDiseaseModel.PrefixSuffixList[0].Code,
            SyusyokuCd2 = ptDiseaseModel.PrefixSuffixList[1].Code,
            Byomei = ptDiseaseModel.Byomei,
            StartDate = ptDiseaseModel.StartDate,
            TenkiKbn = ptDiseaseModel.TenkiKbn,
            TenkiDate = ptDiseaseModel.TenkiDate,
            SyubyoKbn = ptDiseaseModel.SyubyoKbn,
            SikkanKbn = ptDiseaseModel.SikkanKbn,
            NanByoCd = ptDiseaseModel.NanbyoCd,
            IsNodspRece = ptDiseaseModel.IsNodspRece,
            IsNodspKarte = ptDiseaseModel.IsNodspKarte,
            IsDeleted = ptDiseaseModel.IsDeleted,
            Id = ptDiseaseModel.Id,
            IsImportant = ptDiseaseModel.IsImportant,
            HokenPid = ptDiseaseModel.HokenPid,
            HosokuCmt = ptDiseaseModel.HosokuCmt
        };

        tenant.PtByomeis.Add(ptByomei);
        DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var idList = diseaseRepository.Upsert(new() { ptDiseaseModel }, hpId, userId);

            // Assert
            var ptByomeiAfter = tenant.PtByomeis.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.Id == ptDiseaseModel.Id
                                                                        && item.PtId == ptDiseaseModel.PtId
                                                                        && item.ByomeiCd == ptDiseaseModel.ByomeiCd
                                                                        && item.SortNo == ptDiseaseModel.SortNo
                                                                        && item.SyusyokuCd1 == (ptDiseaseModel.PrefixSuffixList.Count > 0 ? ptDiseaseModel.PrefixSuffixList[0].Code : string.Empty)
                                                                        && item.SyusyokuCd2 == (ptDiseaseModel.PrefixSuffixList.Count > 1 ? ptDiseaseModel.PrefixSuffixList[1].Code : string.Empty)
                                                                        && item.SyusyokuCd3 == (ptDiseaseModel.PrefixSuffixList.Count > 2 ? ptDiseaseModel.PrefixSuffixList[2].Code : string.Empty)
                                                                        && item.SyusyokuCd4 == (ptDiseaseModel.PrefixSuffixList.Count > 3 ? ptDiseaseModel.PrefixSuffixList[3].Code : string.Empty)
                                                                        && item.SyusyokuCd5 == (ptDiseaseModel.PrefixSuffixList.Count > 4 ? ptDiseaseModel.PrefixSuffixList[4].Code : string.Empty)
                                                                        && item.SyusyokuCd6 == (ptDiseaseModel.PrefixSuffixList.Count > 5 ? ptDiseaseModel.PrefixSuffixList[5].Code : string.Empty)
                                                                        && item.SyusyokuCd7 == (ptDiseaseModel.PrefixSuffixList.Count > 6 ? ptDiseaseModel.PrefixSuffixList[6].Code : string.Empty)
                                                                        && item.SyusyokuCd8 == (ptDiseaseModel.PrefixSuffixList.Count > 7 ? ptDiseaseModel.PrefixSuffixList[7].Code : string.Empty)
                                                                        && item.SyusyokuCd9 == (ptDiseaseModel.PrefixSuffixList.Count > 8 ? ptDiseaseModel.PrefixSuffixList[8].Code : string.Empty)
                                                                        && item.SyusyokuCd10 == (ptDiseaseModel.PrefixSuffixList.Count > 9 ? ptDiseaseModel.PrefixSuffixList[9].Code : string.Empty)
                                                                        && item.SyusyokuCd11 == (ptDiseaseModel.PrefixSuffixList.Count > 10 ? ptDiseaseModel.PrefixSuffixList[10].Code : string.Empty)
                                                                        && item.SyusyokuCd12 == (ptDiseaseModel.PrefixSuffixList.Count > 11 ? ptDiseaseModel.PrefixSuffixList[11].Code : string.Empty)
                                                                        && item.SyusyokuCd13 == (ptDiseaseModel.PrefixSuffixList.Count > 12 ? ptDiseaseModel.PrefixSuffixList[12].Code : string.Empty)
                                                                        && item.SyusyokuCd14 == (ptDiseaseModel.PrefixSuffixList.Count > 13 ? ptDiseaseModel.PrefixSuffixList[13].Code : string.Empty)
                                                                        && item.SyusyokuCd15 == (ptDiseaseModel.PrefixSuffixList.Count > 14 ? ptDiseaseModel.PrefixSuffixList[14].Code : string.Empty)
                                                                        && item.SyusyokuCd16 == (ptDiseaseModel.PrefixSuffixList.Count > 15 ? ptDiseaseModel.PrefixSuffixList[15].Code : string.Empty)
                                                                        && item.SyusyokuCd17 == (ptDiseaseModel.PrefixSuffixList.Count > 16 ? ptDiseaseModel.PrefixSuffixList[16].Code : string.Empty)
                                                                        && item.SyusyokuCd18 == (ptDiseaseModel.PrefixSuffixList.Count > 17 ? ptDiseaseModel.PrefixSuffixList[17].Code : string.Empty)
                                                                        && item.SyusyokuCd19 == (ptDiseaseModel.PrefixSuffixList.Count > 18 ? ptDiseaseModel.PrefixSuffixList[18].Code : string.Empty)
                                                                        && item.SyusyokuCd20 == (ptDiseaseModel.PrefixSuffixList.Count > 19 ? ptDiseaseModel.PrefixSuffixList[19].Code : string.Empty)
                                                                        && item.SyusyokuCd21 == (ptDiseaseModel.PrefixSuffixList.Count > 20 ? ptDiseaseModel.PrefixSuffixList[20].Code : string.Empty)
                                                                        && item.Byomei == ptDiseaseModel.Byomei
                                                                        && item.StartDate == ptDiseaseModel.StartDate
                                                                        && item.TenkiKbn == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? TenkiKbnConst.Cured : ptDiseaseModel.TenkiKbn)
                                                                        && item.TenkiDate == ptDiseaseModel.TenkiDate
                                                                        && item.SyubyoKbn == ptDiseaseModel.SyubyoKbn
                                                                        && item.SikkanKbn == ptDiseaseModel.SikkanKbn
                                                                        && item.NanByoCd == ptDiseaseModel.NanbyoCd
                                                                        && item.HosokuCmt == ptDiseaseModel.HosokuCmt
                                                                        && item.HokenPid == ptDiseaseModel.HokenPid
                                                                        && item.TogetuByomei == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? 1 : 0)
                                                                        && item.IsNodspRece == ptDiseaseModel.IsNodspRece
                                                                        && item.IsNodspKarte == ptDiseaseModel.IsNodspKarte
                                                                        && item.IsDeleted == 0);

            var result = idList.Any() && ptByomeiAfter != null;

            Assert.True(result);
        }
        finally
        {
            diseaseRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptByomei != null)
            {
                tenant.PtByomeis.Remove(ptByomei);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_004_UpsertDiseaseRepository_TestUpdateByomeiWithIsModifiedSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PrefixSuffixModel prefixSuffixModel = new PrefixSuffixModel("codeUT", "nameUT");
        PtDiseaseModel ptDiseaseModel = new PtDiseaseModel(
                                            hpId,
                                            random.Next(9999, 9999999),
                                            random.Next(9999, 9999999),
                                            "ByoCdUT",
                                            random.Next(999, 99999),
                                            new() { prefixSuffixModel },
                                            new() { prefixSuffixModel },
                                            "ByomeiPtDisease",
                                            20220202,
                                            random.Next(999, 99999),
                                            20220202,
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            0,
                                            0,
                                            0,// isDeleted
                                            random.Next(999, 99999),// id
                                            0,
                                            0,
                                            "",
                                            "",
                                            "",
                                            "",
                                            random.Next(999, 99999),
                                            "HosokuCmtPtDisease");
        PtByomei? ptByomei = new PtByomei()
        {
            HpId = hpId,
            PtId = ptDiseaseModel.PtId,
            Id = ptDiseaseModel.Id,
        };
        PtByomei? ptByomeiAfter = null;

        tenant.PtByomeis.Add(ptByomei);
        DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var idList = diseaseRepository.Upsert(new() { ptDiseaseModel }, hpId, userId);

            // Assert
            ptByomeiAfter = tenant.PtByomeis.FirstOrDefault(item => item.HpId == hpId
                                                                       && idList.Contains(item.Id)
                                                                       && item.PtId == ptDiseaseModel.PtId
                                                                       && item.ByomeiCd == ptDiseaseModel.ByomeiCd
                                                                       && item.SortNo == ptDiseaseModel.SortNo
                                                                       && item.SyusyokuCd1 == (ptDiseaseModel.PrefixSuffixList.Count > 0 ? ptDiseaseModel.PrefixSuffixList[0].Code : string.Empty)
                                                                       && item.SyusyokuCd2 == (ptDiseaseModel.PrefixSuffixList.Count > 1 ? ptDiseaseModel.PrefixSuffixList[1].Code : string.Empty)
                                                                       && item.SyusyokuCd3 == (ptDiseaseModel.PrefixSuffixList.Count > 2 ? ptDiseaseModel.PrefixSuffixList[2].Code : string.Empty)
                                                                       && item.SyusyokuCd4 == (ptDiseaseModel.PrefixSuffixList.Count > 3 ? ptDiseaseModel.PrefixSuffixList[3].Code : string.Empty)
                                                                       && item.SyusyokuCd5 == (ptDiseaseModel.PrefixSuffixList.Count > 4 ? ptDiseaseModel.PrefixSuffixList[4].Code : string.Empty)
                                                                       && item.SyusyokuCd6 == (ptDiseaseModel.PrefixSuffixList.Count > 5 ? ptDiseaseModel.PrefixSuffixList[5].Code : string.Empty)
                                                                       && item.SyusyokuCd7 == (ptDiseaseModel.PrefixSuffixList.Count > 6 ? ptDiseaseModel.PrefixSuffixList[6].Code : string.Empty)
                                                                       && item.SyusyokuCd8 == (ptDiseaseModel.PrefixSuffixList.Count > 7 ? ptDiseaseModel.PrefixSuffixList[7].Code : string.Empty)
                                                                       && item.SyusyokuCd9 == (ptDiseaseModel.PrefixSuffixList.Count > 8 ? ptDiseaseModel.PrefixSuffixList[8].Code : string.Empty)
                                                                       && item.SyusyokuCd10 == (ptDiseaseModel.PrefixSuffixList.Count > 9 ? ptDiseaseModel.PrefixSuffixList[9].Code : string.Empty)
                                                                       && item.SyusyokuCd11 == (ptDiseaseModel.PrefixSuffixList.Count > 10 ? ptDiseaseModel.PrefixSuffixList[10].Code : string.Empty)
                                                                       && item.SyusyokuCd12 == (ptDiseaseModel.PrefixSuffixList.Count > 11 ? ptDiseaseModel.PrefixSuffixList[11].Code : string.Empty)
                                                                       && item.SyusyokuCd13 == (ptDiseaseModel.PrefixSuffixList.Count > 12 ? ptDiseaseModel.PrefixSuffixList[12].Code : string.Empty)
                                                                       && item.SyusyokuCd14 == (ptDiseaseModel.PrefixSuffixList.Count > 13 ? ptDiseaseModel.PrefixSuffixList[13].Code : string.Empty)
                                                                       && item.SyusyokuCd15 == (ptDiseaseModel.PrefixSuffixList.Count > 14 ? ptDiseaseModel.PrefixSuffixList[14].Code : string.Empty)
                                                                       && item.SyusyokuCd16 == (ptDiseaseModel.PrefixSuffixList.Count > 15 ? ptDiseaseModel.PrefixSuffixList[15].Code : string.Empty)
                                                                       && item.SyusyokuCd17 == (ptDiseaseModel.PrefixSuffixList.Count > 16 ? ptDiseaseModel.PrefixSuffixList[16].Code : string.Empty)
                                                                       && item.SyusyokuCd18 == (ptDiseaseModel.PrefixSuffixList.Count > 17 ? ptDiseaseModel.PrefixSuffixList[17].Code : string.Empty)
                                                                       && item.SyusyokuCd19 == (ptDiseaseModel.PrefixSuffixList.Count > 18 ? ptDiseaseModel.PrefixSuffixList[18].Code : string.Empty)
                                                                       && item.SyusyokuCd20 == (ptDiseaseModel.PrefixSuffixList.Count > 19 ? ptDiseaseModel.PrefixSuffixList[19].Code : string.Empty)
                                                                       && item.SyusyokuCd21 == (ptDiseaseModel.PrefixSuffixList.Count > 20 ? ptDiseaseModel.PrefixSuffixList[20].Code : string.Empty)
                                                                       && item.Byomei == ptDiseaseModel.Byomei
                                                                       && item.StartDate == ptDiseaseModel.StartDate
                                                                       && item.TenkiKbn == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? TenkiKbnConst.Cured : ptDiseaseModel.TenkiKbn)
                                                                       && item.TenkiDate == ptDiseaseModel.TenkiDate
                                                                       && item.SyubyoKbn == ptDiseaseModel.SyubyoKbn
                                                                       && item.SikkanKbn == ptDiseaseModel.SikkanKbn
                                                                       && item.NanByoCd == ptDiseaseModel.NanbyoCd
                                                                       && item.HosokuCmt == ptDiseaseModel.HosokuCmt
                                                                       && item.HokenPid == ptDiseaseModel.HokenPid
                                                                       && item.TogetuByomei == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? 1 : 0)
                                                                       && item.IsNodspRece == ptDiseaseModel.IsNodspRece
                                                                       && item.IsNodspKarte == ptDiseaseModel.IsNodspKarte
                                                                       && item.IsDeleted == 0);

            var ptByomeiDeleted = tenant.PtByomeis.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.Id == ptDiseaseModel.Id
                                                                        && item.PtId == ptDiseaseModel.PtId
                                                                        && item.IsDeleted == 1);

            var result = idList.Any() && ptByomeiAfter != null && ptByomeiDeleted != null;

            Assert.True(result);
        }
        finally
        {
            diseaseRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptByomei != null)
            {
                tenant.PtByomeis.Remove(ptByomei);
            }
            if (ptByomeiAfter != null)
            {
                tenant.PtByomeis.Remove(ptByomeiAfter);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_005_UpsertDiseaseRepository_TestDeletedSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PrefixSuffixModel prefixSuffixModel = new PrefixSuffixModel("codeUT", "nameUT");
        PtDiseaseModel ptDiseaseModel = new PtDiseaseModel(
                                            hpId,
                                            random.Next(9999, 9999999),
                                            random.Next(9999, 9999999),
                                            "ByoCdUT",
                                            random.Next(999, 99999),
                                            new() { prefixSuffixModel },
                                            new() { prefixSuffixModel },
                                            "ByomeiPtDisease",
                                            20220202,
                                            random.Next(999, 99999),
                                            20220202,
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            0,
                                            0,
                                            1,// isDeleted
                                            random.Next(999, 99999),// id
                                            0,
                                            0,
                                            "",
                                            "",
                                            "",
                                            "",
                                            random.Next(999, 99999),
                                            "HosokuCmtPtDisease");
        PtByomei? ptByomei = new PtByomei()
        {
            HpId = hpId,
            PtId = ptDiseaseModel.PtId,
            Id = ptDiseaseModel.Id,
        };

        tenant.PtByomeis.Add(ptByomei);
        DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var idList = diseaseRepository.Upsert(new() { ptDiseaseModel }, hpId, userId);

            // Assert
            var ptByomeiAfter = tenant.PtByomeis.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.Id == ptDiseaseModel.Id
                                                                        && item.PtId == ptDiseaseModel.PtId
                                                                        && item.IsDeleted == 1);

            var result = !idList.Any() && ptByomeiAfter != null;

            Assert.True(result);
        }
        finally
        {
            diseaseRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptByomei != null)
            {
                tenant.PtByomeis.Remove(ptByomei);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion UpsertDiseaseRepository

    #region IsModified
    [Test]
    public void TC_006_UpsertDiseaseRepository_TestIsModifiedFalse()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PrefixSuffixModel prefixSuffixModel = new PrefixSuffixModel("codeUT", "nameUT");
        PtDiseaseModel ptDiseaseModel = new PtDiseaseModel(
                                            hpId,
                                            random.Next(9999, 9999999),
                                            random.Next(9999, 9999999),
                                            "ByoCdUT",
                                            random.Next(999, 99999),
                                            new() { prefixSuffixModel },
                                            new() { prefixSuffixModel },
                                            "ByomeiPtDisease",
                                            20220202,
                                            random.Next(999, 99999),
                                            20220202,
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            0,
                                            0,
                                            0,// isDeleted
                                            random.Next(999, 99999),// id
                                            0,
                                            0,
                                            "",
                                            "",
                                            "",
                                            "",
                                            random.Next(999, 99999),
                                            "HosokuCmtPtDisease");
        PtByomei? ptByomei = new PtByomei()
        {
            HpId = hpId,
            PtId = ptDiseaseModel.PtId,
            SeqNo = ptDiseaseModel.SeqNo,
            ByomeiCd = ptDiseaseModel.ByomeiCd,
            SortNo = ptDiseaseModel.SortNo,
            SyusyokuCd1 = ptDiseaseModel.PrefixSuffixList[0].Code,
            SyusyokuCd2 = ptDiseaseModel.PrefixSuffixList[1].Code,
            Byomei = ptDiseaseModel.Byomei,
            StartDate = ptDiseaseModel.StartDate,
            TenkiKbn = ptDiseaseModel.TenkiKbn,
            TenkiDate = ptDiseaseModel.TenkiDate,
            SyubyoKbn = ptDiseaseModel.SyubyoKbn,
            SikkanKbn = ptDiseaseModel.SikkanKbn,
            NanByoCd = ptDiseaseModel.NanbyoCd,
            IsNodspRece = ptDiseaseModel.IsNodspRece,
            IsNodspKarte = ptDiseaseModel.IsNodspKarte,
            IsDeleted = ptDiseaseModel.IsDeleted,
            Id = ptDiseaseModel.Id,
            IsImportant = ptDiseaseModel.IsImportant,
            HokenPid = ptDiseaseModel.HokenPid,
            HosokuCmt = ptDiseaseModel.HosokuCmt
        };

        tenant.PtByomeis.Add(ptByomei);
        DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var result  = diseaseRepository.IsModified(ptByomei, ptDiseaseModel);

            Assert.True(!result);
        }
        finally
        {
            diseaseRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptByomei != null)
            {
                tenant.PtByomeis.Remove(ptByomei);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_007_UpsertDiseaseRepository_TestIsModifiedTrue()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PrefixSuffixModel prefixSuffixModel = new PrefixSuffixModel("codeUT", "nameUT");
        PtDiseaseModel ptDiseaseModel = new PtDiseaseModel(
                                            hpId,
                                            random.Next(9999, 9999999),
                                            random.Next(9999, 9999999),
                                            "ByoCdUT",
                                            random.Next(999, 99999),
                                            new() { prefixSuffixModel },
                                            new() { prefixSuffixModel },
                                            "ByomeiPtDisease",
                                            20220202,
                                            random.Next(999, 99999),
                                            20220202,
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            0,
                                            0,
                                            0,// isDeleted
                                            random.Next(999, 99999),// id
                                            0,
                                            0,
                                            "",
                                            "",
                                            "",
                                            "",
                                            random.Next(999, 99999),
                                            "HosokuCmtPtDisease");
        PtByomei? ptByomei = new PtByomei()
        {
            HpId = hpId,
            PtId = ptDiseaseModel.PtId,
            Id = ptDiseaseModel.Id,
        };

        tenant.PtByomeis.Add(ptByomei);
        DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            var result  = diseaseRepository.IsModified(ptByomei, ptDiseaseModel);

            Assert.True(result);
        }
        finally
        {
            diseaseRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptByomei != null)
            {
                tenant.PtByomeis.Remove(ptByomei);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion IsModified

    #region ConvertFromModelToPtByomei
    [Test]
    public void TC_008_ConvertFromModelToPtByomei_TestTenkiKbn()
    {
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PrefixSuffixModel prefixSuffixModel = new PrefixSuffixModel("codeUT", "nameUT");
        PtDiseaseModel ptDiseaseModel = new PtDiseaseModel(
                                            hpId,
                                            random.Next(9999, 9999999),
                                            random.Next(9999, 9999999),
                                            "ByoCdUT",
                                            random.Next(999, 99999),
                                            new() { prefixSuffixModel },
                                            new() { prefixSuffixModel },
                                            "ByomeiPtDisease",
                                            20220202,
                                            TenkiKbnConst.InMonth,
                                            20220202,
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            0,
                                            0,
                                            0,// isDeleted
                                            random.Next(999, 99999),// id
                                            0,
                                            0,
                                            "",
                                            "",
                                            "",
                                            "",
                                            random.Next(999, 99999),
                                            "HosokuCmtPtDisease");
 

        DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);

        // Act
        var ptByomei = diseaseRepository.ConvertFromModelToPtByomei(ptDiseaseModel, hpId, userId);
        var result = ptByomei.HpId == hpId
                     && ptByomei.PtId == ptDiseaseModel.PtId
                     && ptByomei.ByomeiCd == ptDiseaseModel.ByomeiCd
                     && ptByomei.SortNo == ptDiseaseModel.SortNo
                     && ptByomei.SyusyokuCd1 == (ptDiseaseModel.PrefixSuffixList.Count > 0 ? ptDiseaseModel.PrefixSuffixList[0].Code : string.Empty)
                     && ptByomei.SyusyokuCd2 == (ptDiseaseModel.PrefixSuffixList.Count > 1 ? ptDiseaseModel.PrefixSuffixList[1].Code : string.Empty)
                     && ptByomei.SyusyokuCd3 == (ptDiseaseModel.PrefixSuffixList.Count > 2 ? ptDiseaseModel.PrefixSuffixList[2].Code : string.Empty)
                     && ptByomei.SyusyokuCd4 == (ptDiseaseModel.PrefixSuffixList.Count > 3 ? ptDiseaseModel.PrefixSuffixList[3].Code : string.Empty)
                     && ptByomei.SyusyokuCd5 == (ptDiseaseModel.PrefixSuffixList.Count > 4 ? ptDiseaseModel.PrefixSuffixList[4].Code : string.Empty)
                     && ptByomei.SyusyokuCd6 == (ptDiseaseModel.PrefixSuffixList.Count > 5 ? ptDiseaseModel.PrefixSuffixList[5].Code : string.Empty)
                     && ptByomei.SyusyokuCd7 == (ptDiseaseModel.PrefixSuffixList.Count > 6 ? ptDiseaseModel.PrefixSuffixList[6].Code : string.Empty)
                     && ptByomei.SyusyokuCd8 == (ptDiseaseModel.PrefixSuffixList.Count > 7 ? ptDiseaseModel.PrefixSuffixList[7].Code : string.Empty)
                     && ptByomei.SyusyokuCd9 == (ptDiseaseModel.PrefixSuffixList.Count > 8 ? ptDiseaseModel.PrefixSuffixList[8].Code : string.Empty)
                     && ptByomei.SyusyokuCd10 == (ptDiseaseModel.PrefixSuffixList.Count > 9 ? ptDiseaseModel.PrefixSuffixList[9].Code : string.Empty)
                     && ptByomei.SyusyokuCd11 == (ptDiseaseModel.PrefixSuffixList.Count > 10 ? ptDiseaseModel.PrefixSuffixList[10].Code : string.Empty)
                     && ptByomei.SyusyokuCd12 == (ptDiseaseModel.PrefixSuffixList.Count > 11 ? ptDiseaseModel.PrefixSuffixList[11].Code : string.Empty)
                     && ptByomei.SyusyokuCd13 == (ptDiseaseModel.PrefixSuffixList.Count > 12 ? ptDiseaseModel.PrefixSuffixList[12].Code : string.Empty)
                     && ptByomei.SyusyokuCd14 == (ptDiseaseModel.PrefixSuffixList.Count > 13 ? ptDiseaseModel.PrefixSuffixList[13].Code : string.Empty)
                     && ptByomei.SyusyokuCd15 == (ptDiseaseModel.PrefixSuffixList.Count > 14 ? ptDiseaseModel.PrefixSuffixList[14].Code : string.Empty)
                     && ptByomei.SyusyokuCd16 == (ptDiseaseModel.PrefixSuffixList.Count > 15 ? ptDiseaseModel.PrefixSuffixList[15].Code : string.Empty)
                     && ptByomei.SyusyokuCd17 == (ptDiseaseModel.PrefixSuffixList.Count > 16 ? ptDiseaseModel.PrefixSuffixList[16].Code : string.Empty)
                     && ptByomei.SyusyokuCd18 == (ptDiseaseModel.PrefixSuffixList.Count > 17 ? ptDiseaseModel.PrefixSuffixList[17].Code : string.Empty)
                     && ptByomei.SyusyokuCd19 == (ptDiseaseModel.PrefixSuffixList.Count > 18 ? ptDiseaseModel.PrefixSuffixList[18].Code : string.Empty)
                     && ptByomei.SyusyokuCd20 == (ptDiseaseModel.PrefixSuffixList.Count > 19 ? ptDiseaseModel.PrefixSuffixList[19].Code : string.Empty)
                     && ptByomei.SyusyokuCd21 == (ptDiseaseModel.PrefixSuffixList.Count > 20 ? ptDiseaseModel.PrefixSuffixList[20].Code : string.Empty)
                     && ptByomei.Byomei == ptDiseaseModel.Byomei
                     && ptByomei.StartDate == ptDiseaseModel.StartDate
                     && ptByomei.TenkiKbn == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? TenkiKbnConst.Cured : ptDiseaseModel.TenkiKbn)
                     && ptByomei.TenkiDate == ptDiseaseModel.TenkiDate
                     && ptByomei.SyubyoKbn == ptDiseaseModel.SyubyoKbn
                     && ptByomei.SikkanKbn == ptDiseaseModel.SikkanKbn
                     && ptByomei.NanByoCd == ptDiseaseModel.NanbyoCd
                     && ptByomei.HosokuCmt == ptDiseaseModel.HosokuCmt
                     && ptByomei.HokenPid == ptDiseaseModel.HokenPid
                     && ptByomei.TogetuByomei == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? 1 : 0)
                     && ptByomei.IsNodspRece == ptDiseaseModel.IsNodspRece
                     && ptByomei.IsNodspKarte == ptDiseaseModel.IsNodspKarte
                     && ptByomei.IsDeleted == 0;

        Assert.True(result);
    }

    [Test]
    public void TC_009_ConvertFromModelToPtByomei_TestOtherCase()
    {
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PrefixSuffixModel prefixSuffixModel = new PrefixSuffixModel("codeUT", "nameUT");
        PtDiseaseModel ptDiseaseModel = new PtDiseaseModel(
                                            hpId,
                                            random.Next(9999, 9999999),
                                            random.Next(9999, 9999999),
                                            "ByoCdUT",
                                            random.Next(999, 99999),
                                            new() { prefixSuffixModel },
                                            new() { prefixSuffixModel },
                                            "ByomeiPtDisease",
                                            20220202,
                                            random.Next(999, 99999),
                                            20220202,
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            random.Next(999, 99999),
                                            0,
                                            0,
                                            0,// isDeleted
                                            random.Next(999, 99999),// id
                                            0,
                                            0,
                                            "",
                                            "",
                                            "",
                                            "",
                                            random.Next(999, 99999),
                                            "HosokuCmtPtDisease");
 

        DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);

        // Act
        var ptByomei = diseaseRepository.ConvertFromModelToPtByomei(ptDiseaseModel, hpId, userId);
        var result = ptByomei.HpId == hpId
                     && ptByomei.PtId == ptDiseaseModel.PtId
                     && ptByomei.ByomeiCd == ptDiseaseModel.ByomeiCd
                     && ptByomei.SortNo == ptDiseaseModel.SortNo
                     && ptByomei.SyusyokuCd1 == (ptDiseaseModel.PrefixSuffixList.Count > 0 ? ptDiseaseModel.PrefixSuffixList[0].Code : string.Empty)
                     && ptByomei.SyusyokuCd2 == (ptDiseaseModel.PrefixSuffixList.Count > 1 ? ptDiseaseModel.PrefixSuffixList[1].Code : string.Empty)
                     && ptByomei.SyusyokuCd3 == (ptDiseaseModel.PrefixSuffixList.Count > 2 ? ptDiseaseModel.PrefixSuffixList[2].Code : string.Empty)
                     && ptByomei.SyusyokuCd4 == (ptDiseaseModel.PrefixSuffixList.Count > 3 ? ptDiseaseModel.PrefixSuffixList[3].Code : string.Empty)
                     && ptByomei.SyusyokuCd5 == (ptDiseaseModel.PrefixSuffixList.Count > 4 ? ptDiseaseModel.PrefixSuffixList[4].Code : string.Empty)
                     && ptByomei.SyusyokuCd6 == (ptDiseaseModel.PrefixSuffixList.Count > 5 ? ptDiseaseModel.PrefixSuffixList[5].Code : string.Empty)
                     && ptByomei.SyusyokuCd7 == (ptDiseaseModel.PrefixSuffixList.Count > 6 ? ptDiseaseModel.PrefixSuffixList[6].Code : string.Empty)
                     && ptByomei.SyusyokuCd8 == (ptDiseaseModel.PrefixSuffixList.Count > 7 ? ptDiseaseModel.PrefixSuffixList[7].Code : string.Empty)
                     && ptByomei.SyusyokuCd9 == (ptDiseaseModel.PrefixSuffixList.Count > 8 ? ptDiseaseModel.PrefixSuffixList[8].Code : string.Empty)
                     && ptByomei.SyusyokuCd10 == (ptDiseaseModel.PrefixSuffixList.Count > 9 ? ptDiseaseModel.PrefixSuffixList[9].Code : string.Empty)
                     && ptByomei.SyusyokuCd11 == (ptDiseaseModel.PrefixSuffixList.Count > 10 ? ptDiseaseModel.PrefixSuffixList[10].Code : string.Empty)
                     && ptByomei.SyusyokuCd12 == (ptDiseaseModel.PrefixSuffixList.Count > 11 ? ptDiseaseModel.PrefixSuffixList[11].Code : string.Empty)
                     && ptByomei.SyusyokuCd13 == (ptDiseaseModel.PrefixSuffixList.Count > 12 ? ptDiseaseModel.PrefixSuffixList[12].Code : string.Empty)
                     && ptByomei.SyusyokuCd14 == (ptDiseaseModel.PrefixSuffixList.Count > 13 ? ptDiseaseModel.PrefixSuffixList[13].Code : string.Empty)
                     && ptByomei.SyusyokuCd15 == (ptDiseaseModel.PrefixSuffixList.Count > 14 ? ptDiseaseModel.PrefixSuffixList[14].Code : string.Empty)
                     && ptByomei.SyusyokuCd16 == (ptDiseaseModel.PrefixSuffixList.Count > 15 ? ptDiseaseModel.PrefixSuffixList[15].Code : string.Empty)
                     && ptByomei.SyusyokuCd17 == (ptDiseaseModel.PrefixSuffixList.Count > 16 ? ptDiseaseModel.PrefixSuffixList[16].Code : string.Empty)
                     && ptByomei.SyusyokuCd18 == (ptDiseaseModel.PrefixSuffixList.Count > 17 ? ptDiseaseModel.PrefixSuffixList[17].Code : string.Empty)
                     && ptByomei.SyusyokuCd19 == (ptDiseaseModel.PrefixSuffixList.Count > 18 ? ptDiseaseModel.PrefixSuffixList[18].Code : string.Empty)
                     && ptByomei.SyusyokuCd20 == (ptDiseaseModel.PrefixSuffixList.Count > 19 ? ptDiseaseModel.PrefixSuffixList[19].Code : string.Empty)
                     && ptByomei.SyusyokuCd21 == (ptDiseaseModel.PrefixSuffixList.Count > 20 ? ptDiseaseModel.PrefixSuffixList[20].Code : string.Empty)
                     && ptByomei.Byomei == ptDiseaseModel.Byomei
                     && ptByomei.StartDate == ptDiseaseModel.StartDate
                     && ptByomei.TenkiKbn == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? TenkiKbnConst.Cured : ptDiseaseModel.TenkiKbn)
                     && ptByomei.TenkiDate == ptDiseaseModel.TenkiDate
                     && ptByomei.SyubyoKbn == ptDiseaseModel.SyubyoKbn
                     && ptByomei.SikkanKbn == ptDiseaseModel.SikkanKbn
                     && ptByomei.NanByoCd == ptDiseaseModel.NanbyoCd
                     && ptByomei.HosokuCmt == ptDiseaseModel.HosokuCmt
                     && ptByomei.HokenPid == ptDiseaseModel.HokenPid
                     && ptByomei.TogetuByomei == (ptDiseaseModel.TenkiKbn == TenkiKbnConst.InMonth ? 1 : 0)
                     && ptByomei.IsNodspRece == ptDiseaseModel.IsNodspRece
                     && ptByomei.IsNodspKarte == ptDiseaseModel.IsNodspKarte
                     && ptByomei.IsDeleted == 0;

        Assert.True(result);
    }
    #endregion ConvertFromModelToPtByomei
}
