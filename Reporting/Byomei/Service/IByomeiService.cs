using Reporting.Mappers.Common;

namespace Reporting.Byomei.Service;

public interface IByomeiService
{
    CommonReportingRequestModel GetByomeiReportingData(int hpId, long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds);

    void ReleaseResource();
}
