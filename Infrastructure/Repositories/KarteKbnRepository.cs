using Domain.Models.KarteInfs;
using Domain.Models.KarteKbn;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Text;

namespace Infrastructure.Repositories
{
    public class KarteKbnRepository : IKarteKbnRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public KarteKbnRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<KarteKbnMst> GetList(int hpId, bool isDeleted)
        {
            var karteInfEntity = _tenantDataContext.KarteKbnMst.Where(k => k.HpId == hpId && (isDeleted || k.IsDeleted == 0));

            if (karteInfEntity == null)
            {
                return new List<KarteKbnMst>();
            }

            return karteInfEntity.Select(k =>
                    new KarteKbnMst(
                            k.HpId,
                            k.KarteKbn,
                            k.KbnName,
                            k.KbnShortName,
                            k.CanImg,
                            k.SortNo,
                            k.IsDeleted
                    )
                  ).ToList();
        }
    }
}
