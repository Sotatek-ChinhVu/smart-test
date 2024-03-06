using Reporting.Mappers.Common;

namespace Reporting.NameLabel.Service;

public interface INameLabelService
{
    CommonReportingRequestModel GetNameLabelReportingData(int hpId,long ptId, string kanjiName, int sinDate);
}
