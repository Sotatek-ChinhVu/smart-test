using Reporting.Accounting.Model;
using Reporting.CommonMasters.Enums;
using Reporting.OrderLabel.Model;

namespace Reporting.ReportServices;

public interface ICheckOpenReportingService
{
    bool CheckOpenAccountingForm(int hpId, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> multiAccountDueListModels, bool isPrintMonth, bool ryoshusho, bool meisai);

    bool CheckOpenAccountingForm(int hpId, long ptId, int printTypeInput, List<long> raiinNoList, List<long> raiinNoPayList, bool isCalculateProcess = false);

    CoPrintExitCode CheckOpenOrderLabel(int mode, int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, List<RsvkrtOdrInfModel> rsvKrtOdrInfModels);
}
