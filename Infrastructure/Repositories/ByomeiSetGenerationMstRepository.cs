using Domain.Models.ByomeiSetGenerationMst;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class ByomeiSetGenerationMstRepository : RepositoryBase, IByomeiSetGenerationMstRepository
    {
        public ByomeiSetGenerationMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }
        public List<ByomeiSetGenerationMstModel> GetAll(int hpId)
        {
            var listSetGenerationMstList = NoTrackingDataContext.ByomeiSetGenerationMsts.Where(s => s.HpId == hpId && s.IsDeleted == 0).OrderByDescending(s => s.StartDate).Select(s =>
                    new ByomeiSetGenerationMstModel(
                        s.HpId,
                        s.GenerationId,
                        s.StartDate,
                        s.IsDeleted
                    )
                  ).ToList();
            return listSetGenerationMstList;
        }
        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
