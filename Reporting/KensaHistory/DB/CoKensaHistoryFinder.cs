using Domain.Models.HpInf;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Reporting.KensaHistory.DB
{
    public class CoKensaHistoryFinder : RepositoryBase, ICoKensaHistoryFinder
    {
        public CoKensaHistoryFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {
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
