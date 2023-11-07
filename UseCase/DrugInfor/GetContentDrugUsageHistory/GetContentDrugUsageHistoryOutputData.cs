using UseCase.Core.Sync.Core;
using UseCase.DrugInfor.Model;

namespace UseCase.DrugInfor.GetContentDrugUsageHistory;

public class GetContentDrugUsageHistoryOutputData : IOutputData
{
    public GetContentDrugUsageHistoryOutputData(List<DrugUsageHistoryContentModel> drugUsageHistory, GetContentDrugUsageHistoryStatus status)
    {
        DrugUsageHistory = drugUsageHistory;
        Status = status;
    }

    public List<DrugUsageHistoryContentModel> DrugUsageHistory { get; private set; }

    public GetContentDrugUsageHistoryStatus Status { get; private set; }
}
