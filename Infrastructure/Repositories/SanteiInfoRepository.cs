using Domain.Models.SanteiInfo;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class SanteiInfoRepository : ISanteiInfoRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public SanteiInfoRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public IEnumerable<SanteiInfoModel> GetList(int hpId, long ptId, int sinDate)
    {
        List<int> listAletTermIsValid = new List<int>() { 2, 3, 4, 5, 6 };

        var santeiInfos = _tenantDataContext.SanteiInfs.Where(x => (x.PtId == ptId || x.PtId == 0) && x.HpId == hpId && x.AlertDays > 0 && listAletTermIsValid.Contains(x.AlertTerm)).Select(x => new SanteiInfoModel(
                x.HpId,
                x.PtId,
                x.ItemCd,
                x.SeqNo,
                x.AlertDays,
                x.AlertTerm,
                x.Id,
                _tenantDataContext.SanteiInfDetails.Where(s => s.HpId == hpId && s.PtId == ptId && s.KisanDate > 0 && s.EndDate >= sinDate && s.IsDeleted == 0 && s.ItemCd == x.ItemCd)
                .Select(sd => new SanteiInfoDetailModel(
                    sd.HpId,
                    sd.PtId,
                    sd.ItemCd ?? String.Empty,
                    sd.SeqNo,
                    sd.EndDate,
                    sd.KisanSbt,
                    sd.KisanDate,
                    sd.Byomei,
                    sd.HosokuComment,
                    sd.Comment,
                    sd.IsDeleted,
                    sd.Id
                )).AsEnumerable()
            ));
        return santeiInfos;
    }
}
