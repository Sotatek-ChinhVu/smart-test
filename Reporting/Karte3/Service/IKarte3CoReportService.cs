using Reporting.Mappers.Common;

namespace Reporting.Karte3.Service;

public interface IKarte3CoReportService
{
    CommonReportingRequestModel GetKarte3PrintData(int hpId, long ptId, int startSinYm, int endSinYm, bool includeHoken, bool includeJihi);
}
