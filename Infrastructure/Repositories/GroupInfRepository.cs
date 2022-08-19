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
            var result =
                from groupPatient in _tenantDataContext.PtGrpInfs.Where(x => x.IsDeleted == 0 && ptIdList.Contains(x.PtId))
                join groupDetailMst in _tenantDataContext.PtGrpItems.Where(p => p.IsDeleted == 0)
                on groupPatient.GroupCode equals groupDetailMst.GrpCode
                select new GroupInfModel
                (
                    groupPatient.HpId,
                    groupPatient.PtId,
                    groupPatient.GroupId,
                    groupPatient.GroupCode ?? string.Empty,
                    groupDetailMst.GrpCodeName
                );

            return result.ToList();
        }

        public IEnumerable<GroupInfModel> GetDataGroup(int hpId, long ptId)
        {
            var dataPtGrpInfs = _tenantDataContext.PtGrpInfs.Where(x => x.IsDeleted == 0 && x.HpId == hpId && x.PtId == ptId)
                                .Select( x => new GroupInfModel(
                                    hpId,
                                    ptId,
                                    x.GroupId,
                                    x.GroupCode ?? string.Empty,
                                    ""
                                    )).ToList();

            return dataPtGrpInfs;
        }
    }
}
