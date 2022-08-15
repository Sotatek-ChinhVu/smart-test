using Domain.Models.KensaMst;
using Domain.Models.PtCmtInf;
using Domain.Models.PtInfection;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class KensaMstRepository : IKensaMstRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public KensaMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<KensaMstModel> GetList(int hpId)
    {
        var ptInfections = _tenantDataContext.KensaMsts.Where(x => x.HpId == hpId && x.IsDelete == 0 && x.KensaItemCd.StartsWith("V")).OrderBy(x => x.SortNo).Select(x => new KensaMstModel(
              x.HpId,
              x.KensaItemCd,
              x.KensaItemSeqNo,
              x.CenterCd ?? String.Empty,
              x.KensaName ?? String.Empty,
              x.KensaKana ?? String.Empty,
              x.Unit ?? String.Empty,
              x.MaterialCd,
              x.ContainerCd,
              x.MaleStd ?? String.Empty,
              x.MaleStdLow ?? String.Empty,
              x.FemaleStdHigh ?? String.Empty,
              x.FemaleStd ?? String.Empty,
              x.FemaleStdLow ?? String.Empty,
              x.FemaleStdHigh ?? String.Empty,
              x.Formula ?? String.Empty,
              x.OyaItemCd ?? String.Empty,
              x.OyaItemSeqNo,
              x.SortNo,
              x.CenterItemCd1 ?? String.Empty,
              x.CenterItemCd2 ?? String.Empty,
              x.IsDelete,
              x.Digit
            ));
        return ptInfections.ToList();
    }
}
