using PostgreDataContext;
using Reporting.NameLabel.Models;

namespace Reporting.NameLabel.DB
{
    public class CoNameLabelFinder
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        public CoNameLabelFinder(TenantNoTrackingDataContext tenantNoTrackingDataContext)
        {
            _tenantNoTrackingDataContext = tenantNoTrackingDataContext;
        }

        public CoPtInfModel FindPtInf(long ptId)
        {
            var ptInfs = _tenantNoTrackingDataContext.PtInfs.Where(p =>
                p.HpId == 1 &&
                p.PtId == ptId &&
                p.IsDelete == 0
            );

            CoPtInfModel result = new();

            if (ptInfs != null && ptInfs.Any())
            {
                result = new CoPtInfModel(ptInfs.First());
            }

            return result;
        }
    }
}
