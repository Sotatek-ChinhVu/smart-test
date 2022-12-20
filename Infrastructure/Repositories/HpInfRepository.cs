using Domain.Models.HpInf;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class HpInfRepository : RepositoryBase, IHpInfRepository
    {
        public HpInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public bool CheckHpId(int hpId)
        {
            var check = NoTrackingDataContext.HpInfs.Any(hp => hp.HpId == hpId);

            return check;

        }

        public HpInfModel GetHpInf(int hpId)
        {
            var hpInf = NoTrackingDataContext.HpInfs.FirstOrDefault(item => item.HpId == hpId);
            return hpInf != null ? new HpInfModel(hpId,
                                                    hpInf.StartDate,
                                                    hpInf.HpCd ?? string.Empty,
                                                    hpInf.RousaiHpCd ?? string.Empty,
                                                    hpInf.HpName ?? string.Empty,
                                                    hpInf.ReceHpName ?? string.Empty,
                                                    hpInf.KaisetuName ?? string.Empty,
                                                    hpInf.PostCd ?? string.Empty,
                                                    hpInf.PrefNo,
                                                    hpInf.Address1 ?? string.Empty,
                                                    hpInf.Address2 ?? string.Empty,
                                                    hpInf.Tel ?? string.Empty,
                                                    hpInf.FaxNo ?? string.Empty,
                                                    hpInf.OtherContacts ?? string.Empty
                                                ) : new HpInfModel();
        }
    }
}
