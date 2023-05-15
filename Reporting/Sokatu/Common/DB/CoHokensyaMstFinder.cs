using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Sokatu.Common.Models;

namespace Reporting.Sokatu.Common.DB
{
    public class CoHokensyaMstFinder : RepositoryBase, ICoHokensyaMstFinder
    {
        public CoHokensyaMstFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {

        }

        public List<CoHokensyaMstModel> GetHokensyaName(int hpId, List<string> hokensyaNos)
        {
            hokensyaNos = hokensyaNos.Distinct().ToList();

            var coHokensyaMsts = NoTrackingDataContext.HokensyaMsts.Where(
                h => h.HpId == hpId && hokensyaNos.Contains(h.HokensyaNo)
            ).ToList();

            return coHokensyaMsts.Select(h => new CoHokensyaMstModel(h)).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
