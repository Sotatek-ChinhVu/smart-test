using Domain.Models.SetKbn;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class SetKbnRepository : ISetKbnMstRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public SetKbnRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<SetKbnMst> GetList(int hpId, int setKbnFrom, int setKbnTo)
        {
            var setEntities = _tenantDataContext.SetKbnMsts.Where(s => s.HpId == hpId && s.SetKbn >= setKbnFrom && s.SetKbn <= setKbnTo && s.IsDeleted == 0).OrderBy(s => s.SetKbn).ToList();

            if (setEntities == null)
            {
                return new List<SetKbnMst>();
            }

            return setEntities.Select(s =>
                    new SetKbnMst(
                        s.HpId,
                        s.SetKbn,
                        s.SetKbnEdaNo,
                        string.IsNullOrEmpty(s.SetKbnName) ? String.Empty : s.SetKbnName,
                        s.KaCd,
                        s.DocCd,
                        s.IsDeleted,
                        s.GenerationId
                    )
                  ).ToList();
        }
    }
}
