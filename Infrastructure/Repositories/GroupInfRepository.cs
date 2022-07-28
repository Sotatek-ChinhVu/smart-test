using Domain.Models.GroupInf;
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
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public GroupInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<GroupInfModel> GetAllByPtIdList(List<long> ptIdList)
        {
            var dataGroupPatient = _tenantDataContext.PtGrpInfs.Where(x => x.IsDeleted == 0 && ptIdList.Contains(x.PtId))
                .Select(x => new GroupInfModel(
                    x.HpId,
                    x.PtId,
                    x.GroupId,
                    x.GroupCode ?? string.Empty
                    ))
                .ToList();
            return dataGroupPatient;
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
                .ToList();
            return dataGroupPatient;
        }
    }
}
