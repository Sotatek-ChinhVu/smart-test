using Domain.Models.SetKbnMst;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class SetKbnMstRepository : ISetKbnMstRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public SetKbnMstRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<SetKbnMstModel> GetList(int hpId, int setKbnFrom, int setKbnTo)
        {
            var setEntities = _tenantDataContext.SetKbnMsts.Where(s => s.HpId == hpId && s.SetKbn >= setKbnFrom && s.SetKbn <= setKbnTo && s.IsDeleted == 0).OrderBy(s => s.SetKbn).ToList();

            if (setEntities == null)
            {
                return new List<SetKbnMstModel>();
            }

            return setEntities.Select(s =>
                    new SetKbnMstModel(
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
