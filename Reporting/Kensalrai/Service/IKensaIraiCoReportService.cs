using Reporting.Mappers.Common;

namespace Reporting.Kensalrai.Service
{
    public interface IKensaIraiCoReportService
    {
        CommonReportingRequestModel GetKensalraiData(int HpId, int SystemDate, int FromDate, int ToDate, string CenterCd);
    }
}
