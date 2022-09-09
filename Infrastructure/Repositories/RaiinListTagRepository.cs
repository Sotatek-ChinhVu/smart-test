using Domain.Models.RainListTag;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class RaiinListTagRepository : IRaiinListTagRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public RaiinListTagRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<RaiinListTagModel> GetList(int hpId, long ptId, bool isNoWithWhiteStar)
        {
            var result = _tenantDataContext.RaiinListTags.Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0 && (!isNoWithWhiteStar && r.TagNo != 0));

            return result.Select(x => new RaiinListTagModel(
                    x.HpId,
                    x.PtId,
                    x.SinDate,
                    x.RaiinNo,
                    x.SeqNo,
                    x.TagNo,
                    x.IsDeleted
                ));
        }
    }
}
