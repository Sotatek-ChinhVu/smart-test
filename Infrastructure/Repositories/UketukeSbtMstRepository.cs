﻿using Domain.Models.UketukeSbtMst;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class UketukeSbtMstRepository : IUketukeSbtMstRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public UketukeSbtMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public UketukeSbtMstModel? GetByKbnId(int kbnId)
    {
        var entity = _tenantDataContext.UketukeSbtMsts.Where(u => u.KbnId == kbnId && u.IsDeleted == DeleteTypes.None).FirstOrDefault();
        return entity is null ? null : ToModel(entity);
    }

    public List<UketukeSbtMstModel> GetList()
    {
        return _tenantDataContext.UketukeSbtMsts
            .Where(u => u.IsDeleted == DeleteTypes.None)
            .OrderBy(u => u.SortNo).ToList()
            .Select(u => ToModel(u)).ToList();
    }

    private UketukeSbtMstModel ToModel(UketukeSbtMst u)
    {
        return new UketukeSbtMstModel(
            u.KbnId,
            u.KbnName,
            u.SortNo,
            u.IsDeleted);
    }
}
