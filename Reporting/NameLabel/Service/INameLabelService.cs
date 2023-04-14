using Reporting.Mappers.Common;

namespace Reporting.NameLabel.Service;

public interface INameLabelService
{
    CommonReportingRequestModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate);
}
