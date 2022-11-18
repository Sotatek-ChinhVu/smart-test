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
            var listGroupPatient = _tenantDataContext.PtGrpInfs.Where(x => x.IsDeleted == 0 && ptIdList.Contains(x.PtId)).ToList();
            var listGroupDetailMst = _tenantDataContext.PtGrpItems.Where(p => p.IsDeleted == 0).ToList();
            var result =
                from groupPatient in listGroupPatient
                join groupDetailMst in listGroupDetailMst
                on new {p1 = groupPatient.GroupCode, p2 = groupPatient.GroupId} equals new { p1 = groupDetailMst.GrpCode, p2 = groupDetailMst.GrpId} 
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
