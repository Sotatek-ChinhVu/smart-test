﻿using Domain.Models.PtAlrgyDrug;
using Domain.Models.PtAlrgyElse;
using Domain.Models.PtAlrgyFood;
using Domain.Models.PtCmtInf;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtAlrgyDrugRepository : IPtAlrgyDrugRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtAlrgyDrugRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtAlrgyDrugModel> GetList(long ptId)
    {
        var ptAlrgyDrugs = _tenantDataContext.PtAlrgyDrugs.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyDrugModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ItemCd,
               x.DrugName,
               x.StartDate,
               x.EndDate,
               x.Cmt,
               x.IsDeleted
            ));
        return ptAlrgyDrugs.ToList();
    }
}
