using Infrastructure.Interfaces;
using Reporting.CommonMasters.Config;
using Reporting.Mappers.Common;
using Reporting.ReceiptCheck.DB;

namespace Reporting.ReceiptCheck.Service;

public class ReceiptCheckCoReportService: IReceiptCheckCoReportService
{
    private readonly ITenantProvider _tenantProvider;

    public ReceiptCheckCoReportService(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
    }

    public CommonReportingRequestModel GetMedicalRecordWebIdReportingData(int hpId, List<long> ptIds, int seikyuYm)
    {
        using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
        {
            var finder = new CoReceiptCheckFinder(_tenantProvider);

            // データ取得
            var coModel = finder.GetCoReceiptChecks(hpId, ptIds, seikyuYm);

            return new CoMedicalRecordWebIdMapper(coModel).GetData();
        }
    }
}
