using Domain.Models.Family;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class FamilyRepository : RepositoryBase, IFamilyRepository
{
    public FamilyRepository(ITenantProvider tenantProvider) : base(tenantProvider) { }

    public List<FamilyModel> GetListFamily(int hpId, long ptId, int sinDate)
    {
        var ptFamilys = NoTrackingDataContext.PtFamilys.Where(item => item.HpId == hpId
                                                                        && item.PtId == ptId
                                                                        && item.IsDeleted != 1)
                                                        .ToList();

        var listFamilyPtIds = ptFamilys.Select(item => item.FamilyPtId).ToList();
        var listFamilyIds = ptFamilys.Select(item => item.FamilyId).ToList();

        var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                && listFamilyPtIds.Contains(item.PtId)
                                                                && item.IsDelete != 1)
                                                 .ToList();

        var ptFamilyRekis = NoTrackingDataContext.PtFamilyRekis.Where(u => u.HpId == hpId
                                                                            && listFamilyIds.Contains(u.FamilyId)
                                                                            && !string.IsNullOrEmpty(u.Byomei)
                                                                            && u.IsDeleted != 1)
                                                               .OrderBy(u => u.SortNo)
                                                               .ToList();

        return ptFamilys.Select(item => ConvertToFamilyModel(sinDate, item, ptInfs, ptFamilyRekis))
                        .OrderBy(item => item.SortNo)
                        .ToList();
    }

    public List<FamilyModel> GetFamilyListByPtId(int hpId, long ptId, int sinDate)
    {
        var ptFamilyRepo = NoTrackingDataContext.PtFamilys.Where(item =>
            item.HpId == hpId && item.PtId == ptId && item.IsDeleted == 0);
        var ptInfRepo = NoTrackingDataContext.PtInfs.Where(item =>
            item.HpId == hpId && item.IsDelete == 0);
        var ptFamilyRekis = NoTrackingDataContext.PtFamilyRekis
            .Where(u => u.HpId == hpId && !string.IsNullOrEmpty(u.Byomei) && u.IsDeleted == 0)
            .OrderBy(u => u.SortNo);
        var query =
        (
            from ptFamily in ptFamilyRepo
            join ptInf in ptInfRepo on ptFamily.FamilyPtId equals ptInf.PtId into ptInfList
            from ptInfItem in ptInfList.DefaultIfEmpty()
            join rekiInfo in ptFamilyRekis on ptFamily.FamilyId equals rekiInfo.FamilyId into listPtFamilyRekiInfo
            select new
            {
                PtFamily = ptFamily,
                PtInf = ptInfItem,
                ListPtFamilyRekiInfo = listPtFamilyRekiInfo
            }
        );
        return query.AsEnumerable().Select(data => new FamilyModel(
                data.PtFamily.SeqNo,
                data.PtFamily.ZokugaraCd ?? string.Empty,
                data.PtFamily.FamilyId,
                data.PtFamily.Name ?? string.Empty,
                data.PtFamily.Sex,
                data.PtFamily.Birthday,
                CIUtil.SDateToAge(data.PtInf.Birthday, sinDate),
                data.PtFamily.IsDead,
                data.PtFamily.IsSeparated,
                data.PtFamily.Biko ?? string.Empty,
                data.PtFamily.SortNo,
                data.ListPtFamilyRekiInfo.Select(
                        r => new PtFamilyRekiModel(
                                r.Id,
                                r.ByomeiCd ?? string.Empty,
                                r.Byomei ?? string.Empty,
                                r.Cmt ?? string.Empty,
                                r.SortNo
                            )
                ).ToList()
            )).OrderBy(item => item.SortNo).ToList();
    }

    #region private function
    private FamilyModel ConvertToFamilyModel(int sinDate, PtFamily ptFamily, List<PtInf> ptInfs, List<PtFamilyReki> ptFamilyRekis)
    {
        var ptInf = ptInfs.FirstOrDefault(item => ptFamily.FamilyPtId > 0 && item.PtId == ptFamily.FamilyPtId);
        var ptFamilyRekiFilter = ptFamilyRekis.Where(item => item.FamilyId == ptFamily.FamilyId)
                                              .Select(item => ConvertToPtFamilyRekiModel(item))
                                              .ToList();

        long familyPtNum = ptInf != null ? ptInf.PtNum : 0;
        string name = ptInf != null ? ptInf.Name ?? string.Empty : ptFamily.Name ?? string.Empty;
        int sex = ptInf != null ? ptInf.Sex : ptFamily.Sex;
        int birthday = ptInf != null ? ptInf.Birthday : ptFamily.Birthday;
        int isDead = ptInf != null ? ptInf.IsDead : ptFamily.IsDead;
        return new FamilyModel(
                                    ptFamily.SeqNo,
                                    ptFamily.ZokugaraCd ?? string.Empty,
                                    familyPtNum,
                                    name,
                                    sex,
                                    birthday,
                                    CIUtil.SDateToAge(birthday, sinDate),
                                    isDead,
                                    ptFamily.IsSeparated,
                                    ptFamily.Biko ?? string.Empty,
                                    ptFamily.SortNo,
                                    ptFamilyRekiFilter
                               );
    }

    private PtFamilyRekiModel ConvertToPtFamilyRekiModel(PtFamilyReki ptFamilyReki)
    {
        return new PtFamilyRekiModel(
                                        ptFamilyReki.Id,
                                        ptFamilyReki.ByomeiCd ?? string.Empty,
                                        ptFamilyReki.Byomei ?? string.Empty,
                                        ptFamilyReki.Cmt ?? string.Empty,
                                        ptFamilyReki.SortNo
                                    );
    }
    #endregion

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
