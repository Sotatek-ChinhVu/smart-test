using Domain.Models.PostCodeMst;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Linq.Expressions;

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

        public List<PostCodeMstModel> PostCodeMstModels(string postCode1, string postCode2, string address)
        {
            var entities = _tenantDataContextNoTracking.PostCodeMsts.Where(x => x.IsDeleted == 0);
            if (!string.IsNullOrEmpty(postCode1))
                entities = entities.Where(item => item.PostCd.StartsWith(postCode1));

            if (!string.IsNullOrEmpty(postCode2))
                entities = entities.Where(item => item.PostCd.EndsWith(postCode2));

            if (!string.IsNullOrEmpty(postCode1) && !string.IsNullOrEmpty(postCode2))
                entities = entities.Where(item => item.PostCd.Contains(postCode1 + postCode2));

            var query = entities;

            if (!string.IsNullOrEmpty(address))
            {
                // PrefName + CityName + Banti
                Expression<Func<PostCodeMst, bool>> containsAddressFull = x =>
                (x.PrefName + x.CityName + x.Banti).Contains(address)
                && !string.IsNullOrEmpty(x.CityName);
                query = entities.Where(containsAddressFull);

                // PrefName + CityName
                if (query.Count() == 0)
                {
                    Expression<Func<PostCodeMst, bool>> containsAddressHalf = x =>
                    ((x.PrefName + x.CityName).Contains(address));
                    query = entities.Where(containsAddressHalf);
                }

                // PrefName
                if (query.Count() == 0)
                {
                    query = entities.Where(item => item.PrefName == address);
                }
            }

            var result = query.OrderBy(x => x.PostCd)
                                  .ThenBy(x => x.PrefName)
                                  .ThenBy(x => x.CityName)
                                  .ThenBy(x => x.Banti)
                                  .Select(x => new PostCodeMstModel(
                                      x.Id,
                                      x.HpId,
                                      x.PostCd,
                                      x.PrefKana,
                                      x.CityKana,
                                      x.PostalTermKana,
                                      x.PrefName,
                                      x.CityName,
                                      x.Banti,
                                      x.IsDeleted))
                                  .ToList();
            return result;
        }
    }
}
