using Reporting.Mappers.Common;
using Reporting.OrderLabel.Model;

namespace Reporting.OrderLabel.Service;

public interface IOrderLabelCoReportService
{
    CommonReportingRequestModel GetOrderLabelReportingData(int mode, int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, List<RsvkrtOdrInfModel> rsvKrtOdrInfModels);

    void ReleaseResource();
}
