﻿using Domain.Models.GroupInf;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GroupInfRepository: IGroupInfRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public GroupInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<GroupInfModel> GetDataGroup(int hpId, long ptId)
        {
            var dataGroupPatient = _tenantDataContext.PtGrpInfs.Where(x => x.IsDeleted == 0 && x.HpId == hpId && x.PtId == ptId)
                .Select( x => new GroupInfModel(
                    x.HpId,
                    x.PtId,
                    x.GroupId,
                    x.GroupCode ?? string.Empty
                    ))
                .OrderBy(x => x.GroupId)
                .ToList();
            return dataGroupPatient;
        }
    }
}
