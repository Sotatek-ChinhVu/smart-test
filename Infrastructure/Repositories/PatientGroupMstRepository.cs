using Domain.Models.PatientGroupMst;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class PatientGroupMstRepository : IPatientGroupMstRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public PatientGroupMstRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<PatientGroupMstModel> GetAll()
        {
            var groupMstList = _tenantDataContext.PtGrpNameMsts.Where(p => p.IsDeleted == 0).ToList();
            var groupDetailList = _tenantDataContext.PtGrpItems.Where(p => p.IsDeleted == 0).ToList();

            List<PatientGroupMstModel> result = new List<PatientGroupMstModel>();
            foreach (var groupMst in groupMstList)
            {
                int groupId = groupMst.GrpId;
                var groupDetailListByGroupId = groupDetailList.Where(p => p.GrpId == groupId).ToList();

                result.Add(ConvertToModel(groupMst, groupDetailListByGroupId));
            }

            return result;
        }

        private PatientGroupMstModel ConvertToModel(PtGrpNameMst groupMst, List<PtGrpItem> groupDetailList)
        {
            return new PatientGroupMstModel
                (
                    groupMst.GrpId,
                    groupMst.SortNo,
                    groupMst.GrpName ?? string.Empty,
                    groupDetailList.Select(g => new PatientGroupDetailModel(g.GrpId, g.GrpCode, g.SeqNo, g.SortNo, g.GrpCodeName)).ToList()
                );
        }
    }
}
