using Domain.Models.Set;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class SetRepository : ISetRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public SetRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<SetMst> GetList(int hpId, int setKbn, int setKbnEdaNo, string textSearch)
        {
            var setEntities = _tenantDataContext.SetMsts.Where(s => s.HpId == hpId && s.SetKbn == setKbn && s.SetKbnEdaNo == setKbnEdaNo - 1 && s.IsDeleted == 0 && (string.IsNullOrEmpty(textSearch) || (s.SetName != null && s.SetName.Contains(textSearch))))
              .OrderBy(s => s.Level1)
              .ThenBy(s => s.Level2)
              .ThenBy(s => s.Level3)
              .ToList();

            if (setEntities == null)
            {
                return new List<SetMst>();
            }

            return setEntities.Select(s =>
                    new SetMst(
                        s.HpId,
                        s.SetCd,
                        s.SetKbn,
                        s.SetKbnEdaNo,
                        s.GenerationId,
                        s.Level1,
                        s.Level2,
                        s.Level3,
                        s.SetName == null ? String.Empty : s.SetName,
                        s.WeightKbn,
                        s.Color,
                        s.IsDeleted,
                        s.IsGroup
                    )
                  ).ToList();
        }
    }
}
