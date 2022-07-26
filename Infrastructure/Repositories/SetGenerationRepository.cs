using Domain.Models.SetGeneration;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class SetGenerationRepository : ISetGenerationRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public SetGenerationRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<SetGenerationMst> GetList(int hpId, int sinDate)
        {
            var setEntities = _tenantDataContext.SetGenerationMsts.Where(s => s.HpId == hpId && s.IsDeleted == 0 && s.StartDate <= sinDate).OrderByDescending(x => x.StartDate);

            if (setEntities == null)
            {
                return new List<SetGenerationMst>();
            }

            return setEntities.Select(s =>
                    new SetGenerationMst(
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
                var generation = _tenantDataContext.SetGenerationMsts.Where(x => x.HpId == hpId && x.StartDate <= sinDate && x.IsDeleted == 0).OrderByDescending(x => x.StartDate).FirstOrDefault();
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
