using Domain.Models.ListSetGenerationMst;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class ListSetGenerationMstRepository : RepositoryBase, IListSetGenerationMstRepository
    {
        public ListSetGenerationMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }
        public List<ListSetGenerationMstModel> GetAll(int hpId)
        {
            var listSetGenerationMstList = NoTrackingDataContext.ListSetGenerationMsts.Where(s => s.HpId == hpId && s.IsDeleted == 0).OrderByDescending(s => s.StartDate).Select(s =>
                    new ListSetGenerationMstModel(
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
