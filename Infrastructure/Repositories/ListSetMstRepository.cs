using Domain.Models.ListSetMst;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class ListSetMstRepository : RepositoryBase, IListSetMstRepository
    {
        public ListSetMstRepository(ITenantProvider _tenantProvider) : base(_tenantProvider)
        {
        }
        public int GetGenerationId(int sinDate)
        {
            ListSetGenerationMst? generation = NoTrackingDataContext.ListSetGenerationMsts.Where
                            (item => item.StartDate <= sinDate)
                            .OrderByDescending(item => item.StartDate)
                            .FirstOrDefault();

            return generation?.GenerationId ?? 0;
        }
        public List<ListSetMstModel> GetListSetMst(int hpId, int setKbn, int generationId)
        {
            var list = NoTrackingDataContext.ListSetMsts.Where(x => x.HpId == hpId
                                                        && x.GenerationId == generationId
                                                        && x.IsDeleted == 0
                                                        && x.SetKbn == setKbn);

            return (from item in list
                    select new ListSetMstModel(item.HpId,
                                               item.GenerationId,
                                               item.SetId,
                                               item.SetName ?? string.Empty,
                                               item.ItemCd ?? string.Empty,
                                               item.IsTitle,
                                               item.SetKbn,
                                               item.SelectType,
                                               item.Suryo,
                                               item.Level1,
                                               item.Level2,
                                               item.Level3,
                                               item.Level4,
                                               item.Level5,
                                               item.CmtName ?? string.Empty,
                                               item.CmtOpt ?? string.Empty)).ToList();
        }
        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
