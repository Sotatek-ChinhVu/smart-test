using Domain.Models.PostCodeMst;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class PostCodeMstRepostitory : IPostCodeMstRepository
    {
        private readonly TenantDataContext _tenantDataContextTracking;
        private readonly TenantDataContext _tenantDataContextNoTracking;

        public PostCodeMstRepostitory(ITenantProvider tenantProvider)
        {
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
            _tenantDataContextNoTracking = tenantProvider.GetNoTrackingDataContext();
        }

        public List<PostCodeMstModel> PostCodeMstModels(int hpId, string postCode1, string postCode2, string address, int pageIndex, int pageCount)
        {
            var entities = _tenantDataContextNoTracking.PostCodeMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0);

            if (!string.IsNullOrEmpty(postCode1) && !string.IsNullOrEmpty(postCode2))
                entities = entities.Where(e => e.PostCd.Contains(postCode1 + postCode2));

            else if (!string.IsNullOrEmpty(postCode1))
                entities = entities.Where(e => e.PostCd.StartsWith(postCode1));

            else if (!string.IsNullOrEmpty(postCode2))
                entities = entities.Where(e => e.PostCd.EndsWith(postCode2));

            if (!string.IsNullOrEmpty(address))
            {
                entities = entities.Where(e => (e.PrefName + e.CityName + e.Banti).Contains(address)
                                                || (e.PrefName + e.CityName).Contains(address)
                                                || e.PrefName.Contains(address));
            }

            var result = entities.OrderBy(x => x.PostCd)
                                  .ThenBy(x => x.PrefName)
                                  .ThenBy(x => x.CityName)
                                  .ThenBy(x => x.Banti)
                                  .Select(x => new PostCodeMstModel(
                                      x.Id,
                                      x.HpId,
                                      x.PostCd ?? string.Empty,
                                      x.PrefKana ?? string.Empty,
                                      x.CityKana ?? string.Empty,
                                      x.PostalTermKana ?? string.Empty,
                                      x.PrefName ?? string.Empty,
                                      x.CityName ?? string.Empty,
                                      x.Banti ?? string.Empty,
                                      x.IsDeleted))
                                  .Skip(pageIndex).Take(pageCount)
                                  .ToList();
            return result;
        }
    }
}
