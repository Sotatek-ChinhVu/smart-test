using Domain.Models.SetGenerationMst;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class SetGenerationMstRepository : RepositoryBase, ISetGenerationMstRepository
    {
        public SetGenerationMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public IEnumerable<SetGenerationMstModel> GetList(int hpId, int sinDate)
        {
            var setEntities = NoTrackingDataContext.SetGenerationMsts.Where(s => s.HpId == hpId && s.IsDeleted == 0 && s.StartDate <= sinDate).OrderByDescending(x => x.StartDate);

            if (setEntities == null)
            {
                return new List<SetGenerationMstModel>();
            }

            return setEntities.Select(s =>
                    new SetGenerationMstModel(
                        s.HpId,
                        s.GenerationId,
                        s.StartDate,
                        s.IsDeleted
                    )
                  ).ToList();
        }

        public int GetGenerationId(int hpId, int sinDate)
        {
            int generationId = 0;
            try
            {
                var generation = NoTrackingDataContext.SetGenerationMsts.Where(x => x.HpId == hpId && x.StartDate <= sinDate && x.IsDeleted == 0).OrderByDescending(x => x.StartDate).FirstOrDefault();
                if (generation != null)
                {
                    generationId = generation.GenerationId;
                }
            }
            catch
            {
                return 0;
            }
            return generationId;
        }
    }
}
