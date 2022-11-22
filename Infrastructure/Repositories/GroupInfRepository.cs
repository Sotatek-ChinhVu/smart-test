using Domain.Models.GroupInf;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class GroupInfRepository : IGroupInfRepository
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
                (from groupPatient in listGroupPatient
                 join groupDetailMst in listGroupDetailMst
                 on new
                 {
                     p1 = groupPatient.GroupCode,
                     p2 = groupPatient.GroupId,
                     p3 = groupPatient.HpId
                 } equals new
                 {
                     p1 = groupDetailMst.GrpCode,
                     p2 = groupDetailMst.GrpId,
                     p3 = groupDetailMst.HpId
                 }
                 select new GroupInfModel
                 (
                     groupPatient.HpId,
                     groupPatient.PtId,
                     groupPatient.GroupId,
                     groupPatient.GroupCode ?? string.Empty,
                     groupDetailMst.GrpCodeName
                 )).ToList();
            var resultData = result.DistinctBy(item => new { item.GroupCode, item.PtId }).ToList();
            return resultData;
        }

        public IEnumerable<GroupInfModel> GetDataGroup(int hpId, long ptId)
        {
            var dataPtGrpInfs = _tenantDataContext.PtGrpInfs.Where(x => x.IsDeleted == 0 && x.HpId == hpId && x.PtId == ptId)
                                .Select(x => new GroupInfModel(
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
