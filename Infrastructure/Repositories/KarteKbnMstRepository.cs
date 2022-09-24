using Domain.Models.KarteKbnMst;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class KarteKbnMstRepository : IKarteKbnMstRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public KarteKbnMstRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public bool CheckKarteKbn(int karteKbn)
        {
            var check = _tenantDataContext.KarteKbnMst.Any(k => k.KarteKbn == karteKbn);
            return check;
        }

        public List<KarteKbnMstModel> GetCheckKarteKbns(List<int> karteKbns)
        {
            var result = _tenantDataContext.KarteKbnMst.Where(k => karteKbns.Contains(k.KarteKbn)).ToList();
            return result?.Select(
                k => new KarteKbnMstModel(
                            k.HpId,
                            k.KarteKbn,
                            k.KbnName ?? string.Empty,
                            k.KbnShortName ?? string.Empty,
                            k.CanImg,
                            k.SortNo,
                            k.IsDeleted
                    )
                ).ToList() ?? new List<KarteKbnMstModel>();
        }


        public List<KarteKbnMstModel> GetList(int hpId, bool isDeleted)
        {
            var karteInfEntity = _tenantDataContext.KarteKbnMst.Where(k => k.HpId == hpId && (isDeleted || k.IsDeleted == 0));

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
    }
}
