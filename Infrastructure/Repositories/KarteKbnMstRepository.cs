using Domain.Models.KarteKbnMst;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class KarteKbnMstRepository : RepositoryBase, IKarteKbnMstRepository
    {
        public KarteKbnMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<KarteKbnMstModel> GetList(int hpId, bool isDeleted)
        {
            var karteInfEntity = NoTrackingDataContext.KarteKbnMst.Where(k => k.HpId == hpId && (isDeleted || k.IsDeleted == 0));

            if (karteInfEntity == null)
            {
                return new List<KarteKbnMstModel>();
            }

            return karteInfEntity.Select(k =>
                    new KarteKbnMstModel(
                            k.HpId,
                            k.KarteKbn,
                            k.KbnName ?? string.Empty,
                            k.KbnShortName ?? string.Empty,
                            k.CanImg,
                            k.SortNo,
                            k.IsDeleted
                    )
                  ).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
