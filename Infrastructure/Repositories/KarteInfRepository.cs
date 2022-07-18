using Domain.Models.KarteInfs;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class KarteInfRepository : IKarteInfRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public KarteInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public List<KarteInf> GetList(long ptId, long rainNo, long sinDate)
        {
            var karteInfEntity = _tenantDataContext.KarteInfs.Where(k => k.PtId == ptId && k.RaiinNo == rainNo && k.SinDate == sinDate);

            if (karteInfEntity == null)
            {
                return new List<KarteInf>();
            }

            return karteInfEntity.Select(k =>
                    new KarteInf(
                        k.HpId,
                        k.RaiinNo,
                        k.KarteKbn,
                        k.SeqNo,
                        k.PtId,
                        k.SinDate,
                        k.Text,
                        k.IsDeleted,
                        k.RichText
                    )
                  ).ToList();
        }
    }
}
