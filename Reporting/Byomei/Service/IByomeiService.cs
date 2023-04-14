using Reporting.Mappers.Common;

namespace Reporting.Byomei.Service;

public interface IByomeiService
{
    CommonReportingRequestModel GetByomeiReportingData(long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds);
}
