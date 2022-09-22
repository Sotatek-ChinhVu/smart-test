﻿using Domain.Models.MonshinInf;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MonshinInforRepository : IMonshinInforRepository
    {
        private readonly TenantDataContext _tenantDataContextTracking;
        private readonly TenantDataContext _tenantDataContextNoTracking;

        public MonshinInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
            _tenantDataContextNoTracking = tenantProvider.GetNoTrackingDataContext();
        }

        public List<MonshinInforModel> MonshinInforModels(int hpId, long ptId)
        {
            var monshinList = _tenantDataContextNoTracking.MonshinInfo
                .Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0)
                .OrderByDescending(x => x.SinDate)
                .ThenByDescending(x => x.RaiinNo)
                .Select(x => new MonshinInforModel(
                x.HpId,
                x.PtId,
                x.RaiinNo,
                x.SinDate,
                x.Text))
                .ToList();
            return monshinList;
        }
    }
}
