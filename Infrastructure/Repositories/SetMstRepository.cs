using Domain.Models.SetMst;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class SetMstRepository : ISetMstRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public SetMstRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<SetMstModel> GetList(int hpId, int setKbn, int setKbnEdaNo, string textSearch)
        {
            var setEntities = _tenantDataContext.SetMsts.Where(s => s.HpId == hpId && s.SetKbn == setKbn && s.SetKbnEdaNo == setKbnEdaNo - 1 && s.IsDeleted == 0 && (string.IsNullOrEmpty(textSearch) || (s.SetName != null && s.SetName.Contains(textSearch))))
              .OrderBy(s => s.Level1)
              .ThenBy(s => s.Level2)
              .ThenBy(s => s.Level3)
              .ToList();

            if (setEntities == null)
            {
                return new List<SetMstModel>();
            }

            return setEntities.Select(s =>
                    new SetMstModel(
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
